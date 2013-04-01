var PageCollection = Backbone.Collection.extend({
    url: '/api/page',
    model: Page,
    initialize: function () {
        console.log("People Collection is initialized");
    }
});