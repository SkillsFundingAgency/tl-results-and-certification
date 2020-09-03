"use strict";
$(document).ready(function () {
    var root = this;
    if (typeof root.GOVUK === 'undefined') { root.GOVUK = {}; }

    var firstTabFocus = null;
    var lastTabFocus = null;
    var initialSessionTimeoutModalTimerHandle = null;
    var sessionTimeoutModalTimerHandle = null;
    var sessionTimeoutCountDownTimerHandle = null;
    var currentGetSessionActivityDurationXhr = null;
    var currentRenewSessionXhr = null;
    var delayTimeInMs = 2000;
    var defaultValueToShowTimeoutModalInMinutes = 1;
    var defaultValueForCountDownTimerInMinutes = 1;

    GOVUK.clearAllTimeoutTimers = function () {
        GOVUK.clearInitialSessionTimeoutModalTimer();
        GOVUK.clearSessionTimeoutModalTimer();
        GOVUK.clearSessionTimeoutCountDownTimer();        
    };

    GOVUK.clearInitialSessionTimeoutModalTimer = function () {
        clearTimeout(initialSessionTimeoutModalTimerHandle);
    };

    GOVUK.setInitialSessionTimeoutModalTimer = function (timeInMs) {
        initialSessionTimeoutModalTimerHandle = setTimeout(function () {
            GOVUK.checkSessionActiveDuration(0, 0, true)
        }, (timeInMs - delayTimeInMs));
    };

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
        GOVUK.hideTimeoutModal();
        GOVUK.setSessionTimeoutModalTimer(timeoutModalInMs);
    };

    GOVUK.hideTimeoutModal = function () {
        $("#timeout-message-container").addClass('hidden');
        $("#skipToMainContent").removeAttr("tabindex");
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
                GOVUK.checkSessionActiveDuration(0, 2, false);
            }

            if (minutes === 0 && seconds === 0) {
                GOVUK.clearAllTimeoutTimers();
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

        var focusableElements = $('.tl-modal a');
        $(".tl-modal").unbind("keydown");
        $(".tl-modal").on("keydown",
            function (e) {
                const keyTab = 9;
                if (e.keyCode === keyTab) {
                    if (e.shiftKey) {
                        if (document.activeElement === firstTabFocus) {
                            lastTabFocus.focus();
                            e.preventDefault();
                        }
                    } else {
                        if (document.activeElement === lastTabFocus) {
                            firstTabFocus.focus();
                            e.preventDefault();
                        }
                    }
                }
            });

        if (focusableElements.length > 1) {
            firstTabFocus = focusableElements[0];
            lastTabFocus = focusableElements[focusableElements.length - 1];
        }
    }

    GOVUK.checkSessionActiveDuration = function (timerMinutesValue, timerSecondsValue, isPreCheck) {
        if (currentGetSessionActivityDurationXhr != null)
            currentGetSessionActivityDurationXhr.abort();

        currentGetSessionActivityDurationXhr = $.ajax({
            type: "get",
            url: "/active-duration",
            contentType: "application/json",
            success: function (result) {
                if (result) {
                    if (isPreCheck) {
                        if (result.minutes > defaultValueToShowTimeoutModalInMinutes) {
                            var diffInMinutes = result.minutes - defaultValueToShowTimeoutModalInMinutes;
                            var resetTimmerInMilliSeconds = (diffInMinutes * 60000) + (result.seconds * 1000);
                            GOVUK.clearAllTimeoutTimers();
                            GOVUK.setInitialSessionTimeoutModalTimer(resetTimmerInMilliSeconds);
                        }
                        else if (result.minutes == defaultValueToShowTimeoutModalInMinutes && result.seconds > 2) {
                            GOVUK.clearAllTimeoutTimers();
                            GOVUK.setInitialSessionTimeoutModalTimer(result.seconds * 1000);
                        }
                        else if (result.minutes == defaultValueToShowTimeoutModalInMinutes && result.seconds <= 2) {
                            GOVUK.clearAllTimeoutTimers();
                            GOVUK.setSessionTimeoutModalTimer(result.seconds * 1000);
                        }
                        else if (result.minutes < defaultValueToShowTimeoutModalInMinutes) {
                            GOVUK.clearAllTimeoutTimers();
                            GOVUK.setSessionTimeoutModalTimer(0);
                        }
                    }
                    else {
                        if (timerMinutesValue == 0 && result.minutes == 0 && result.seconds > timerSecondsValue) {
                            GOVUK.clearAllTimeoutTimers();
                            GOVUK.setSessionTimeoutCountDownTimer(0, result.seconds);
                        }
                        else if (timerMinutesValue == 0 && (result.minutes > 0 && result.minutes < defaultValueToShowTimeoutModalInMinutes)) {
                            GOVUK.clearAllTimeoutTimers();
                            GOVUK.setSessionTimeoutCountDownTimer(result.minutes, result.seconds);
                        }
                        else if (timerMinutesValue == 0 && result.minutes > defaultValueToShowTimeoutModalInMinutes) {
                            var diffInMinutes = result.minutes - defaultValueToShowTimeoutModalInMinutes;
                            var resetTimmerInMilliSeconds = (diffInMinutes * 60000) + (result.seconds * 1000);
                            GOVUK.hideTimeoutModal();
                            GOVUK.clearAllTimeoutTimers();
                            GOVUK.setInitialSessionTimeoutModalTimer(resetTimmerInMilliSeconds);
                        }
                        else if (timerMinutesValue == 0 && result.minutes == defaultValueToShowTimeoutModalInMinutes && result.seconds > 2) {
                            GOVUK.hideTimeoutModal();
                            GOVUK.clearAllTimeoutTimers();
                            GOVUK.setInitialSessionTimeoutModalTimer(result.seconds * 1000);
                        }
                        else if (timerMinutesValue == 0 && result.minutes == defaultValueToShowTimeoutModalInMinutes && result.seconds <= 2) {
                            GOVUK.clearAllTimeoutTimers();
                            GOVUK.resetSessionTimeoutModalTimer(result.seconds * 1000);
                        }
                    }
                }
            },
            timeout: 3000,
            error: function (xhr, textStatus, errorThrown) {
                alert('error: ' + xhr + textStatus + errorThrown);
                if (textStatus != "abort")
                    console.log(xhr + textStatus + errorThrown);
            },
            complete: function (d) {
                currentGetSessionActivityDurationXhr = null;
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
                    var renewTimeoutValueInMs = ((result.minutes - defaultValueToShowTimeoutModalInMinutes) * 60000) + (result.seconds * 1000);
                    GOVUK.hideTimeoutModal();
                    GOVUK.clearAllTimeoutTimers();
                    GOVUK.setInitialSessionTimeoutModalTimer(renewTimeoutValueInMs);
                }
            },
            timeout: 3000,
            error: function (xhr, textStatus, errorThrown) {
                alert('error: ' + xhr + textStatus + errorThrown);
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
        GOVUK.clearAllTimeoutTimers();
        var timeoutValueInMinutes = $('#timeout-message-container').data('timeout');
        if (timeoutValueInMinutes > defaultValueToShowTimeoutModalInMinutes) {
            GOVUK.setInitialSessionTimeoutModalTimer((timeoutValueInMinutes - defaultValueToShowTimeoutModalInMinutes) * 60000);
        }
    }
});