$('.rate-star').click(function () {

    var bookId = $(this).attr("data-bookid");
    var clickedVal = $(this).attr("data-val");
    var userId = $(this).attr("data-userid");
    var objType = $(this).attr("data-objType");
    console.log(bookId)
    if (userId == "") {
        window.location.href = "/Account/Login?ReturnUrl=" + window.location.pathname;
    } else {
        var url = '/UserReviews/AddUserRating';
        var model = { "UserId": bookId, "ObjectType": objType, "ObjectId": bookId, "value": clickedVal };
        $.post(url, model)
            .done(function (data) {
                window.location.replace("/book/" + bookId);
            })
            .fail(function (data) {
                //console.log(data);
            });
    }
});
$('#submit-review').click(function () {

    var bookId = $(this).attr("data-bookid");
    var comment = $('#user-review').val();
    var userId = $(this).attr("data-userid");
    var objType = $(this).attr("data-objType");
    //console.log(bookId)
    if (userId == "") {
        location.replace("/Account/Login?ReturnUrl=" + window.location.pathname);
    } else {
        var url = '/UserReviews/AddUserComment';
        var isanonymous = true;
        var isspolier = false;
        if ($("#isspolier").prop("checked")) {
            isspolier = true;
        } else {
            isspolier = false;
        }
        if ($("#isanonymous").prop("checked")) {
            isanonymous = true;
        } else {
            isanonymous = false;
        }
        var model = { "UserId": bookId, "ObjectType": objType, "ObjectId": bookId, "UserComment": comment, "isspolier": isspolier, "isanonymous": isanonymous };
        $.post(url, model)
            .done(function (data) {
                $('.submited-success').show();
                $('#rvw-comment-area').hide();
               // event.preventDefault();
                window.location.replace("/book/" + bookId);
            })
            .fail(function (data) {
                //console.log(data);
            });
    }
});
$("#love-book").click(function (e) {
    var bookId = $(this).attr("data-bookid");
    var model = { "BookId": bookId };
    var url = '/UserReviews/NotLoveThisItem';
    $.post(url, model)
        .done(function (data) {
            console.log(data);
            $("#no-love-book").show();
            $("#love-book").hide();
        })
        .fail(function (data) {
            console.log(data);
        });
});

$("#no-love-book").click(function (e) {
    var bookId = $(this).attr("data-bookid");
    var model = { "BookId": bookId };
    var url = '/UserReviews/LoveThisItem';
    $.post(url, model)
        .done(function (data) {
            $("#no-love-book").hide();
            $("#love-book").show();
            console.log(data);
        })
        .fail(function (data) {
            console.log(data);
        });
});
$("#spoil-message").click(function (e) {
    $("#spoil-message").hide();
    $("#spoil-message-content").show();
});