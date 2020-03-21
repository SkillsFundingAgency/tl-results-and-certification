$(document).ready(function () {

    var element = document.querySelector('#search-autocomplete-container');
    var id = 'search-autocomplete';

    accessibleAutocomplete({
        element: element,
        id: id,
        minLength: 3,
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
            $('#search').val(val);
        }
    });

    //$('#FindProviderForm').submit(function () {
    //    var typeHeadValue = $("input.autocomplete__input").val() 
    //    console.log(typeHeadValue);
    //    $('#search').val(typeHeadValue);
    //    $(this).unbind('submit').submit();
    //});
});