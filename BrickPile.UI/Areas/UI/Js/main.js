// Filename: main.js

// Require.js allows us to configure shortcut alias
// There usage will become more apparent further along in the tutorial.

require.config({
    paths: {
        jquery: '/areas/ui/js/libs/jquery/jquery',
        underscore: '/areas/ui/js/libs/underscore/underscore',
        backbone: '/areas/ui/js/libs/backbone/backbone',
        form2js: '/areas/ui/js/libs/form2js/form2js',
        form2jsToObject: '/areas/ui/js/libs/form2js/jquery.toObject'
    },   
    shim: {
        underscore: {
            exports: '_'
        },
        backbone: {
            deps: ["underscore", "jquery"],
            exports: "Backbone"
        },
        form2js: {
            exports: 'forms2js'
        },
        form2jsToObject: {
            exports: 'form2jsToObject'
        }
    }
});

require([
        // Load our app module and pass it to our definition function
        'app'], function (App) {
        // The "app" dependency is passed in as "App"
        App.initialize();
});