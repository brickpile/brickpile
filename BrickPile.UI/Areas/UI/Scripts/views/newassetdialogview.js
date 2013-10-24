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

var NewAssetDialogView = Backbone.View.extend({

    list: [],
    totalSize: 0,
    maxRequestLength: null,
    totalProgress: 0,
    views: [],
    currentView: null,
    totalFiles: 0,
    totalSizeUploaded: 0,
    selectedProvider: '',
    currentView: null,
    close: function () {

        $(this.el).fadeOut('fast', function () {
            $(this).remove();
            $('html').unbind('click');
            $('body').unbind('click');
        });

    },

    uploadFile: function (file) {
        var self = this;

        // prepare FormData
        var formData = new FormData();
        formData.append(file.name, file);

        $.ajax({
            xhr: function () {
                var xhr = new window.XMLHttpRequest();

                $('.ui-progress').css('display', 'block');

                xhr.upload.addEventListener("progress", function (ev) {

                   
                    if (xhr.status === 409) {
                        xhr.abort();
                    }
                    
                    self.handleProgress(ev);
                    
                }, false);
                
                return xhr;
            },
            type: "POST",
            url: "/api/asset?virtualDirectoryPath=" + self.selectedProvider,
            contentType: false,
            processData: false,
            data: formData,
            success: function (res) {

                var item = res[0];

                var fileview = new VirtualFileView({
                    model: new VirtualFile({
                        Id: item.Id,
                        ContentType: item.ContentType,
                        ContentLength: item.ContentLength,
                        Name: item.Name,
                        DateUploaded: item.DateUploaded,
                        VirtualPath: item.VirtualPath,
                        Url: item.Url,
                        Thumbnail: item.Thumbnail,
                        Width: item.Width,
                        Height: item.Height
                    })
                });

                $('#files > ul').prepend(fileview.render().$el);

                // Shorthand for the application namespace
                var app = brickpile.app;
                // Trigger asset delete event
                app.trigger('brickpile:asset-uploaded');
                
                self.totalProgress += file.size;
                self.uploadNext();

            },
            error: function (xhr, ajaxOptions, thrownError) {

                $(self.currentView.el).append('<a href="#">Overwrite...</a>');
                
                //console.log(xhr);
                //alert(xhr.status);
                //alert(thrownError);
            }
        });
    },

    uploadNext: function () {
        if (this.list.length) {
            var nextFile = this.list.shift();
            this.uploadFile(nextFile);
        }
        if (this.views.length) {
            this.currentView = this.views.shift();
        }
    },

    handleProgress: function (ev) {
        if (ev.lengthComputable) {
            var progress = ev.loaded / ev.total;
            this.currentView.trigger('brickpile:upload-progress', progress);
        }
    },

    processFiles: function (files) {
        var self = this;
        if (!files || !files.length || this.list.length) return;

        this.totalSize = 0;
        this.totalProgress = 0;

        for (var i = 0; i < files.length; i++) {
            
             self.currentView = new DroppedFileView({
                model: new DroppedFile({
                    name: files[i].name,
                    size: files[i].size,
                    fileSize: bytesToSize(files[i].size)
                })
            });
            $('#droparea ul').append(self.currentView.render().$el);

            if (files[i].size < this.maxRequestLength) {

                this.list.push(files[i]);
                this.views.push(self.currentView);
                this.totalSize += files[i].size;
                this.totalFiles++;
                this.totalSizeUploaded += files[i].size;

                $(this.el).find('#files-status').html(this.totalFiles + ' files <span>' + bytesToSize(this.totalSizeUploaded) + ' </span>');

            }
        }

        this.uploadNext();

    },

    initialize: function (options) {
        var self = this;

        this.maxRequestLength = options.maxRequestLength;
        this.selectedProvider = options.selectedProvider;

        this.template = _.template($('#view-template-new-asset-dialog').html());

        // Attach event for handling drag'n drop
        this.$el.bind('dragenter dragover', false).bind('drop', function (ev) {
            ev.stopPropagation();
            ev.preventDefault();
            self.processFiles(ev.originalEvent.dataTransfer.files);
        });

    },

    render: function () {
        var self = this;

        this.$el.html(this.template());

        this.$el.find('.nano').nanoScroller();

        // Attach event for handling single/multiple files added using browse
        this.$el.find('.manual-file-chooser').change(function () {
            self.processFiles($(this)[0].files);
        });

        self.$el.click(function (e) {
            e.stopPropagation();
        });

        // Bind event closing the dialog on esc
        $(document).keyup(function (e) {
            if (e.keyCode == 27) {
                self.close();
            }
        });

        // Bind event closing the dialog on body click
        $('body').bind('click', function () {
            self.close();
        });

        self.$el.click(function (e) {
            e.stopPropagation();
        });

        // Bind event closing the dialog on esc
        $(document).keyup(function (e) {
            if (e.keyCode == 27) {
                self.close();
            }
        });

        // Bind event closing the dialog on body click
        $('body').bind('click', function () {
            self.close();
        });

        return this;

    }

});