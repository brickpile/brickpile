$(document).ready(function () {

    //$('.slug').slugify('#CurrentModel_MetaData_Name');
    //$('.slug').slugify('#NewPageModel_MetaData_Name');
    
    var url = $("#NewPageModel_MetaData_Url").val();
    $("#NewPageModel_MetaData_Slug").after('Your URL is ' + url + '<span class="slug"></span>');
    $('.slug').slugify('#NewPageModel_MetaData_Name', {
        slugFunc: function (str, originalFunc) {
            $("#NewPageModel_MetaData_Url").val(url + originalFunc(str));
            return originalFunc(str);
        }
    });

//    var url = $("#CurrentModel_MetaData_Url").val();
//    //$("#CurrentModel_MetaData_Slug").after('Your URL is ' + url + '<span class="slug"></span>');
//    $('span.slug').text(url);
//    $('.slug').slugify('#CurrentModel_MetaData_Name', {
//        slugFunc: function (str, originalFunc) {
//            $("#CurrentModel_MetaData_Url").val(url + originalFunc(str));
//            return originalFunc(str);
//        }
//    });


    //$('.slug').slugify('#title');

    //    $('#CurrentModel_MetaData_Slug').stringToSlug({
    //        setEvents: 'keyup keydown blur',
    //        getPut: '#CurrentModel_MetaData_Url',
    //        space: '-'
    //    });

    //    var currentUrl = $('#CurrentModel_MetaData_Url').val();
    //    $('#CurrentModel_MetaData_Slug').keyup(function () {
    //        var value = $(this).val();
    //        $(this).val(accentsTidy(value));
    //        $('#CurrentModel_MetaData_Url').val(function () {
    //            return currentUrl + accentsTidy(value);
    //        });
    //    });

    //    var newPageUrl = $('#NewPageModel_MetaData_Url').val();
    //    $('#NewPageModel_MetaData_Slug').bind('keyup', function () {
    //        var value = $(this).val();
    //        $(this).val(accentsTidy($(this).val()));
    //        $('#NewPageModel_MetaData_Url').val(function () {
    //            return newPageUrl + accentsTidy(value);
    //        });
    //    });

    //    $('#CurrentModel_MetaData_Name').keyup(function () {
    //        // Remove the class if it exists
    //        $(this).removeClass('input-validation-error');

    //        // Split the string with urlsegments into an array
    //        //var segments = $('#StructureInfo_CurrentItem_UrlSegments').val().split(',');

    //        var value = $(this).val();
    //        if (value == '') { $(this).addClass('input-validation-error'); }
    //        //        if ($.inArray(value, segments) != -1 || value == '') {
    //        //            $(this).addClass('input-validation-error');
    //        //        }
    //        // Remove diacritics
    //        //$(this).val(accentsTidy(value));
    //        // Update the value
    //        $('#CurrentModel_MetaData_Slug').val(accentsTidy($(this).val()));
    //        $('#CurrentModel_MetaData_Url').val(accentsTidy($(this).val()));
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