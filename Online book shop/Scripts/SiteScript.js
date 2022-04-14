$(function () {
    $("#tabs").tabs();
    $("#open-edit-profile").click(function (e) {
        $("#edit-profile").show();
    });
    $('.page-number').on('click', function (e) {
        var url = $(this).attr("data-url");
        window.location.replace(url);
    });
});
jQuery(function ($) {
    $("#main_image, .xzoom-gallery").xzoom({
        position: 'right',
        Xoffset: 15
    });
    //$(".xzoom").xzoom({
    //    position: 'right',
    //    Xoffset: 15
    //});
});
$(".remove-from-whish-list").click(function (e) {
    var bookId = $(this).attr("data-bookid");
    var model = { "BookId": bookId };
    var url = '/UserReviews/NotLoveThisItem';
    $.post(url, model)
        .done(function (data) {
            location.replace("/Account/UserProfile?open=wish-list")
           // location.reload();
        })
        .fail(function (data) {
            console.log(data);
        });
});
$('.edit-user-name').click('click', function (e) {
    $('.edit-user-name').hide();
    $('.update-user-name').removeClass("hidden"); 
    $("#staticName").removeClass("form-control-plaintext ");
    $("#staticName").attr("readonly", false); 
});
$('.update-user-name').on('click', function () {
    $('.edit-user-name').show();
    $('.update-user-name').addClass("hidden");
    $("#staticName").addClass("form-control-plaintext ");
    $("#staticName").attr("readonly", true); 
    var model = { "NickName": $("#staticName").val() };
    var url = '/User/UpdateNickName';
    $.post(url, model)
        .done(function (data) {
           // location.reload();
        })
        .fail(function (data) {
           // console.log(data);
        });
});
$('.edit-user-email').click('click', function (e) {
    $('.edit-user-email').hide();
    $('.update-user-email').removeClass("hidden");
    $("#staticEmail").removeClass("form-control-plaintext ");
    $("#staticEmail").attr("readonly", false);
});
$('.update-user-email').on('click', function () {
    $('.edit-user-email').show();
    $('.update-user-email').addClass("hidden");
    $("#staticEmail").addClass("form-control-plaintext ");
    $("#staticEmail").attr("readonly", true);
    var model = { "Email": $("#staticEmail").val() };
    var url = '/User/UpdateEmail';
    $.post(url, model)
        .done(function (data) {
            // location.reload();
        })
        .fail(function (data) {
            // console.log(data);
        });
});
$('.edit-user-bday').click('click', function (e) {
    $('.edit-user-bday').hide();
    $('.update-user-bday').removeClass("hidden");
    $("#staticbday").removeClass("hidden");
    $('#lblstaticbday').addClass("hidden");
    $("#staticbday").removeClass("form-control-plaintext ");
    $("#staticbday").attr("readonly", false);
});
$('.update-user-bday').on('click', function () {
    $('.edit-user-bday').show();
    $('.update-user-bday').addClass("hidden");
    $("#staticbday").addClass("form-control-plaintext ");
    $("#staticbday").attr("readonly", true);
    $("#staticbday").addClass("hidden");
    $('#lblstaticbday').removeClass("hidden");
    $('#lblstaticbday').text($("#staticbday").val());

    var model = { "Birthday": $("#staticbday").val() };
    var url = '/User/UpdateBirthday';
    $.post(url, model)
        .done(function (data) {
            // location.reload();
        })
        .fail(function (data) {
            // console.log(data);
        });
});

$('.remove-address').on('click', function () {
    var Id = $(this).attr("data-id");
    var model = { "Id": Id };
    var url = '/User/RemoveAddress';
    $.post(url, model)
        .done(function (data) {
             location.reload();
        })
        .fail(function (data) {
            location.reload();
            // console.log(data);
        });
});
$('.set-default').on('click', function (e) {
    var Id = $(this).attr("data-id");
    var model = { "Id": Id };
    var url = '/User/SetDefault';
    $('.set-default').removeClass("bg-success");
    $('.set-default').addClass("bg-info");
    $('#set-default-' + Id).removeClass("bg-info");
    $('#set-default-' + Id).addClass("bg-success");
    $.post(url, model)
        .done(function (data) {
            
        })
        .fail(function (data) {
            // console.log(data);
        });
});

