$(document).ready(function () {

    var element = document.querySelector('#search');

    var providerNames = new Array();
    var providerIds = new Object();
    var searchResults = new Array();

    $('#FindProviderForm').submit(function () {
        var typeHeadValue = $("input.autocomplete__input").val();

        var isFound = false;
        $.each(searchResults, function (idx, val) {
            if (val.displayName == typeHeadValue) {
                isFound = true;
                return;
            }
        });

        if (!isFound)
            $('#SelectedProviderId').val(0);

        $(this).unbind('submit').submit();
    });

    accessibleAutocomplete.enhanceSelectElement({
        defaultValue: '',
        autoSelect: true,
        selectElement: element,
        minLength: 3,
        name: "Search",
        source:
            function (query, process) {
                $.ajax({
                    url: "search-provider/" + query,
                    type: "get",
                    contentType: "json",
                    success: function (data) {
                        // initialise/reset 
                        providerNames = [];
                        providerIds = new Object();
                        $('#SelectedProviderId').val(0);
                        searchResults = data;

                        $.each(data, function (idx, provider) {
                            providerNames.push(provider.displayName);
                            providerIds[provider.displayName] = provider.id;
                        });

                        process(providerNames);
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
            },
        onConfirm: (val) => {
            if (val != null) {
                var id = providerIds[val];
                $('#SelectedProviderId').val(id);
                console.log('you choose: ' + val + ' id is: ' + id);
            }
            else
                $('#SelectedProviderId').val(0);
        }
    });
});