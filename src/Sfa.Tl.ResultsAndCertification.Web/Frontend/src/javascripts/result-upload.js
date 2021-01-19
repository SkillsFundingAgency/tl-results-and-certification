"use strict";
$(document).ready(function () {
    $('#uploadResultsForm').submit(function () {
        $('#uploadResultsButton').attr('disabled', 'disabled');
        // set screen-reader attributes
        $('#uploadResultsContainer').attr('aria-hidden', 'true');
        $('#spinnerText').attr('role', 'true');
        $('#spinnerText').attr('aria-live', 'assertive');
        setTimeout(function () {
            $(window).scrollTop(0);
            $('.govuk-breadcrumbs').toggleClass('tl-hide');
            $('#uploadResultsContainer').toggleClass('tl-hide');
            $('#processingResultsContainer').toggleClass('tl-hide');
        }, 500);
    });
});