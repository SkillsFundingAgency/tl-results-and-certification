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
    showhidelink.click();

    $(window).scroll(function () {

        var bottom_of_screen = $(window).scrollTop() + $(window).innerHeight();
        var top_of_screen = $(window).scrollTop();

        var ele = $('#stickyref1')
        var bottom_of_element = ele.offset().top + ele.outerHeight();

        var ele2 = $('#stickyref2')
        var top_of_element2 = ele2.offset().top;
        var bottom_of_element2 = ele2.offset().top + ele2.outerHeight();

        var stickyElement = $('.app-c-contents-list-with-body__link-wrapper');

        if (((top_of_screen < bottom_of_element)) ||
            ((bottom_of_screen > top_of_element2) && (top_of_screen < bottom_of_element2))) {
            stickyElement.removeClass('govuk-sticky-element--stuck-to-window');
        } else {
            stickyElement.addClass('govuk-sticky-element--stuck-to-window');
        }
    });
});
