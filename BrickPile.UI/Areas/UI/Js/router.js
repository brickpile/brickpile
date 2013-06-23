// Filename: router.js

define([
    'jquery',
    'underscore',
    'backbone',
    'brickpile',
    'models/session',
    'models/user',
    'views/auth/status',
    'views/auth/login',
    'views/auth/register',
    'views/pages/list',
    'views/pages/edit',
    'collections/pages'
], function ($, _, Backbone, Brickpile, Session, User, LoginStatusView, LoginView, RegisterView, PageListView, PageEditView, PageCollection) {
    
    var Router = Backbone.Router.extend({
        routes: {
            'ui/': 'index',
            'ui/login/': 'login',
            'ui/setup/': 'setup',
            'ui/page/': 'pages',
            'ui/page/:id': 'viewPage',
            'ui/page/edit/:id': 'edit'
        },
        
        index: function() {
            var route = this;
            // Shorthand the application namespace
            var app = brickpile.app;
            // load a session object from the server
            app.session = new Session();
            app.session.fetch({
                success: function(model, response, options) {
                    var view = new LoginStatusView({ model: model });
                    $('nav').append(view.render().el);
                },
                error: function(model, response, options) {
                    if (response.status == 403) {
                        route.navigate('ui/setup/', true);
                    } else {
                        route.navigate('ui/login/', true);
                    }
                }
            });
        },
        setup: function () {
            $('#bg-wrap').append(new RegisterView().render().el);
        },
        login: function () {
            $('#bg-wrap').append(new LoginView({ model: new User() }).render().el);
        },     
        pages: function () {

            // Shorthand the application namespace
            var app = brickpile.app;

            app.pages = new PageCollection([], { id: '' });

            app.pages.fetch({

                success: function () {

                    if (app.pages.currentPage) {
                        app.pages.unshift(app.pages.currentPage);
                    }

                    var view = new PageListView({ collection: app.pages });
                    var $el = view.render().$el;

                    $('#gutter').html($el);

                    $('#content').empty();

                },
                error: function () {
                    //console.log('err');
                }
            });

        },
        viewPage: function (id) {

            // Shorthand the application namespace
            var app = brickpile.app;

            app.pages = new PageCollection([], { id: id });
            app.pages.fetch({
                success: function () {
                    var view = new PageListView({ collection: app.pages });
                    $('#gutter').html(view.render().$el);
                    $('#content').empty();
                }
            });

        },        
        edit: function(id) {

            var pageEditView = null;

            // Shorthand the application namespace
            var app = brickpile.app;

            var page = app.pages.get(id);

            // If the model is not present locally, refresh from the server
            if (!page) {

                var parentId;
                if (app.pages.currentPage && app.pages.currentPage.get('parent')) {
                    parentId = app.pages.currentPage.get('parent').id;
                } else {
                    parentId = '';
                }

                app.pages = new PageCollection([], { id: parentId });

                app.pages.fetch({
                    success: function() {

                        if (app.pages.currentPage) {
                            app.pages.unshift(app.pages.currentPage);
                        }

                        var pageListview = new PageListView({ collection: app.pages });
                        $('#gutter').html(pageListview.render().$el);

                        page = app.pages.get(id);

                        pageEditView = new PageEditView({ model: page });
                        $('#content').html(pageEditView.render().el);
                        app.trigger('brickpile:editorLoaded');
                    }
                });
            } else {
                pageEditView = new PageEditView({ model: page });
                $('#content').html(pageEditView.render().el);

                app.trigger('brickpile:editorLoaded');

            }
        }
    });

    var initialize = function () {
        // Shorthand the application namespace
        var app = brickpile.app;

        // Define your master router on the application namespace and trigger all
        // navigation from this instance.
        app.router = new Router;
    };

    return {
        initialize: initialize
    };
    
});