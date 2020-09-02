"use strict";
$(document).ready(function () {
    var root = this;
    if (typeof root.GOVUK === 'undefined') { root.GOVUK = {}; }

    var firstTabStop;
    var lastTabStop;
    var signoutModalTimerHandle;
    var signoutCountDownTimerHandle;
    var currentGetTimeoutStatusXhr = null;

    GOVUK.clearSessionTimeoutModalTimer = function () {
        clearTimeout(signoutModalTimerHandle);
    };

    GOVUK.setSessionTimeoutModalTimer = function () {
        signoutModalTimerHandle = setTimeout(function () {
            $("#timeout-message-container").removeClass('hidden');
            $("#keep-me-signed-in").focus();
            $("#skipToMainContent").attr("tabindex", "-1");
            GOVUK.handleKeydownEventsForModal();
            GOVUK.setSessionTimeoutCountDownTimer(3, 0);
        }, 10000);
    };

    GOVUK.resetSessionTimeoutModalTimer = function () {
        GOVUK.clearSessionTimeoutModalTimer();
        $("#timeout-message-container").addClass('hidden');
        $("#skipToMainContent").removeAttr("tabindex");
        GOVUK.setSessionTimeoutModalTimer();
    };

    GOVUK.setSessionTimeoutCountDownTimer = function (minutes, seconds) {
        function startSignoutCountDownTimer() {
            var minutesCounterElement = $("#minutes-counter"), secondsCounterElement = $("#seconds-counter");
            minutesCounterElement.text(minutes > 0 ? minutes.toString() + (minutes === 1 ? " minute" : " minutes") : "");
            secondsCounterElement.text(seconds > 0 ? seconds.toString() + " seconds" : "");

            if (minutes === 0 && seconds === 0) {
                clearTimeout(signoutModalTimerHandle);
                clearTimeout(signoutCountDownTimerHandle);
                //window.location.href = "/find-provider";
            }
            else {
                seconds--;
                if (seconds >= 0) {
                    signoutCountDownTimerHandle = setTimeout(startSignoutCountDownTimer, 1000);
                } else {
                    if (minutes >= 1) {
                        clearTimeout(signoutCountDownTimerHandle);
                        signoutCountDownTimerHandle = setTimeout(function () {
                            GOVUK.setSessionTimeoutCountDownTimer(minutes - 1, 59);
                        }, 1000);
                    }
                }
            }
        }
        startSignoutCountDownTimer();
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

    GOVUK.checkSessionActiveDuration = function () {
        if (currentGetTimeoutStatusXhr != null)
            currentGetTimeoutStatusXhr.abort();

        currentGetTimeoutStatusXhr = $.ajax({
            type: "get",
            url: "/timeout/active-duration",
            contentType: "application/json",
            success: function (result) {

            },
            timeout: 5000,
            error: function (xhr, textStatus, errorThrown) {
                if (textStatus != "abort")
                    console.log(xhr + textStatus + errorThrown);
            },
            complete: function (d) {
                currentGetTimeoutStatusXhr = null;
            }
        });
    }

    $("#keep-me-signed-in").click(function () {
        // TODO: renew session and on success call
        GOVUK.resetSessionTimeoutModalTimer();
    });

    if (window.GOVUK) {
        GOVUK.setSessionTimeoutModalTimer();
    }
});