define([
        'jquery',
        'underscore',
        'backbone'
], function ($, _, Backbone) {
    
        var Session = Backbone.Model.extend({
            url: '/api/auth/session'
        });

        return Session;
    });