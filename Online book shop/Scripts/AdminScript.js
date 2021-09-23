$(document).ready(function () {
    $(".btn-b-prop-1").click(function (e) {
        e.preventDefault();
        $('.b-prop-2').show();
        $(".b-prop-2").focus();
        $('#isAvailable_2').val(1);
       
    });
    $(".btn-b-prop-2").click(function (e) {
        e.preventDefault();
        $('.b-prop-3').show();
        $(".b-prop-3").focus();
        $('#isAvailable_3').val(1);
        
    });
    $(".btn-b-prop-3").click(function (e) {
        e.preventDefault();
        $('.b-prop-4').show();
        $(".b-prop-4").focus();
        $('#isAvailable_4').val(1);
    });
    $(".btn-b-prop-4").click(function (e) {
        e.preventDefault();
        $('.b-prop-5').show();
        $(".b-prop-5").focus();
        $('#isAvailable_5').val(1);
    });
    $("#searchInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $(".author-card").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});

$('.category-check-box').change(function () {
    var selectedCategories = $('input[class=category-check-box]:checked');
    var vals = [];
    $.each(selectedCategories, function (key, value) {
        vals.push($(value).val());
    });
 
    if (vals != null && vals.length > 0) {
        $('#Categories').val(vals.join());
    } else {
        $('#Categories').val("");
    }
});
$('.checkmark').click(function () {
    var selectedCategories = $('input[class=category-check-box]:checked');
    var vals = [];
    $.each(selectedCategories, function (key, value) {
        vals.push($(value).val());
    });

    if (vals != null && vals.length > 0) {
        $('#Categories').val(vals.join());
    } else {
        $('#Categories').val("");
    }
});
$('.refresh-publishers').click(function () {
    var selected_publisher = $('#PublisherId').val();
    var url = '/Admin/Publisher/GetPublishers';
    //var model = { "BookId": bookId, "BookPropertyId": bookPropId, "NumberOfItems": numberOfItems }
    $.post(url)
        .done(function (data) {
            $.each(JSON.parse(data), function (key, value) {
                console.log(value);
                if ($("#PublisherId option[value='" + value.Id + "']").length < 1) {
                    $("#PublisherId").append("<option value=\"" + value.Id + "\">" + value.Name + "</option>");
                }
            });
        })
        .fail(function (data) {
            //console.log(data);
        });
});
$('.refresh-authors').click(function () {
    var selected_authors = $('#AuthorId').val();
    var url = '/Admin/Author/GetAuthors';
    //var model = { "BookId": bookId, "BookPropertyId": bookPropId, "NumberOfItems": numberOfItems }
    $.post(url)
        .done(function (data) {
            $.each(JSON.parse(data), function (key, value) {
                console.log(value);
                if ($("#AuthorId option[value='" + value.Id + "']").length < 1) {
                    $("#AuthorId").append("<option value=\"" + value.Id + "\">" + value.Name + "</option>");
                }
            });
        })
        .fail(function (data) {
            //console.log(data);
        });
});
// Wait for the DOM to be ready
$(function () {
    // Initialize form validation on the registration form.
    // It has the name attribute "registration"
    $("#form-add-category").validate({
        // Specify validation rules
        rules: {
            // The key name on the left side is the name attribute
            // of an input field. Validation rules are defined
            // on the right side
            CategoryName: "required",
            CategoryDescription: "required",
            //email: {
            //    required: true,
            //    // Specify that email should be validated
            //    // by the built-in "email" rule
            //    email: true
            //},
            //password: {
            //    required: true,
            //    minlength: 5
            //}
        },
        // Specify validation error messages
        messages: {
            CategoryName: "Please enter Category Name",
            CategoryDescription: "Please enter Category Description",
            //password: {
            //    required: "Please provide a password",
            //    minlength: "Your password must be at least 5 characters long"
            //},
            //email: "Please enter a valid email address"
        },
        // Make sure the form is submitted to the destination defined
        // in the "action" attribute of the form when valid
        submitHandler: function (form) {
            form.submit();
        }
    });
    $("#form-add-author").validate({
        rules: {
            Name: "required",
            //Description: "required",
            //Email: {
            //    required: true,
            //    email: true
            //}
        },
        messages: {
            Name: "Please enter Name",
            //Description: "Please enter Description",
            //email: "Please enter a valid email address"
        },
        submitHandler: function (form) {
            form.submit();
        }
    });
    $("#form-add-book").validate({
        rules: {
            Title: "required",
            ISBN: "required",
            Description: "required",
            LanguageId: "required",
            SaleType : "required"
        },
        messages: {
            Title: "Please enter Title",
            ISBN : "Please enter ISBN",
            Description: "Please enter Description",
            LanguageId: "Please select  language",
            SaleType: "Please select  Sale type"
        },
        submitHandler: function (form) {
            form.submit();
        }
    });
    $("#form-add-publisher").validate({
        rules: {
            Name: "required",
            //Description: "required",
            Email: {
                //required: true,
                email: true
            }

        },
        messages: {
            Name: "Please enter Name",
            Description: "Please enter Description",
            email: "Please enter a valid email address"
        },
        submitHandler: function (form) {
            form.submit();
        }
    });
    $("#form-add-article").validate({
        rules: {
            Title: "required",
            Summary: "required",
            Content: "required",
            WrittenBy: "required"
        },
        messages: {
            Title: "Please enter Name",
            Summary: "Please enter Summary",
            Content: "Please enter content",
            WrittenBy: "Please enter author name"
        },
        submitHandler: function (form) {
            form.submit();
        }
    });
    $("#form-add-content-page").validate({
        rules: {
            Title: "required",
            URLSubString: "required",
            HTMLContent: "required"
        },
        messages: {
            Title: "Please enter Name",
            URLSubString: "Please enter Summary",
            HTMLContent: "Please enter content"
        },
        submitHandler: function (form) {
            form.submit();
        }
    });
    $("#form-add-promotion").validate({
        rules: {
            PromotionTitle: "required",
            PromotionDescription: "required",
            DiscountValue: "required",
            StartDate: "required",
            EndDate: "required"
        },
        messages: {
            PromotionTitle: "Please enter Title",
            PromotionDescription: "Please enter Summary",
            DiscountValue: "Please enter Discount Value",
            StartDate: "Please enter Start Date",
            EndDate: "Please enter End Date"
        },
        submitHandler: function (form) {
            form.submit();
        }
    });
    $("#frm-add-nav-root-menu").validate({
        rules: {
            Title: "required",
            LocalTitle: "required",
            Url: "required"
        },
        messages: {
            Title: "Please enter Title",
            LocalTitle: "Please enter Local Title",
            Url: "Please enter Url"
        },
        submitHandler: function (form) {
            form.submit();
        }
    });
    $("#frm-add-nav-sub-root-menu").validate({
        rules: {
            Title: "required",
            LocalTitle: "required",
            Url: "required",
            Parent: "required"
        },
        messages: {
            Title: "Please enter Title",
            LocalTitle: "Please enter Local Title",
            Url: "Please enter Url",
            Parent: "Please select parent menu"
        },
        submitHandler: function (form) {
            form.submit();
        }
    });

    $("#frm-add-nav-footer-menu").validate({
        rules: {
            Title: "required",
            LocalTitle: "required",
            Url: "required"
        },
        messages: {
            Title: "Please enter Title",
            LocalTitle: "Please enter Local Title",
            Url: "Please enter Url"
        },
        submitHandler: function (form) {
            form.submit();
        }
    });
    $("#frm-add-nav-sub-footer-menu").validate({
        rules: {
            Title: "required",
            LocalTitle: "required",
            Url: "required",
            Parent: "required"
        },
        messages: {
            Title: "Please enter Title",
            LocalTitle: "Please enter Local Title",
            Url: "Please enter Url",
            Parent: "Please select parent menu"
        },
        submitHandler: function (form) {
            form.submit();
        }
    });
});

function readURL(input, setfor) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#' + setfor).attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]); // convert to base64 string
    }
};

$(".author_prof").change(function () {
    readURL(this,"temp_img_author_prof");
});
$(".author_cover").change(function () {
    readURL(this, "temp_img_author_cover");
});
$(".bk_front_img").change(function () {
    readURL(this, "temp_img_bk_front_img");
});
$(".bk_back_img").change(function () {
    readURL(this, "temp_img_bk_back_img");
});
$(".Banner_1").change(function () {
    readURL(this, "temp_img_Banner_1");
});
$(".Banner_2").change(function () {
    readURL(this, "temp_img_Banner_2");
});
$(".Banner_3").change(function () {
    readURL(this, "temp_img_Banner_3");
});
$(".Banner_3").change(function () {
    readURL(this, "temp_img_Banner_3");
});

$(".delivery-status-dd").change(function () {
    var order_id = $(this).attr("data-order-id");
    var dom = this;
    var url = '/Admin/Order/ChangeDeliveryStatus';
    var status = $(this).val();
    if (status == 4) {
        var dialog = bootbox.prompt({
            title: "Please add Tracking Id",
            centerVertical: true,
            callback: function (result) {
                if (result == null || result == '') {
                    result = "XXXXXX";
                }
                //if (result == null || result == '') {
                //    var alert = bootbox.alert("Invalid Tracking ID");
                //    alert.modal('hide');
                //    dialog.modal('hide');
                //    location.reload(); 
                //} else {
                    var model = { "OrderId": order_id, "StatusId": status, "TrackingId": result };
                    $.post(url, model)
                        .done(function (data) {
                            location.reload();
                        })
                        .fail(function (data) {
                            //console.log(data);
                        });
                //}
            }
        });
    } else {
        bootbox.confirm({
            message: "Do you really want to change delivery status?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                console.log(result);
                if (result) {

                    var model = { "OrderId": order_id, "StatusId": status };
                    console.log(model);
                    $.post(url, model)
                        .done(function (data) {
                            location.reload();
                        })
                        .fail(function (data) {
                            //console.log(data);
                        });
                } else {
                    $(dom).prop('checked', false); 
                }

            }
        });
    }
    
});
//$("#DeliveryStatus_3").change(function () {
//    var order_id = $(this).attr("data-order-id");
//    var url = '/Admin/Order/ChangeDeliveryStatus';
//    var status = $(this).val();
//    var dialog = bootbox.prompt({
//        title: "Please add Tracking Id",
//        centerVertical: true,
//        callback: function (result) {
//            if (result === '') {
//                var alert = bootbox.alert("Invalid Tracking ID");
//                alert.modal('hide');
//                dialog.modal('hide');
//            } else {
//                var model = { "OrderId": order_id, "StatusId": status, "TrackingId": result };
//                console.log(model);
//                $.post(url, model)
//                    .done(function (data) {
//                        location.reload();
//                    })
//                    .fail(function (data) {
//                        //console.log(data);
//                    });
//            }
//        }
//    });
//});
$("#StartDate").datepicker({ dateFormat: 'yy/mm/dd' }); 
$("#SaleStartDate").datepicker({ dateFormat: 'yy/mm/dd' }); 
$("#SaleEndDate").datepicker({ dateFormat: 'yy/mm/dd' }); 
$("#EndDate").datepicker({ dateFormat: 'yy/mm/dd' });
$("#PreReleaseEndDate").datepicker({ dateFormat: 'yy/mm/dd' });