$('.btn-add-new-address').on("click", function () {
    var type = $(this).attr("data-type");

    $('.page-content').hide();
    $('.pop-up-content').show();
    $('.btn-update-address').attr('data-type', $(this).attr("data-type"));
});
$('.close-pop-up').on('click', function () {
    $('.page-content').show();
    $('.pop-up-content').hide();
});

$('.btn-update-address').on('click', function (e) {
    var type = $(this).attr("data-type");
    if ($('#frm-address').valid()) {
        var url = "/Address/Add";
        var id = $("#addressId").val();
        var isPublic = true;
        var RefId = $('#ref').val();
        var FirstName = $('#FirstName').val();
        var LastName = $('#LastName').val();
        var Company = $('#Company').val();
        var AddressLine01 = $('#AddressLine01').val();
        var AddressLine02 = $('#AddressLine02').val();
        var AddressLine03 = $('#AddressLine03').val();
        var ContactNumber1 = $('#ContactNumber1').val();
        var ContactNumber2 = $('#ContactNumber2').val();
        var EmailAddress = $('#EmailAddress').val();
        var PostalCode = $('#PostalCode').val();
        var Country = $('#Country').val();
        var City = $('#City').val();
        var District = $('#District').val();//$('#shoping-cart-district-selection').val();
        
        var State = $('#State').val();
        if ($('#Country').val() != "Sri Lanka") {
            District = City ;
        } 


        var model = {
            "Id": id,
            "isPublic": isPublic,
            "RefId": RefId,
            "FirstName": FirstName,
            "LastName": LastName,
            "Company": Company,
            "AddressLine01": AddressLine01,
            "AddressLine02": AddressLine02,
            "AddressLine03": AddressLine03,
            "ContactNumber1": ContactNumber1,
            "ContactNumber2": ContactNumber2,
            "EmailAddress": EmailAddress,
            "City": City,
            "PostalCode": PostalCode,
            "Country": Country,
            "District": District,
            "State": State
        };
        $.post(url, model)
            .done(function (data) {
                if (type == "user-profile-new" || type == "user-profile-edit") {
                    window.location.replace("/Account/UserProfile?open=address&selected=" + data.Id);
                } else if (type == "delivery") {
                    window.location.replace("/delivery/?open=delivery_address&delivery=" + data.Id + "&billing=" + $('#billing_address_selected_address').val());
                } else if (type == "billing") {
                    window.location.replace("/delivery/?open=billing_address&billing=" + data.Id + "&delivery=" + $('#delivery_address_selected_address').val());
                }
                
            })
            .fail(function (data) {
                //console.log(data);
            });
    }
    else {
        //alert("InValid");
    }
});
$('#Country').on("change", function (e) {
    if ($('#Country').val() != "Sri Lanka") {
        $('#District').hide();
        $('#lbl-district').hide();
    } else {
        $('#District').show();
        $('#lbl-district').show();
    }
});
$("#frm-address").validate({
    rules: {
        FirstName: "required",
        LastName: "required",
        ContactNumber1: { isValidPhoneNumber: true, minlength: 10, maxlength: 15 },
        AddressLine01: "required",
        EmailAddress: {
            required: true,
            email: true
        },
        City: "required",
        District: { valueNotEquals: "not selected" },
        Country: { valueNotEquals: "not selected" }
    },
    messages: {
        FirstName: "Please specify your first name",
        LastName: "Please specify your last name",
        ContactNumber1: "Please add your contact number",
        AddressLine01: "Please add your address",
        EmailAddress: "Please add your email address",
        City: "Please add your nearest city/state",
        District: "Please select your district",
        Country: "Please select your district"
    }
});
$.validator.addMethod("valueNotEquals", function (value, element, arg) {
    return arg !== value;
}, "Value must not equal arg.");

