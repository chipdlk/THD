jQuery(document).ready(function () {
    jQuery(window).scroll(function () {
        console.log("111");
        if (jQuery(window).scrollTop() == 0) {
            jQuery("#my-header").slideDown();
            jQuery('#main-menu').removeClass("fix-nav");
            //jQuery(".blockSlideshow").css("margin-top", 0);
        }
        else if (jQuery(window).scrollTop() > 0 && jQuery(window).scrollTop() < 140) {
            

            //jQuery('#main-menu').animate({top:"50"});
            //jQuery("#my-header").slideUp();
            let mgtop = 140 - jQuery(window).scrollTop();
            jQuery(".blockSlideshow").css("margin-top", mgtop);
        } else if (jQuery(window).scrollTop() >= 140) {
            jQuery("#my-header").hide();
            jQuery('#main-menu').addClass("fix-nav");

        } else {
            jQuery('#main-menu').removeClass("fix-nav");
            //jQuery(".blockSlideshow").css("margin-top", 0);

        }
    });


    

});

// When the user scrolls the page, execute myFunction 
/*
window.onscroll = function () { myFunction() };

function myFunction() {
    var winScroll = document.body.scrollTop || document.documentElement.scrollTop;
    var height = document.documentElement.scrollHeight - document.documentElement.clientHeight;
    var scrolled = (winScroll / height) * 100;
    document.getElementById("myBar").style.width = scrolled + "%";
}*/