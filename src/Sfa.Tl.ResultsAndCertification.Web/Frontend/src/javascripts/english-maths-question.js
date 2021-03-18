"use strict";

$(document).ready(function () {

    function ShowDeclarationText(status) {
        if (status === "AchievedWithSend")
            $('#declarationText').removeClass('tl-hide');
        else
            $('#declarationText').addClass('tl-hide');
    }

    var englishMathsRadioOptions = "input[type='radio'][name='EnglishAndMathsStatus']";
    var status = $(englishMathsRadioOptions + ":checked").val();
    ShowDeclarationText(status);

    $(englishMathsRadioOptions).click(function () {
        var status = $(englishMathsRadioOptions + ":checked").val();
        ShowDeclarationText(status);
    });

});