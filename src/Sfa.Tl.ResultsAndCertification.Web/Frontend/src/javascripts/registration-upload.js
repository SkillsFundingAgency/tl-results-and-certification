"use strict";
$(document).ready(function () {
    $('#uploadRegistrationsForm').submit(function () {
        $('#uploadRegistrationsButton').attr('disabled', 'disabled');

        // set screen-reader attributes
        $('#uploadRegistrationsContainer').attr('aria-hidden', 'true');
        $('#spinnerText').attr('role', 'true');
        $('#spinnerText').attr('aria-live', 'assertive');

        setTimeout(function () {
            $(window).scrollTop(0);
            $('#uploadRegistrationsContainer').toggleClass('tl-hide');
            $('#processingRegistrationsContainer').toggleClass('tl-hide');
        }, 500);
    });
});