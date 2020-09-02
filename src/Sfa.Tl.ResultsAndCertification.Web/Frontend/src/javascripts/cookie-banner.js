(function () {
    "use strict";
    var root = this;
    if (typeof root.GOVUK === 'undefined') { root.GOVUK = {}; }
    var signoutModalTimerHandle;
    var signoutCountDownTimerHandle;
    var currentGetTimeoutStatusXhr = null;
    var firstTabStop;
    var lastTabStop;
    /*
      Cookie methods
      ==============
  
      Usage:
  
        Setting a cookie:
        GOVUK.cookie('hobnob', 'tasty', { days: 30 });
  
        Reading a cookie:
        GOVUK.cookie('hobnob');
  
        Deleting a cookie:
        GOVUK.cookie('hobnob', null);
    */
    GOVUK.cookie = function (name, value, options) {
        if (typeof value !== 'undefined') {
            if (value === false || value === null) {
                return GOVUK.setCookie(name, '', { days: -1 });
            } else {
                return GOVUK.setCookie(name, value, options);
            }
        } else {
            return GOVUK.getCookie(name);
        }
    };
    GOVUK.setCookie = function (name, value, options) {
        if (typeof options === 'undefined' || options === null) {
            options = {};
        }
        var cookieString = name + "=" + value + "; path=/";
        if (options.days) {
            var date = new Date();
            date.setTime(date.getTime() + (options.days * 24 * 60 * 60 * 1000));
            cookieString = cookieString + "; expires=" + date.toGMTString();
        }
        if (document.location.protocol === 'https:') {
            cookieString = cookieString + "; Secure";
        }
        document.cookie = cookieString;
    };
    GOVUK.getCookie = function (name) {
        var nameEq = name + "=";
        var cookies = document.cookie.split(';');
        for (var i = 0, len = cookies.length; i < len; i++) {
            var cookie = cookies[i];
            while (cookie.charAt(0) === ' ') {
                cookie = cookie.substring(1, cookie.length);
            }
            if (cookie.indexOf(nameEq) === 0) {
                return decodeURIComponent(cookie.substring(nameEq.length));
            }
        }
        return null;
    };
    GOVUK.acceptAllCookies = function (value) {
        GOVUK.cookie('cookies_preferences_set', value, { days: 365 });
        GOVUK.cookie('analytics_consent', value, { days: 365 });
    };

    GOVUK.handleKeydownEventsForModal = function () {

        //var elementsThatAreFocusable = document.querySelector('.tl-modal').querySelectorAll('a');
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

    GOVUK.clearSignoutModalTimer = function () {
        clearTimeout(signoutModalTimerHandle);
    };

    GOVUK.setSignoutModalTimer = function () {
        signoutModalTimerHandle = setTimeout(function () {
            $("#timeout-message").removeClass('hidden');
            $("#keepMeSignedIn").focus();
            $("#skipToMainContent").attr("tabindex", "-1");
            GOVUK.handleKeydownEventsForModal();
            GOVUK.setSignoutCountDownTimer(3, 0);
        }, 10000);
    };

    $("#keepMeSignedIn").click(function () {
        // TODO: renew session and on success call
        GOVUK.resetSignoutModalTimer();
    });

    GOVUK.resetSignoutModalTimer = function () {
        GOVUK.clearSignoutModalTimer();
        $("#timeout-message").addClass('hidden');
        $("#skipToMainContent").removeAttr("tabindex");
        GOVUK.setSignoutModalTimer();
    };

    GOVUK.setSignoutCountDownTimer = function (minutes, seconds) {
        function startSignoutCountDownTimer() {
            var minutesCounterElement = $("#minutesCounter"), secondsCounterElement = $("#secondsCounter");
            minutesCounterElement.text(minutes > 0 ? minutes.toString() + (minutes === 1 ? " minute" : " minutes") : "");
            secondsCounterElement.text(seconds > 0 ? (seconds < 10 ? "0" : "") + seconds.toString() + " seconds" : "");

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
                            GOVUK.setSignoutCountDownTimer(minutes - 1, 59);
                        }, 1000);
                    }
                }
            }
        }
        startSignoutCountDownTimer();
    }

    GOVUK.checkTimeoutStatus = function () {
        if (currentGetTimeoutStatusXhr != null)
            currentGetTimeoutStatusXhr.abort();

        currentGetTimeoutStatusXhr = $.ajax({
            type: "get",
            url: "/timeout-status",
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

}).call(this);
(function () {
    "use strict";
    var root = this;
    if (typeof root.GOVUK === 'undefined') { root.GOVUK = {}; }

    GOVUK.addCookieMessage = function () {
        var cookieBannerContainerElement = document.getElementById('tl-cookie-banner-container'),
            cookieMessageContainerElement = document.getElementById('tl-cookie-message-container'),
            cookieConfirmationContainerElement = document.getElementById('tl-cookie-confirmation-container'),
            showCookieMessage = (cookieBannerContainerElement && GOVUK.cookie('cookies_preferences_set') === null) &&
                (document.getElementById('tl-cookie-preferences') === null);

        if (showCookieMessage) {
            cookieBannerContainerElement.style.display = 'block';

            $('#accept-all-cookies').click(function (e) {
                GOVUK.acceptAllCookies(true);
                cookieMessageContainerElement.style.display = 'none';
                cookieConfirmationContainerElement.style.display = 'block';
                cookieConfirmationContainerElement.setAttribute("role", "alert");
                e.preventDefault();
            });

            $('#hide-cookie-confirmation').click(function (e) {
                cookieConfirmationContainerElement.removeAttribute("role");
                cookieConfirmationContainerElement.style.display = 'none';
                cookieBannerContainerElement.style.display = 'none';
                e.preventDefault();
            });
        }
    };
}).call(this);
(function () {
    "use strict";

    // add cookie message
    if (window.GOVUK && GOVUK.addCookieMessage) {
        GOVUK.addCookieMessage();
    }

    if (window.GOVUK) {             
        GOVUK.setSignoutModalTimer();
    }
}).call(this);