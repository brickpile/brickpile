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

var PageReferenceView = Backbone.View.extend({
    basePath:null,
    events: {
        'click button.clear': 'clear'
    },

    initialize: function (options) {
        this.basePath = options.basePath;
        this.pageInput = $(this.el).find('input[type=text]');
        this.pageHidden = $(this.el).find('input[type=hidden]');
        this.emptyMessage = $(this.el).find('.field-validation-error');
        var self = this;

        this.pageInput.keyup(function () {
            if (self.pageInput.val() == "") {
                self.clear();
            }
        });


        $(this.el).find('input[type=text]').autocomplete({
            autoFocus: true,
            source: self.basePath + 'pages/search',
            select: function (event, ui) {
                self.pageInput.val(ui.item ? ui.item.label : '');
                self.pageHidden.val(ui.item ? ui.item.id : '');
                return false;
            },
            search: function (event, ui) {
                self.pageInput.addClass("spinning");
            },
            response: function (event, ui) {
                self.pageInput.removeClass("spinning");
                self.emptyMessage.removeClass("empty");
                if (ui.content.length == 0) {
                    self.pageHidden.val("");
                    self.emptyMessage.addClass("empty");
                }
            }
        });
    },

    clear: function () {
        this.pageInput.val("");
        this.pageHidden.val("");
        this.emptyMessage.removeClass("empty");
    },

    render: function () {

    }
});