var ns = ns || {};

ns.chatStorage = (function () {
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
            if (typeof (fn) !== 'function') {
                return;
            }

            var handler = function () {
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