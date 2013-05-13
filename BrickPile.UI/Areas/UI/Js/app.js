var Router = Backbone.Router.extend({
    routes: {
        'ui/': 'index',
        'ui/login/': 'login',
        'ui/setup/': 'setup',
        //'ui/assets/': 'assets',
        'ui/pages/': 'pages',

        'ui/pages/:id': 'viewPage',
        'ui/pages/edit/:id': 'editPage'
    },

    index: function () {
        
        var route = this;
        // load a session object from the server
        app.session = new Session();
        app.session.fetch({
            success: function (model, response, options) {
                var view = new LoginStatusView({ model: model });
                $('nav').append(view.render().el);
            },
            error: function (model, response, options) {
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
        
        app.pages = new PageCollection([], { id: '' });
        
        app.pages.fetch({
            
            success: function () {

                var view = new PageListView({ collection: app.pages });
                var $el = view.render().$el;

                app.pages.push(app.pages.currentPage);

                //var homeView = new PageListItemView({ model: app.pages.currentPage });
                
                //$el.find('ul').prepend(homeView.render().el);
                
                $('#gutter').html($el);
                
                $('#content').empty();
                
            },
            error: function () {
                //console.log('err');
            }
        });

    },
    viewPage: function (id) {

        app.pages = new PageCollection([], { id: id });
        app.pages.fetch({
            success: function () {
                var view = new PageListView({ collection: app.pages });
                $('#gutter').html(view.render().$el);
                $('#content').empty();
            }
        });
        
    },

    editPage: function (id) {
        
        var pageEditView = null;
        
        var page = app.pages.get('pages/' + id);
        
        // If the model is not present locally, refresh from the server
        if (!page) {            
            app.pages = new PageCollection([], { id: app.pages.currentPage.get('parent').id.substring(6) });

            app.pages.fetch({
                success: function() {

                    var pageListview = new PageListView({ collection: app.pages });
                    $('#gutter').html(pageListview.render().$el);

                    page = app.pages.get('pages/' + id );
                    pageEditView = new PageEditView({ model: page });
                    $('#content').html(pageEditView.render().el);

                }
            });
        } else {
            pageEditView = new PageEditView({ model: page });
            $('#content').html(pageEditView.render().el);
        }

        //        var content = new Content({ id: page.get('contentReference') });
        //        content.fetch({
        //            success: function (content) {
        //                var contentView = new ContentEditView({ model: content });
        //                $('#content').append(contentView.render().el);
        //            }
        //        });

    }

});

// Shorthand the application namespace
var app = brickpile.app;

// Define your master router on the application namespace and trigger all
// navigation from this instance.
app.router = new Router();

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