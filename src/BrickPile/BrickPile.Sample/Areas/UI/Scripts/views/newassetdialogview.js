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

    data: null,

    models: null,

    events: {
        'click #btn-upload': '_upload',
        'click #btn-cancel': 'close',
        'click a.remove': 'remove'
    },

    // Close the dialog
    close: function () {

        $(this.el).fadeOut('fast', function () {

            $(this).remove();
            $('html').unbind('click');

        });

    },

    _upload: function () {

        var self = this;

        $.each(self.data, function (i, item) {

            var formData = new window.FormData();

            formData.append("file" + i, item);

            $.ajax({

                xhr: function () {

                    var xhr = new window.XMLHttpRequest();

                    //Upload progress
                    $('.ui-progress').css('display', 'block');

                    xhr.upload.addEventListener("progress", function (e) {

                        if (e.lengthComputable) {

                            var percentComplete = e.loaded / e.total;

                            // Trigger the event on the current model
                            var file = self.models[i];

                            file.trigger('showProgress', percentComplete);

                            //Do something with upload progress
                            console.log(Math.round((percentComplete * 100)) + '%');

                        }
                    }, false);

                    return xhr;
                },

                type: "POST",
                url: "/api/asset",
                contentType: false,
                processData: false,
                data: formData,
                success: function (res) {

                    $.each(res, function (f, file) {

                        var fileview = new VirtualFileView({

                            model: new VirtualFile({

                                Id: file.Id,
                                Name: file.Name,
                                VirtualPath: file.VirtualPath,
                                ContentType: file.ContentType,
                                Thumbnails: {
                                    Small: file.Thumbnails.Small,
                                    Medium: file.Thumbnails.Medium
                                },
                                Url: file.Url

                            })

                        });

                        var $li = fileview.render().$el;
                        $('#files > ul').prepend($li);

                    });

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        });
    },

    // Bind events for clicking the html element and for triggering the esc key

    initialize: function () {

        var self = this;

        this.template = _.template($('#view-template-new-asset-dialog').html());

        this.$el.bind('dragenter dragover', false).bind('drop', function (evt) {

            evt.stopPropagation();

            evt.preventDefault();

            var files = evt.originalEvent.dataTransfer.files;

            if (files.length > 0) {

                if (window.FormData !== undefined) {

                    for (var i = 0; i < files.length; i++) {

                        self.data.push(files[i]);

                        var droppedFile = new DroppedFile({
                            name: files[i].name,
                            fileSize: bytesToSize(files[i].size)
                        });

                        var view = new DroppedFileView({ model: droppedFile });

                        self.models.push(droppedFile);

                        var $li = view.render().$el;
                        $('#droparea ul').append($li);

                        self._updateStatus();

                    }

                } else {
                    alert("your browser sucks!");
                }
            }

        });

        
        self.data = new Array();

        self.models = new Array();

        var app = brickpile.app;

        app.bind('remove', this.remove, this);

    },

    remove: function (model) {

        var index = this.models.indexOf(model);
        delete this.models.splice(index, 1);
        delete this.data.splice(index, 1);

        this._updateStatus();
    },

    _updateStatus: function () {

        var totalSize = 0;
        $.each(this.data, function (i, file) {
            totalSize += file.size;
        });

        $(this.el).find('#files-status').html(this.data.length + ' files <span>' + bytesToSize(totalSize) + ' </span>');

    },

    render: function () {

        this.$el.html(this.template());
        return this;

    }

});