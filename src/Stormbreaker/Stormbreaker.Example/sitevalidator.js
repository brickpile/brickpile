if (typeof jQuery == 'undefined') {
    // http://www.hunlock.com/blogs/Howto_Dynamically_Insert_Javascript_And_CSS
    var jQ = document.createElement('script');
    jQ.type = 'text/javascript';
    jQ.onload = runthis;
    jQ.src = 'http://ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js';
    document.body.appendChild(jQ);
} else {
    runthis();
}

function runthis() {
    if ($("#wikiframe").length == 0) {
        $("body").append("\
		<div id='wikiframe'>\
			<div id='wikiframe_veil' style=''>\
				<p>Loading...</p>\
			</div>\
			<iframe src='http://static.kloojed.com/sitevalidator.html?uri=" + window.location + "' onload=\"$('#wikiframe iframe').slideDown(500);\">Enable iFrames.</iframe>\
			<style type='text/css'>\
				#wikiframe_veil { display: none; position: fixed; width: 100%; height: 100%; top: 0; left: 0; background-color: rgba(255,255,255,.25); cursor: pointer; z-index: 900; }\
				#wikiframe_veil p { color: black; font: normal normal bold 20px/20px Helvetica, sans-serif; position: absolute; top: 50%; left: 50%; width: 10em; margin: -10px auto 0 -5em; text-align: center; }\
				#wikiframe iframe { display: none; position: fixed; top: 10%; left: 10%; width: 80%; height: 80%; z-index: 999; border: 10px solid rgba(0,0,0,.5); margin: -5px 0 0 -5px; }\
			</style>\
		</div>");
        $("#wikiframe_veil").fadeIn(750);
    } else {
        $("#wikiframe_veil").fadeOut(750);
        $("#wikiframe iframe").slideUp(500);
        setTimeout("$('#wikiframe').remove()", 750);
    }
    $("#wikiframe_veil").click(function (event) {
        $("#wikiframe_veil").fadeOut(750);
        $("#wikiframe iframe").slideUp(500);
        setTimeout("$('#wikiframe').remove()", 750);
    });
}

function getSelText() {
    var s = '';
    if (window.getSelection) {
        s = window.getSelection();
    } else if (document.getSelection) {
        s = document.getSelection();
    } else if (document.selection) {
        s = document.selection.createRange().text;
    }
    return s;
}