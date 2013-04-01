var AssetCollection = Backbone.Collection.extend({
    url: '/api/asset',
    model: Asset,
    initialize: function () {}
});