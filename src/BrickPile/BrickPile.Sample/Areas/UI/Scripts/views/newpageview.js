/* Copyright (C) 2011 by Marcus Lindblom

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

NewPageView = Backbone.View.extend({

    events: { },

    initialize: function () {
        this.render();
    },

    
    evaluateSlug: function (str) {
        if ($('.slug').hasClass('slugify-locked')) {
            return false;
        }
        if (jQuery.inArray(str.toLowerCase(), this.options.slugsInUse) > -1) {
            $(this.el).find('#slug .field-validation-error').addClass('illigal');
            $(this.el).find(':submit').attr('disabled', 'disabled');
        } else {
            $(this.el).find('#slug .field-validation-error').removeClass('illigal');
            $(this.el).find(':submit').removeAttr('disabled', 'disabled');
        }
    },

    render: function () {
        var self = this;
        // Ensure that we have a valid slug
        $('.slug').slugify('input.title', {
            slugFunc: function (str, originalFunc) {
                self.evaluateSlug(str);
                return originalFunc(str);
            }
        });
    }

});