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
    Id: null,
    ContentType: null,
    ContentLength: null,
    Name: null,
    DateUploaded: null,
    VirtualPath: null,
    Url: null,
    Thumbnail: null,
    Width: null,
    Height: null,
    idAttribute: "Id",
    url: function () {
        return '/api/asset/?id=' + this.id;
    },
});

var DroppedFile = Backbone.Model.extend({
    name: null,
    fileSize: null
});

var DroppedFileView = Backbone.View.extend({
    tagName: 'li',
    template: _.template($('#view-template-dropped-file').html()),
    progress: function(percentComplete) {
        this.$el.find('.ui-progress').animateProgress((percentComplete), function () { }, 2000);
    },
    initialize: function() {
        this.bind('brickpile:upload-progress', this.progress, this);
    },
    render: function() {
        this.$el.html(this.template(this.model.toJSON()));
        return this;
    },
});

// VirtualFile Collection
// ----------------------

var VirtualFileCollection = Backbone.Collection.extend({
    skippedResults: null,
    totalResults: null,
    query: null,
    registredProvider: null,
    url: function () {
        if(this.query) {
            return '/api/asset/?page=' + this.page + '&' + this.query;    
        } else {
            return '/api/asset/?page=' + this.page;
        }
    },
    model: VirtualFile,
    page: 0,
    parse: function (response) {
        this.skippedResults = response.SkippedResults;
        this.totalResults = response.TotalResults;
        this.registredProvider = response.RegistredProvider;
        return response.Assets;
    },
    comparator: function (a, b) {
        a = a.get('DateUploaded');
        b = b.get('DateUploaded');
        if (a > b)
            return -1;
        if (a < b)
            return 1;
        return 0;
    }
});

// VirtualFile property modal view
// -------------------------------

var VirtualFileSelectorModalView = Backbone.View.extend({
    maxRequestLength:null,
    tagName:  'div',
    currentSelectedModel: null,
    currentPath: null,
    currentTarget: null,
    prevTarget: null,
    isLoading: false,
    totalResults:null,
    skippedResults: null,
    selectedProvider: '',
    nrOfFiles: 0,
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
        'click a.select': 'selectAndClose',

        "change #provider-selector": "selectProvider"
    },
    all: function (ev) {
        var self = this;
        this.collection.page = 0;
        this.collection.query = 'virtualDirectory=' + this.selectedProvider;
        this.collection.fetch({
            success: function() {
                self.updateStatus();
            }
        });
    },
    recent: function (ev) {
        var self = this;
        this.collection.page = 0;
        this.collection.query = 'recent=1&virtualDirectory=' + this.selectedProvider;
        this.collection.fetch({
            success: function () {
                self.updateStatus();
            }
        });
    },
    images: function (ev) {
        var self = this;
        this.collection.page = 0;
        this.collection.query = 'type=image&virtualDirectory=' + this.selectedProvider;
        this.collection.fetch({
            success: function () {
                self.updateStatus();
            }
        });
    },
    videos: function (ev) {
        var self = this;
        this.collection.page = 0;
        this.collection.query = 'type=video&virtualDirectory=' + this.selectedProvider;
        this.collection.fetch({
            success: function () {
                self.updateStatus();
            }
        });
    },
    audios: function (ev) {
        var self = this;
        this.collection.page = 0;
        this.collection.query = 'type=audio&virtualDirectory=' + this.selectedProvider;
        this.collection.fetch({
            success: function () {
                self.updateStatus();
            }
        });
    },
    documents: function (ev) {
        var self = this;
        this.collection.page = 0;
        this.collection.query = 'type=document&virtualDirectory=' + this.selectedProvider;
        this.collection.fetch({
            success: function () {
                self.updateStatus();
            }
        });
    },
    addAsset: function (ev) {
        var self = this;
        if ($('#droparea').length > 0) return false;
        ev.preventDefault();
        ev.stopPropagation();
        var modal = new NewAssetDialogView({
            maxRequestLength: self.maxRequestLength,
            selectedProvider: self.selectedProvider,
            collection: self.collection
        });
        this.$el.find('#asset-dialog').append(modal.render().el);
    },
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
    initialize: function (options) {

        var self = this;

        this.template = _.template($('#view-template-virtual-file-dialog').html());

        // set max request length
        this.maxRequestLength = options.maxRequestLength;

        this.isLoading = false;
        this.collection = new VirtualFileCollection();
        
        // Shorthand for the application namespace
        var app = brickpile.app;

        // bind to the select event
        app.bind('select', this.select, this);
        app.bind('brickpile:asset-deleted', this.assetDeleted, this);
        app.bind('brickpile:asset-uploaded', this.assetUploaded, this);
       
        this.collection.on('sort', function (e) {
            self.renderResults();
        }, this);

    },
    render: function () {

        var self = this;
        
        this.$el.html(this.template());
        
        $(this.el).find('.nano').debounce("scrollend", function() {
            // check if we are at the last page
            
            if (self.isLoading || self.collection.page >= Math.round((parseInt(self.collection.totalResults) / 50))) {
                return;
            }
            self.collection.page += 1; // Load next page
            self.isLoading = true;
            self.collection.fetch({
                remove: false,
                success: function () {
                    self.isLoading = false;
                }
            });
            
        }, 500);
        
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
        
        self.isLoading = true;

        self.collection.fetch({
            success: function () {                
                var t = _.template($('#view-template-available-vpps').html());
                self.$el.find('#provider-selector').html(t({ 'items': self.collection.registredProvider }));
                self.$el.find('#provider-selector').selectBox();
                self.isLoading = false;
            }    
        });
        
        return this;
    },
    renderResults: function () {

        var self = this;

        $(self.el).find('#files ul').empty();

        $.each(this.collection.models, function (index, file) {
            var fileview = new VirtualFileView({
                model: file,
                inputUrl: self.$el.find('input:hidden.url'),
                inputVirtualUrl: self.$el.find('input:hidden.virtualUrl'),
                thumbnail: self.$el.find('.centerbox img'),
            });
            $(self.el).find('#files ul').append(fileview.render().$el);
        });
        
        self.updateStatus();        
    },
    appendResults: function (file) {
        var self = this;
        
        var fileview = new VirtualFileView({
            model: file,
            inputUrl: self.$el.find('input:hidden.url'),
            inputVirtualUrl: self.$el.find('input:hidden.virtualUrl'),
            thumbnail: self.$el.find('.centerbox img'),
        });
        
        $(self.el).find('#files ul').append(fileview.render().$el);
        
        self.updateStatus();

    },
    prependResults: function (file) {

        var self = this;

        var fileview = new VirtualFileView({
            model: file,
            inputUrl: self.$el.find('input:hidden.url'),
            inputVirtualUrl: self.$el.find('input:hidden.virtualUrl'),
            thumbnail: self.$el.find('.centerbox img'),
        });

        $(self.el).find('#files ul').prepend(fileview.render().$el);

        self.updateStatus();
    },

    assetUploaded: function (file) {        
        this.collection.fetch({ remove: false });
        this.collection.totalResults++;
        this.updateStatus();        
    },
    assetDeleted: function () {
        this.collection.totalResults--;
        this.updateStatus();
    },
    updateStatus: function () {
        
        var self = this;
        
        self.totalResults = self.collection.totalResults;
        self.skippedResults = self.collection.skippedResults;
        self.totalPages = Math.round((parseInt(self.collection.totalResults) / 50));
        self.nrOfFiles = $("#files li").size();

        $(this.el).find('.modal-footer span').html('Viewing ' + self.nrOfFiles + ' files of ' + self.totalResults);
        $(self.el).find('.nano').nanoScroller();
    },
    selectProvider: function (e) {
        this.selectedProvider = $(e.target).val();
        this.all(null);
    }
});

