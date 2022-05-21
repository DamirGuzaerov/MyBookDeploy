let pageNumber = 1;
let searchString = "";
function showMoreAuthors(){
    $.ajax(
        {
            type: "GET",
            url:`/Search/SearchAuthors?page=${++pageNumber}`,
            success: function (data) {
                var $response=$(data);
                var resultDiv = $response.find("#list").html();
                $("#list").html(resultDiv);
            },
            error: function (data){
                alert("Чота не так пошло");
            }
        }
    )
}

function searchEditAuthor(){
    searchValue = $("#searchString").val();
    console.log("change",searchValue);
    searchString = searchValue;
    $.ajax(
        {
            type: "GET",
            url:`/Search/SearchEditAuthors?page=${pageNumber}&searchString=${searchString}`,
            success: function (data) {
                var $response=$(data);
                var resultDiv = $response.find("#list").html();
                $(".books_information").html(resultDiv);
            },
            error: function (data){
                alert("Что-то пошло не так, повторите попытку, елси проблема не исчезнет повторите через некоторое время");
            }
        }
    )
}
