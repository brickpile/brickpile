var Page = Backbone.Model.extend({
    defaults:{
        metadata: {
            name: null,
            title: null,
            keywords: null,
            description: null,
            displayInMenu: null,
            published: null,
            changed: null,
            changedBy: null,
            isPublished: null,
            isDeleted: null,
            slug: null,
            url: null,
            sortOrder: null
        }
    },    
    url: function () {
        if (this.id) {
            return '/api/page/' + this.id.substring(6);
        }
        return '/api/page/';
    },
    initialize: function () {
        //console.log('init page');
    }
});
