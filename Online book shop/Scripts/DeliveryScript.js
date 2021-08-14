
$("#frm-delivery-details").validate({
    // Specify validation rules
    rules: {
        FirstName: "required",
        LastName: "required",
        DeliveryAddress: "required",
        ContactNumber: "required",
        DeliveryMethod: "required",
        PaymentMethod: "required",
        EmailAddress: {
            required: true,
            email: true
        }
    },
    // Specify validation error messages
    messages: {
        FirstName: "Please enter First Name",
        LastName: "Please enter Last Name",
        DeliveryAddress: "Please enter Delivery Address",
        ContactNumber: "Please enter Contact Number",
        DeliveryMethod: "Please select Delivery Method",
        PaymentMethod: "Please select Payment Method",
        EmailAddress: "Please enter a valid email address"
    },
    submitHandler: function (form) {
        form.submit();
    }
});
$("#frm-payment-details").validate({
    // Specify validation rules
    rules: {
        merchant_id: "required",
        return_url: "required",
        cancel_url: "required",
        notify_url: "required",
        order_id: "required",
        items: "required",
        currency: "required",
        amount: "required",
        first_name: "required",
        last_name: "required",
        address: "required",
        phone: "required",
        city: "required",
        country: "required",
        EmailAddress: {
            required: true,
            email: true
        }
    },
    // Specify validation error messages
    messages: {
        first_name: "Please enter First Name",
        last_name: "Please enter Last Name",
        address: "Please enter Delivery Address",
        phone: "Please enter Contact Number",
        city: "Please enter City",
        country: "Please select Country",
        EmailAddress: "Please enter a valid email address"
    },
    submitHandler: function (form) {
        form.submit();
    }
});
$(".deliver-country-list").change(function () {
    if (this.value != "Sri Lanka") {
        $("#delivery-courier-options").hide();
        $(".area-container").hide();
        $("#delivery-postal-options").prop("checked", true);
    } else {
        $("#delivery-courier-options").show();
        $(".area-container").show();
    }
});
//$('input[type=radio][name=DeliveryMethod]').change(function () {
//    if (this.value == 0) {
//        $('#postal_area').show();
//        $('#AreaId').hide();
//    }
//    else if (this.value == 1) {
//        $('#postal_area').hide();
//        $('#AreaId').show();
//    }
//});



$('input[type=radio][name=payment_methods]').change(function () {
    var selected_method = $('input[name="payment_methods"]:checked').val();
    $('#PaymentMethod').val(selected_method);
});
$('input[type=radio][name=delivery_method]').change(function () {
    var selected_method = $('input[name="delivery_method"]:checked').val();
    if (selected_method == "post") {
        $('#DeliveryMethod').val(0);
    } else if (selected_method == "courier") {
        $('#DeliveryMethod').val(1);
    }
    else if (selected_method == "in_store_pickup") {
        $('#DeliveryMethod').val(2);
    }
   
});
$("#frm-delivery-details").validate({
    rules: {
        BillingAddressId: "required",
        DeliveryAddressId: "required",
        DeliveryMethod: "required",
        country: "required",
        AreaId: "required",
        PaymentMethod: "required"
    },
    messages: {
       BillingAddressId: "required",
        DeliveryAddressId: "required",
        DeliveryMethod: "required",
        country: "required",
        AreaId: "required",
        PaymentMethod: "required"
    },
    submitHandler: function (form) {
        form.submit();
    }
});
$("#frm-add-new-address").validate({
    rules: {
        FirstName: "required",
        LastName: "required",
        AddressLine01: "required",
        ContactNumber1: "required",
        EmailAddress: "required",
        PaymentMethod: "required"
    },
    messages: {
        FirstName: "required",
        LastName: "required",
        AddressLine01: "required",
        ContactNumber1: "required",
        AreaEmailAddressId: "required",
        PaymentMethod: "required"
    },
    submitHandler: function (form) {
        form.submit();
    }
});

