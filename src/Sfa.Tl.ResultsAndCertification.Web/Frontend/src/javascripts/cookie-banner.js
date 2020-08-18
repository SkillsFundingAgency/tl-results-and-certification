﻿(function () {
    "use strict";
    var root = this;
    if (typeof root.GOVUK === 'undefined') { root.GOVUK = {}; }

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
                cookieConfirmationContainerElement.style.display = 'inline-block';
                e.preventDefault();
            });

            $('#hide-cookie-confirmation').click(function (e) {
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
}).call(this);