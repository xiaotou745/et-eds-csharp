//kendo.data.binders.items = kendo.data.Binder.extend({
//    refresh: function () {
//        var items = this.bindings["items"].get();
//        var htmlItems = $.map(items, function (item) {
//            return webExpress.utility.string.format("<span class='label label-default' id={0}>{1}</span>", item.Value, item.Text);
//        });
//        var html = htmlItems.join(" ");
//        $(this.element).html(html);
//    }
//});

//kendo.data.binders.item = kendo.data.Binder.extend({
//    refresh: function () {
//        var item = this.bindings["item"].get();
//        var html = "";
//        if (item) {
//            html = webExpress.utility.string.format("<span class='label label-default' id={0}>{1}</span>", item.Value, item.Text);
//        }
//        $(this.element).html(html);
//    }
//});

//kendo.data.binders.imgSrc = kendo.data.Binder.extend({
//    init: function (element, bindings, options) {
//        kendo.data.Binder.fn.init.call(this, element, bindings, options);
//        this._change = $.proxy(this.change, this);
//        $(element).on("load", this._change);
//    },
//    change: function () {
//        var url = $(this.element).attr("src");
//        this.bindings["imgSrc"].set(url);
//    },
//    refresh: function (key) {
//        var url = this.bindings["imgSrc"].get();
//        if (url) {
//            $(this.element).attr("src", url);
//        }
//        else {
//            $(this.element).removeAttr("src");
//        }
//    }
//});