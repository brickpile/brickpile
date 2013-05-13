var ContentTypeListItemView = Backbone.View.extend({
    
    events: {
        'click a' : 'add'
    },
    
    tagName: 'li',
    
    template: _.template($('#tpl-content-type-list-item').html()),
    
    render: function () {
        
        var html = this.template(this.model.toJSON());
        $(this.el).html(html);
        return this;
        
    },
    add: function (ev) {
        
        ev.preventDefault();

        var app = brickpile.app;
        
        var $input = $('<input type="text" style="margin-top:15px;margin-left:21px;" placeholder="My awesome page..." />');

        $input.bind('keypress', function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                // create a new instance of a page
                var page = new Page();
                // set the correct metadata
                page.set({
                    metadata: {
                        name: $(this).val(),
                        url: 'my-name-is-eminem',
                        slug: 'my-name-is-eminem',
                        sortOrder: app.pages.length + 1,
                        isPublished: false,
                        pubblished: new Date(),
                        Changed: new Date()
                    }
                });
                // add parent if the start page exists
                if (app.pages.currentPage) {
                    page.set({
                        parent: {
                            id: app.pages.currentPage.id,
                            url: app.pages.currentPage.get('metadata').url,
                            slug: app.pages.currentPage.get('metadata').slug
                        }
                    });
                }
                // save the page back to the server
                page.save(null, {
                    success: function (model) {
                        app.pages.add(model);
                        $input.remove();
                    }
                });
            }
        });

        $input.appendTo($('#gutter ul')).wrap('<li />');

        //var app = brickpile.app;

        //// create a new instance of a page
        //var page = new Page();
        //// set the correct metadata
        //page.set({
        //    id: '',
        //    metadata: {
        //        name: 'Foo',
        //        url: 'my-name-is-eminem',
        //        slug: 'my-name-is-eminem',
        //        sortOrder: app.pages.length + 1,
        //        isPublished: false,
        //        pubblished: new Date(),
        //        Changed: new Date()
        //    }
        //});

        //app.pages.add(page);

        $('.content-types').remove();

        return false;
    }
    
});