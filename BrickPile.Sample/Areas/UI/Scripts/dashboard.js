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
    publish: function ($input) {
        var self = this;
        $.ajax({
            url: '/pages/publish',
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
            url: '/pages/delete',
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
            url: '/pages/permanentdelete',
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
    selectPage: function ($input) {
        var self = this;
        var id = $input.attr('id');
        $.ajax({
            url: '/dialog/editmodelreference',
            dataType: 'html',
            success: function (data) {
                $('body').append(data);
                var $dialog = $('.overlay');
                $dialog.find('div#scroll').lionbars('dark', true, false, false);
                $dialog.find('a.close').live('click', function () {
                    $dialog.fadeOut('fast', function () {
                        $dialog.remove();
                    });
                    return false;
                });
                $dialog.find('a.cancel').live('click', function () {
                    $dialog.fadeOut('fast', function () {
                        $dialog.remove();
                    });
                    return false;
                });

                $dialog.find('#pages a.link').live('click', function () {
                    $('#' + id + '_Id').val($(this).attr('data-id'));
                    $('#pages li').removeClass('selected');
                    $('.select').parent().removeClass('disabled');
                    $(this).parent('li').addClass('selected');
                    return false;
                });

                $dialog.find('a.select').click(function () {
                    $('#' + id + '_Name').val($('#pages li.selected a').text());
                    $dialog.fadeOut('fast', function () {
                        $dialog.remove();
                    });

                    return false;
                });

                $dialog.find('.navigator, .parent a').live('click', function () {
                    $.ajax({
                        url: '/dialog/GetChildrenModel',
                        type: 'GET',
                        dataType: 'html',
                        data: { id: $(this).attr('data-id') },
                        success: function (data) {
                            $('.additional-block').html(data);
                            $('div#scroll').lionbars('dark', true, false, false);
                        }
                    });
                    return false;
                });
            }
        });
    },
    browse: function ($anchor) {
        var self = this;
        $.ajax({
            url: '/assets',
            dataType: 'html',
            success: function (data) {

                $('body').append(data);

                var $dialog = $('.overlay');
                $dialog.find('div#scroll').lionbars('dark', true, false, false);
                $dialog.find('a.close').live('click',function () {
                    $dialog.fadeOut('fast', function () {
                        $dialog.remove();
                    });
                    return false;
                });

                $dialog.find('a.cancel').live('click',function () {
                    $dialog.fadeOut('fast', function () {
                        $dialog.remove();
                    });
                    return false;
                });

                $dialog.find('li a.insert').click(function () {

                    var self = this;
                    $dialog.find('a.insert').removeClass('selected');
                    $dialog.find('.select').parent().removeClass('disabled');
                    $(self).addClass('selected');

                    return false;
                });

                $dialog.find('a.select').live('click',function () {

                    var $selectedItem = $('a.selected',$dialog);
                    $anchor.parent().parent().parent().find('input:hidden.virtualUrl').val($selectedItem.attr('data-virtualpath'));
                    $anchor.parent().parent().parent().find('input:hidden.url').val($selectedItem.attr('data-url'));
                    $dialog.fadeOut('fast', function () {
                        $dialog.remove();
                        $.ajax({
                            url: '/assets/getthumbnailurl',
                            type: 'GET',
                            dataType: 'text',
                            data: { path: $selectedItem.attr('data-virtualpath') },
                            beforeSend: function (jqXHR, settings) {
                                $anchor.parent().parent().parent().find('img').attr('src', '');
                            },
                            success: function (data) {
                                $anchor.parent().parent().parent().find('img').attr('src', data);
                            }
                        });

                    });

                    return false;
                });

                $dialog.find('a.directory').live('click', function () {
                    $.ajax({
                        url: '/assets/getdirectory',
                        type: 'GET',
                        dataType: 'html',
                        data: { path: $(this).attr('data-virtualpath') },
                        success: function (data) {
                            $('.additional-block').html(data);
                            $('div#scroll').lionbars('dark', true, false, false);
                            $dialog.find('li a.insert').click(function () {
                                var self = this;
                                $dialog.find('a.insert').removeClass('selected');
                                $dialog.find('.select').parent().removeClass('disabled');
                                $(self).addClass('selected');

                                return false;
                            });                            
                        }
                    });
                    return false;
                });


                //                                var $dialog = $('div.overlay aside');
                //                                $dialog.click(function (event) {
                //                                    event.stopPropagation();
                //                                });
                //                                var $overlay = $('div.overlay');
                //                                 $('body').click(function () {
                //                                    $overlay.fadeOut('fast', function () {
                //                                        $overlay.remove();
                //                                    });
                //                                });
                //                                $dialog.find('a.close').click(function () {
                //                                    $overlay.fadeOut('fast', function () {
                //                                        $overlay.remove();
                //                                    });
                //                                    return false;
                //                                });
                //                                $('div.overlay aside tr').click(function (event) {
                //                                    $('div.overlay aside tr').removeClass('selected');
                //                                    $(this).toggleClass('selected');
                //                                });
                //                                $('div.overlay aside td .insert').click(function () {
                //                                    $anchor.parent().parent().parent().find('input:hidden').val($(this).attr('data-val'));
                //                                    $('div.overlay').remove();
                //                                    $anchor.parent().parent().find('img').attr('src', $(this).attr('data-val'));
                //                                    return false;
                //                                });
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
    $('.imageurl input').live('click', function () { Dashboard.browse($(this)); });
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


    $("table.sortable tbody").sortable({
        handle: 'td.sort',
        items: "tr:not(.ui-state-disabled)",
        //helper: fixHelper,
        helper: 'clone',
        //placeholder: "ui-state-highlight",
        opacity: 0.7,
        //forcePlaceholderSize: true,
        update: function (event, ui) {
            $.ajax({
                type: 'POST',
                url: '/pages/sort',
                data: { items: $(this).sortable('toArray') },
                traditional: true,
                success: function (data) { }
            });
        }
//        start: function (event, ui) {
//            ui.placeholder.height(ui.item.height());
//        },
//      stop: function (event, ui) {
//            console.log(ui);
//            $(ui.item).css('opacity', '1');
//        }
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