$("#form-add-promotion #ObjectType").change(function () {
    var val = $("#form-add-promotion #ObjectType").val();
    $('.obj_select').hide();
    $('.obj_' + val).show();
    $("#form-add-promotion #ObjectId").val($('.obj_' + val+" option:first").val());
});
$("#form-add-promotion #ObjectId").change(function () {
    var otherProperty = $(this).find(':selected').attr('data-otherPara');
    $("#form-add-promotion #OtherParameters").val(otherProperty);
});
$("#form-add-promotion #ObjectId").change(function () {
    var otherProperty = $(this).find(':selected').attr('data-otherPara');
    $("#form-add-promotion #OtherParameters").val(otherProperty);
});

//$("#OrderfilteringStatus").change(function () {
//    var selected = $("#OrderfilteringStatus").val();
//    var startDate = $("#start-date").val();
//    var endDate = $("#end-date").val();
//    if (selected == -1) {
//        location.replace("/Admin/Order/GetAll")
//    } else {
//        location.replace("/Admin/Order/GetAll?deliveryStatus=" + selected + "&OrderType=" + $("#OrderfilteringType").val() + "&StartDate=" + startDate + "&EndDate=" + endDate);
//    }
//});
//$("#OrderfilteringType").change(function () {
//    var selected = $("#OrderfilteringStatus").val();
//    var startDate = $("#start-date").val();
//    var endDate = $("#end-date").val();
//    if (selected == -1) {
//        location.replace("/Admin/Order/GetAll")
//    } else {
//        location.replace("/Admin/Order/GetAll?deliveryStatus=" + selected + "&OrderType=" + $("#OrderfilteringType").val() + "&StartDate=" + startDate + "&EndDate=" + endDate);
//    }
//});
$("#filter-orders").click(function () {
    var selected = $("#OrderfilteringStatus").val();
    var startDate = $("#start-date").val();
    var endDate = $("#end-date").val();
    location.replace("/Admin/Order/GetAll?deliveryStatus=" + selected + "&OrderType=" + $("#OrderfilteringType").val() + "&StartDate=" + startDate + "&EndDate=" + endDate);
});
$('.add-more-author').click(function () {
    var content = $('.main-author-block').html();   
    $(".author-block").append("<div class='select mo'>" + content.replaceAll("AuthorId", "OtherAthors[" + $(".mo").length+"]") +"</div>");
});
$('.selected-author-drop').on('change', function (e) {
    var authorsselection = $('.selected-author-drop');
    $('.obj_select').hide();
    $.each(authorsselection, function (key, value) {
        //alert(key + ": " + value);
        var id = $(value).val();
        $('.bk_author_' + id).show();
    });
});

