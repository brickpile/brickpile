define([
        'jquery',
        'underscore',
        'backbone',
        'shortcuts',
        'router', // Requests router.js
        'views/base',
        'views/pages/view',
        'views/content/list'
    ],
    function ($, _, Backbone, Shortcuts, Router, BaseView, PageListItemView, ContentTypeListView) {

        var PageListView = BaseView.extend({
            
            template: _.template($('#tpl-page-current').html()),

            events: {
                'click #add': 'add'
            },
            shortcuts: {
                'ctrl+n': 'add',
                '⌘+n': 'add'
            },
            initialize: function () {
                _.extend(this, new Backbone.Shortcuts);
                this.delegateShortcuts();
                this.collection.bind("reset", this.render, this);
                this.collection.bind("add", this.render, this);
                this.collection.bind("change", this.render, this);
            },

            render: function() {

                this.$el.empty();

                var html = this.template(this.collection.toJSON());

                var $ul = $(html);

                this.collection.each(function (page) {
                    var pageview = new PageListItemView({ model: page });
                    var $li = pageview.render().$el;
                    $ul.closest('ul').append($li);
                }, this);

                this.$el.append($ul);

                return this;
            },
            add: function (e) {
                e.preventDefault();
                var view = new ContentTypeListView({ collection: this.collection.contentTypes });
                this.$el.before(view.render().$el);
                $('.content-types li:first-child > a').focus();
            }
        });

        return PageListView;
    });