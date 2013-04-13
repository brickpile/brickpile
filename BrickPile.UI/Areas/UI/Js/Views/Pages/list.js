var PageListView = Backbone.View.extend({
    tagName: 'ul',
    initialize: function () {
        var self = this;
        this.collection.bind("add", function () {
            self.render();
        });
    },
    addPage: function () {

        var self = this;

        var $input = $('<input type="text" style="margin-top:15px;margin-left:21px;" placeholder="My awesome page..." />');

        $input.bind('keypress', function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {

                var page = new Page();
                page.set({
                    metadata: {
                        name: $(this).val(),
                        url: 'my-name-is-eminem',
                        slug: 'my-name-is-eminem',
                        sortOrder: self.collection.length + 1,
                        isPublished: false,
                        pubblished: new Date(),
                        Changed: new Date()
                    }, parent: {
                        id: self.collection.currentPage.id,
                        url: self.collection.currentPage.get('metadata').url,
                        slug: self.collection.currentPage.get('metadata').slug
                    }
                });

                page.save(null, {
                    success: function (model, response) {
                        self.collection.add(model);
                        $input.remove();
                    }
                });

            }
        });

        $input.appendTo(this.$el).wrap('<li />');
    },
    render: function () {
        var self = this;
        this.$el.empty();

        var $add = $('<a id="add" data-icon="+">Add new page</a>');
        $add.bind('click', function (ev) {
            ev.preventDefault();
            self.addPage();
        });

        this.$el.append($add).wrapInner('<li />');

        this.collection.each(function (page) {
            var pageview = new PageListItemView({
                model: page
            });
            var $li = pageview.render().$el;
            this.$el.append($li);
        }, this);
        return this;
    }
});