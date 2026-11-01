$(window).on("load",function(){
    let preloaderFadeOutTime = 5500;
function hidePreloader() {
var preloader = $('.spinner-wrapper');
preloader.fadeOut(preloaderFadeOutTime);
}
hidePreloader();
      // $(".spinner-wrapper").fadeOut("slow");
});
