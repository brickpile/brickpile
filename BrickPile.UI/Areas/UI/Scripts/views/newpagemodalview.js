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

var NewPageModalView = Backbone.View.extend({

    el: $('div#main'),

    events: {
        'click input[type=radio]': 'newPage'
    },

    newPage: function (event) {
        $(event.currentTarget).closest('form').submit();
    },

    // Close the dialog
    close: function () {
        
        $('#models').addClass('inactive').delay(400);
        
        $('#models').fadeOut('fast', function () {
            $(this).remove();
            $('html').unbind('click');
        });
        
    },

    // Bind events for clicking the html element and for triggering the esc key
    initialize: function () {

        this.template = _.template($('#view-template-new-page-dialog').html());

        var self = this;

        $('html').bind('click', this.close);
        $(document).keyup(function (e) {
            if (e.keyCode == 27) {
                self.close();
            }
        });
    },

    render: function () {
        $(this.el).append(this.template());
        return this;
    }

});