$('#add_new_address').on('click', function (e) {
   
    var type = $(this).attr("data-type");
    if ($('#frm-add-new-address').valid()) {
        var url = "/Address/Add";
        var id = $("#addressId").val();
        var isPublic = true;
        var RefId = $('#ref').val();
        var FirstName = $('#FirstName').val();
        var LastName = $('#LastName').val();
        var Company = $('#Company').val();
        var AddressLine01 = $('#AddressLine01').val();
        var AddressLine02 = $('#AddressLine02').val();
        var AddressLine03 = $('#AddressLine03').val();
        var ContactNumber1 = $('#ContactNumber1').val();
        var ContactNumber2 = $('#ContactNumber2').val();
        var EmailAddress = $('#EmailAddress').val();
        var PostalCode = $('#PostalCode').val();
        var Country = $('#Country').val();
        var City = $('#City').val();
        var District = "";//$('#shoping-cart-district-selection').val();
        if ($('#frm-add-new-address #Country').val() == "Sri Lanka") {
            District = $('#shoping-cart-district-selection').val();
        } else {
            District = $('#Country').val();
        }
        var State = $('#State').val();


        var model = {
            "Id": id,
            "isPublic": isPublic,
            "RefId": RefId,
            "FirstName": FirstName,
            "LastName": LastName,
            "Company": Company,
            "AddressLine01": AddressLine01,
            "AddressLine02": AddressLine02,
            "AddressLine03": AddressLine03,
            "ContactNumber1": ContactNumber1,
            "ContactNumber2": ContactNumber2,
            "EmailAddress": EmailAddress,
            "City": City,
            "PostalCode": PostalCode,
            "Country": Country,
            "District": District,
            "State": State
        };
        $.post(url, model)
            .done(function (data) {
                if (type == "user-profile-new" || type == "user-profile-edit") {
                    window.opener.location.replace("/Account/UserProfile?open=address&selected=" + data.Id);
                } else if (type == "delivery") {
                    window.opener.location.replace("/delivery/?open=delivery_address&selected=" + data.Id);
                } else if (type == "billing") {
                    window.opener.location.replace("/delivery/?open=billing_address&selected=" + data.Id);
                }
                
                //window.opener.$(".close-modal ").trigger("click");
                //window.opener.$("billing_address_selected_address")
                //window.opener.$(".address_selection").append(new Option(data.FirstName + " " + data.LastName + " (" + data.AddressLine01 + ", " + data.AddressLine02 + ", " + data.AddressLine03 + ", " + data.City + " )", data.Id));
                //if (locationId == "billing") {
                //    window.opener.$('#billing_address_selected_address').val(data.Id);
                //} else if (locationId == "delivery") {
                //    window.opener.$('#delivery_address_selected_address').val(data.Id);
                //}
                window.close();
            })
            .fail(function (data) {
                //console.log(data);
            });
    }
});



$('.edit-address').on("click", function () {

    $('.page-content').hide();
    $('.pop-up-content').show();
    
    //var locationId = $(this).attr("data-id");
    //var url = "/address/Edit/" + locationId;
    //var height = 800;
    //var width = 700;
    //var left = (window.screen.width / 2) - ((width / 2) + 10);
    //var top = (window.screen.height / 2) - ((height / 2) + 50);

    //EditWindow = window.open(url, '',
    //    "status=no,height=" + height + ",width=" + width + ",resizable=no,left="
    //    + left + ",top=" + top + ",screenX=" + left + ",screenY="
    //    + top + ",toolbar=no,menubar=no,scrollbars=no,location=no,directories=no");

});

$('.up-delivery-status-dd').click("click", function (e) {
    var order_id = $(this).attr("data-order-id");
    var url = '/Delivery/ChangeDeliveryStatus';
    if (confirm("Do you really want to change delivery status?")) {
            var model = { "OrderId": order_id };
            console.log(model);
            $.post(url, model)
                .done(function (data) {
                    // window.opener.location.replace("/Account/UserProfile?open=address&selected=" + data.Id);
                    //location.reload();
                })
                .fail(function (data) {
                    //console.log(data);
                });
      
    } else {
        $(this).prop('checked', false);
    }

  
});

