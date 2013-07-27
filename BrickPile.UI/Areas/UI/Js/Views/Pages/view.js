define([
        'jquery',
        'underscore',
        'backbone'
    ],
    function($, _, Backbone) {
        var PageListItemView = Backbone.View.extend({
            template: _.template($('#tpl-page-list-item').html()),
            initialize: function () {
            },
            render: function() {
                var html = this.template(this.model.toJSON());
                this.setElement($(html));
                return this;
            }
        });
        return PageListItemView;
    });