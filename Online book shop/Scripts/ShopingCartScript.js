$(".add-to-cart").click(function (e) {
    if (!$(this).hasClass('tile-addto-cart-dissabled')) {
        var bookId = $(this).attr("data-book_id");
        var bookPropId = $(this).attr("data-book_prop_id");
        var numberOfItems = $(this).attr("data-number_of_items");
        var _htmlmsg = "<div class='addedmsg'>Item Added to your Cart</div>";

        if ($("#quantity-" + bookPropId).length > 0) {
            numberOfItems = $("#quantity-" + bookPropId).val();
        }
        if (numberOfItems < 1) {
            PopupDangerMessage("Invalid item count !");  
            $("#quantity-" + bookPropId).val(1);
        } else if (!(numberOfItems % 1 === 0)) {
            PopupDangerMessage("Invalid item count !"); 
            $("#quantity-" + bookPropId).val(1);
        }
        else {
            var url = '/ShopingCart/AddToCart';
            var model = { "BookId": bookId, "BookPropertyId": bookPropId, "NumberOfItems": numberOfItems };        

            $.post(url, model)
                .done(function (data) {
                    var dataObj = JSON.parse(data);

                    if (dataObj.Message) {
                        PopupDangerMessage(dataObj.Message); 
                        $("#quantity-" + bookPropId).val(1);
                    } else {
                        var dataCartObj = JSON.parse(dataObj.cart);
                        $('.wrapp-minicart').html(dataObj.minicart);
                        $('#cart-item-count').text('(' + dataCartObj.TotalItemsCount + ') Rs. ' + dataCartObj.AmountAfterDiscount.toFixed(2));
                        $('#minicart-amount').text(dataCartObj.AmountAfterDiscount.toFixed(2));
                        $('#minicart-itemCount').text(dataCartObj.Items.length);
                        $("body").remove(".addedmsg").append(_htmlmsg).addClass("addedtocart");
                        setTimeout(function () {
                            $(".addedmsg").fadeIn("slow");
                        }, 100);
                        setTimeout(function () {
                            $(".addedmsg").fadeOut("slow");

                            $("body").remove(".addedmsg").removeClass("addedtocart");
                            $(".addedmsg").remove();
                        }, 3000);
                    }
                })
        }
        
    } 
});

