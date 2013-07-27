// filename: views/app.js

define([
        'jquery',
        'underscore',
        'backbone',
        'shortcuts',
        'views/base'
    ],
    function($, _, Backbone, Shortcuts, BaseView) {
        // Our overall **AppView** is the top-level piece of UI.
        var AppView = BaseView.extend({
            el: $('body'),
            shortcuts: {
                'shift+/': 'help'
            },
            initialize: function() {
                _.extend(this, new Backbone.Shortcuts);
                this.delegateShortcuts();  
            },
            help: function() {
                alert('show help');
            }
        });

        return AppView;
    });