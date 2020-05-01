jQuery(document).ready(function ($) {
    //Menu
    jQuery('.stellarnav').stellarNav({
        theme: 'light',
        breakpoint: 991,
        position: 'right',
        phoneBtn: '02613704226',
        locationBtn: 'https://www.google.com/maps'
    });

    // Link liên kết
    $('#BannerId').on('change', function () {
        var url = $(this).val();
        if (url) {
            window.open(url, '_blank');
        }
        return false;
    });

    //Thống kê truy cập
    $(document).ready(function () {
        return $.ajax({
            type: "GET",
            url: "/SiteVisit/index",
            data: JSON.stringify({}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                $("#SiteVisit_Yesterday").html(numberWithCommas(data.Yesterday));
                $("#SiteVisit_DateOfWeek").html(numberWithCommas(data.DateOfWeek));
                $("#SiteVisit_DateNow").html(numberWithCommas(data.DateNow));
                $("#SiteVisit_Total").html(numberWithCommas(data.Total));
            },
            error: function () {

            }
        });
    });

    $('#image-gallery').lightSlider({
        gallery: true,
        item: 1,
        thumbItem: 9,
        slideMargin: 0,
        speed: 800,
        pause: 6000,
        auto: true,
        pager: false,
        controls: true,
        mode:'fade',
        loop: true,
        onSliderLoad: function () {
            $('#image-gallery').removeClass('cS-hidden');
        }
    });


    $('#Features-Slide').lightSlider({
        gallery: true,
        item: 1,
        thumbItem: 9,
        slideMargin: 0,
        speed: 800,
        pause: 4000,
        auto: true,
        pager: false,
        controls: false,
        mode: 'fade',
        loop: true,
        onSliderLoad: function () {
            $('#Features-Slide').removeClass('cS-hidden');
        }
    });

});



function SubmitSearch() {
    $('form#search-all').submit();
}

function numberWithCommas(number) {
    var parts = number.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}