$('.add-new-bookpackrow').on('click', function (e) {

    if ($('#select-book-for-book-pack').val() != 0) {
        var selectedBook = $('#select-book-for-book-pack').val();
        var bkPropertyDetail = $('#select-book-for-book-pack').find(":selected").attr("data-para")
        var bkdisplayName = $('#select-book-for-book-pack').find(":selected").html();
        var selectedAmount = $('#numberOfBooksForBookPacks').val();
        var current_selection = $('#selected-books').val();
        
        var bookProperty = JSON.parse(bkPropertyDetail);
        var Obj = selectedBook + '-' + bookProperty.BookPropertyId;
        if (current_selection === '') {
            $('#selected-books').val(Obj);
        } else {
            $('#selected-books').val(current_selection + ',' + Obj);
        }
        

        $("#book-pack-menu").append('<li data-amount="' + selectedAmount + '" data-obj="' + Obj + '" class="list-group-item">' + bkdisplayName + '<span data-obj="' +Obj+'" class="text-danger remove-selected-book-pack-item"  style="margin-left: 30px;cursor: pointer;">Remove</span></li>');
        $('#select-book-for-book-pack').find(":selected").hide();
        $('#select-book-for-book-pack').val(0);
        $('#numberOfBooksForBookPacks').val(0);
    }   
    event.preventDefault();
});