// VirtualFile property view
// -------------------------

var VirtualFilePropertyView = Backbone.View.extend({
    maxRequestLength: null,
    events: {
        'click button.browse': 'open',
        'click a.clear': 'clear'
    },
    open: function (e) {
        e.preventDefault();
        var view = new VirtualFileSelectorModalView(
            {
                maxRequestLength: this.maxRequestLength
            });
        this.$el.append(view.render().el);
        view.bind('brickpile:close-assets', this.close, this);
    },
    close: function (model) {
        this.$el.find('input:hidden.id').val(model.get('Id'));
        this.$el.find('input:hidden.contentType').val(model.get('ContentType'));
        this.$el.find('input:hidden.contentLength').val(model.get('ContentLength'));
        this.$el.find('input:hidden.name').val(model.get('Name'));
        this.$el.find('input:hidden.dateUploaded').val(model.get('DateUploaded'));
        this.$el.find('input:hidden.virtualPath').val(model.get('VirtualPath'));
        this.$el.find('input:hidden.url').val(model.get('Url'));
        this.$el.find('input:hidden.thumbnail').val(model.get('Thumbnail'));
        this.$el.find('input:hidden.width').val(model.get('Width'));
        this.$el.find('input:hidden.height').val(model.get('Height'));
        this.$el.find('.centerbox img').attr('src', 'data:image/png;base64,'+ (model.get('Thumbnail')));
    },
    clear: function () {
        this.$el.find(':input').val('');
        this.$el.find('.centerbox img').attr('src', 'http://placehold.it/60x38');
    },
    initialize: function (options) {
        this.maxRequestLength = options.maxRequestLength;
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
        this.model.destroy({
            success: function() {
            // Shorthand for the application namespace
            var app = brickpile.app;
            // Trigger asset delete event
            app.trigger('brickpile:asset-deleted');
            }
        });
        return false;
    },
    initialize: function () {
        var self = this;
        this.$el.hoverIntent(
            function (e) {
                $(e.currentTarget).find('button.delete').fadeIn('fast');
        },  function (e) {
                $(e.currentTarget).find('button.delete').fadeOut('fast');
        });

        this.model.bind("change", this.render, this);
        this.model.bind("remove", function() {
            self.$el.remove();
        } , this);
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

(function ($) {
    function debounce(callback, delay) {
        var self = this, timeout, _arguments;
        return function () {
            _arguments = Array.prototype.slice.call(arguments, 0),
            timeout = clearTimeout(timeout, _arguments),
            timeout = setTimeout(function () {
                callback.apply(self, _arguments);
                timeout = 0;
            }, delay);

            return this;
        };
    }

    $.extend($.fn, {
        debounce: function (event, callback, delay) {
            this.bind(event, debounce.apply(this, [callback, delay]));
        }
    });
})(jQuery);