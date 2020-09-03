"use strict";
$(document).ready(function () {
    var root = this;
    if (typeof root.GOVUK === 'undefined') { root.GOVUK = {}; }

    var firstTabStop;
    var lastTabStop;
    var sessionTimeoutModalTimerHandle;
    var sessionTimeoutCountDownTimerHandle;
    var currentGetTimeoutStatusXhr = null;
    var currentRenewSessionXhr = null;
    var defaultValueToShowTimeoutModalInMinutes = 5;
    var defaultValueForCountDownTimerInMinutes = 1;

    GOVUK.clearSessionTimeoutModalTimer = function () {
        clearTimeout(sessionTimeoutModalTimerHandle);
    };

    GOVUK.setSessionTimeoutModalTimer = function (timeoutModalInMs) {
        sessionTimeoutModalTimerHandle = setTimeout(function () {
            $("#timeout-message-container").removeClass('hidden');
            $("#keep-me-signed-in").focus();
            $("#skipToMainContent").attr("tabindex", "-1");
            GOVUK.handleKeydownEventsForModal();
            GOVUK.setSessionTimeoutCountDownTimer(defaultValueForCountDownTimerInMinutes, 0);
        }, timeoutModalInMs);
    };

    GOVUK.resetSessionTimeoutModalTimer = function (timeoutModalInMs) {
        GOVUK.clearSessionTimeoutModalTimer();
        $("#timeout-message-container").addClass('hidden');
        $("#skipToMainContent").removeAttr("tabindex");
        GOVUK.setSessionTimeoutModalTimer(timeoutModalInMs);
    };

    GOVUK.clearSessionTimeoutCountDownTimer = function () {
        clearTimeout(sessionTimeoutCountDownTimerHandle);
    };

    GOVUK.setSessionTimeoutCountDownTimer = function (minutes, seconds) {
        function startSessionTimeoutCountDownTimer() {
            var minutesCounterElement = $("#minutes-counter"), secondsCounterElement = $("#seconds-counter");
            minutesCounterElement.text(minutes > 0 ? minutes.toString() + (minutes === 1 ? " minute" : " minutes") : "");
            secondsCounterElement.text(seconds > 0 ? seconds.toString() + " seconds" : "");

            if (minutes === 0 && seconds === 2) {
                GOVUK.checkSessionActiveDuration(0, 2);
            }

            if (minutes === 0 && seconds === 0) {
                clearTimeout(sessionTimeoutModalTimerHandle);
                clearTimeout(sessionTimeoutCountDownTimerHandle);
                window.location.href = "/timeout";
            }
            else {
                seconds--;
                if (seconds >= 0) {
                    sessionTimeoutCountDownTimerHandle = setTimeout(startSessionTimeoutCountDownTimer, 1000);
                } else {
                    if (minutes >= 1) {
                        clearTimeout(sessionTimeoutCountDownTimerHandle);
                        sessionTimeoutCountDownTimerHandle = setTimeout(function () {
                            GOVUK.setSessionTimeoutCountDownTimer(minutes - 1, 59);
                        }, 1000);
                    }
                }
            }
        }
        startSessionTimeoutCountDownTimer();
    };

    GOVUK.handleKeydownEventsForModal = function () {

        var elementsThatAreFocusable = $('.tl-modal a');
        $(".tl-modal").unbind("keydown");
        $(".tl-modal").on("keydown",
            function (e) {
                const keyTab = 9;
                if (e.keyCode === keyTab) {
                    if (e.shiftKey) {
                        if (document.activeElement === firstTabStop) {
                            lastTabStop.focus();
                            e.preventDefault();
                        }
                    } else {
                        if (document.activeElement === lastTabStop) {
                            firstTabStop.focus();
                            e.preventDefault();
                        }
                    }
                }
            });

        if (elementsThatAreFocusable.length > 1) {
            firstTabStop = elementsThatAreFocusable[0];
            lastTabStop = elementsThatAreFocusable[elementsThatAreFocusable.length - 1];
        }
    }

    GOVUK.checkSessionActiveDuration = function (timerMinutesValue, timerSecondsValue) {
        if (currentGetTimeoutStatusXhr != null)
            currentGetTimeoutStatusXhr.abort();

        currentGetTimeoutStatusXhr = $.ajax({
            type: "get",
            url: "/active-duration",
            contentType: "application/json",
            success: function (result) {
                if (result) {
                    if (timerMinutesValue == 0 && result.minutes == 0 && result.seconds > timerSecondsValue) {
                        GOVUK.clearSessionTimeoutCountDownTimer();
                        GOVUK.setSessionTimeoutCountDownTimer(0, result.seconds);
                    }
                    else if (timerMinutesValue == 0 && (result.minutes > 0 && result.minutes < defaultValueToShowTimeoutModalInMinutes)) {
                        GOVUK.clearSessionTimeoutCountDownTimer();
                        GOVUK.setSessionTimeoutCountDownTimer(result.minutes, result.seconds);
                    }
                    else if (timerMinutesValue == 0 && result.minutes > defaultValueToShowTimeoutModalInMinutes) {
                        var resetSessionTimeoutValueInMs = ((result.minutes * 60000) + (result.seconds * 1000)); 
                        GOVUK.clearSessionTimeoutCountDownTimer();
                        GOVUK.resetSessionTimeoutModalTimer(resetSessionTimeoutValueInMs);
                    }
                }
            },
            timeout: 3000,
            error: function (xhr, textStatus, errorThrown) {
                if (textStatus != "abort")
                    console.log(xhr + textStatus + errorThrown);
            },
            complete: function (d) {
                currentGetTimeoutStatusXhr = null;
            }
        });
    }

    GOVUK.renewUserSessionActivity = function () {
        if (currentRenewSessionXhr != null)
            currentRenewSessionXhr.abort();

        currentRenewSessionXhr = $.ajax({
            type: "get",
            url: "/renew-activity",
            contentType: "application/json",
            success: function (result) {
                if (result) {
                    var resetSessionTimeoutValueInMs = ((result.minutes * 60000) + (result.seconds * 1000));
                    GOVUK.clearSessionTimeoutCountDownTimer();
                    GOVUK.resetSessionTimeoutModalTimer(resetSessionTimeoutValueInMs)
                }
            },
            timeout: 3000,
            error: function (xhr, textStatus, errorThrown) {
                if (textStatus != "abort")
                    console.log(xhr + textStatus + errorThrown);
            },
            complete: function (d) {
                currentRenewSessionXhr = null;
            }
        });
    }
    $("#keep-me-signed-in").click(function () {
        GOVUK.renewUserSessionActivity();
    });

    if (window.GOVUK) {
        var timeoutValue = $('#timeout-message-container').data('timeout');
        GOVUK.setSessionTimeoutModalTimer(60000);
    }
});