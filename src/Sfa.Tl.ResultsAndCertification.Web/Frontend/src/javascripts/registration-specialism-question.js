"use strict";
$(document).ready(function () {
    $('input:radio[name=HasLearnerDecidedSpecialism]').click(function () {
        var continueButton = $("#continueButton");
        continueButton.text($(this).val() === "true" ? continueButton.data("continue-text") : continueButton.data("change-text"));
    });
});