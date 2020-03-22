$(document).ready(function () {

    var element = document.querySelector('#search');

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
                        process(data);
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
            },
        onConfirm: (val) => {
            console.log('you choose the value:' + val);
            if (val != null) {
                $('#selectedProviderId').val(val);
            }
        }
    });

    //$('#FindProviderForm').submit(function () {
    //    var typeHeadValue = $("input.autocomplete__input").val() 
    //    console.log(typeHeadValue);
    //    $('#search').val(typeHeadValue);
    //    $(this).unbind('submit').submit();
    //});
});