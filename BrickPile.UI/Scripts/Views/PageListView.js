var PageListView = Backbone.View.extend({
    tagName: 'ul',
    initialize: function () {
        this.collection.bind("reset", this.render, this);
    },
    render: function () {
        this.$el.empty();
        this.collection.each(function (page) {
            var pageview = new PageListItemView({ model: page });
            var $li = pageview.render().$el;
            this.$el.append($li);
        }, this);
        return this;
    }
});