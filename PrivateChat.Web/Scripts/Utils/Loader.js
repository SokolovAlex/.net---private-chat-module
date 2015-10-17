var ns = ns || {};

ns.loader = (function () {
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