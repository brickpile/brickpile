var Page = Backbone.Model.extend({
    url: function () {
        if (this.id) {
            return '/api/page/' + this.id.substring(6);
        }
        return '/api/page/';
    },
    initialize: function () {
        console.log('init page');
    }
});
