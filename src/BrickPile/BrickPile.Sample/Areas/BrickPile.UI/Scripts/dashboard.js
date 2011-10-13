 var Dashboard = function (config) {
    this.init(config);
    return this;
};

Dashboard.prototype = {
    init: function (config) {
        //map external functions
        $.extend(this, config);
    },
    add: function ($anchor) {
        var self = this;
        $.ajax({
            url: $anchor.attr('href'),
            dataType: 'html',
            success: function (data) {
                $('body').append(data);
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
    publish: function ($input) {
        var self = this;
        $.ajax({
            url: '/dashboard/content/publish',
            type: 'POST',
            dataType: 'html',
            data: { id: $input.attr('name'), published: $input.is(':checked') },
            success: function (data) {
                var $growl = $('<aside />');
                $('body').append($growl)
                $growl.hide().html(data).fadeIn('fast');
                $('html').mousemove(function () {
                    $growl.delay(3000).fadeOut('fast', function () {
                        $(this).remove();
                    });
                });
            }
        });
    },
    remove: function ($anchor) {
        var self = this;
        var $row = $anchor.closest('tr');
        $.ajax({
            url: '/dashboard/content/delete',
            type: 'POST',
            dataType: 'html',
            data: { id: $row.attr('id') },
            success: function (data) {
                $row.fadeTo('fast', 0);
                $row.slideUp('fast');
                var $growl = $('<aside />');
                $('body').append($growl)
                $growl.hide().html(data).fadeIn('fast');
                $('html').mousemove(function () {
                    $growl.delay(3000).fadeOut('fast', function () {
                        $(this).remove();
                    });
                });
            }
        });
    },
    permanentRemove: function ($anchor) {
        var self = this;
        var $row = $anchor.closest('tr');
        $.ajax({
            url: '/dashboard/content/permanentdelete',
            type: 'POST',
            dataType: 'html',
            data: { id: $row.attr('id') },
            success: function (data) {
                $row.fadeTo('fast', 0);
                $row.slideUp('fast');
                var $growl = $('<aside />');
                $('body').append($growl)
                $growl.hide().html(data).fadeIn('fast');
                $('html').mousemove(function () {
                    $growl.delay(3000).fadeOut('fast', function () {
                        $(this).remove();
                    });
                });
            }
        });
    },
    undelete: function ($anchor) {
        var self = this;
        var $row = $anchor.closest('tr');
        $.ajax({
            url: '/dashboard/content/undelete',
            type: 'POST',
            dataType: 'html',
            data: { id: $row.attr('id') },
            success: function (data) {
                $row.fadeTo('fast', 1);
                var $growl = $('<aside />');
                $('body').append($growl)
                $growl.hide().html(data).fadeIn('fast');
                $('html').mousemove(function () {
                    $growl.delay(3000).fadeOut('fast', function () {
                        $(this).remove();
                    });
                });
            }
        });
    },
    selectPage: function ($input) {
        var self = this;
        var id = $input.attr('id');
        $.ajax({
            url: '/dashboard/dialog/editmodelreference',
            dataType: 'html',
            success: function (data) {
                $('body').append(data);
                var $dialog = $('.overlay');
                //$('.overlay aside.dialog').stopPropagation();
                //                $('html').click(function () {
                //                    $dialog.fadeOut('fast', function () {
                //                        $dialog.remove();
                //                    });
                //                });
                $dialog.find('a.close').click(function () {
                    $dialog.fadeOut('fast', function () {
                        $dialog.remove();
                    });
                    return false;
                });
                $dialog.find('a.cancel').click(function () {
                    $dialog.fadeOut('fast', function () {
                        $dialog.remove();
                    });
                    return false;
                });
                $('#pagetree a').live('click', function (e) {
                    $('#' + id + '_Id').val($(this).attr('data-val'));
                    $('#pagetree a').removeClass('selected');
                    $('.select').parent().removeClass('disabled');
                    $(this).addClass('selected');
                    return false;
                });
                $dialog.find('a.select').click(function () {
                    $('#' + id + '_Name').val($('#pagetree a.selected').text());
                    $dialog.fadeOut('fast', function () {
                        $dialog.remove();
                    });
                    return false;
                });

            }
        });
        return false;

    },
    browse: function ($anchor) {
        var self = this;
        $.ajax({
            url: '/dashboard/library/openbucket',
            dataType: 'html',
            success: function (data) {
                $('body').append(data);
                //                var $dialog = $('div.overlay aside');
                //                $dialog.click(function (event) {
                //                    event.stopPropagation();
                //                });
                //                var $overlay = $('div.overlay');
                //                 $('body').click(function () {
                //                    $overlay.fadeOut('fast', function () {
                //                        $overlay.remove();
                //                    });
                //                });
                //                $dialog.find('a.close').click(function () {
                //                    $overlay.fadeOut('fast', function () {
                //                        $overlay.remove();
                //                    });
                //                    return false;
                //                });
                //                $('div.overlay aside tr').click(function (event) {
                //                    $('div.overlay aside tr').removeClass('selected');
                //                    $(this).toggleClass('selected');
                //                });
                //                $('div.overlay aside td .insert').click(function () {
                //                    $anchor.parent().parent().parent().find('input:hidden').val($(this).attr('data-val'));
                //                    $('div.overlay').remove();
                //                    $anchor.parent().parent().find('img').attr('src', $(this).attr('data-val'));
                //                    return false;
                //                });
            }
        });
    }
};

$(document).ready(function () {

    Dashboard = new Dashboard();
    $('.add a').live('click', function () { Dashboard.add($(this)); return false; });
    $('.publish').live('click', function () { Dashboard.publish($(this)); });
    $('.delete').live('click', function () { Dashboard.remove($(this)); return false; });
    $('.perma-delete').live('click', function () { Dashboard.permanentRemove($(this)); return false; });
    //$('.undo').live('click', function () { Dashboard.undelete($(this)); return false; });
    $('.browse').live('click', function () { Dashboard.browse($(this)); });
    $('.modelreference input').live('click', function () { Dashboard.selectPage($(this)); });

    // Hide error labels when clicked
    $('.field-validation-error').live('click', function () {
        $(this).fadeOut('normal', function () {
            $(this).remove();
        });
    });

    // Disable the submit button until all fields is filled
    var $input = $('body.register form fieldset input'), $register = $('body.register input:submit');
    $register.attr('disabled', true);
    $register.closest('span').addClass('disabled');
    $input.keyup(function () {
        var trigger = false;
        $input.each(function () {
            if (!$(this).val()) {
                trigger = true;
            }
        });
        trigger ? $register.attr('disabled', true).closest('span').addClass('disabled') : $register.removeAttr('disabled').closest('span').removeClass('disabled');
    });


    $("#pages table tbody").sortable({
        handle: 'td.sort',
        items: "tr:not(.ui-state-disabled)",
        helper: fixHelper,
        update: function (event, ui) {
            $.ajax({
                type: 'POST',
                url: '/dashboard/content/sort',
                data: { items: $(this).sortable('toArray') },
                traditional: true,
                success: function (data) { }
            });
        }
    });

    $('#metadata').hide();
    $('.advanced button').click(function () {
        $('#metadata').toggle();
    });

    // Handle the slug and url
    var url = $("input.url").val();
    if (url != null) {
        var to = url.lastIndexOf('/');
        url = url.substring(0, to + 1);

        $('.slug').slugify('input.title', {
            slugFunc: function (str, originalFunc) {
                $("input.url").val(url + accentsTidy(str));
                $("input.slug").val(accentsTidy(str));
                return accentsTidy(str);
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

accentsTidy = function (s) {
    var r = s.toLowerCase();
    r = r.replace(new RegExp("\\s", 'g'), "-");
    r = r.replace(new RegExp("[àáâãäå]", 'g'), "a");
    r = r.replace(new RegExp("æ", 'g'), "ae");
    r = r.replace(new RegExp("ç", 'g'), "c");
    r = r.replace(new RegExp("[èéêë]", 'g'), "e");
    r = r.replace(new RegExp("[ìíîï]", 'g'), "i");
    r = r.replace(new RegExp("ñ", 'g'), "n");
    r = r.replace(new RegExp("[òóôõö]", 'g'), "o");
    r = r.replace(new RegExp("œ", 'g'), "oe");
    r = r.replace(new RegExp("[ùúûü]", 'g'), "u");
    r = r.replace(new RegExp("[ýÿ]", 'g'), "y");
    r = r.replace(new RegExp("\\W", 'g'), "-");
    r = r.replace(new RegExp("-+", 'g'), "-");
    return r;
};