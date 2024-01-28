$(function() {
    "use strict";
    $(function() {
            $(".preloader").fadeOut();
        }),

        jQuery(document).on("click", ".mega-dropdown", function(i) {
            i.stopPropagation();
        });

        //DatePicker Js  Start here
//how to use 
//Create input elements with the class "datepicker-input" that you want to associate with datepickers.
//
    //Introduction
//The data - dateStart and data - dateEnd attributes are custom attributes that can be used to configure the start and end dates for datepicker components in your HTML form.These attributes work in conjunction with the provided initialization code to set specific date ranges for datepickers.

//    data - dateStart Attribute
//The data - dateStart attribute is used to specify the start date for the datepicker.
//It accepts the following values:
//"Current": Sets the start date to the current date.
//Specific Date(e.g., "01/01/2023"): Sets the start date to the specified date.
//Empty or Not Present: Defaults to null.
//    data - dateEnd Attribute
//The data - dateEnd attribute is used to specify the end date for the datepicker.
//It accepts the same values as data - dateStart:
//"Current": Sets the end date to the current date.
//Specific Date(e.g., "31/12/2023"): Sets the end date to the specified date.
//Empty or Not Present: Defaults to null.
$(".datepicker-input").each(function () {
    var idAttributeValue = $(this).attr("id");
    var startDateValue = $(this).data("datestart");
    var endDateValue = $(this).data("dateend");

    // If startDateValue is not specified or is "Current", set it to the current date
    if (!startDateValue) {
        startDateValue = null;
    } else if (startDateValue.toLowerCase() === "current") {
        startDateValue = new Date();
    } else {
        // Parse the date string manually to handle "dd/mm/yyyy" format
        var StartDateParts = startDateValue.split("/");
        startDateValue = new Date(StartDateParts[2], StartDateParts[1] - 1, StartDateParts[0]);
    }

    // end date 
    if (!endDateValue) {
        endDateValue = null;
    } else if (endDateValue.toLowerCase() === "current") {
        endDateValue = new Date();
    } else {
        // Parse the date string manually to handle "dd/mm/yyyy" format
        var endDateParts = endDateValue.split("/");
        endDateValue = new Date(endDateParts[2], endDateParts[1] - 1, endDateParts[0]);
    }

    $("#" + idAttributeValue).datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        todayBtn: "linked",
        todayHighlight: true,
        forceParse: true,
        startDate: startDateValue,
        endDate: endDateValue,
    });
});
//datepicker End here
    var i = function () {
        (window.innerWidth > 0 ? window.innerWidth : this.screen.width) < 1170 ?
            (
                $("body").addClass("mini-sidebar"),
                $(".navbar-brand span").hide(),
                $(".scroll-sidebar, .slimScrollDiv").css("overflow-x", "visible").parent().css("overflow", "visible"),
                $(".sidebartoggler i").addClass("ti-menu")
            ) :
            (
                $("body").removeClass("mini-sidebar"), 
                $(".navbar-brand span").show()
            );
        var i = (window.innerHeight > 0 ? window.innerHeight : this.screen.height) - 1;
        (i -= 70) < 1 && (i = 1), i > 70 && $(".page-wrapper").css("min-height", i + "px");
    };


        $(window).ready(i), $(window).on("resize", i), $(".sidebartoggler").on("click", function() {
            $("body").hasClass("mini-sidebar") ? ($("body").trigger("resize"), $(".scroll-sidebar, .slimScrollDiv").css("overflow", "hidden").parent().css("overflow", "visible"),
                $("body").removeClass("mini-sidebar"), $(".navbar-brand span").show()) : ($("body").trigger("resize"),
                $(".scroll-sidebar, .slimScrollDiv").css("overflow-x", "visible").parent().css("overflow", "visible"),
                $("body").addClass("mini-sidebar"), $(".navbar-brand span").hide());
        }),



        //$(".fix-header .header").stick_in_parent({}),
        //    $(".nav-toggler").click(function () {
        //        $("body").toggleClass("show-sidebar"), $(".nav-toggler i").toggleClass("mdi mdi-menu"),
        //        $(".nav-toggler i").addClass("mdi mdi-close");
        //    }),



        $(".search-box a, .search-box .app-search .srh-btn").on("click", function() {
            $(".app-search").slideToggle(200);
        }),



        $(".floating-labels .form-control").on("focus blur", function(i) {
            $(this).parents(".form-group").toggleClass("focused", "focus" === i.type || this.value.length > 0);
        }).trigger("blur"), $(function() {
            for (var i = window.location, o = $("ul#sidebarnav a").filter(function() {
                    return this.href == i;
                }).addClass("active").parent().addClass("active");;) {
                if (!o.is("li")) break;
                o = o.parent().addClass("in").parent().addClass("active");
            }
        }),

        $(function() {
            $("#sidebarnav").metisMenu();
        }),

        $(".scroll-sidebar").slimScroll({
            position: "left",
            size: "5px",
            height: "100%",
            color: "#dcdcdc"
        }),

        $(".message-center").slimScroll({
            position: "right",
            size: "5px",
            color: "#dcdcdc"
        }),

        $(".aboutscroll").slimScroll({
            position: "right",
            size: "5px",
            height: "80",
            color: "#dcdcdc"
        }),

        $(".message-scroll").slimScroll({
            position: "right",
            size: "5px",
            height: "570",
            color: "#dcdcdc"
        }),

        $(".chat-box").slimScroll({
            position: "right",
            size: "5px",
            height: "470",
            color: "#dcdcdc"
        }),

        $(".slimscrollright").slimScroll({
            height: "100%",
            position: "right",
            size: "5px",
            color: "#dcdcdc"
        }),



        $("body").trigger("resize"), $(".list-task li label").click(function() {
            $(this).toggleClass("task-done");
        }),



        $("#to-recover").on("click", function() {
            $("#loginform").slideUp(), $("#recoverform").fadeIn();
        }),



        $('a[data-action="collapse"]').on("click", function(i) {
            i.preventDefault(), $(this).closest(".card").find('[data-action="collapse"] i').toggleClass("ti-minus ti-plus"),
                $(this).closest(".card").children(".card-body").collapse("toggle");
        }),



        $('a[data-action="expand"]').on("click", function(i) {
            i.preventDefault(), $(this).closest(".card").find('[data-action="expand"] i').toggleClass("mdi-arrow-expand mdi-arrow-compress"),
                $(this).closest(".card").toggleClass("card-fullscreen");
        }),



        $('a[data-action="close"]').on("click", function() {
            $(this).closest(".card").removeClass().slideUp("fast");
        });

});
$(document).ready(function(){
    //$("#open-modal").click(function () {
    //    $("#myModal").addClass("show");
    //    $("#myModal").css("display", "block");
    //    $(".modal-backdrop").css("display", "block");
    //    $(".modal-backdrop").css("opacity", "0.5");
    //});
    //$("#close-modal").click(function(){
    //    $("#imgEnlarge").toggleClass("hideModal");
    //    $(".modal-backdrop").toggleClass("hideModal");
    //});
    $(document).ready(function () {
        $('.sidebartoggler').click(function () {
            $("#logo-mini").toggleclass("hide");
        })
    })

if ($(window).width() < 1024) {
    $("body").addClass("mini-sidebar");
    $("#logo-mini").removeClass("hide");
    $("#logo-big").addClass("hide");
    $('.nav-toggler, .sidebartoggler').click(function () {
        $("body").toggleClass("show-sidebar");
    });
}
else {
    $(".sidebartoggler").click(function () {
        //$("body").toggleClass("mini-sidebar");
        //$("#logo-big").toggleClass("hide");
        //$("#logo-mini").toggleClass("hide");
        //$(".slimScrollDiv").css("overflow", "visible");
        //$(".scroll-sidebar").css("overflow", "visible");
    });
}
})
