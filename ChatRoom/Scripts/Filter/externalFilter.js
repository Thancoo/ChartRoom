chatApp.filter('externalLink', function () {
    // find & remove protocol (http, ftp, etc.) and get domain
    function extractDomain(url) {
        var domain;
        if (url.indexOf("://") > -1) {
            domain = url.split('/')[2];
        }
        else {
            domain = url.split('/')[0];
        }
        return domain.split(':')[0];
    }

    // parse url
    function urlify(text) {
        var urlRegex = /(https?:\/\/[^\s]+)/g;
        return text.replace(urlRegex, function (url) {
            return '<a href="' + url + '">' + extractDomain(url) + '</a>';
        });
    }
    return function (message) {
        return urlify(message);
    };
});
chatApp.filter('EncodeEnjoy', function() {
    return function (content) {
        var imgPt = /^<img src="[a-zA-Z0-9/\.]+Images\/emjoy\/([0-9]+)\.gif">$/;
        var imgPtall = /(<img src="[a-zA-Z0-9/\.]+Images\/emjoy\/[0-9]+\.gif">)/;
        var ec=content.split(imgPtall).map(function(itm) {
            if (imgPt.test(itm))
                return itm.replace(imgPt, '[emj_$1]');
            return $('<div>').text(itm).html();
        });
        return ec.join('');
    }
});
chatApp.filter('DecodeEnjoy', function () {
    return function (content) {
        var cdImg = /(\[emj_[0-9]+\])/;
        var cdRp = /\[emj_([0-9]+)\]/;
        var nc=content.split(cdImg).map(function(itm) {
            return cdRp.test(itm) ? itm.replace(cdRp, '<img src="../../Images/emjoy/$1.gif">') : $('<div>').html(itm).text();
        });
        return nc.join('');
    }
});
chatApp.filter('reverse', function () {
    return function (items) {
        return items.slice().reverse();
    };
});