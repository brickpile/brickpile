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
    Etag: null,
    LocalPath: null,
    IsDirectory: null,
    Name: null,
    VirtualPath: null,
    Url: null,
    Thumbnail:null    
});

var DroppedFile = Backbone.Model.extend({
    name: null,
    fileSize: null
});

var DroppedFileView = Backbone.View.extend({
    tagName: 'li',
    template: _.template($('#view-template-dropped-file').html()),
    progress: function(percentComplete) {
        this.$el.find('.ui-progress').animateProgress((percentComplete * 100), function () { }, 2000);
        //this.$el.find('.percentCompleted').text( Math.round(percentComplete * 100) + '%');
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
    skippedResults:null,
    totalResults: null,
    query: null,
    url: function () {
        if(this.query) {
            console.log(this.query);
            return '/api/asset/?page=' + this.page + '&' + this.query;    
        } else {
            console.log(this.query);
            return '/api/asset/?page=' + this.page;
        }
    },
    model: VirtualFile,
    page: 0,
    parse: function (response) {
        this.skippedResults = response.SkippedResults;
        this.totalResults = response.TotalResults;
        return response.Assets;
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
    totalPages:null,
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
        this.collection.page = 0;
        this.collection.query = null;
        this.render();
    },
    recent: function() {
        this.collection.page = 0;
        this.collection.query = 'recent=1';
        this.render();
    },
    images: function() {
        this.collection.page = 0;
        this.collection.query = 'type=image';
        this.render();
    },
    videos: function() {
        this.collection.page = 0;
        this.collection.query = 'type=video';
        this.render();
    },
    audios: function() {
        this.collection.page = 0;
        this.collection.query = 'type=audio';
        this.render();
    },
    documents: function() {
        this.collection.page = 0;
        this.collection.query = 'type=document';
        this.render();
    },
    addAsset: function(ev) {
        if ($('#droparea').length > 0) return false;
        ev.preventDefault();
        ev.stopPropagation();
        var modal = new NewAssetDialogView({
            maxRequestLength: this.maxRequestLength
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
        this.maxRequestLength = options.maxRequestLength;
        this.template = _.template($('#view-template-virtual-file-dialog').html());
        // Shorthand for the application namespace
        var app = brickpile.app;
        // bind to the select event
        app.bind('select', this.select, this);
        app.bind('brickpile:asset-deleted', this.assetDeleted, this);
        app.bind('brickpile:asset-uploaded', this.assetUploaded, this);

        // Display a loading indication whenever the Collection is fetching.
//        this.collection.on("fetch", function() {
//            this.$el.find('#files').append("<img src='/areas/ui/content/images/spinner.gif' class=\"spinner\" style='position:absolute;left:60%;top:45%;' />");
//        }, this);
        
    },
    render: function () {
        var self = this;
        this.$el.html(this.template());
        // isLoading is a useful flag to make sure we don't send off more than
        // one request at a time
        this.isLoading = false;
        this.loadResults();
        $(this.el).find('.nano').debounce("scrollend", function() {
            // check if we are at the last page
            //if((parseInt(self.collection.page)+1) >= self.totalPages) return;
            self.collection.page += 1; // Load next page
            self.loadResults();
            
        },500);
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
    },
    loadResults: function () {
        var self = this;
        // we are starting a new load of results so set isLoading to true
        this.isLoading = true;
        // fetch is Backbone.js native function for calling and parsing the collection url
        this.collection.fetch({
            success: function (assets) {
                // Once the results are returned lets populate our template
                $.each(assets.models, function(index, file) {
                    var fileview = new VirtualFileView({
                        model: file,
                        inputUrl: self.$el.find('input:hidden.url'),
                        inputVirtualUrl: self.$el.find('input:hidden.virtualUrl'),
                        thumbnail: self.$el.find('.centerbox img'),
                    });
                    $(self.el).find('#files ul').append(fileview.render().$el);
                });
                // Now we have finished loading set isLoading back to false
                self.isLoading = false;
                self.totalResults = self.collection.totalResults;
                self.skippedResults = self.collection.skippedResults;
                self.totalPages = Math.round((parseInt(self.collection.totalResults) / 50));
                self.updateStatus();
                // Update the scroll bar
                $(self.el).find('.nano').nanoScroller();
            }                
        });
        
    },
    assetUploaded: function () {
        this.totalResults++;
        this.totalPages = Math.round((parseInt(this.totalResults) / 50));
        this.updateStatus();
        // Update the scroll bar
        $(this.el).find('.nano').nanoScroller();
    },
    assetDeleted: function () {
        this.totalResults--;
        this.totalPages = Math.round((parseInt(this.totalResults) / 50));
        this.updateStatus();
        // Update the scroll bar
        $(this.el).find('.nano').nanoScroller();
    },
    updateStatus: function () {
        $(this.el).find('.modal-footer span').html('Viewing ' + $("#files li").size() + ' files of ' + this.totalResults);
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
        var coll = new VirtualFileCollection();
        var view = new VirtualFileSelectorModalView(
            {
                collection: coll,
                maxRequestLength: this.maxRequestLength
            });
        this.$el.append(view.render().el);
        view.bind('brickpile:close-assets', this.close, this);
        coll.fetch();
    },
    close: function (model) {
        this.$el.find('input:hidden.url').val(model.get('Url'));
        this.$el.find('input:hidden.virtualPath').val(model.get('VirtualPath'));
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
        var self = this;
        var data = this.model.get('Id');
        this.model.destroy({
            success:function () {
                    $.ajax({
                        type: "DELETE",
                        url: "/api/asset/?id=" + data,
                        success: function () {
                            // Shorthand for the application namespace
                            var app = brickpile.app;
                            // Trigger asset delete event
                            app.trigger('brickpile:asset-deleted');
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

 (function($) {
  function debounce(callback, delay) {
    var self = this, timeout, _arguments;
    return function() {
      _arguments = Array.prototype.slice.call(arguments, 0),
      timeout = clearTimeout(timeout, _arguments),
      timeout = setTimeout(function() {
        callback.apply(self, _arguments);
        timeout = 0;
      }, delay);

      return this;
    };
  }

  $.extend($.fn, {
    debounce: function(event, callback, delay) {
      this.bind(event, debounce.apply(this, [callback, delay]));
    }
  });
})(jQuery);