function UpdateSessionMenu(Id) {
    return $.ajax({
        type: "GET",
        url: '/Admin/Home/SetSeccionMenu/' + Id,             
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log(data);
        },
        error: function () {
            console.log('dsds');
        }
    });
}

$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

jQuery('#DeleteModal').on('show.bs.modal', function (event) {
    var button = jQuery(event.relatedTarget); // Button that triggered the modal
    var recipient = button.data('whatever'); // Extract info from data-* attributes
    var ids = button.data('ids');
    var modal = jQuery(this);
    jQuery("#DeleteItemTrue").attr("data-ids", ids);
    modal.find('.delete-title').text('Bạn có chắc chắn xóa ' + recipient + '?')
})



$("#DeleteItemTrue").click(function () {
    //var url = "/Admin/@ControllerName/";
    var form = $('#AjaxDeleteForm');
    var token = jQuery('input[name="__RequestVerificationToken"]', form).val();
    var ids = jQuery("#DeleteItemTrue").attr("data-ids");
    var url = jQuery("#DeleteItemTrue").attr("data-url");
    var headers = {};
    headers["RequestVerificationToken"] = token;
    return $.ajax({
        type: "POST",
        url: url + 'DeleteItem/' + ids,
        headers: headers,
        data: JSON.stringify({ "Ids": ids }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            window.location.reload();
        },
        error: function () {
            window.location.reload();
        }
    });
});


function UpdateStatus(Controller, Ids, Status) {

    var form = $('#AjaxDeleteForm');
    var token = jQuery('input[name="__RequestVerificationToken"]', form).val();
    var headers = {};
    headers["RequestVerificationToken"] = token;
    return $.ajax({
        type: "POST",
        url: '/Admin/' + Controller + '/UpdateStatus?Ids=' + Ids + "&Status=" + Status,
        headers: headers,
        data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            window.location.reload();
        },
        error: function () {
            window.location.reload();
        }
    });
}

function UpdateFeatured(Controller, Ids, Featured) {

    var form = $('#AjaxDeleteForm');
    var token = jQuery('input[name="__RequestVerificationToken"]', form).val();
    var headers = {};
    headers["RequestVerificationToken"] = token;
    return $.ajax({
        type: "POST",
        url: '/Admin/' + Controller + '/UpdateFeatured?Ids=' + Ids + "&Featured=" + Featured,
        headers: headers,
        data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            window.location.reload();
        },
        error: function () {
            window.location.reload();
        }
    });
}

function UpdateNotification(Controller, Ids, Notification) {

    var form = $('#AjaxDeleteForm');
    var token = jQuery('input[name="__RequestVerificationToken"]', form).val();
    var headers = {};
    headers["RequestVerificationToken"] = token;
    return $.ajax({
        type: "POST",
        url: '/Admin/' + Controller + '/UpdateNotification?Ids=' + Ids + "&Notification=" + Notification,
        headers: headers,
        data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            window.location.reload();
        },
        error: function () {
            window.location.reload();
        }
    });
}


function UpdateFeaturedHome(Controller, Ids, FeaturedHome) {

    var form = $('#AjaxDeleteForm');
    var token = jQuery('input[name="__RequestVerificationToken"]', form).val();
    var headers = {};
    headers["RequestVerificationToken"] = token;
    return $.ajax({
        type: "POST",
        url: '/Admin/' + Controller + '/UpdateFeaturedHome?Ids=' + Ids + "&FeaturedHome=" + FeaturedHome,
        headers: headers,
        data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            window.location.reload();
        },
        error: function () {
            window.location.reload();
        }
    });
}

function UpdateStaticPage(Controller, Ids, StaticPage) {

    var form = $('#AjaxDeleteForm');
    var token = jQuery('input[name="__RequestVerificationToken"]', form).val();
    var headers = {};
    headers["RequestVerificationToken"] = token;
    return $.ajax({
        type: "POST",
        url: '/Admin/' + Controller + '/UpdateStaticPage?Ids=' + Ids + "&StaticPage=" + StaticPage,
        headers: headers,
        data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            window.location.reload();
        },
        error: function () {
            window.location.reload();
        }
    });
}

function MySearchForm() {
    jQuery("#MySearchForm").submit();
}


$(function () {
    $('.namepr').keyup(function () { $('.titlepr').val($('.namepr').val()); });
});
$(function () {
    $('.Changetitle').keyup(function () { $('.slug').val(string_to_slug($('.Changetitle').val())); });
});

function string_to_slug(str) {
    str = str.replace(/^\s+|\s+$/g, ''); // trim
    str = str.toLowerCase();

    // remove accents, swap ñ for n, etc
    var from = "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴáàạảãâấầậẩẫăắằặẳẵéèẹẻẽêếềệểễÉÈẸẺẼÊẾỀỆỂỄóòọỏõôốồộổỗơớờợởỡÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠúùụủũưứừựửữÚÙỤỦŨƯỨỪỰỬỮíìịỉĩÍÌỊỈĨđĐýỳỵỷỹÝỲỴỶỸ";
    var to = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaeeeeeeeeeeeeeeeeeeeeeeoooooooooooooooooooooooooooooooooouuuuuuuuuuuuuuuuuuuuuuiiiiiiiiiiddyyyyyyyyyy";
    for (var i = 0, l = from.length; i < l; i++) {
        str = str.replace(new RegExp(from.charAt(i), 'g'), to.charAt(i));
    }

    str = str.replace(/[^a-z0-9 -]/g, '') // remove invalid chars
        .replace(/\s+/g, '-') // collapse whitespace and replace by -
        .replace(/-+/g, '-'); // collapse dashes
    return str;
}

function openCustomRoxy() {
    $('#roxyCustomPanel').modal('show');
}
function closeCustomRoxy() {
    $('#roxyCustomPanel').modal('hide');
}