// Filename: app.js

define([
    'jquery',
    'underscore',
    'backbone',
    'shortcuts',
    'router', // Requests router.js
    'brickpile', // Requests brickpile.js
    'views/app'
],
function ($, _, Backbone, Shortcuts, Router, Brickpile, AppView) {
    var initialize = function() {
        // Pass in our router module and call it's initialize function
        Router.initialize();
        
        // Shorthand the application namespace
        var app = Brickpile.app;

        // Trigger the initial route and enable HTML5 History API support
        Backbone.history.start({ pushState: true });
        $(document).on('click', 'a:not([data-bypass])', function (evt) {

            var href = $(this).attr('href');
            var protocol = this.protocol + '//';

            if (href.slice(protocol.length) !== protocol) {
                evt.preventDefault();
                app.router.navigate(href, true);
            }
        });

        $(document).ajaxStart(function () {
            $('html').addClass('loading');
        });

        $(document).ajaxStop(function () {
            $('html').removeClass('loading');
        });
        
        // override keymaster filter settings
        // always return true means shortcuts
        // will work in any context
        // remove this for default behaviour
        key.filter = function (event) {
            return true;    
        };

        // Finally, we kick things off by creating the **App**.
        var App = new AppView;
    };

    return {
        initialize: initialize
    };

});