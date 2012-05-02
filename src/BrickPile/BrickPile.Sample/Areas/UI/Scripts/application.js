/* Copyright (C) 2011 by Marcus Lindblom

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

(function ($) {

    var pageListView = new PageListView();

    var Router = Backbone.Router.extend({

        routes: {},

        // set up routing table
        initialize: function () {

            var router = this,

                routes = [
                    ['', 'ui', this.ui],
                    ['dashboard/', 'dashboard', this.dashboard],
                    [/^pages\/((?!(.*?)\w*new).*)$/, 'pages', this.pages]
                    //[/^pages\/((?!(.*?)\.*new).*)$/, 'pages', this.pages]
            //[/^((?!(.*?)submit).*)$/, 'pages', this.pages]                    
            //[/^pages\/(.*?)$/, 'pages', this.pages]

            //[/(.*?)\/edit/, 'edit', this.edit],
            //['pages/*url', 'pages', this.pages]
            //[/^(.*?)\/[^edit]/, 'edit', this.pages]
            //[/^(.*?)\/edit/, 'edit', this.edit],
                ];

            $(routes).each(function (index, route) {
                router.route.apply(router, route);
            });

        },
        ui: function () {

            var route = this;
            var ui = new UiView();

        },
        dashboard: function () {

            var route = this;
            var dashboard = new DashboardView();

        },
        pages: function (url) {

            var route = this;

            console.log('pages: ' + url);

            pageListView.url = url;
            pageListView.render();

            //var pageList = new PageListView({ url: url });

            // Fix for hashes in pushState and hash fragment
            //            if (url && !route._alreadyTriggered) {
            //                // Reset to home, pushState support automatically converts hashes
            //                Backbone.history.navigate("", false);

            //                // Trigger the default browser behavior
            //                location.hash = url;

            //                // Set an internal flag to stop recursive looping
            //                route._alreadyTriggered = true;
            //            }

            //            this.pageList = new PageCollection();
            //            this.pageListView = new PageListView({ model: this.pageList });
            //            this.pageList.fetch();
            //            $('#main').html(this.pageListView.render().el);

        }
        //        edit: function (url) {

        //            this.editView = new EditView({ url: url });

        //            // Fix for hashes in pushState and hash fragment
        //            if (url && !route._alreadyTriggered) {
        //                // Reset to home, pushState support automatically converts hashes
        //                Backbone.history.navigate("", false);

        //                // Trigger the default browser behavior
        //                location.hash = url;

        //                // Set an internal flag to stop recursive looping
        //                route._alreadyTriggered = true;
        //            }
        //        }
    });

    // Shorthand the application namespace
    var app = brickpile.app;

    // Define your master router on the application namespace and trigger all
    // navigation from this instance.
    app.router = new Router();

    // Trigger the initial route and enable HTML5 History API support
    Backbone.history.start({ pushState: true });

    // Ensure that we have a valid slug
    //$('.slug').slugify('input.title');

    // Handle the slug and url

    $('.slug').slugify('input.title');

//    var url = $("input.url").val();
//    if (url != null) {
//        var to = url.lastIndexOf('/');
//        url = url.substring(0, to + 1);

//        $('.slug').slugify('input.title', {
//            slugFunc: function (str, originalFunc) {
//                $("input.url").val(url + accentsTidy(str));
//                //$("input.slug").val(accentsTidy(str));
//                return accentsTidy(str);
//            }
//        });
//    }

    accentsTidy = function (s) {
        var r = s.toLowerCase();
        r = r.replace(new RegExp("\\s", 'g'), "-");
        r = r.replace(new RegExp("[àáâãäå]", 'g'), "a");
        r = r.replace(new RegExp("æ", 'g'), "ae");
        r = r.replace(new RegExp("ç", 'g'), "c");
        r = r.replace(new RegExp("[èéêë]", 'g'), "e");
        r = r.replace(new RegExp("[ìíîï]", 'g'), "i");
        r = r.replace(new RegExp("ñ", 'g'), "n");
        r = r.replace(new RegExp("[òóôõö]", 'g'), "o");
        r = r.replace(new RegExp("œ", 'g'), "oe");
        r = r.replace(new RegExp("[ùúûü]", 'g'), "u");
        r = r.replace(new RegExp("[ýÿ]", 'g'), "y");
        r = r.replace(new RegExp("\\W", 'g'), "-");
        r = r.replace(new RegExp("-+", 'g'), "-");
        return r;
    };

    // All navigation that is relative should be passed through the navigate
    // method, to be processed by the router.  If the link has a data-bypass
    // attribute, bypass the delegation completely.
    $(document).on("click", "a:not([data-bypass])", function (evt) {
        // Get the anchor href and protcol
        var href = $(this).attr("href");
        var protocol = this.protocol + "//";

        // Ensure the protocol is not part of URL, meaning its relative.
        if (href && href.slice(0, protocol.length) !== protocol) {
            // Stop the default event to ensure the link will not cause a page
            // refresh.
            evt.preventDefault();

            // This uses the default router defined above, and not any routers
            // that may be placed in modules.  To have this work globally (at the
            // cost of losing all route events) you can change the following line
            // to: Backbone.history.navigate(href, true);
            app.router.navigate(href, true);
        }
    });

})(jQuery);    
    
