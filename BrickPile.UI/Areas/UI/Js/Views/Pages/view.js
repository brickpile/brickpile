var PageListItemView = Backbone.View.extend({
    events: {
        'click button': 'destroy',
        'mouseenter': 'hoverOn',
        'mouseleave': 'hoverOff'
    },
    tagName: 'li',
    template: _.template($('#view-template-page-list-item').html()),
    initialize: function () {
        _.bindAll(this, 'render');
        //this.render = _.bind(this.render, this);
        this.model.bind('change', this.render);
        this.model.bind('remove', this.render);
    },
    render: function () {
        var html = this.template(this.model.toJSON());
        $(this.el).html(html);
        return this;
    },
    destroy: function () {

        console.log('destroy');

        var app = brickpile.app;
        app.pages.remove(this.model.id);

        this.remove();
        this.unbind();
        this.model.destroy();

    },
    hoverOn: function (e) {
        this.$el.find('button').show();
        this.$el.find('a.view').show();
    },
    hoverOff: function (e) {
        this.$el.find('button').hide();
        this.$el.find('a.view').hide();
    }
});