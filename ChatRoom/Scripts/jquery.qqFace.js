// QQ表情插件
(function($){  
	$.fn.qqFace = function(options){
		var defaults = {
			id : 'facebox',
			path : 'face/',
			assign : 'content',
			tip : 'em_'
		};
	    if (window.customerData === undefined)
	        window.customerData = {};
	    window.customerData.qqFaceConfig = defaults;
		var option = $.extend(defaults, options);
		var id = option.id;
		var path = option.path;
		var tip = option.tip;
		
		$(this).click(function (e) {
		    if ($('#' + id).data('showState') == "show") {
		        return;
		    }
		    var strFace;
		    if ($('#' + id).length <= 0) {
		        strFace = '<div id="' +
		            id +
		            '" data-faceinitial="1" style="position:absolute;display:none;z-index:1000;" class="qqFace">' +
		            '<table border="0" cellspacing="0" cellpadding="0"><tr>';
		        for (var i = 1; i <= 75; i++) {
		            strFace += '<td><img src="' + path + i + '.gif" class="emjoy_itm" data-eledata="' + i + '" /></td>';
		            if (i % 15 == 0) strFace += '</tr><tr>';
		        }
		        strFace += '</tr></table></div>';
		        $(this).parent().append(strFace);
		        $('#' + id).off('click', '.emjoy_itm');
		        $('#' + option.assign).length > 0 && $('#' + id).on('click', '.emjoy_itm',
		            function(ele) {
		                $('#' + option.assign).insertAtCaret(('<img src="' + path + $(ele.target).data('eledata') + '.gif" />'), ('[' + tip + $(ele.target).data('eledata') + ']'));
		                ele.stopPropagation();
		            });
		        var offsetBottom = $(this).position().top + $('#' + id).height() + 40;
		        var offsetLeft = $(this).position().left;
		        $('#' + id).css('top', -offsetBottom);
		        $('#' + id).css('left', offsetLeft);
		    }
		    $('#' + id).show();
		    $('#' + id).data('showState', 'show');
			e.stopPropagation();
		});
	};
	$(document).on('click', '*', function (e) {
	    if (!((window.customerData || {}).qqFaceConfig || {}).id)
	        return;
	    var id = window.customerData.qqFaceConfig.id;
	    if ($('#' + id).data('showState') == "hidden") {
            return;
        }
        if ($('#' + id).length > 0 && (e.pageX > $('#' + id).offset().left + parseInt($('#' + id).css("width").substring(0, $('#' + id).css("width").length - 2)) || e.pageX < $('#' + id).offset().left || e.pageY > $('#' + id).offset().top + parseInt($(".heard_hover_action_list").css("height").substring(0, $(".heard_hover_action_list").css("height").length - 2)) || e.pageY < $('#' + id).offset().top)) {
            $('#' + id).hide();
            $('#' + id).data('showState', 'hidden');
        }
        e.stopPropagation();
    });

})(jQuery);

jQuery.extend({ 
unselectContents: function(){ 
	if(window.getSelection) 
		window.getSelection().removeAllRanges(); 
	else if(document.selection) 
		document.selection.empty(); 
	} 
}); 
jQuery.fn.extend({ 
	selectContents: function(){ 
		$(this).each(function(i){ 
			var node = this; 
			var selection, range, doc, win; 
			if ((doc = node.ownerDocument) && (win = doc.defaultView) && typeof win.getSelection != 'undefined' && typeof doc.createRange != 'undefined' && (selection = window.getSelection()) && typeof selection.removeAllRanges != 'undefined'){ 
				range = doc.createRange(); 
				range.selectNode(node); 
				if(i == 0){ 
					selection.removeAllRanges(); 
				} 
				selection.addRange(range); 
			} else if (document.body && typeof document.body.createTextRange != 'undefined' && (range = document.body.createTextRange())){ 
				range.moveToElementText(node); 
				range.select(); 
			} 
		}); 
	}, 
	insertAtCaret: function (textFeildValue, saveEle) {
	    $(this).html($(this).html() + textFeildValue);
	} 
});
jQuery.extend({
    browser: function () {
        var
            rwebkit = /(webkit)\/([\w.]+)/,
            ropera = /(opera)(?:.*version)?[ \/]([\w.]+)/,
            rmsie = /(msie) ([\w.]+)/,
            rmozilla = /(mozilla)(?:.*? rv:([\w.]+))?/,
            browser = {},
            ua = window.navigator.userAgent,
            browserMatch = uaMatch(ua, rwebkit, ropera, rmsie, rmozilla);
        if (browserMatch.browser) {
            browser[browserMatch.browser] = true;
            browser.msie = browserMatch.browser;
            browser.version = browserMatch.version;
        }
        return { browser: browser };
    }
});
function uaMatch(ua, rwebkit, ropera, rmsie, rmozilla) {
    ua = ua.toLowerCase();

    var match = rwebkit.exec(ua)
        || ropera.exec(ua)
        || rmsie.exec(ua)
        || ua.indexOf("compatible") < 0 && rmozilla.exec(ua)
        || [];

    return {
        browser: match[1] || "",
        version: match[2] || "0"
    };
}