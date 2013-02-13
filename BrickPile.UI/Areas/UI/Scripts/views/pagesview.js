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

var PagesView = Backbone.View.extend({

    events: {
        'click td input[type=checkbox]': 'publishPage',
        'click a.new': 'addPage',
        'click tr a.delete': 'deletePage',
        'click tr a.perma-delete': 'permanentDelete',
        'click tr a.restore': 'restorePage'
    },

    isCtrl: false,

    // Publish or unpublish a page
    publishPage: function (event) {
        try {
            var $elm = $(event.currentTarget).parents('tr').find('abbr.timeago');
            var d = new Date();
            $elm.html(jQuery.timeago(ISODateString(d)));
            $(event.currentTarget).parents('tr').find('span').effect('highlight', { color: '#ffffaa' }, 3000);
            $.post('/pages/publish/', { id: $(event.currentTarget).attr('name'), published: event.currentTarget.checked });
        } catch (exception) {
            throw "An error occured when trying to publish a page";
        }
    },

    // Open the new page dialog
    addPage: function (event) {

        if ($('#models').length > 0) {
            return false;
        }

        event.preventDefault();
        event.stopPropagation();

        var modal = new NewPageModalView();
        modal.render();

    },

    // Deletes a page
    deletePage: function (event) {
        event.preventDefault();
        var $row = $(event.currentTarget).closest('tr');
        try {
            $.ajax({
                url: '/pages/delete/',
                type: 'POST',
                dataType: 'html',
                data: { id: $row.attr('id'), permanent: false },
                success: function () {
                    $row.fadeTo('fast', 0);
                    $row.slideUp('fast');
                }
            });
        } catch (exception) {
            throw "An error occured when trying to delete a page";
        }
    },

    // Remove the page permanently
    permanentDelete: function (event) {
        event.preventDefault();
        var $row = $(event.currentTarget).closest('tr');
        try {
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
        } catch (exception) {
            throw "An error occured when trying to delete a page";
        }
    },

    // Restore the page
    restorePage: function (event) {
        event.preventDefault();
        var $row = $(event.currentTarget).closest('tr');
        try {
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
        } catch (exception) {
            throw "An error occured when trying to delete a page";
        }
    },

    // Initialize this view
    initialize: function () {
        this.render();

        $('table.sortable tbody').sortable({
            handle: 'td.sort',
            items: 'tr:not(.ui-state-disabled)',
            helper: function (e, ui) {
                ui.children().each(function () {
                    $(this).width($(this).width());
                });
                return ui;
            },
            opacity: 0.7,
            update: function (event, ui) {
                try {
                    $.ajax({
                        type: 'POST',
                        url: '/pages/sort/',
                        data: { items: $(this).sortable('toArray') },
                        traditional: true,
                        success: function () {
                            $(ui.item).find('span').effect('highlight', { color: '#ffffaa' }, 3000);
                        }
                    });
                } catch (exception) {
                    throw "An error occured when trying to update the sort order";
                }
            }
        });
    },

    // Render view
    render: function () {
        $(this.$el).find('.timeago').timeago();

//        $.each($('table tbody tr'), function (i, el) {

//            setTimeout(function () {
//                $(el).addClass('show');
//            }, 70 + (i * 70));

//        });

    }
});

function ISODateString(d) {
    function pad(n) {
        return n < 10 ? '0' + n : n
    }
    return d.getUTCFullYear() + '-'
    + pad(d.getUTCMonth() + 1) + '-'
    + pad(d.getUTCDate()) + 'T'
    + pad(d.getUTCHours()) + ':'
    + pad(d.getUTCMinutes()) + ':'
    + pad(d.getUTCSeconds()) + 'Z'
}