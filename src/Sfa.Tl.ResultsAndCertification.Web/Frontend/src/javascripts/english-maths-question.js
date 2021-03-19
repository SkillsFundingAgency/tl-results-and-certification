"use strict";

$(document).ready(function () {

    var englishMathsRadioOptions = "input[type='radio']";

    function ShowDeclarationText() {
        var selectedId = $(englishMathsRadioOptions + ":checked").attr('id');
        if (selectedId === 'status-achieved-send')
            $('#declarationText').removeClass('tl-hide');
        else
            $('#declarationText').addClass('tl-hide');
    }

    $(englishMathsRadioOptions).click(function () {
        ShowDeclarationText();
    });

    ShowDeclarationText();
});