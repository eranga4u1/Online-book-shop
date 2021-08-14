$(function () {

    $('#frm-add-new-address #Country').on('click', function (e) {
        if ($('#frm-add-new-address #Country').val() == "Sri Lanka") {
            $("#frm-add-new-address #shoping-cart-district-selection").show();
        } else {
            $("#frm-add-new-address #shoping-cart-district-selection").hide();
        }
    });
});


