"use strict";
$(document).ready(function () {
    $('#uploadIndustryPlacementsForm').submit(function () {
        $('#uploadIndustryPlacementsButton').attr('disabled', 'disabled');
        // set screen-reader attributes
        $('#uploadIndustryPlacementsContainer').attr('aria-hidden', 'true');
        $('#spinnerText').attr('role', 'true');
        $('#spinnerText').attr('aria-live', 'assertive');
        setTimeout(function () {
            $(window).scrollTop(0);
            $('.govuk-breadcrumbs').toggleClass('tl-hide');
            $('#uploadIndustryPlacementsContainer').toggleClass('tl-hide');
            $('#processingIndustryPlacementsContainer').toggleClass('tl-hide');
        }, 500);
    });
});