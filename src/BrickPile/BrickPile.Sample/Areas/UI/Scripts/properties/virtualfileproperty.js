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
    
    Url: null,
    
    initialize: function () { }

});

// VirtualDirectory View
// ----------------------

//var VirtualDirectoryView = Backbone.View.extend({

//    template: _.template($('#view-template-virtual-directory').html()),

//    events: {
//        'click a': 'select',
//    },

//    initialize: function () {
//        
//    },

//    render: function () {
//        var html = this.template(this.model.toJSON());
//        this.setElement($(html));
//        return this;
//    }
//});

// VirtualDirectory
// ----------------------

//var VirtualDirectory = Backbone.Model.extend({
//    
//    name: null,

//    VirtualPath: null,
//    
//    Parent: null
//    
//});

// VirtualFile Collection
// ----------------------

var VirtualFileCollection = Backbone.Collection.extend({

    url: '/api/asset/',

    model: VirtualFile,

//    parse: function (response) {
//        this.Directories = response.Directories;
//        this.Parent = response.Parent;
//        return response.Files;
//    }
    
    initialize: function() {
        console.log('Fetching');
    }

});

// VirtualFile property modal view
// -------------------------------

var VirtualFileSelectorModalView = Backbone.View.extend({

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
        'click .modal-header button' : 'addAsset',
        
        'click a.close': 'cancelAndClose',
        'click a.cancel': 'cancelAndClose',
        'click a.select': 'selectAndClose'
        //'click a.newDirectory': 'newDirectory',
        //'click a.deleteDirectory': 'deleteDirectory',
    },
    
    all: function(ev) {
        this.collection.url = '/api/asset';
        this.collection.fetch();
    },

    recent: function(ev) {
        this.collection.url = '/api/asset?recent=1';
        this.collection.fetch();
    },
    
    images: function(ev) {
        this.collection.url = '/api/asset?type=image';
        this.collection.fetch();
    },

    videos: function(ev) {
        this.collection.url = '/api/asset?type=video';
        this.collection.fetch();
    },

    audios: function(ev) {
        this.collection.url = '/api/asset?type=audio';
        this.collection.fetch();
    },
    
    documents: function(ev) {
        this.collection.url = '/api/asset?type=document';
        this.collection.fetch();
    },
    
    addAsset: function(ev) {

        if ($('#droparea').length > 0) {
            return false;
        }

        event.preventDefault();
        event.stopPropagation();

        var modal = new NewAssetDialogView({ el: ('.modal') });
        modal.render();
        
    },

    // Cancel and close the dialog
    cancelAndClose: function () {

        var self = this;
        self.destroy();

        $(this.el).find('.modal').fadeOut('fast', function () {
            $(self.el).unbind('dragenter dragover drop');
            $(this).remove();

            $('.modal-backdrop').remove();
        });
        return false;
    },
    
    // removes this view to free up memory 
    destroy: function(){
        console.log('Destroy');
        this.unbind();
        //this.remove(); 
    }, 

    selectAndClose: function (e) {
        
        e.preventDefault();
        
        this.$el.find('input:hidden.url').val(currentSelectedModel.get('Url'));
        this.$el.find('input:hidden.virtualPath').val(currentSelectedModel.get('VirtualPath'));
        this.$el.find('.centerbox img').attr('src', (currentSelectedModel.get('Thumbnails').Small.Url));


        $(this.el).find('.modal').fadeOut('fast', function () {
            $(this).remove();
            $('.modal-backdrop').remove();
        });

        return false;
    },

    select: function (model) {
        currentSelectedModel = model;
    },

//    openFolder: function (event, model) {
//        
//        event.stopImmediatePropagation();

//        this.prevTarget = this.currentTarget;
//        this.currentTarget = event.currentTarget;

//        $(event.currentTarget).children('i').attr('class', 'icon-folder-open');

//        this.currentPath = model.get('VirtualPath');
//        this.collection.url = '/assets?path=' + model.get('VirtualPath');
//        this.collection.fetch();
//        console.log('Open');
//    },
//    
//    closeFolder: function(event,model) {
//        
//        event.stopImmediatePropagation();

////        this.currentPath = this.collection.Parent.VirtualPath;
////        this.collection.url = '/assets?path=' + this.collection.Parent.VirtualPath;
////        this.collection.fetch();
////        
//        console.log('Closing: ' + this.collection.Parent.VirtualPath);
//    },

//    newDirectory: function () {
//        var view = new NewVirtualDirectoryView(
//            {
//                el: this.currentTarget != null ? $(this.currentTarget).siblings('ul') : $(this.el).find('#directories>ul'),
//                virtualPath: this.currentPath != null ? this.currentPath : null
//            });
//        view.render();
//    },

//    deleteDirectory: function (e) {
//        
//        if(!this.currentTarget) return;

//        var self = this;

//        $.ajax({
//            url: '/assets/deletedirectory',
//            data: { virtualPath: self.currentPath != null ? self.currentPath : '' },
//            success: function () {
//                $(self.currentTarget).closest('li').remove();
//                self.currentTarget = null;
//                self.currentPath = null;
//            }
//        });
//    },
//    
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

        //this.infiniScroll = new Backbone.InfiniScroll(this.collection, {success: this.appendRender, pageSize: 5, scrollOffset: 500 });

    },

    render: function () {

        var self = this;

        // Render dialog, change this name to a more proper dialog name
        if ($('.modal').length < 1) {
            this.$el.append(this.template());
        }
        
        $('.modal').find('.modal-body').scroll(function() {
            console.log('Scrolling');
        });


//        var $directories = $('<ul></ul>');

//        jQuery.each(this.collection.Directories, function(i, directory) {
//            var dir = new VirtualDirectoryView({
//                model: new VirtualDirectory({ Name: directory.Name, VirtualPath: directory.VirtualPath })
//            });
//            var $li = dir.render().$el;
//            $directories.append($li);
//        });

//        if (this.collection.Parent == null) {
//            
//            $(this.el).find('#directories').prepend($directories);
//            
//        }
//        else if ($(this.currentTarget).parents('ul').length == 1) {
//            jQuery.each($(this.currentTarget).parent('li').siblings(), function(i, listItem) {
//                $(listItem).find('i').attr('class', 'icon-folder-close');
//                $(listItem).find('ul').remove();
//            });
//            $(self.currentTarget).parent('li').children('ul').remove();
//            $(self.currentTarget).parent('li').append($directories);
//        }
//        else {
//            $(self.currentTarget).parent('li').children('ul').remove();
//            $(self.currentTarget).parent('li').append($directories);
//        }

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
            var $li = fileview.render().$el;
            $ul.append($li);
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
	
        return false;
    },
    
    checkScroll: function() {
        console.log('Scrolling...');
    }
});

