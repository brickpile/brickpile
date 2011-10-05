$(document).ready(function () {
    $('figcaption').hide();
    $('figure').hover(function () {
        $('figcaption').fadeIn('fast');
    }, function () {
        $('figcaption').fadeOut('fast');
    });
    Cufon.replace('h1,h2,body.caselist #content > p,.introduction,nav#global li a', { hover: true });

    $('body.news article .main').append('<fb:like href="" layout="button_count" show_faces="true" width="450" font="arial"></fb:like>');
});