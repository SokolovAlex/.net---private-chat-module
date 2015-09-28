var ns = ns || {};

ns.loader = (function() {
    var template = $("#chatLoaderTemplate").html();
    var $loaderEl;

    var model = {
        template: template,
        showIn: function ($element) {
            var $el = $element || $('body');
            $el.append(template);
            $loaderEl = $("#chatLoader", $el);
        },
        hide: function ($element) {
            $loaderEl = $loaderEl || $("#chatLoader", $element || $('body'));
            $loaderEl.fadeOut(function () {
                $loaderEl.remove();
                $loaderEl = null;
            });
        }
    }
    return model;
})();

ns.scrollableContainer = function($element,  opt) {
    var $el = $element,
        loader = ns.loader;

    var lockrefresh = false,
        page = 0,
        refresfUrl = opt.url;

    var onPullDownCallback = opt.onPullDown;

    $el.on('scroll', function (e) {
        if (lockrefresh || !onPullDownCallback) {
            return;
        }
        var top = $(e.target).scrollTop();

        if (top == 0) {
            lockrefresh = true;
            page++;

            loader.showIn($el);

            $.ajax({
                url: refresfUrl,
                data: { page: page, itemsPerPage: view.itemsPerPage },
                success: function (response) {
                    loader.hide();
                    if (!response.length) {
                        view.hasAllMessages = true;
                        return;
                    }
                    onPullDownCallback(response);

                    lockrefresh = false;
                }
            });
        }
    });

    var view = {
        $el: $el,
        hasAllMessages: false,
        itemsPerPage: 20,
        scrollDown: function() {
            $el[0].scrollTop = $el[0].scrollHeight;
        },
        onPullDown: function (fn) {
            if (typeof(fn) == "function") {
                onPullDownCallback = fn;
            }
        }
    };

    return view;
};

ns.chatStorage = (function() {
    var storage = ns.storage;

    var authorId,
        recipientId;

    var timeoutId;

    function getClientMessageId() {
        return (new Date()).getTime();
    }

    function getFor() {
        var all = storage.get();
        var forChat = _.filter(all, function (item) {
            return item.authorId == authorId && item.recipientId == recipientId;
        });
        return forChat;
    }

    return {
        getClientId: getClientMessageId,
        startChecking: function (fn, timeout) {
            timeout = timeout || 5 * 1000;
            if (typeof(fn) !== 'function') {
                return;
            }

            var handler = function() {
                var all = getFor();
                fn(all);
                if (all.length) {
                    setTimeout(handler, timeout);
                }
            };

            handler();
        },
        stopCheck: function () {
            clearTimeout(timeoutId);
        },
        setChatData: function (authorIdParam, recipientIdParam) {
            authorId = authorIdParam;
            recipientId = recipientIdParam;
        },
        get: getFor,
        clear: function () {
            storage.sync([]);
        },
        save: function (text, clientMessageIdOuter) {
            var clientMessageId = clientMessageIdOuter || getClientMessageId();
            storage.set({ text: text, clientMessageId: clientMessageId, authorId: authorId, recipientId: recipientId });
            return clientMessageId;
        },
        remove: function (clientMessageId) {
            var all = getFor();

            var rejected = _.reject(all, function (item) {
                return item.clientMessageId == clientMessageId;
            });

            storage.sync(rejected);
        }
    };
})();

//Static - ns.Message.templateFn;
//Static - ns.Message.$container;
ns.Message = function (opt) {
    var self = this;
    this.text = opt.text;
    this.date = opt.date;
    this.id = opt.id;
    this.isMine = !_.isUndefined(opt.isMine) ? opt.isMine : true;
    this.isRead = opt.isRead || false;

    var $container = ns.Message.$container || $('body');
    var templateFn = ns.Message.templateFn || _.noop;

    if (this.id) {
        this.$el = $("#message_" + this.id, $container);
    }

    this.render = function () {
        if (!ns.Message.templateFn) {
            return "";
        }
        return templateFn(self);
    };

    this.append = function (newEl) {
        $container.append(newEl);
        self.$el = $('.newMessage', $container).last();
        self.$el.fadeIn(function () {
            self.$el.removeClass('newMessage');
        });
    };

    this.markAsRead = function () {
        self.$el.removeClass('notRead');
        self.$el.addClass('read');
    };

    this.setDate = function (date) {
        var messageDateEl = self.$el.find('.messageDate');
        messageDateEl.fadeOut(function() {
            messageDateEl.html(date);
            messageDateEl.fadeIn();
        });
    };
};

