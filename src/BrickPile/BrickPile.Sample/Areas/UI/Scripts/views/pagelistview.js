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
        "click tr a.delete": "deletePage"
    },

    //tagName: 'table',

    //template: _.template($('#tpl-page-list').html()),

    url: null,
    initialize: function () {

        //      this.model.bind("reset", this.render, this);
        //      this.render();

    },
    render: function (ev) {

        $.ajax({
            url: '/pages/' + this.url,
            beforeSend: function () {

                //$('#main').empty();
                //$('body').append('<div class="spinner s48 blue" style="height:48px;width:48px;position:absolute;left:50%;top:50%;" />');

            },
            success: function (data) {

                //$('.spinner').remove();
                $('#main').html(data);

                $('#main').find('abbr.timeago').timeago();

                $("table.sortable tbody").sortable({
                    handle: 'td.sort',
                    items: "tr:not(.ui-state-disabled)",
                    helper: fixHelper,
                    //helper: 'clone',
                    //placeholder: "ui-state-highlight",
                    opacity: 0.7,
                    //forcePlaceholderSize: true,
                    update: function (event, ui) {
                        console.log('Update sort order: ' + $(this).sortable('toArray'));
                        //$.post('/pages/sort/', { items: $(this).sortable('toArray') });
                        $.ajax({
                            type: 'POST',
                            url: '/pages/sort/',
                            data: { items: $(this).sortable('toArray') },
                            traditional: true,
                            success: function (data) { }
                        });

                        $(ui.item).find('span').effect("highlight", { color: '#ffffaa' }, 3000);
                    }
                });


                // Handle the slug and url

                $('.slug').slugify('input.title');

//                var url = $("input.url").val();
//                if (url != null) {
//                    var to = url.lastIndexOf('/');
//                    url = url.substring(0, to + 1);

//                    $('.slug').slugify('input.title', {
//                        slugFunc: function (str, originalFunc) {
//                            //$("input.url").val(url + accentsTidy(str));
//                            //$("input.slug").val(accentsTidy(str));
//                            return accentsTidy(str);
//                        }
//                    });
//                }

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
    publishPage: function (event) {

        var $elm = $(event.currentTarget).parents('tr').find('abbr.timeago');
        $elm.text(jQuery.timeago(new Date()));
        $(event.currentTarget).parents('tr').find('span').effect("highlight", { color: '#ffffaa' }, 3000);

        // Publish or unpublish the page
        $.post('/pages/publish/', { id: $(event.currentTarget).attr('name'), published: event.currentTarget.checked });

    },
    deletePage: function (event) {

        // Show confirm dialog and then delete the page
        //return confirm("Do you really want to delete this page?");

        var $row = $(event.currentTarget).closest('tr');
        
        $.ajax({
            url: '/pages/delete/',
            type: 'POST',
            dataType: 'html',
            data: { id: $row.attr('id') },
            success: function (data) {
                $row.fadeTo('fast', 0.6);
                $row.slideUp('fast');
//                var $growl = $('<aside />');
//                $('body').append($growl)
//                $growl.hide().html(data).fadeIn('fast');
//                $('html').mousemove(function () {
//                    $growl.delay(3000).fadeOut('fast', function () {
//                        $(this).remove();
//                    });
//                });
            }
        });

    }
});

// Return a helper with preserved width of cells
var fixHelper = function (e, ui) {
    ui.children().each(function () {
        $(this).width($(this).width());
    });
    return ui;
};