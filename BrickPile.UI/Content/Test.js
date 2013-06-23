// Filename: /content/test.js

define([
        'jquery',
        'underscore',
        'backbone'
], function($, _, Backbone) {
    return Backbone.View.extend({
        events: {
            'click button' : 'fire'
        },
        fire: function (e) {
            if (e.keyCode == 13) {
                return false;
            }
            e.preventDefault();
            alert('fire');
        }
    });
});