$(".remove-from-cart").click(function () {
    var txt;
    var r = confirm("This Item Will Be Removed From Your Cart, Press OK If You Want To Proceed?");
    if (r == true) {
        var bookId = $(this).attr("data-book_id");
        var bookPropId = $(this).attr("data-book_prop_id");
        var numberOfItems = $(this).attr("data-number_of_items");
        var url = '/ShopingCart/RemoveFromCart';
        var model = { "BookId": bookId, "BookPropertyId": bookPropId, "NumberOfItems": numberOfItems };
        $.post(url, model)
            .done(function (data) {
                //console.log(data);
                var dataObj = JSON.parse(data);
                $('#cart-item-count').text('(' + dataObj.Items.length + ')');
                location.reload();
            })
            .fail(function (data) {
                //console.log(data);
            });
    //.always(function () {
    //    alert("finished");
    //});
    } 
});
$(".bookProperty").change(function () {
    var url = '/ShopingCart/ChangeBookProperty';
    var model = jQuery.parseJSON($(this).val());
    console.log(model);
    $.post(url, model)
        .done(function (data) {
            var dataObj = JSON.parse(data);
            if (dataObj.message == "success") {
                location.reload();
            } else {
                alert("Failed, Please reload page");
            }
        })
        .fail(function (data) {

        });
});
$(".numberOfItems").change(function () {
    var bookId = $(this).attr("data-book_id");
    var bookPropId = $(this).attr("data-book_prop_id");
    var numberOfItems = $(this).val();
    var maxnumberOfItems = $(this).attr("data-max-item");
    if ($("#quantity-" + bookPropId).length > 0) {
        numberOfItems = $("#quantity-" + bookPropId).val();
    }
    if (numberOfItems < 1) {
        PopupDangerMessage("Invalid item count !");
    } else if (!(numberOfItems % 1 === 0)) {
        PopupDangerMessage("Invalid item count !");
    } else if (numberOfItems > maxnumberOfItems) {
        PopupDangerMessage("Invalid item count, Updated for maximum");
        $("#quantity-" + bookPropId).val(maxnumberOfItems);
        var url = '/ShopingCart/ChangeAmountOfItems';
        var model = { "BookId": bookId, "BookPropertyId": bookPropId, "NumberOfItems": maxnumberOfItems };
        $.post(url, model)
            .done(function (data) {
                var dataObj = JSON.parse(data);
                if (dataObj.message == "success") {
                    location.reload();
                } else {
                    location.reload();
                    // alert("Failed, Please reload page");
                }
            })
            .fail(function (data) {

            });
    } else {
        var url = '/ShopingCart/ChangeAmountOfItems';
        var model = { "BookId": bookId, "BookPropertyId": bookPropId, "NumberOfItems": numberOfItems };
        $.post(url, model)
            .done(function (data) {
                var dataObj = JSON.parse(data);
                if (dataObj.message == "success") {
                    location.reload();
                } else {
                    location.reload();
                    // alert("Failed, Please reload page");
                }
            })
            .fail(function (data) {

            });
    }
   
});
$('#add-voucher-code').click(function () {
    if ($(".voucher-code").val().length == 6) {
        var url = '/ShopingCart/AddVoucherCode?code=' + $(".voucher-code").val();
            $.post(url)
                .done(function (data) {
                    var dataObj = JSON.parse(data);
                    if (dataObj.Ok == "True") {
                        alert(dataObj.message);
                    } else {
                        $(".voucher-code").val("");
                        alert(dataObj.message);
                    }
                })
                .fail(function (data) {

                });
        } else {
            alert("Invalid voucher code");
         }
});
//$(".voucher-code").change(function () {
//    if ($(".voucher-code").val().length > 0) {
//        $('.addto-cart').attr("disabled", "disabled");
//        alert("Dissabled");
//        //check voucher code using ajax for validate
//       // $('.addto-cart').prop("disabled", false);
//    } else {
//        $('.addto-cart').prop("disabled", false);
//    }
//});
$("#select-book-property").change(function () {
    var selectedValue = $("#select-book-property").val();
    $(".bp-price-wrapper").hide();
    $(".book-details-" + selectedValue).show();
    var stock = $("#stock_" + selectedValue).val();
    if (stock > 0) {
        $(".preorder").html("Stock Available");
    } else {
        $(".preorder").html("Out of Stock");
    }
});
$('input[type=radio][name=delivery_method]').change(function () {
    if ($('input[name="delivery_method"]:checked').val() == 'courier') {
        $('#DeliveryMethod').val(1);
        $('#PaymentMethod').val(1);
        $('#country option').hide();
        $('#origin-country').show();
        if (!$('#country').val()) {
            $('#country').val("Sri Lanka");
        }
        estimate_price($('#cart-weight').text(), 1, $('#shoping-cart-district-selection').val());
    }
    else if ($('input[name="delivery_method"]:checked').val() == 'post') {
        $('#payment_methods_credit_card').prop("checked", true);
        $('#DeliveryMethod').val(0);
        $('#PaymentMethod').val(1);
        $('#country option').show();
        if (!$('#country').val()) {
            $('#country').val("Sri Lanka");
        }
        $('#shoping-cart-district-selection-row').show();
        if (!$('#shoping-cart-district-selection').val()) {
            $('#shoping-cart-district-selection').val("Colombo");
        }
        estimate_price($('#cart-weight').text(), 0, $('#shoping-cart-district-selection').val());
    } else if ($('input[name="delivery_method"]:checked').val() == 'in_store_pickup') {
        $('#DeliveryMethod').val(2);
        $('#PaymentMethod').val(1);
        estimate_price($('#cart-weight').text(), 4, $('#shoping-cart-district-selection').val());
    }
});
$('#country').change(function (e) {
    if ($('#country').val() == "Sri Lanka") {
        $('#shoping-cart-district-selection-row').show();
        $('#shoping-cart-district-selection').val("Colombo");
        estimate_price($('#cart-weight').text(), 0, $('#shoping-cart-district-selection').val());
    } else {
        $('#shoping-cart-district-selection-row').hide();
        $('#shoping-cart-district-selection').val(null);
        estimate_price($('#cart-weight').text(), 0, $('#country').val());
    }
    
});
$('#shoping-cart-district-selection').change(
    function (e) {
        if ($('#country').val() == "Sri Lanka" && $('input[name="delivery_method"]:checked').val() == 'courier') {
            estimate_price($('#cart-weight').text(), 1, $('#shoping-cart-district-selection').val());
        }
    }
);

function estimate_price(weight, delivery_type, area) {
    var delivery_method = 1;
    var url = "/Delivery/GetEstimateCost";
    if (delivery_type == 'post' || delivery_type == 0) {
        delivery_method = 0;
    }
    var model = {
        "weight": weight,
        "delivery_type": delivery_type,
        "area": area,
        "cart_amount": $('#cart-amout').text(),
        "country": $('#country').val()
    };
    $.post(url, model)
        .done(function (data) {
            var price1 = parseFloat(data.total_delivery_amount);
            var price2 = parseFloat(data.total_cost);
            $('#cart-delivery-cost').text(price1.toFixed(2) );
            $('#cart-delivery-total').text(price2.toFixed(2));
        })
        .fail(function (data) {
            //console.log(data);
        });
}
;
$("#redirect").click(function () {
    $('#alertWindow').show();
    return false;
});
$('#btnCancel').click(function () {
    $('#alertWindow').hide();
});
$('#btnOk').click(function () {
    location.href = 'http://www.google.com';
    return false;
});

function PopupDangerMessage(message) {
    var _htmlmsg = "<div class='added-failed-msg'>" + message+"</div>";
    $("body").remove(".added-failed-msg").append(_htmlmsg).addClass("addedtocart");
    setTimeout(function () {
        $(".added-failed-msg").fadeIn("slow");
    }, 100);
    setTimeout(function () {
        $(".added-failed-msg").fadeOut("slow");

        $("body").remove(".added-failed-msg").removeClass("addedtocart");
        $(".added-failed-msg").remove();
    }, 3000);
}
