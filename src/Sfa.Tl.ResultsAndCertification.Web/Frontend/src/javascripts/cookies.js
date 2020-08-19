$(document).ready(function () {

    var cookieName = 'analytics_consent';
    var options = $('input:radio[name=cookie-answer]');
    var saveButton = $('#SaveChanges');
    var confirmation = $('#ConfirmationBox');
    var selectedOption = false;

    // Get cookie value and initialize radio option
    var consent = GOVUK.getCookie(cookieName);
    if (consent != null && consent === 'true') {
        selectedOption = true;
    }
    options.filter('[value=' + selectedOption + ']').attr('checked', true);

    saveButton.click(function () {
        var checkedValue = options.filter(':checked').val();

        if (checkedValue) {
            GOVUK.acceptAllCookies(checkedValue);

            // show confirmation message on top
            confirmation.removeClass('tl-hide');
            window.scrollTo(0, 0);
            $(this).
        }
    });
});