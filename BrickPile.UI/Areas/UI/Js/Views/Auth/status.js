define([
        'jquery',
        'underscore',
        'backbone'
    ], function($, _, Backbone) {

        _.templateSettings = {
            evaluate: /\{\[([\s\S]+?)\]\}/g,
            interpolate: /\{\{([\s\S]+?)\}\}/g,
            escape: /\{\{-([\s\S]+?)\}\}/g
        };

        var LoginStatusView = Backbone.View.extend({
            events: {
                'click button': 'logoff'
            },
            initialize: function() {

            },
            template: _.template($('#view-template-login-status-view').html()),
            render: function() {
                $(this.el).html(this.template(this.model.toJSON()));
                return this;
            },
            logoff: function() {

                var app = brickpile.app;

                $('#login-screen').removeClass('hidden').removeAttr('style');
                $('#login-screen').addClass('animated fadeInDown');
                app.router.navigate('ui/login', true);

                var url = '/api/auth/logoff';
                $.post(url);
            }
        });
        return LoginStatusView;
    });