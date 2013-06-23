// Filename: app.js

define([
    'jquery',
    'underscore',
    'backbone',
    'router', // Requests router.js
    'brickpile' // Requests brickpile.js
],
function ($, _, Backbone, Router, Brickpile) {
    var initialize = function() {
        // Pass in our router module and call it's initialize function
        Router.initialize();
        
        // Shorthand the application namespace
        var app = brickpile.app;

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

    };

    return {
        initialize: initialize
    };

});


//var Router = Backbone.Router.extend({
//    routes: {
//        'ui/': 'index',
//        'ui/login/': 'login',
//        'ui/setup/': 'setup',
//        //'ui/assets/': 'assets',
//        'ui/pages/': 'pages',

//        'ui/pages/:id': 'viewPage',
//        'ui/pages/edit/:id': 'edit'
//    },

//    index: function () {
        
//        var route = this;
//        // load a session object from the server
//        app.session = new Session();
//        app.session.fetch({
//            success: function (model, response, options) {
//                var view = new LoginStatusView({ model: model });
//                $('nav').append(view.render().el);
//            },
//            error: function (model, response, options) {
//                if (response.status == 403) {
//                    route.navigate('ui/setup/', true);
//                } else {
//                    route.navigate('ui/login/', true);
//                }
//            }
//        });

//    },
//    setup: function () {
//        $('#bg-wrap').append(new RegisterView().render().el);
//    },
//    login: function () {
//        $('#bg-wrap').append(new LoginView({ model: new User() }).render().el);
//    },
//    pages: function () {

//        app.pages = new PageCollection([], { id: '' });
        
//        app.pages.fetch({
            
//            success: function () {

//                if (app.pages.currentPage) {
//                    app.pages.unshift(app.pages.currentPage);
//                }
                
//                var view = new PageListView({ collection: app.pages });
//                var $el = view.render().$el;
 
//                $('#gutter').html($el);
                
//                $('#content').empty();
                
//            },
//            error: function () {
//                //console.log('err');
//            }
//        });

//    },
//    viewPage: function (id) {

//        app.pages = new PageCollection([], { id: id });
//        app.pages.fetch({
//            success: function () {
//                var view = new PageListView({ collection: app.pages });
//                $('#gutter').html(view.render().$el);
//                $('#content').empty();
//            }
//        });
        
//    },

//    edit: function (id) {

//        var pageEditView = null;
        
//        var page = app.pages.get(id);
        
//        // If the model is not present locally, refresh from the server
//        if (!page) {

//            var parentId;
//            if (app.pages.currentPage && app.pages.currentPage.get('parent')) {
//                parentId = app.pages.currentPage.get('parent').id;
//            } else {
//                parentId = '';
//            }

//            app.pages = new PageCollection([], { id: parentId });

//            app.pages.fetch({
                
//                success: function () {
                    
//                    if (app.pages.currentPage) {
//                        app.pages.unshift(app.pages.currentPage);
//                        console.log(app.pages.currentPage.get('id'));
//                    }

//                    var pageListview = new PageListView({ collection: app.pages });
//                    $('#gutter').html(pageListview.render().$el);
                    
//                    page = app.pages.get(id);
                    
//                    console.log(page);
//                    pageEditView = new PageEditView({ model: page });
//                    $('#content').html(pageEditView.render().el);
//                    app.trigger('brickpile:editorLoaded');
//                }
//            });
//        } else {
//            pageEditView = new PageEditView({ model: page });
//            $('#content').html(pageEditView.render().el);
            
//            app.trigger('brickpile:editorLoaded');

//            //console.log(page.get('contentReference'));

//            //var content = new Content({ id: page.get('contentReference') });
//            //content.fetch({
//            //    success: function (model) {
//            //        var contentView = new ContentEditView({ model: model });
//            //        $('#content').append(contentView.render().el);
//            //    }
//            //});
//        }

//    }

//});

//// Shorthand the application namespace
//var app = brickpile.app;

//// Define your master router on the application namespace and trigger all
//// navigation from this instance.
//app.router = new Router();

//// Trigger the initial route and enable HTML5 History API support
//Backbone.history.start({ pushState: true });
//$(document).on('click', 'a:not([data-bypass])', function (evt) {

//    var href = $(this).attr('href');
//    var protocol = this.protocol + '//';

//    if (href.slice(protocol.length) !== protocol) {
//        evt.preventDefault();
//        app.router.navigate(href, true);
//    }
//});

//$(document).ajaxStart(function () {
//    $('html').addClass('loading');
//});

//$(document).ajaxStop(function () {
//    $('html').removeClass('loading');
//});