/* Copyright (C) 2012 by Marcus Lindblom

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

// VirtualFile Model
// ----------

var VirtualFile = Backbone.Model.extend({
    Url: null,

    Etag: null,

    LocalPath: null,

    IsDirectory: null,

    Name: null,
    
    VirtualPath: null,

    initialize: function () { }
    
});

// VirtualFile Collection
// ----------------------

var VirtualFileCollection = Backbone.Collection.extend({
    
    url: '/assets',

    model: VirtualFile
    
});

// VirtualFile property modal view
// -------------------------------

var VirtualFileSelectorModalView = Backbone.View.extend({

    currentSelectedModel: null,

    events: {
        'click a.close': 'cancelAndClose',
        'click a.cancel': 'cancelAndClose',
        'click a.select': 'selectAndClose'
    },

    // Cancel and close the dialog
    cancelAndClose: function () {
        $(this.el).find('.modal').fadeOut('fast', function () {
            $(this).remove();
            $('.modal-backdrop').remove();
        });
        return false;
    },

    selectAndClose: function () {

        var self = this;
        this.$el.find('input:hidden.url').val(currentSelectedModel.get('Url'));
        this.$el.find('input:hidden.virtualPath').val(currentSelectedModel.get('VirtualPath'));

        $.ajax({
            url: '/assets/getthumbnailurl',
            data: { path: currentSelectedModel.get('VirtualPath') },
            success: function (data) {
                self.$el.find('.centerbox img').attr('src', data);
            }
        });

        $(this.el).find('.modal').fadeOut('fast', function () {
            $(this).remove();
            $('.modal-backdrop').remove();
        });
        return false;
    },

    select: function (model) {
        currentSelectedModel = model;
    },

    initialize: function () {
        this.collection.bind("reset", this.render, this);
        this.template = _.template($('#view-template-virtual-file-dialog').html());

        // Shorthand for the application namespace
        var app = brickpile.app;
        // bind to the select event
        app.bind('select', this.select, this);
    },

    render: function () {
        var self = this;
        // Render dialog
        this.$el.append(this.template());

        // Find the dialog body and append the thumbnails
        var $ul = $(this.el).find('.modal-body ul');
        this.collection.each(function (virtualFile) {
            var fileview = new VirtualFileView({
                model: virtualFile,
                inputUrl: this.$el.find('input:hidden.url'),
                inputVirtualUrl: this.$el.find('input:hidden.virtualUrl'),
                thumbnail: this.$el.find('.centerbox img')
            });
            var $li = fileview.render().$el;
            $ul.append($li);
        }, this);
        // Add the backdrop
        $('body').append('<div class="modal-backdrop"></div>');
        // Bind event closing the dialog on esc
        $(document).keyup(function (e) {
            if (e.keyCode == 27) {
                self.cancel();
            }
        });
        return this;
    }

});

// VirtualFile property view
// -------------------------

var VirtualFilePropertyView = Backbone.View.extend({

    events: {
        'click input[type=button]': 'openDialog'
    },

    openDialog: function () {
        console.log('open dialog');
        var coll = new VirtualFileCollection();
        var view = new VirtualFileSelectorModalView({ el: this.el, collection: coll });
        coll.fetch();
    },

    initialize: function () {},

    render: function () { }

});

// VirtualFile item view
// -------------------------

var VirtualFileView = Backbone.View.extend({
    
    template: _.template($('#view-template-virtual-file').html()),

    events: {
        'click a': 'select'
    },

    select: function () {
        
        this.$el.parents('ul').children('li').removeClass('selected');
        this.$el.addClass('selected');

        // Shorthand for the application namespace
        var app = brickpile.app;
        // Trigger the selected event
        app.trigger('select', this.model);

    },

    initialize: function () { },

    render: function () {
        var html = this.template(this.model.toJSON());
        this.setElement($(html));
        return this;
    }

});