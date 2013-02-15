﻿var HtmlEditorVirtualFilePropertyView = Backbone.View.extend({
    maxRequestLength: null,
    open: function () {
        var coll = new VirtualFileCollection();
        var view = new VirtualFileSelectorModalView(
            {
                collection: coll,
                maxRequestLength: this.maxRequestLength
            });

        this.$el.append(view.render().el);

        view.bind('brickpile:close-assets', this.close, this);

        coll.fetch();
    },

    close: function (model) {
        this.options.callbackfield.val(model.get("VirtualPath"));
    },

    initialize: function (options) {
        this.maxRequestLength = options.maxRequestLength;
        var app = brickpile.app;
        app.bind("brickpile:opendialog", this.open, this);
    },

    render: function () {

    }
});