$('.remove-selected-book-pack-item').on('click', function (e) {
    var Obj = $(this).attr("data-obj");
    if (Obj) {

    }
})

$('.update-contact-config').on('click',function(e){
    var Objtype = $(this).attr("data-type");
    if(Objtype == "contact_numbers"){
        var url = "/Admin/Configuration/AddContactNumber";
        var model = { "NumberValue": $('#contact_numbers_value').val(), "ShowsOnContactPage": $('#contact_numbers_show_in_contact').prop("checked"), "ShowsOnHomePage": $('#contact_numbers_show_in_home').prop("checked"), "TitleForNumber": $('#contact_numbers_title').val()};
    } else if (Objtype == "contact_emails") {
        var url = "/Admin/Configuration/AddEmailAddress";
        var model = { "EmailValue": $('#contact_emails_value').val(), "ShowsOnContactPage": $('#contact_emails_show_in_contact').prop("checked"), "ShowsOnHomePage": $('#contact_emails_show_in_home').prop("checked"), "TitleForEmail": $('#contact_emails_title').val()};
    } else if (Objtype == "contact_address") {
        var url = "/Admin/Configuration/AddContactAddress";
        var model = { "AddressValue": $('#contact_address_value').val(), "ShowsOnContactPage": $('#contact_address_show_in_contact').prop("checked"), "ShowsOnHomePage": $('#contact_address_show_in_home').prop("checked"), "TitleForAddress": $('#contact_address_title').val()};
    }
    $.post(url, model)
        .done(function (data) {
            location.reload();
            //if (Objtype == "contact_numbers") {

            //} else if (Objtype == "contact_emails") {

            //} else if (Objtype == "contact_address") {

            //}
        })
        .fail(function (data) {
            console.log(data);
        });
});
$('.book_property-config').on('click', function (e) {
    if ($('#book_property_title')) {
        var url = "/Admin/Configuration/AddBookPropertyType?title=" + $('#book_property_title').val();
        $.get(url)
            .done(function (data) {
                location.reload("/Admin/Configuration/Localization#property_types");
            })
            .fail(function (data) {
                console.log(data);
            });
    }
});

