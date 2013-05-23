var PageCollection = Backbone.Collection.extend({
    
    url: function () {
        return '/api/page/' + this.id;    
    },
    
    model: Page,
    
    parse: function (response) {
        if (response.currentPage) {
            this.currentPage = new Page(response.currentPage);
        }
        this.contentTypes = new ContentTypeCollection(response.contentTypes);
        return response.children;
    },
    
    toJSON: function () {
        var obj = {};
        if (this.currentPage) {
            obj.currentPage = this.currentPage.toJSON();
        }
        obj.contentTypes = this.contentTypes.toJSON();
        obj.children = this.map(function(model){ return model.toJSON(); });
        return obj;
    },
    
    initialize: function (models, options) {
        this.id = options.id;
    },
});