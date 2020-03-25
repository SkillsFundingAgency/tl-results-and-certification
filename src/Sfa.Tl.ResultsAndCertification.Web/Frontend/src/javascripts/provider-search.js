"use strict";
$(document).ready(function () {

    var providerIds = null;
    var providerNames = null;
    var currentGetProviderSearchXhr = null;

    accessibleAutocomplete.enhanceSelectElement({
        defaultValue: $("#previousSearch").val(),
        autoSelect: true,
        selectElement: document.querySelector('#search'),
        minLength: 3,
        name: "Search",
        source:
            function (query, process) {
                if (currentGetProviderSearchXhr != null)
                    currentGetProviderSearchXhr.abort();

                currentGetProviderSearchXhr = $.ajax({
                    type: "get",
                    url: "search-provider/" + query,
                    contentType: "json",
                    timeout: 3000,
                    success: function (data) {
                        // initialise/reset 
                        providerNames = [];
                        providerIds = new Object();

                        $.each(data, function (idx, provider) {
                            providerNames.push(provider.displayName);
                            providerIds[provider.displayName] = provider.id;
                        });

                        process(providerNames);
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        if (textStatus != "abort")
                            console.log(xhr + textStatus + errorThrown);
                    },
                    complete: function (d) {
                        currentGetProviderSearchXhr = null;
                        $('#SelectedProviderId').val(0);
                    }
                });
            },
        onConfirm: (val) => {
            if (val != null) {
                $('#SelectedProviderId').val(providerIds[val]);
            }
            else {
                $('#SelectedProviderId').val(0);
            }
        }
    });

    $("#search").attr('maxlength', '400');
});