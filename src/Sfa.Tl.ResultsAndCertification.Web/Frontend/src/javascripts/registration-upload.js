"use strict";
$(document).ready(function () {
    $('#uploadRegistrationsForm').submit(function () {
        $('#uploadRegistrationsButton').attr('disabled', 'disabled');

        setTimeout(function () {
            $('#uploadRegistrationsContainer').toggleClass('tl-hide');
            $('#processingRegistrationsContainer').toggleClass('tl-hide');
        }, 500);
    });
});