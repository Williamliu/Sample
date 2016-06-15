$.fn.extend({
	lwhTooltip:function( opts ){
			var def_settings = {
									hAlign:		"center",   //"left", "center", "right"
									vAlign:		"middle",  //"top", 	"middle", "bottom"
									offsetww:	20,
									offsethh:	20 	
							};
			$.extend(def_settings, opts);
			return this.each( function(idx, el) { 
				$(el).data("default_settings", def_settings);
			});
	},

	TShow: function( msg ) {
		return this.each( function(idx, el) {
			var def_settings = $(el).data("default_settings");
			$(".lwhTooltip_message", el).html(msg);
            $(el).show();
		});
	},
	
	THide: function() {
		$(el).fadeOut(1000);
	},
	
	autoTShow: function( msg ) {
		return this.each( function(idx, el) {
			var def_settings = $(el).data("default_settings");
			$(".lwhTooltip_message", el).html(msg);
			
			var el_ww 	= $(el).outerWidth();  // include border and padding
			var el_hh 	= $(el).outerHeight();
			//console.log( $(el).outerWidth() + " : " + $(el).outerHeight() );  // include border and padding
			//console.log( $(el).innerWidth() + " : " + $(el).innerHeight() );   // include padding
			//console.log( $(el).width() + " : " + $(el).height() );			   // no border and no padding

			var win_ww = $(window).width();  
			var win_hh = $(window).height();  
			// $(window).height();	  browser height 
			// $(document).height();  whole html document height; 
			// width
			if( def_settings ) {
				switch( def_settings.hAlign.toLowerCase() ) {
					case "left":
						$(el).css({"margin-left":"0px", "left":"20px"});
						break;
					case "right":
						var pos_left = ( win_ww - el_ww )> 0 ? ( win_ww - el_ww ) - 20 : 20;
						$(el).css({"margin-left":"0px", "left": pos_left + "px" });						
						break;
					default:
						var margin_left = ( win_ww - el_ww )> 0 ? el_ww / 2 : 20;
						margin_left = margin_left * -1;  							
						$(el).css("margin-left", margin_left + "px");
						break;
				}
				// end of width
				// height
				switch( def_settings.vAlign.toLowerCase() ) {
					case "top":
						$(el).css({"margin-top":"0px", "top":"20px"});
						break;
					case "bottom":
						var pos_top = ( win_hh - el_hh )> 0 ? ( win_hh - el_hh ) - 20 : 20;
						$(el).css({"margin-top":"0px", "top": pos_top + "px" });						
						break;
					default:
						var margin_top = ( win_hh - el_hh )> 0 ? el_hh / 2 : 20;
						margin_top = margin_top * -1;  							
						$(el).css("margin-top", margin_top + "px");
						break;
				}
				// end of height
			}
			$(el).stop(true, true).fadeIn(10).delay(2000).fadeOut(1000);
		});
	}
});