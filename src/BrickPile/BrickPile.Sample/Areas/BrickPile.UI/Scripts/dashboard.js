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
                var $dialog = $('.overlay');
                //                $dialog.click(function (event) {
                //                    event.stopPropagation();
                //                });
                var $overlay = $('.overlay');
                $('html').click(function () {
                    $overlay.fadeOut('fast', function () {
                        $overlay.remove();
                    });
                });
                $dialog.find('a.close').click(function () {
                    $overlay.fadeOut('fast', function () {
                        $overlay.remove();
                    });
                    return false;
                });
                $(':radio').live('click', function (e) {
                    $(this).closest('form').submit();
                });
                //                var url = $("#Url").val();
                //                $('.slug').slugify('#Name', {
                //                    slugFunc: function (str, originalFunc) {
                //                        $("#Url").val(url + accentsTidy(str));
                //                        return accentsTidy(str);
                //                    }
                //                });
            }
        });
        return false;
    },
    publish: function ($input) {
        var self = this;
        $.ajax({
            url: '/dashboard/content/publish/' + $input.attr('name') + '/' + $input.attr('checked'),
            dataType: 'html',
            success: function (data) {
                $.jGrowl(data);
            }
        });
    }
    //    ,
    //    browse: function ($anchor) {
    //        var self = this;
    //        $.ajax({
    //            url: '/dashboard/library/openbucket/',
    //            dataType: 'html',
    //            success: function (data) {
    //                $('body').append(data);
    //                var $dialog = $('div.overlay aside');
    //                $dialog.click(function (event) {
    //                    event.stopPropagation();
    //                });
    //                var $overlay = $('div.overlay');
    //                $('body').click(function () {
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
    //            }
    //        });
    //    },
    //    edit: function ($anchor) {
    //        pixlr.overlay.show({ image: $anchor.parent().parent().find('img').attr('src'), referrer: 'BrickPile CMS', service: 'editor', target: 'http://erie.kloojed.com:81/dashboard/library/save', exit: 'http://erie.kloojed.com:81/dashboard/library/save' });
    //    }
};

$(document).ready(function () {

    Dashboard = new Dashboard();
    $('table caption .add').live('click', function () {
        Dashboard.add($(this));
        return false;
    });

    $('.publish').live('click', function () { Dashboard.publish($(this)); });

    //$('.browse').live('click', function () { Dashboard.browse($(this)); });
    //$('.edit').live('click', function () { Dashboard.edit($(this)); });

    // set default hover delay to 500
    //$.event.special.hover.delay = 500;

    // Handle the slug and url


    var url = $("#NewPageModel_Metadata_Url").val();
    if (url != null) {
        var to = url.lastIndexOf('/');
        url = url.substring(0, to + 1);

        $('.slug').slugify('#NewPageModel_Metadata_Name', {
            slugFunc: function (str, originalFunc) {
                $("#NewPageModel_Metadata_Url").val(url + accentsTidy(str));
                $("#NewPageModel_Metadata_Slug").val(accentsTidy(str));
                return accentsTidy(str);
            }
        });
    }


    //    $('nav ul a span.cut').each(function () {
    //        $(this).click(function () {
    //            localStorage.setItem('cut', $(this).closest('li').attr('data-item-id'));
    //            return false;
    //        });
    //    });

    //    $('nav ul a span.paste').each(function () {
    //        $(this).click(function () {
    //            var $action = $(this);
    //            $.ajax({
    //                type: 'POST',
    //                url: '/dashboard/content/paste',
    //                data: 'sourceId=' + localStorage.getItem('cut') + '&destinationId=' + $(this).closest('li').attr('data-item-id') + '',
    //                success: function (data) {
    //                    window.location = $action.attr('data-val');
    //                }
    //            });
    //            $(this).fadeOut('fast');
    //            return false;
    //        });
    //    });

    //    $('nav ul a span.delete').each(function () {
    //        $(this).click(function () {
    //            $.ajax({
    //                url: $(this).attr('data-val'),
    //                success: function (data) {
    //                    $('body').append(data);
    //                    var $dialog = $('div.overlay aside');
    //                    $dialog.click(function (event) {
    //                        event.stopPropagation();
    //                    });
    //                    var $overlay = $('div.overlay');
    //                    $('body').click(function () {
    //                        $overlay.fadeOut('fast', function () {
    //                            $overlay.remove();
    //                        });
    //                    });
    //                    $dialog.find('a.close').click(function () {
    //                        $overlay.fadeOut('fast', function () {
    //                            $overlay.remove();
    //                        });
    //                        return false;
    //                    });
    //                }
    //            });
    //            $(this).fadeOut('fast');
    //            return false;
    //        });
    //    });

    //    $('nav ul a span.add').each(function () {
    //        $(this).click(function () {
    //            $.ajax({
    //                url: $(this).attr('data-val'),
    //                success: function (data) {
    //                    $('body').append(data);
    //                    var $dialog = $('div.overlay aside');
    //                    $dialog.click(function (event) {
    //                        event.stopPropagation();
    //                    });
    //                    var $overlay = $('div.overlay');
    //                    $('body').click(function () {
    //                        $overlay.fadeOut('fast', function () {
    //                            $overlay.remove();
    //                        });
    //                    });
    //                    $dialog.find('a.close').click(function () {
    //                        $overlay.fadeOut('fast', function () {
    //                            $overlay.remove();
    //                        });
    //                        return false;
    //                    });
    //                    var url = $("#Url").val();
    //                    $('.slug').slugify('#Name', {
    //                        slugFunc: function (str, originalFunc) {
    //                            $("#Url").val(url + accentsTidy(str));
    //                            return accentsTidy(str);
    //                        }
    //                    });
    //                }
    //            });
    //            $(this).fadeOut('fast');
    //            return false;
    //        });
    //    });

    //    $('nav ul a').hover(function (e) {
    //        $(this).children('span').show();
    //    }, function (e) {
    //        $(this).children('span').fadeOut('fast');
    //    });

});

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