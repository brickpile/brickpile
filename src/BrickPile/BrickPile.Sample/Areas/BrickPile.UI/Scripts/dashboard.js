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
            url: '/dashboard/content/publish/' + $input.attr('name') + '/' + $input.attr('checked'),
            dataType: 'html',
            success: function (data) {
                //$.jGrowl(data);
            }
        });
    }
};

$(document).ready(function () {

    Dashboard = new Dashboard();
    $('.add a').live('click', function () { Dashboard.add($(this)); return false; });
    $('.publish').live('click', function () { Dashboard.publish($(this)); });

    $("#pages table tbody").sortable({
        handle: 'td.sort',
        items: "tr:not(.ui-state-disabled)",
        helper: fixHelper,
        update: function (event, ui) {
            $.ajax({
                type: 'POST',
                url: '/dashboard/content/sort',
                data: { items : $(this).sortable('toArray') },
                traditional: true,
                success: function (data) { }
            });
        }
    });

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