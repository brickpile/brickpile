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
    
    defaults: {
        
        Thumbnails: {
            Small: null,
            Medium: null
        }
        
    },
    Etag: null,
    LocalPath: null,
    IsDirectory: null,
    Name: null,
    VirtualPath: null,
    Url: null
    
});

var DroppedFile = Backbone.Model.extend({
    
    name: null,
    
    fileSize: null
    
});

var DroppedFileView = Backbone.View.extend({
    
    tagName: 'li',

    template: _.template($('#view-template-dropped-file').html()),
    
    events: {
        'click a': 'remove'
    },
    
    _progress: function(percentComplete) {
        
        this.$el.find('.ui-progress').animateProgress((percentComplete * 100), function () { }, 2000);
        this.$el.find('.percentCompleted').text( Math.round(percentComplete * 100) + '%');
        
    },
    
    initialize: function() {

        this.$el.hoverIntent(
            function (ev) {
                $(ev.currentTarget).find('a.remove').fadeIn('fast');
            }, function (ev) {
                $(ev.currentTarget).find('a.remove').fadeOut('fast');
            });
        
        this.model.bind('showProgress', this._progress, this);
    },

    render: function() {
        this.$el.html(this.template(this.model.toJSON()));
        return this;
    },
    
    remove: function (e) {
        
        e.preventDefault();

        var app = brickpile.app;
        
        app.trigger('remove', this.model);
        
        this.$el.remove();
        
    }

});

// VirtualFile Collection
// ----------------------

var VirtualFileCollection = Backbone.Collection.extend({

    url: '/api/asset/',

    model: VirtualFile

});

// VirtualFile property modal view
// -------------------------------

var VirtualFileSelectorModalView = Backbone.View.extend({
    
    tagName:  'div',

    currentSelectedModel: null,

    currentPath: null,
    
    currentTarget: null,
    
    prevTarget: null,

    events: {
        'click a.all': 'all',
        'click a.recent': 'recent',
        'click a.images': 'images',
        'click a.videos' : 'videos',
        'click a.audios' : 'audios',
        'click a.documents' : 'documents',
        'click .modal-header button': 'addAsset',
        
        'click a.close': 'cancelAndClose',
        'click a.cancel': 'cancelAndClose',
        'click a.select': 'selectAndClose'
    },
    
    all: function() {
        this.collection.url = '/api/asset';
        this.collection.fetch();
    },

    recent: function() {
        this.collection.url = '/api/asset?recent=1';
        this.collection.fetch();
    },
    
    images: function() {
        this.collection.url = '/api/asset?type=image';
        this.collection.fetch();
    },

    videos: function() {
        this.collection.url = '/api/asset?type=video';
        this.collection.fetch();
    },

    audios: function() {
        this.collection.url = '/api/asset?type=audio';
        this.collection.fetch();
    },
    
    documents: function() {
        this.collection.url = '/api/asset?type=document';
        this.collection.fetch();
    },
    
    addAsset: function(ev) {

        if ($('#droparea').length > 0) {
            return false;
        }

        ev.preventDefault();
        ev.stopPropagation();

        var modal = new NewAssetDialogView();
        
        this.$el.find('#asset-dialog').append(modal.render().el);
        
    },

    // Cancel and close the dialog
    cancelAndClose: function () {

        var self = this;

        $(this.el).fadeOut('fast', function () {
            
            $(self.el).unbind('dragenter dragover drop');
            
            $(this).remove();
            
            $('.modal-backdrop').remove();
            
        });
        return false;
    },

    selectAndClose: function (e) {
        
        e.preventDefault();

        this.trigger('brickpile:close-assets', currentSelectedModel);

        $(this.el).fadeOut('fast', function () {
            
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

        // Display a loading indication whenever the Collection is fetching.
        this.collection.on("fetch", function() {
            this.$el.find('#files').html("<img src='/areas/ui/content/images/spinner.gif' style='position:absolute;left:60%;top:45%;' />");
        }, this);

        // Automatically re-render whenever the Collection is populated.
        this.collection.on("reset", this.render, this);
        
    },

    render: function () {

        var self = this;

        this.$el.html(this.template());

        // Find the dialog body and append the thumbnails
        var $ul = $('<ul></ul>');
        $ul.empty();
        
        this.collection.each(function (virtualFile) {
            
            var fileview = new VirtualFileView({
                model: virtualFile,
                inputUrl: this.$el.find('input:hidden.url'),
                inputVirtualUrl: this.$el.find('input:hidden.virtualUrl'),
                thumbnail: this.$el.find('.centerbox img'),
            });
            
            $ul.append(fileview.render().$el);
        }, this);
                
        $(this.el).find('#files').html($ul);

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
        
        'click button.browse': 'open',
        'click a.clear': 'clear'
        
    },

    open: function (e) {

        e.preventDefault();
        
        var coll = new VirtualFileCollection();
        
        var view = new VirtualFileSelectorModalView(
            {
                collection: coll
            });
        
        this.$el.append(view.render().el);

        view.bind('brickpile:close-assets', this.close, this);

        coll.fetch();
    },
    
    close: function (model) {
        this.$el.find('input:hidden.url').val(model.get('Url'));
        this.$el.find('input:hidden.virtualPath').val(model.get('VirtualPath'));
        this.$el.find('.centerbox img').attr('src', (model.get('Thumbnails').Small.Url));
    },

    clear: function () {
        
        this.$el.find(':input').val('');
        
        this.$el.find('.centerbox img').attr('src', 'http://placehold.it/60x38');
        
    },

    initialize: function () {
        
    },

    render: function () {
        $('.dropdown-toggle').dropdown();
    }
});

// VirtualFile item view
// -------------------------

var VirtualFileView = Backbone.View.extend({
    
    tagName: 'li',
    
    template: _.template($('#view-template-virtual-file').html()),

    events: {
        'click a.asset-item': 'select',
        'click button.delete': 'remove',
    },

    select: function (ev) {

        ev.stopPropagation();

        this.$el.closest('ul').find('li a').removeClass('selected');
            
        $(ev.currentTarget).addClass('selected');    

        // Shorthand for the application namespace
        var app = brickpile.app;
        // Trigger the selected event
        app.trigger('select', this.model);

    },
    
    remove: function () {
        
        var self = this;
        
        var data = this.model.get('Id');
        
        this.model.destroy({
            success:function () {
                    $.ajax({
                        type: "DELETE",
                        url: "/api/asset/?id=" + data,
                        success: function () {
                            self.$el.fadeOut('fast', function() {
                                $(this).remove();
                            });
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
            }
        });
        return false;
    },

    initialize: function () {
        this.$el.hoverIntent(
            function (e) {
                $(e.currentTarget).find('button.delete').fadeIn('fast');
        },  function (e) {
                $(e.currentTarget).find('button.delete').fadeOut('fast');
        });
    },

    render: function () {
        
        this.$el.html(this.template(this.model.toJSON()));
        return this;
        
    }

});

function bytesToSize(bytes) {
    var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    if (bytes == 0) return 'n/a';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    return Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[[i]];
};