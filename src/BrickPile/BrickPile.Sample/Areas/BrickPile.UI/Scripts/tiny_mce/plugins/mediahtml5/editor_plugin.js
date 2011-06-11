        function doTinyMCECleanUp(frm, id) {
           var c = frm[id].value;
           // tinyMCE doesn't handle close the <source> tag properly
           // so we remove the closing </source>
           c = c.replace(/&nbsp;<\/source>/gim, '');
           c = c.replace(/<\/source>/gim, '');
           // comment the following out if 'html' not 'xhtml'
           c = c.replace(/<source([^>]*)>/gim, '<source $1 />');
           frm[id].value = c;
        }