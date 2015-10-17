var ns = window.ns || {};

ns.storage = (function () {
    var key = "privateChat";
    var localStorage = (function () {
        try {
            return 'localStorage' in window && window['localStorage'];
        } catch (e) {
            return false;
        }
    })();

    var view = {};

    view.get = function () {
        if (!localStorage) {
            return [];
        }
        var args = Array.prototype.slice.call(arguments);

        if (args.length == 0) {
            localStorage.getItem(key);
            var arrayString = localStorage.getItem(key);
            try {
                return JSON.parse(arrayString) || [];
            } catch (ex) {
                return [];
            }
        }
    };

    view.sync = function (array) {
        if (!localStorage) {
            return;
        }
        localStorage.setItem(key, JSON.stringify(array));
    };

    view.set = function (value) {
        if (!localStorage) {
            return [];
        }

        var arrayString = localStorage.getItem(key);
        var array;
        try {
            array = JSON.parse(arrayString) || [];
        } catch (ex) {
            return;
        }
        array.push(value);
        localStorage.setItem(key, JSON.stringify(array));
    };

    return view;
})();