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

PageListView = Backbone.View.extend({

    el: $("body"),

    events: {
        "click #main .new": "newPage",
        "click tr input[type=checkbox]": "publishPage",
        "click tr a.delete": "deletePage",
        "click tr a.perma-delete": "permanentDelete",
        "click tr a.restore": "restorePage"
    },

    //tagName: 'table',

    //template: _.template($('#tpl-page-list').html()),

    url: null,

    initialize: function () {

        //      this.model.bind("reset", this.render, this);
        //      this.render();

    },
    render: function (e) {

        $.ajax({
            url: '/pages/' + this.url,
            success: function (data) {

                $('#main').html(data);

                $('#main').find('abbr.timeago').timeago();

                $("table.sortable tbody").sortable({
                    handle: 'td.sort',
                    items: "tr:not(.ui-state-disabled)",
                    helper: fixHelper,
                    opacity: 0.7,
                    update: function (event, ui) {
                        $.ajax({
                            type: 'POST',
                            url: '/pages/sort/',
                            data: { items: $(this).sortable('toArray') },
                            traditional: true,
                            success: function () { }
                        });

                        $(ui.item).find('span').effect("highlight", { color: '#ffffaa' }, 3000);
                    }
                });

                // Handle the slug and url
                $('.slug').slugify('input.title');

            }
        });

        //var html = this.template({ 'pageList': this.model.toJSON() });
        //$('#main').html(html);

        //        _.each(this.model.models, function (page) {
        //            $(this.el).append(new PageListItemView({ model: page }).render().el);
        //        }, this);

        //        return this;

    },
    newPage: function (ev) {

        $.ajax({
            url: $(ev.currentTarget).attr('href'),
            dataType: 'html',
            success: function (data) {

                $('div#main').append(data);

                var $dialog = $('#models');

                $('html').click(function () {
                    $dialog.fadeOut('fast', function () {
                        $dialog.remove();
                    });
                });

                $dialog.find('a.close').click(function () {
                    $dialog.fadeOut('fast', function () {
                        $dialog.remove();
                    });
                    return false;
                });

                $(':radio').live('click', function (e) {
                    $(this).closest('form').submit();
                });

            }
        });
        return false;

    },
    publishPage: function (e) {

        var $elm = $(e.currentTarget).parents('tr').find('abbr.timeago');
        $elm.text(jQuery.timeago(new Date()));
        $(e.currentTarget).parents('tr').find('span').effect("highlight", { color: '#ffffaa' }, 3000);

        // Publish or unpublish the page
        $.post('/pages/publish/', { id: $(e.currentTarget).attr('name'), published: e.currentTarget.checked });

    },
    deletePage: function (event) {

        var $row = $(event.currentTarget).closest('tr');
        $.ajax({
            url: '/pages/delete/',
            type: 'POST',
            dataType: 'html',
            data: { id: $row.attr('id'), permanent: false },
            success: function () {
                $row.fadeTo('fast', 0.6);
                $row.slideUp('fast');
            }
        });

        return false;

    },
    permanentDelete: function (event) {
        var $row = $(event.currentTarget).closest('tr');
        $.ajax({
            url: '/pages/delete/',
            type: 'POST',
            dataType: 'html',
            data: { id: $row.attr('id'), permanent: true },
            success: function () {
                $row.fadeTo('fast', 0);
                $row.slideUp('fast');
            }
        });

        return false;
    },
    restorePage: function (event) {
        var $row = $(event.currentTarget).closest('tr');
        $.ajax({
            url: '/pages/restore/',
            type: 'POST',
            dataType: 'html',
            data: { id: $row.attr('id') },
            success: function () {
                $row.fadeTo('fast', 0);
                $row.slideUp('fast');
            }
        });

        return false;
    }
});

// Return a helper with preserved width of cells
var fixHelper = function (e, ui) {
    ui.children().each(function () {
        $(this).width($(this).width());
    });
    return ui;
};