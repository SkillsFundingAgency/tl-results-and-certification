"use strict";

$(document).ready(function () {

    var englishMathsRadioOptions = "input[type='radio']";

    function ShowDeclarationText() {
        var selectedData = $(englishMathsRadioOptions + ":checked").attr('data-declaration');
        if (selectedData !== null && selectedData === 'true')
            $('#declarationText').removeClass('tl-hide');
        else
            $('#declarationText').addClass('tl-hide');
    }

    $(englishMathsRadioOptions).click(function () {
        ShowDeclarationText();
    });

    ShowDeclarationText();
});