$('.rmv-contact-config').on("click", function (e) {
    var Objtype = $(this).attr("data-type");
    var value = $(this).attr("data-id");
    if (Objtype == "contact_numbers") {
        var url = "/Admin/Configuration/RemoveContactNumber";
        var model = { "NumberValue": value};
    } else if (Objtype == "contact_emails") {
        var url = "/Admin/Configuration/RemoveEmailAddress";
        var model = { "EmailValue": value};
    } else if (Objtype == "contact_address") {
        var url = "/Admin/Configuration/RemoveContactAddress";
        var model = { "AddressValue": value};
    }
    $.post(url, model)
        .done(function (data) {
            location.reload();
            //if (Objtype == "contact_numbers") {

            //} else if (Objtype == "contact_emails") {

            //} else if (Objtype == "contact_address") {

            //}
        })
        .fail(function (data) {
            console.log(data);
        });
});

$('.update-payment-method').on('click', function (e) {
    var enumId = $(this).attr("data-id");
    var title = $('#payment-method-title-' + enumId).val();
    var message = $('#payment-method-message-' + enumId).val();
    var isEnable = $('#payment-method-enable-' + enumId).prop("checked");
    var model = { "EnumId": enumId, "Title": title, "Message": message, "isEnable": isEnable };
    var url = "/Admin/Configuration/UpdatePaymentMethod";
    $.post(url, model)
        .done(function (data) {
            location.reload();
        })
        .fail(function (data) {
            console.log(data);
        });
});
$('.update-srilankapostalCharges').on('click', function (e) {
    var startweight = $('#srilankapostalCharges_StartWeightByGrams').val();
    var endweight = $('#srilankapostalCharges_EndWeightByGrams').val();
    var district = $('#srilankapostalCharges_district').val();
    var amount = $('#srilankapostalCharges_in_amount').val();
    var slicetyp = false;
    if ($('#srilankapostalCharges_in_sliceType').val() == 1) { slicetyp = true; };
    var slicegram = $('#srilankapostalCharges_slicebygram').val();
    var sliceprice = $('#srilankapostalCharges_unitpriceperslice').val();
    var model = {
        "DeliveryType": 0,
        "StartWeightByGrams": startweight,
        "EndWeightByGrams": endweight,
        "Amount": amount,
        "Area": district,
        "Country": "Sri Lanka",
        "SliceByGrams": slicegram,
        "UnitPricePerSlice": sliceprice,
        "isDynamic": slicetyp
    };
    var url = "/Admin/Configuration/UpdateSriLankaPostalCharges";
    $.post(url, model)
        .done(function (data) {
            location.reload();
        })
        .fail(function (data) {
            console.log(data);
        });
});

