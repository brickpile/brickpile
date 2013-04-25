var Pages = Backbone.Collection.extend({
    url: function () {
        return '/api/page/' + this.id;    
    },            
    model: Page,
    parse: function (response) {
        this.currentPage = new Page(response.currentPage);
        return response.children;
    },
    toJSON: function() {
        var obj={};
        obj.currentPage = this.currentPage.toJSON();
        obj.children = this.map(function(model){ return model.toJSON(); });
        return obj;
   },    
    initialize: function (models, options) {
        this.id = options.id;
    },
});