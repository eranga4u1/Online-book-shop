var general = {
  init: function() {
    general.hamburger();
    general.mobileSearch();
  },
  hamburger: function() {
    $(".hamRotate").on("click", function() {
      $(this).toggleClass("active");
      $("body").toggleClass("hamactive");
    });
  },
  mobileSearch: function() {
    $(".btn-mobile-search").on("click", function() {
      $("body").toggleClass("searchopen");
    });
  }
};

var forms = {
  init: function() {
    forms.passwordToggle();
    forms.emailValidation();
    forms.checkPasswordStrength();
    forms.showHint();
  },
  passwordToggle: function() {
    if ($("div").hasClass("eye-icon")) {
      $(".eye-icon").on("click", function() {
        if (
          $(this)
            .closest(".form-group")
            .find(".password")
            .prop("type") == "text"
        ) {
          $(this)
            .closest(".form-group")
            .find(".password")
            .prop("type", "password");
        } else {
          $(this)
            .closest(".form-group")
            .find(".password")
            .prop("type", "text");
        }
      });
    }
  },
  emailValidation: function() {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
  },
  checkPasswordStrength: function() {
    $(".registration-form .passwordstrenght").keyup(function() {
      var number = /([0-9])/;
      var alphabets = /([a-zA-Z])/;
      var special_characters = /([~,!,@,#,$,%,^,&,*,-,_,+,=,?,>,<])/;
      if ($(this).val().length < 6) {
        $("#password-strength-status").removeClass();
        $("#password-strength-status").addClass("weak-password");
        $("#password-strength-status").html(
          "Weak (should be atleast 6 characters.)"
        );
      } else {
        if (
          $(this)
            .val()
            .match(number) &&
          $(this)
            .val()
            .match(alphabets) &&
          $(this)
            .val()
            .match(special_characters)
        ) {
          $("#password-strength-status").removeClass();
          $("#password-strength-status").addClass("strong-password");
          $("#password-strength-status").html("Strong");
        } else {
          $("#password-strength-status").removeClass();
          $("#password-strength-status").addClass("medium-password");
          $("#password-strength-status").html(
            "Medium (should include alphabets, numbers and special characters.)"
          );
        }
      }
    });
  },
  showHint: function() {
    $(".registration-form #pass-word").on("mouseover", function() {
      $(".password-hint").show("fast");
    });
    $(".registration-form #pass-word").on("mouseleave", function() {
      $(".password-hint").hide("fast");
    });
    $(".registration-form #pass-word").on("click", function() {
      $(".password-hint").hide("fast");
    });
  }
};

var slider = {
  homeSlider: function() {
    if ($("div").hasClass("homeslider")) {
      $(".homeslider").slick({
        dots: true,
        infinite: false,
        speed: 300,
        slidesToShow: 1,
        slidesToScroll: 1,
        adaptiveHeight: false,
        autoplaySpeed: 3000,
        autoplay: true
      });
    }
  },

  bookSlider: function(id) {
    if ($("div").hasClass("book-overview")) {
      $(id).slick({
        dots: false,
        infinite: false,
        speed: 300,
        slidesToShow: 4,
        adaptiveHeight: false,
        autoplaySpeed: 3000,
        autoplay: false,
        responsive: [
          {
            breakpoint: 1024,
            settings: {
              slidesToShow: 3,
              slidesToScroll: 3,
              infinite: false,
              dots: true
            }
          },
          {
            breakpoint: 769,
            settings: {
              slidesToShow: 2,
              slidesToScroll: 2
            }
          },
          {
            breakpoint: 600,
            settings: {
              slidesToShow: 1,
              slidesToScroll: 1
            }
          }
        ]
      });
    }
  },
  bestOffersSlider: function() {
    if ($("div").hasClass("bestoffers-slider")) {
      $(".bestoffers-slider").slick({
        dots: true,
        infinite: false,
        speed: 300,
        slidesToShow: 1,
        slidesToScroll: 1,
        adaptiveHeight: false,
        autoplaySpeed: 3000,
        autoplay: true
      });
    }
  },
  bookOverviewSlider: function() {
    $(".slider-for").slick({
      slidesToShow: 1,
      slidesToScroll: 1,
      arrows: false,
      fade: true,
      asNavFor: ".slider-nav"
    });
    $(".slider-nav").slick({
      slidesToShow: 3,
      slidesToScroll: 1,
      asNavFor: ".slider-for",
      dots: true,
      centerMode: true,
      focusOnSelect: true,
      arrows: true
    });
  }
};

var home = {
  init: function() {
    home.accordian();
  },
  accordian: function() {
    $(".acordian-tab").on("click", function() {
      //$(".bookcat-col").removeClass("active");
      $(this)
        .closest(".bookcat-col")
        .toggleClass("active");
    });
  }
};

var rating = {
  init: function() {
    rating.starRatingMouseOver();
    rating.starRatingClick();
  },
  starRatingMouseOver: function() {
    $(".starbox > div").on("mouseover", function() {
      if (!$("div").hasClass("rategiven")) {
        var ratingval = $(this)
          .attr("class")
          .split(" ")[0]
          .split("-");
        $(this)
          .closest(".rating")
          .find(".stars")
          .attr("data-rating", ratingval[1]);
      }
    });
  },
  starRatingClick: function() {
    $(".starbox > div").on("click", function() {
      $(".starbox").addClass("rategiven");
    });
  }
};

var writeReview = {
  init: function() {
    writeReview.openWriteReview();
    writeReview.closeReview();
  },
  openWriteReview: function() {
    $(".writeareview").on("click", function() {
      $(".reviewform-wrapper").fadeIn("slow");
      $("body").addClass("openreview");
    });
  },
  closeReview: function() {
    $(".reviewform .btn-close").on("click", function() {
      $(".reviewform-wrapper").fadeOut("slow");
      $("body").removeClass("openreview");
      $(".textreview").val("");
    });
  }
};

var viewSample = {
  init: function() {
    viewSample.show();
    viewSample.closeSample();
  },
  show: function() {
    $(".btn-readsample").on("click", function() {
      $(".booksample-wrapper").fadeIn("slow");
      $("body").addClass("opensample");
    });
  },
  closeSample: function() {
    $(".booksample .btn-close").on("click", function() {
      $(".booksample-wrapper").fadeOut("slow");
      $("body").removeClass("opensample");
    });
  }
};

var serach = {
  init: function() {
    serach.searchHint();
  },
  searchHint: function() {
    $(".search").after("<div class='search-sugges'></div>");
    $(".search").on("keyup", function() {
      if ($.trim($(this).val()).length > 2) {
        $(".search-sugges").addClass("active");
        $(".search-sugges").empty();
        let _html = "";
        $.ajax({
          url: "/search/GetSearchSuggestion?term=" + $.trim($(this).val()),
          cache: false,
          type: "GET",
          dataType: "json",
          success: function(data) {
            if (data.length > 0) {
              $.each(data, function(key, value) {
                _html +=
                  "<span><a href='" +
                  value.URL +
                  "'/>" +
                  value.Title +
                  "</a></span>";
              });
              $(".search-sugges")
                .empty()
                .append(_html);
            } else {
                    _html =
                        "<span class=\"text-danger\">No Result found</span>";
              
                      $(".search-sugges")
                          .empty()
                          .append(_html);
                          //$(".search-sugges").empty();
                          //$(".search-sugges").removeClass("active");
            }
          }
        });
      } else {
        $(".search-sugges").empty();
        $(".search-sugges").removeClass("active");
      }
    });
  }
};
var countDown = {
  init: function() {
    if ($("div").hasClass("countdown")) {
      var publishdate = $(".countdown").data("date");
      var countDownDate = new Date(publishdate).getTime();

      // Update the count down every 1 second
      var x = setInterval(function() {
        // Get today's date and time
        var now = new Date().getTime();

        // Find the distance between now and the count down date
        var distance = countDownDate - now;

        // Time calculations for days, hours, minutes and seconds
        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        var hours = Math.floor(
          (distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)
        );
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);

        // Display the result in the element with id="demo"
        document.getElementById("countdown").innerHTML =
          days + "d : " + hours + "h : " + minutes + "m : " + seconds + "s ";

        // If the count down is finished, write some text
        if (distance < 0) {
          clearInterval(x);
          document.getElementById("countdown").innerHTML = "EXPIRED";
        }
      }, 1000);
    }
  }
};

var megaMenu = {
  init: function() {
    megaMenu.openMegaMenu();
  },
  openMegaMenu: function() {
    $(".author-mega-menu").on("mouseover", function() {
      $(".megamenu").addClass("open");
    });

    $(".megamenu").on("mouseleave", function() {
      $(".megamenu").removeClass("open");
    });
  }
};

$(document).ready(function() {
  general.init();
  forms.init();
  slider.homeSlider();
  slider.bestOffersSlider();
  home.init();
  slider.bookSlider("#preorder");
  slider.bookSlider(".commonslider");
slider.bookSlider("#latestarr");
    slider.bookSlider("#bookpacksarr");
slider.bookSlider("#recentview");
slider.bookSlider("#bestselling");
slider.bookSlider(".authorwork-slider");
slider.bookSlider(".recommended-slider");

  slider.bookOverviewSlider();
  countDown.init();
  rating.init();
  writeReview.init();
  viewSample.init();
  serach.init();
  megaMenu.init();
  slider.bookSlider("#booksby");
  setTimeout(function() {
    slider.bookSlider("#youmaylike");
  }, 100);
});