$('.update-srilankaCourierCharges').on('click', function (e) {
    var startweight = $('#srilankaCourierCharges_StartWeightByGrams').val();
    var endweight = $('#srilankaCourierCharges_EndWeightByGrams').val();
    var district = $('#srilankaCourierCharges_district').val();
    var amount = $('#srilankaCourierCharges_in_amount').val();
    var slicetyp = false;
    if ($('#srilankaCourierCharges_in_sliceType').val() == 1) { slicetyp = true; };
    var slicegram = $('#srilankaCourierCharges_slicebygram').val();
    var sliceprice = $('#srilankaCourierCharges_unitpriceperslice').val();
    var model = {
        "DeliveryType": 1,
        "StartWeightByGrams": startweight,
        "EndWeightByGrams": endweight,
        "Amount": amount,
        "Area": district,
        "Country": "Sri Lanka",
        "SliceByGrams": slicegram,
        "UnitPricePerSlice": sliceprice,
        "isDynamic": slicetyp
    };
    var url = "/Admin/Configuration/UpdateSriLankaPostalCharges";
    $.post(url, model)
        .done(function (data) {
            location.reload();
        })
        .fail(function (data) {
            console.log(data);
        });
});
$('.update-foreignAirmailCharges').on('click', function (e) {
    var startweight = $('#foreignAirmailCharges_StartWeightByGrams').val();
    var endweight = $('#foreignAirmailCharges_EndWeightByGrams').val();
    var country = $('#foreignAirmailCharges_country').val();
    var amount = $('#foreignAirmailCharges_in_amount').val();
    var slicetyp = false;
    if ($('#foreignAirmailCharges_in_sliceType').val() == 1) { slicetyp = true; };
    var slicegram = $('#foreignAirmailCharges_slicebygram').val();
    var sliceprice = $('#foreignAirmailCharges_unitpriceperslice').val();
    var model = {
        "DeliveryType": 2,
        "StartWeightByGrams": startweight,
        "EndWeightByGrams": endweight,
        "Amount": amount,
        "Area": 'All',
        "Country": country,
        "SliceByGrams": slicegram,
        "UnitPricePerSlice": sliceprice,
        "isDynamic": slicetyp
    };
    var url = "/Admin/Configuration/UpdateSriLankaPostalCharges";
    $.post(url, model)
        .done(function (data) {
            location.reload();
        })
        .fail(function (data) {
            console.log(data);
        });
});
$('.update-emsCharges').on('click', function (e) {
    var startweight = $('#emsCharges_StartWeightByGrams').val();
    var endweight = $('#emsCharges_EndWeightByGrams').val();
    var country = $('#emsCharges_country').val();
    var amount = $('#emsCharges_in_amount').val();
    var slicetyp = false;
    if ($('#emsCharges_in_sliceType').val() == 1) { slicetyp = true; };
    var slicegram = $('#emsCharges_slicebygram').val();
    var sliceprice = $('#emsCharges_unitpriceperslice').val();
    var model = {
        "DeliveryType": 3,
        "StartWeightByGrams": startweight,
        "EndWeightByGrams": endweight,
        "Amount": amount,
        "Area": 'All',
        "Country": country,
        "SliceByGrams": slicegram,
        "UnitPricePerSlice": sliceprice,
        "isDynamic": slicetyp
    };
    var url = "/Admin/Configuration/UpdateSriLankaPostalCharges";
    $.post(url, model)
        .done(function (data) {
            location.reload();
        })
        .fail(function (data) {
            console.log(data);
        });
});
$('.rmv-delivery-charge-config').on('click', function (e) {
    var optionId = $(this).attr("data-id");
    var model = {
        "Id": optionId
    };
    var url = "/Admin/Configuration/RemoveDeliveryChargesCharges";
    $.post(url, model)
        .done(function (data) {
            location.reload();
        })
        .fail(function (data) {
            console.log(data);
        });

});
$('.rmv-media').on('click', function (e) {
    var mediaId = $(this).attr("data-item-media-id");
    var bookId = $(this).attr("data-book-id");
    var itemType = $(this).attr("data-item-type");
    if (itemType == "book") {
        var model = {
            "Id": mediaId,
            "ObjectId": bookId,
            "ObjectType": 0
        };
    }
    var url = "/Admin/Book/RemoveMediaItem";
    $.post(url, model)
        .done(function (data) {
            location.reload();
        })
        .fail(function (data) {
            console.log(data);
        });
    
});

$('#btn-update-instant-message').click("click", function (e) {
    var model = {
        "Message": CKEDITOR.instances['update-instant-message'].getData(),//$("#cke_update-instant-message").val(),
        "BackgroundColor": $("#bg-instant-message").val(),
        "FontColor": $("#fc-instant-message").val()
    };
    var url = "/Admin/Configuration/UpdateInstantMessage";
    $.post(url, model)
        .done(function (data) {
            location.reload();
        })
        .fail(function (data) {
            console.log(data);
        });
});
$('.remove-multiple-author').on('click', function (e) {
    var authorId = $(this).attr("data-author-id");
    var bookId = $(this).attr("data-book-id");
    var model = {
        "BookId": bookId,
        "AuthorId": authorId
    };
    var url = "/Admin/book/RemoveMultipleAuthor";
    $.post(url, model)
        .done(function (data) {
            location.reload();
        })
        .fail(function (data) {
            location.reload();
        });

});

