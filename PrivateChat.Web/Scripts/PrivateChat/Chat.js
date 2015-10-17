var ns = ns || {};

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

        messageContainer.init();

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