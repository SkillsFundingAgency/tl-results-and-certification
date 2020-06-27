"use strict";
$(document).ready(function () {
    $('#uploadRegistrationsForm').submit(function () {
        $('#uploadRegistrationsButton').attr('disabled', 'disabled');

        setTimeout(function () {
            $(window).scrollTop(0);
            $('#uploadRegistrationsContainer').toggleClass('tl-hide');
            $('#processingRegistrationsContainer').toggleClass('tl-hide');
        }, 500);
    });
});