$('#bulkChangeStatus').on('change', function (e) {
    bootbox.confirm({
        message: "Are you sure you want change selected items status?",
        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                if ($('#bulkChangeStatus').val() != "-1") {
                    var selected = $('.bulk-edit-d-status:checkbox:checked');
                    var statusId = $('#bulkChangeStatus').val();
                    var arr = [];
                    $.each(selected, function (index, value) {
                        var id = $(value).attr('data-order-id');
                        var obj = {
                            "Id": id,
                            "DeliveryStatus": statusId
                        }
                        arr.push(obj);
                    });
                    var model = JSON.stringify(arr);
                    console.log(model);
                    $.ajax({
                        url: '/Admin/Order/BulkUpdate',
                        data: JSON.stringify(arr),
                        type: 'POST',
                        contentType: 'application/json;',
                        dataType: 'json',
                        success: function (result) {
                            location.reload();
                        }
                    });
                }
            }
        }
    });   
});
$('#bulkChangePaymentStatus').on('change', function (e) {
    bootbox.confirm({
        message: "Are you sure you want change selected items status?",
        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                if ($('#bulkChangePaymentStatus').val() != "-1") {
                    var selected = $('.bulk-edit-d-status:checkbox:checked');
                    var statusId = $('#bulkChangePaymentStatus').val();
                    var arr = [];
                    $.each(selected, function (index, value) {
                        var id = $(value).attr('data-order-id');
                        var obj = {
                            "Id": id,
                            "PaymentStatus": statusId
                        }
                        arr.push(obj);
                    });
                    var model = JSON.stringify(arr);
                    console.log(model);
                    $.ajax({
                        url: '/Admin/Order/BulkPaymentStatusUpdate',
                        data: JSON.stringify(arr),
                        type: 'POST',
                        contentType: 'application/json;',
                        dataType: 'json',
                        success: function (result) {
                            location.reload();
                        }
                    });
                }
            }
        }
    });
});
$('.add-new-district').on('click', function () {
    bootbox.confirm({
        message: "Are you sure you want add this item?",
        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                if ($('#new-district').val()) {
                    var model = { "NewName":$('#new-district').val()};
                    $.ajax({
                        url: '/Admin/Configuration/AddDistrict',
                        data: JSON.stringify(model),
                        type: 'POST',
                        contentType: 'application/json;',
                        dataType: 'json',
                        success: function (result) {
                            location.reload();
                        }
                    });
                }               
            }
        }
    }); 
});

$('.update-distric').on('click', function (e) {
    var dom = "#d_" + $(this).attr('data-id');
    bootbox.confirm({
        message: "Are you sure you want update this item?",
        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
           
            if (result) {
                if ($(dom).val()) {
                    
                    var model = { "NewName": $(dom).val(), "OldName": $(dom).attr('data-old-value')};
                    $.ajax({
                        url: '/Admin/Configuration/UpdateDistrict',
                        data: JSON.stringify(model),
                        type: 'POST',
                        contentType: 'application/json;',
                        dataType: 'json',
                        success: function (result) {
                            location.reload();
                        }
                    });
                }
            }
        }
    }); 
});

$('.rmv-distric').on('click', function (e) {
    var dom = "#d_"+$(this).attr('data-id');
    bootbox.confirm({
        message: "Are you sure you want delete this item?",
        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-danger'
            }
        },
        callback: function (result) {

            if (result) {
                if ($(dom).val()) {

                    var model = { "OldName": $(dom).attr('data-old-value') };
                    $.ajax({
                        url: '/Admin/Configuration/RemoveDistrict',
                        data: JSON.stringify(model),
                        type: 'POST',
                        contentType: 'application/json;',
                        dataType: 'json',
                        success: function (result) {
                            location.reload();
                        }
                    });
                }
            }
        }
    }); 
});
$('#sync-order-desc').on("click", function (e) {
    $('#sync-order-desc').hide();
    $.ajax({
        url: '/Delivery/UpdateOrderDescription',
        //data: JSON.stringify(arr),
        type: 'GET',
        contentType: 'application/json;',
        dataType: 'json',
        success: function (result) {
            //console.log(result);
            if (result == "true") {
                bootbox.alert("Successfully Synced");
                //alert();
            } else {
                bootbox.alert("Failed");
               // alert("Failed");
            }
            
        }
    });
    $('#sync-order-desc').show();
});

