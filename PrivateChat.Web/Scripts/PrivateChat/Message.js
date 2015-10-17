var ns = ns || {};

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
        messageDateEl.fadeOut(function () {
            messageDateEl.html(date);
            messageDateEl.fadeIn();
        });
    };
};