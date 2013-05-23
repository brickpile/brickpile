var ContentTypeListView = Backbone.View.extend({
    
    tagName: 'ul',
    
    className: "content-types animated",

    initialize: function () {
        this.collection.bind("reset", this.render, this);
    },
    
    render: function () {
        this.collection.each(function (page) {
            var pageview = new ContentTypeListItemView({ model: page });
            var $li = pageview.render().$el;
            this.$el.append($li);
        }, this);
        
        return this;
    }
});
