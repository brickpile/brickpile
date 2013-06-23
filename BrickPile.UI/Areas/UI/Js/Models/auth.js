define([
        'jquery',
        'underscore',
        'backbone',
        'views/base',
    ], function($, _, Backbone) {
        var RegisterModel = Backbone.Model.extend({
            urlRoot: '/api/auth',
            defaults: {
                username: null,
                password: null,
                confirmPassword: null
            }
        });
        return RegisterModel;
    });