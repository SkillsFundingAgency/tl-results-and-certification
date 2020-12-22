"use strict";
$(document).ready(function () {
    $('#uploadAssessmentsForm').submit(function () {
        $('#uploadAssessmentsButton').attr('disabled', 'disabled');
        // set screen-reader attributes
        $('#uploadAssessmentsContainer').attr('aria-hidden', 'true');
        $('#spinnerText').attr('role', 'true');
        $('#spinnerText').attr('aria-live', 'assertive');
        setTimeout(function () {
            $(window).scrollTop(0);
            $('.govuk-breadcrumbs').toggleClass('tl-hide');
            $('#uploadAssessmentsContainer').toggleClass('tl-hide');
            $('#processingAssessmentsContainer').toggleClass('tl-hide');
        }, 500);
    });
});