$('.btn-bulk-update-promotion').on('click', function () {
    var srat_date = $('#StartDate').val();
    var end_date = $('#EndDate').val();;
    var percentage = $('#percentage').val();;
    var selected_nc = $('.checkbox-nc:checked');
    var selected_hc = $('.checkbox-hb:checked');
    var selected_ov = $('.checkbox-ov:checked');
    var arrselected =[];
    $.each(selected_nc, function (index, value) {
        arrselected.push($(this).val() + '/' + $(this).attr('data-property'));
    });
    $.each(selected_hc, function (index, value) {
        arrselected.push($(this).val() + '/' + $(this).attr('data-property'));
    });
    $.each(selected_ov, function (index, value) {
        arrselected.push($(this).val() + '/' + $(this).attr('data-property'));
    });
    //console.log(arrselected.join(", "));
    if (arrselected.length > 0) {
        var dialog = bootbox.dialog({
            title: 'Wait',
            message: "<p>Please wait until process completed.</p>",
            size: 'large',
            //buttons: {
            //    cancel: {
            //        label: "I'm a cancel button!",
            //        className: 'btn-danger',
            //        callback: function () {
            //            console.log('Custom cancel clicked');
            //        }
            //    },
            //    noclose: {
            //        label: "I don't close the modal!",
            //        className: 'btn-warning',
            //        callback: function () {
            //            console.log('Custom button clicked');
            //            return false;
            //        }
            //    },
            //    ok: {
            //        label: "I'm an OK button!",
            //        className: 'btn-info',
            //        callback: function () {
            //            console.log('Custom OK clicked');
            //        }
            //    }
            //}
        });
        var model = { "StartDate": srat_date, "EndDate": end_date, "Percentage": percentage, "SelectedItems": arrselected.join(", ") };
        $.ajax({
            url: '/Admin/Promotion/AddBulkPromotion   ',
            data: JSON.stringify(model),
            type: 'POST',
            contentType: 'application/json;',
            dataType: 'json',
            success: function (result) {
                location.replace("/admin/promotion/");
            }
        });
        //dialog.modal('hide');
    } else {
        bootbox.alert("Please select at least one book.");
    }
   
    //console.log(selected_hc);
    //console.log(selected_ov);
});

$('.update-order-payment-sttaus').on('click', function (e) {

    var order_id = $(this).attr("data-order-id"); ;
    var status_id = $('#select-payment-status').val();
    var payment_notes = $('#paymentNote').val();
    var hidden_payment_sttaus = $('#hidden-payment-status').val();
    var url = '/Admin/Order/ChangePaymentStatus';
    if (status_id != hidden_payment_sttaus) {
        bootbox.confirm({
            message: "Do you really want to change payment status?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                console.log(result);
                if (result) {

                    var model = { "OrderId": order_id, "StatusId": status_id, "Note": payment_notes };
                    console.log(model);
                    $.post(url, model)
                        .done(function (data) {
                            location.reload();
                        })
                        .fail(function (data) {
                            //console.log(data);
                        });
                } else {
                    $('#select-payment-status').val(hidden_payment_sttaus);
                }

            }
        });
    } else {
        bootbox.alert("Payment status not changed !");
    }

    
});
$('.check-item-type').on('change', function (e) {
    if ($(this).val() == 0 && $(this).is(":checked")) {
        $('.book-property-container').show();
        $('.book-pack-container').hide();
        $('#lbl-pre-release-end-date').text("Pre Release End Date");
        $('.publisherId-container').show();
    } else if ($(this).val() == 1 && $(this).is(":checked")) {
        $('.book-property-container').hide();
        $('.book-pack-container').show();
        $('#lbl-pre-release-end-date').text("Book pack available until");
        $('.publisherId-container').hide();
    }
});

//$('#select-author').on('change', function (e) {
//    var selected_author = $('#select-author').val();
    
//    if (selected_author != 0) {
//        var items = $(".author_" + selected_author);
//        $('.checked_all').prop('checked', false);
//        $('.checked_hard_bind_all').prop('checked', false);
//        $('.checked_other_all').prop('checked', false);
//        $('.check-box-container').hide();
//        items.show();
//    } else {
//        $('.checked_all').prop('checked', false);
//        $('.checked_hard_bind_all').prop('checked', false);
//        $('.checked_other_all').prop('checked', false);
//        $('.check-box-container').show();
//    }
    
//});

//$('#select-publisher').on('change', function (e) {
//    var selected_author = $('#select-publisher').val();

//    if (selected_author != 0) {
//        var items = $(".publisher_" + selected_author);
//        $('.checked_all').prop('checked', false);
//        $('.checked_hard_bind_all').prop('checked', false);
//        $('.checked_other_all').prop('checked', false);
//        $('.check-box-container').hide();
//        items.show();
//    } else {
//        $('.checked_all').prop('checked', false);
//        $('.checked_hard_bind_all').prop('checked', false);
//        $('.checked_other_all').prop('checked', false);
//        $('.check-box-container').show();
//    }

//    // location.replace("/Admin/Promotion/BulkPromotion?author=" + selected_author);
//});

