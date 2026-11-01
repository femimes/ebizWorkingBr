$(function(){
			  $("a").each(function (index, element){
				  var href = $(this).attr("href");
				  $(this).attr("hiddenhref", href);
				  $(this).removeAttr("href");
			  });
			  $("a").click(function(){
				  url = $(this).attr("hiddenhref");
				 // window.open(url);
											window.location.href = url;
			  })
		  }); 