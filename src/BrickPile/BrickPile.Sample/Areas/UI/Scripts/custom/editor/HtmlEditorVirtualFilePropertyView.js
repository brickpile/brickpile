var HtmlEditorVirtualFilePropertyView = Backbone.View.extend({
    events: {
    },
    open: function () {
        var coll = new VirtualFileCollection();
        var view = new VirtualFileSelectorModalView(
            {
                collection: coll
            });

        this.$el.append(view.render().el);

        view.bind('brickpile:close-assets', this.close, this);

        coll.fetch();
    },

    close: function (model) {
        this.options.callbackfield.val(model.get("VirtualPath"));        
    },

    initialize: function (options) {
        var app = brickpile.app;
        app.bind("brickpile:opendialog", this.open, this);
    },

    render: function () {

    }
});