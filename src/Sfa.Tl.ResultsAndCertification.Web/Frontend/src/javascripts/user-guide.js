"use strict";

$(document).ready(function () {

    function ShowHideHistory() {
        var hideAllUpdates = "- hide all updates";
        var showAllUpdates = "+ show all updates";

        if ($(this).html() === showAllUpdates) {
            $(this).html(hideAllUpdates);
        }
        else {
            $(this).html(showAllUpdates);
        }
        $("#full-history").toggleClass('tl-hide');

        return false;
    };

    var showhidelink = $("#show-hide-history");
    showhidelink.on('click', ShowHideHistory);
    $("#full-history").addClass('tl-hide');

    $(window).scroll(function () {

        var screen_top = $(window).scrollTop();
        var screen_bottom = $(window).scrollTop() + $(window).innerHeight();

        var sticky_ele1 = $('#stickyref1');
        var first_ele_bottom = sticky_ele1.offset().top + sticky_ele1.outerHeight();

        var sticky_ele2 = $('#stickyref2');
        var last_ele_top = sticky_ele2.offset().top;
        var last_ele_bottom = sticky_ele2.offset().top + sticky_ele2.outerHeight();

        var stickyElement = $('.app-c-contents-list-with-body__link-wrapper');

        if (((first_ele_bottom > screen_top)) ||
            ((last_ele_top < screen_bottom) && (last_ele_bottom > screen_top))) {
            stickyElement.removeClass('govuk-sticky-element--stuck-to-window');
        } else {
            stickyElement.addClass('govuk-sticky-element--stuck-to-window');
        }
    });
});
