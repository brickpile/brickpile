$(document).ready(function () {

    $('#NewPageModel_MetaData_Slug').keyup(function () {
        // Remove the class if it exists
        $(this).removeClass('error');

        // Split the string with urlsegments into an array
        //var segments = $('#StructureInfo_CurrentItem_UrlSegments').val().split(',');

        var value = $(this).val();
        if (value == '') { $(this).addClass('input-validation-error'); }
//        if ($.inArray(value, segments) != -1 || value == '') {
//            $(this).addClass('input-validation-error');
//        }
        // Remove diacritics
        $(this).val(accentsTidy(value));
        // Update the value
        //$('#StructureInfo_CurrentItem_Url span').text(accentsTidy($(this).val()));
    });
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