ns.PrivateChat = function() {
    var $el,
        $textArea,
        messageContainer,
        chatStorage = ns.chatStorage;

    var messages,
        Message = ns.Message;

    var hub,
        hasConnect = true;

    var authorId,
        recipientId;

    function addMessage(text, isMine, date) {
        var model = new Message({
            isMine: isMine,
            isRead: false,
            text: text,
            date: date
        });
        model.append(model.render());
        messages.push(model);
        messageContainer.scrollDown();
        return model;
    }

    function markMessagesAsRead(isMine) {
        _.each(messages, function(msg) {
            if (msg.isRead == false && msg.isMine == isMine) {
                msg.markAsRead();
            }
        });
    }

    function markHisMessagesAsRead() {
        markMessagesAsRead(false);
    }

    function readMessages() {
        hub.server.readMessages(authorId, recipientId);
        setTimeout(markHisMessagesAsRead, 1000);
    }

    function sendStorageMessages(notSendedMessages) {
        if (!hasConnect) {
            return;
        }
        var storaged = notSendedMessages || chatStorage.get();
        _.each(storaged, function (item) {
            var model = addMessage(item.text, true);
            model.clientMessageId = item.clientMessageId;
            hub.server.sendMessage(item.text, authorId, recipientId, item.clientMessageId);
        });
    }

    function bind() {
        $textArea.on('keydown', function(e) {
            if (e.keyCode === 13) {
                e.preventDefault();
                var messageText = $textArea.val();

                if (!hub || !$.trim(messageText)) {
                    return;
                }

                var clientMessageId = chatStorage.getClientId();
                if (hasConnect) {
                    hub.server.saveMessage(messageText, authorId, recipientId, clientMessageId);
                } else {
                    chatStorage.save(messageText, clientMessageId);
                    // if connections saved on server not in cache (redis etc.) you may resend messeges without reloading page
                    //chatStorage.startChecking(sendStorageMessages); 
                }

                var model = addMessage(messageText, true);
                model.clientMessageId = clientMessageId;

                $textArea.val("");
            }
        });
    }

    function startConnection(callback) {
        hub = $.connection.chatHub;

        hub.client.receiveMessage = function(text, isMine, date, clientId) {
            console.log("receiveMessage!", clientId);
            addMessage(text, isMine, date);
            readMessages();
        };

        hub.client.recipientReadMessages = function() {
            markMessagesAsRead(true);
        };

        hub.client.messageSaved = function (clientMessageId, displayDate) {
            chatStorage.remove(clientMessageId);
            var msg = _.find(messages, function (item) {
                return item.clientMessageId == clientMessageId;
            });
            if (!msg) {
                return console.error("message not found");
            }
            msg.setDate(displayDate);
        };

        $.connection.hub.stateChanged(function (states) {
            hasConnect = states.newState === $.signalR.connectionState.connected;
        });

        $.connection.hub.start().done(callback);
    }

    function messagesFetch(options) {
        messages = [];
        Message.templateFn = _.template(options.template) || $('#messageTemplate').html();
        Message.$container = messageContainer.$el;
        _.each(options.messages, function (item) {
            messages.push(new Message(item));
        });
    }

    function setValues(opt) {
        var options = opt || {};
        options.elements = options.elements || {};

        authorId = options.authorId;
        recipientId = options.recipientId;

        $textArea = options.elements.textArea || $(".newMessageTextArea", $el);

        var $messageListContainerEl = options.elements.messagesContainer || $(".messageListContainer", $el);

        messageContainer = ns.scrollableContainer($messageListContainerEl, {
            url: ns.urls.getMessagesWith + "/" + recipientId,
            onPullDown: function (responce) {
                if (!_.isArray(responce)) {
                    return;
                }
                var virtualEl = "";
                _.each(responce, function(item) {
                    var message = new Message(item);
                    virtualEl += message.render();
                    messages.push(message);
                });
                messageContainer.$el.prepend(virtualEl);

                var newEls = $('.newMessage', messageContainer.$el);
                newEls.fadeIn(function () {
                    newEls.removeClass('newMessage');
                });
            }
        });

        messagesFetch(options);

        chatStorage.setChatData(authorId, recipientId);
    }

    var view = {
        init: function($element, opt) {
            $el = $element;

            setValues(opt);

            bind();

            startConnection(function() {
                hub.server.register(authorId, recipientId);

                readMessages();

                sendStorageMessages();
            });

            messageContainer.scrollDown();
            $textArea.focus();
        },

        fetchMessages: function() {
            $.ajax({
                url: ns.urls.getMessagesWith + "/" + recipientId,
                success: function(responce) {
                    console.info(responce);
                }
            });
        }
    };

    return view;
};