$(".delivery-form-delivery_address").change(function () {
    location.replace("/delivery/?open=delivery_address&delivery=" + $('#delivery_address_selected_address').val() + "&billing=" + $('#billing_address_selected_address').val());
});
//$(".up-bookcat-col").on("click", function (e) {
//    $(".up-bookcat-col").removeClass("active");
//    $(this).addClass("active");
//});
$('.up-acordian-tab').on('click', function (e) {

    if (!$(this).closest("div").hasClass("active")) {
        var sections = $(".up-acordian-tab");
        $.each(sections, function (index, value) {
            var id = $(value)[0].id;
            if ($("#" + id).closest("div").hasClass("active")) {
                $("#" + id).closest("div").removeClass('active');
            }
            // console.log($(value)[0].id);
        });
    } 
});
$('input[type=radio][name=delivery_method]').change(function () {
    if (this.value == 'post') {
        $("#cod-section").hide();
        $("#isp-section").hide();
    }
    else if (this.value == 'courier') {
        $("#cod-section").show();
        $("#isp-section").hide();
    }
    else if (this.value == 'in_store_pickup') {
        $("#cod-section").hide();
        $("#isp-section").show();
    }
});
$('#flexCheckAgreed').change(function() {
    if (this.checked) {
        $('.po-confirm-btn').prop("disabled", false);       

    } else {
        $('.po-confirm-btn').prop("disabled", true);     
    }       
});
$("#frm-payment-details").submit(function (event) {
    if (!$('#flexCheckAgreed').checked) {
        event.preventDefault();
    }     
});
$("#co-confirm-btn").on("click", function () {
    location.replace($("#co-confirm-btn").attr("data-href"));
});
$.validator.addMethod('isValidDistrict', function () {
    if ($("#frm-add-new-address").find("#Country").val() == "Sri Lanka") {
        if ($("#frm-add-new-address").find("#shoping-cart-district-selection").val() == "All") {
            return false;
        } else {
            return true;
        }
    }
    return true;
}, '');

$.validator.addMethod('isValidPhoneNumber', function () {
    var contactnumber = $('#ContactNumber1').val();
    if (contactnumber.trim() == ""  ) {
        return false;
    } else {
        var re = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;
        return re.test(contactnumber.trim());
    }
  
}, '');

$("#frm-add-new-address").validate({
    rules: {
        FirstName: "required",
        AddressLine01: "required",
        LastName: "required",
        ContactNumber1: { isValidPhoneNumber: true },
        EmailAddress: {
            required: true,
            email: true
        },
        City: "required",
        Country: "required",
        District: {
            isValidDistrict: true
        }
    },
    messages: {
        FirstName: "Please enter first name",
        AddressLine01: "Please enter Address",
        LastName: "Please enter last name",
        ContactNumber1: "Please enter contact number",
        EmailAddress: "Please enter email address",
        City: "Please enter nearest city",
        Country: "Please select country"
    },
    submitHandler: function (form) {
        form.submit();
    }
});

$(function () {
    $('#login_email').on('keypress', function (e) {
        if (e.which == 32) {
            console.log('Space Detected');
            return false;
        }
        $('.validation-summary-errors').hide();
    });
});

$('.lightbox-target').on('click', function (e) {
    //location.replace(window.location.pathname);
});

//
if ($(".container").hasClass("pg-bootstrap")) {
    //$('head').append('<link rel="stylesheet" href="/Content/bootstrap.min.css" type="text/css" />');
    $('.menu').addClass('row');       
}

$("#frm-payment-details").submit(function (event) {
    var url = '/Delivery/PartiallyConfirmedOrder';
    var order_id = $('#cp-confirm-btn').attr("data-order-id");
    var model = { "OrderId": order_id, "StatusId": status };
    console.log(model);
    $.post(url, model)
        .done(function (data) {
        })
        .fail(function (data) {
        });
});

$(document).ready(function () {
    $('.alert-modal').show();
   // modal.style.display = "block";
});

$('.alert-close').on('click', function () {
    $('.alert-modal').hide();
});
