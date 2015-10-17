var ns = ns || {};

ns.scrollableContainer = function ($element, opt) {
    var $el = $element,
        loader;

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
        init: function() {
            loader = ns.loader;
        },
        scrollDown: function () {
            $el[0].scrollTop = $el[0].scrollHeight;
        },
        onPullDown: function (fn) {
            if (typeof (fn) == "function") {
                onPullDownCallback = fn;
            }
        }
    };

    return view;
};