// VirtualDirectory item view
// -------------------------

//var NewVirtualDirectoryView = Backbone.View.extend({

//    defaults: {
//        virtualPath: null
//    },

//    events: {
//        'focusout input[type=text]': 'undo',
//        'keypress input[type=text]': 'create'
//    },

//    undo: function(e) {
//        if (!$(e.target).val()) {
//            $(e.target).remove();   
//        }
//    },

//    create: function (e) {

//        e.stopPropagation();

//        if (e.keyCode != 13) return;
//        if (!$(e.target).val()) return;

//        var self = this;

//        $.ajax({
//            url: '/assets/createdirectory',
//            data: { virtualPath: this.defaults.virtualPath == null ? '' : this.defaults.virtualPath, directoryName: $(e.target).val() },
//            success: function (data) {
//                var dir = new VirtualDirectoryView({
//                    model: new VirtualDirectory({ Name: data.name, VirtualPath: data.virtualPath })
//                });
//                var $li = dir.render().$el;
//                $(self.el).find('li:last').replaceWith($li);
//            }
//        });

//        return false;

//    },

//    initialize: function (options) {
//        this.options = _.extend(this.defaults, this.options);
//        this.template = _.template($('#view-template-new-virtual-directory').html());
//    },

//    render: function () {
//        this.$el.append(this.template());
//    }
//});

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
        var view = new VirtualFileSelectorModalView(
            {
                el: this.el,
                collection: coll
            });
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
        'click a': 'select',
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