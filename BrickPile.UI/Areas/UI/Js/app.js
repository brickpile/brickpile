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
        var app = brickpile.app;
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

        var route = this;

        $('#content').empty();

        var app = brickpile.app;
        app.pages = new Pages([], { id: '' });
        app.pages.fetch({
            success: function () {
                console.log('success');
                var view = new PageListView({ collection: app.pages });
                $('#pages').append(view.render().el);
            },
            error: function () {
                console.log('err');
                var view = new PageListView({ collection: app.pages });
                $('#pages').append(view.render().el);
            }
        });

    },
    viewPage: function (id) {

        console.log('Viewing page: ' + id);

        var route = this;
        var app = brickpile.app;

        app.pages = new Pages([], { id: 'pages/' + id });
        app.pages.fetch({
            success: function () {
                var view = new PageListView({ collection: app.pages });
                $('#pages').html(view.render().$el);
            }
        });
    },

    editPage: function (id) {

        console.log('Editing page: ' + id);

        var app = brickpile.app;

        var page = app.pages.get('pages/' + id);
        var view = new PageEditView({ model: page });
        $('#content').html(view.render().el);

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