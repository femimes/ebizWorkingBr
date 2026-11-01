$(window).on("load",function(){
	let preloaderFadeOutTime = 2000;
	function hidePreloader() {
	var preloader = $('.spinner-wrapper');
	preloader.fadeOut(preloaderFadeOutTime);
	}
	hidePreloader();
	// setTimeout(function() {
	// // set timeout is to simulate the loading time
	// // in your code it will not be required.
	// $('.la-anim-1').remove();
	// }, 4000);
	});
