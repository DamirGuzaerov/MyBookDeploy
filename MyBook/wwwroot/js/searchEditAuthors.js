let pageNumberAuthors = 1;
let searchStringAuthor = "";
function showMoreEditAuthors() {
    $.ajax(
        {
            type: "GET",
            url:`/Search/SearchEditAuthors?page=${++pageNumberAuthors}&searchString=${searchStringAuthor}`,
            success: function (data) {
                var $response=$(data);
                var resultDiv = $response.find("#list").html();
                $(".author_information").html(resultDiv);
            },
            error: function (data){
                alert("Чота не так пошло");
            }
        }
    )
    return false;
}

function searchEditAuthor(){
    let searchValue = $("#searchString").val();
    console.log("change",searchValue);
    searchStringAuthor = searchValue;
    $.ajax(
        {
            type: "GET",
            url:`/Search/SearchEditAuthors?page=${pageNumberAuthors}&searchString=${searchStringAuthor}`,
            success: function (data) {
                var $response=$(data);
                var resultDiv = $response.find("#list").html();
                $(".author_information").html(resultDiv);
            },
            error: function (data){
                alert("Что-то пошло не так, повторите попытку, елси проблема не исчезнет повторите через некоторое время");
            }
        }
    )
    return false;
}