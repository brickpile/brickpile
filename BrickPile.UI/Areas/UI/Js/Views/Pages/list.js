var PageListView = BaseView.extend({
    
    template: _.template($('#tpl-page-current').html()),
    
    events: {
        'click #add': 'add'
    },
    
    initialize: function () {
        console.log('init list');
        this.collection.bind("reset", this.render, this);
        this.collection.bind("add", this.render, this);
        this.collection.bind("change", this.render, this);
    },
    
    render: function () {

        this.$el.empty();

        var html = this.template(this.collection.toJSON());
       
        var $ul = $(html);

        this.collection.each(function (page) {
            var pageview = new PageListItemView({ model: page });
            var $li = pageview.render().$el;
            $ul.closest('ul').append($li);
        }, this);

        this.$el.append($ul);

        return this;
    },
    add: function () {
        var view = new ContentTypeListView({ collection: this.collection.contentTypes });
        this.$el.after(view.render().$el);
    }
});