/* Copyright (C) 2012 by Marcus Lindblo

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

// VirtualDirectory View
// ----------------------

var VirtualDirectoryView = Backbone.View.extend({

    template: _.template($('#view-template-virtual-directory').html()),

    events: {
        'click a': 'select'
    },

    select: function (event) {
        
        event.preventDefault();

        // Shorthand for the application namespace
        var app = brickpile.app;
        // Trigger the selected event
        app.trigger('folderSelect', this.model);
        
    },

    initialize: function () { },

    render: function () {
        var html = this.template(this.model.toJSON());
        this.setElement($(html));
        return this;
    }
});

// VirtualDirectory
// ----------------------

var VirtualDirectory = Backbone.Model.extend({

    name: null,

    VirtualPath: null
    
});

// VirtualFile Collection
// ----------------------

var VirtualFileCollection = Backbone.Collection.extend({

    url: '/assets/',

    model: VirtualFile,

    parse: function (response) {
        this.Directories = response.Directories;
        this.Parent = response.Parent;
        return response.Files;
    }

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

    selectAndClose: function (e) {
        e.preventDefault();

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

    folderSelect: function (model) {
        this.collection.url = '/assets?path=' + model.get('VirtualPath');
        this.collection.fetch();
    },

    initialize: function () {

        this.collection.bind("reset", this.render, this);
        this.template = _.template($('#view-template-virtual-file-dialog').html());

        // Shorthand for the application namespace
        var app = brickpile.app;
        // bind to the select event
        app.bind('select', this.select, this);
        app.bind('folderSelect', this.folderSelect, this);
    },

    render: function () {
        var self = this;
        // Render dialog, change this name to a more proper dialog name
        if ($('.modal').length < 1) {
            this.$el.append(this.template());
        }

        var $directories = $(this.el).find('#directories ul');
        $directories.empty();

        // add the parent item first
        if (this.collection.Parent != null) {
            var parent = new VirtualDirectoryView({
                model: new VirtualDirectory({ Name: '..', VirtualPath: this.collection.Parent.VirtualPath })
            });
            var $parentLi = parent.render().$el;
            $parentLi.find('i').attr('class', 'icon-folder-open').css('margin-left', '-10px');
            $directories.append($parentLi);
        }



        jQuery.each(this.collection.Directories, function (i, directory) {
            var dir = new VirtualDirectoryView({
                model: new VirtualDirectory({ Name: directory.Name, VirtualPath: directory.VirtualPath })
            });
            var $li = dir.render().$el;
            $directories.append($li);
        });

        // Find the dialog body and append the thumbnails
        var $ul = $(this.el).find('#files ul');
        $ul.empty();
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
        if ($('.modal-backdrop').length < 1) {
            $('body').append('<div class="modal-backdrop"></div>');
        }
        // Bind event closing the dialog on esc
        $(document).keyup(function (e) {
            if (e.keyCode == 27) {
                self.cancelAndClose();
            }
        });
        return this;
    }

});

// VirtualFile property view
// -------------------------

var VirtualFilePropertyView = Backbone.View.extend({

    events: {
        'click button.browse': 'openDialog',
        'click a.clear': 'clear'
    },

    openDialog: function (e) {
        e.preventDefault();
        var coll = new VirtualFileCollection();
        var view = new VirtualFileSelectorModalView({ el: this.el, collection: coll });
        coll.fetch();
    },

    clear: function () {

        this.$el.find(':input').val('');
        this.$el.find('.centerbox img').attr('src', 'http://placehold.it/60x38');

    },

    teardown: function () {
        this.model.off(null, null, this);
    },

    initialize: function () { },

    render: function () {

        $('.dropdown-toggle').dropdown();

    }

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