$(document).ready(function () {
    $("#searchBox").on("keyup", function () {
        var searchText = $(this).val().toLowerCase();
        $("table tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
        });
    });
});