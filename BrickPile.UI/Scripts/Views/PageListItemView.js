var PageListItemView = Backbone.View.extend({
    tagName: 'li',
    template: _.template($('#view-template-page-list-item').html()),
    initialize: function () { },
    render: function () {
        var html = this.template(this.model.toJSON());
        $(this.el).html(html);
        return this;
    }
});