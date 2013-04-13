var Pages = Backbone.Collection.extend({
    url: function () {
        return '/api/page/' + this.id;    
    },            
    model: Page,
    parse: function (response) {
        this.currentPage = new Page(response.currentPage);
        return response.children;
    },
    initialize: function (models, options) {
        this.id = options.id;
    },
});