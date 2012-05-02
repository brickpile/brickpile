
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

PageListItemView = Backbone.View.extend({

    tagName: "tr",

    template: _.template($('#tpl-page-list-item').html()),

    //template: _.template($('#pageTemplate').html()),

    //var html = customerTemplate({'customerList' : customerList});
    //$("#someDiv").html(html);


    events: {
        "click td input[type=checkbox]": "publish"
    },

    render: function (eventName) {

//        var html = this.template({ 'pageList': this.model.toJSON() });
//        $(this.el).html(html);

        $(this.el).html(this.template(this.model.toJSON()));
        //$(this.el).find('abbr.timeago').timeago();
        return this;
    },

    publish: function (event) {

        alert('Publish');
        
        //var $elm = $('<abbr class="timeago" title="' + ISODateString(new Date()) + '">' + ISODateString(new Date()) + ' </abbr>');
        //$(this.el).find('abbr.timeago').replaceWith($elm);
        //this.model.set({ '__type': 'Page:#BrickPile.Sample.Models', 'metadata': { 'changed': new Date(), 'isPublished': event.currentTarget.checked} });
        //this.model.save();
        //$elm.timeago();
    }

});

function ISODateString(d) {
    function pad(n) {
        return n < 10 ? '0' + n : n;
    }
    return d.getFullYear() + '-'
    + pad(d.getMonth() + 1) + '-'
    + pad(d.getDate()) + ' '
    + pad(d.getHours()) + ':'
    + pad(d.getMinutes()) + ':'
    + pad(d.getSeconds());
}