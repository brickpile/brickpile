
Page = Backbone.Model.extend({

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
    },
    parent: {},
    children: [],
    ancestors: null,
    
    initialize: function () {

    }
    
});