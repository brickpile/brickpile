define([
        'jquery',
        'underscore',
        'backbone',
        'shortcuts',
        'views/base',
        'views/content/view'
    ],
    function ($, _, Backbone, Shortcuts, BaseView, ContentTypeListItemView) {
        var ContentTypeListView = BaseView.extend({
            
            tagName: 'ul',

            className: "content-types animated",
            
            currentItem: null,
            
            shortcuts: {
                'esc'   : 'close',
                'left'    : 'left',
                'right'  : 'right'
            },

            initialize: function () {
                _.extend(this, new Backbone.Shortcuts);
                this.delegateShortcuts();
            },

            render: function () {
                
                this.collection.each(function (page) {
                    var pageview = new ContentTypeListItemView({ model: page });
                    var $li = pageview.render().$el;
                    this.$el.append($li);
                }, this);

                this.currentItem = this.$el.find('li:first-child a');

                return this;
            },
            
            left: function () {

                var previous = $(this.currentItem).parent().prev().find('a');
                
                if (previous.length > 0) {
                    this.currentItem = previous;
                    $(previous).focus();
                }
            },
            
            right: function () {

                var next = $(this.currentItem).parent().next().find('a');

                if (next.length > 0) {
                    this.currentItem = next;
                    $(next).focus();
                }
            },

            close: function () {
                this.dispose();
            }
            
        });
        return ContentTypeListView;
    });
