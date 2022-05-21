let pageNumberUsers = 1;
let searchStringUser = "";
function showMoreEditUsers() {
    $.ajax(
        {
            type: "GET",
            url:`/Search/SearchEditUsers?page=${++pageNumberUsers}&searchString=${searchStringUser}`,
            success: function (data) {
                var $response=$(data);
                var resultDiv = $response.find("#list").html();
                $(".users_information").html(resultDiv);
            },
            error: function (data){
                alert("Что-то пошло не так, повторите попытку, елси проблема не исчезнет повторите через некоторое время");
            }
        }
    )
    return false;
}

function searchEditUser(){
    let searchValue = $("#searchStringUsers").val();
    console.log("change",searchValue);
    searchStringUser = searchValue;
    $.ajax(
        {
            type: "GET",
            url:`/Search/SearchEditUsers?page=${pageNumberUsers}&searchString=${searchStringUser}`,
            success: function (data) {
                var $response=$(data);
                var resultDiv = $response.find("#list").html();
                $(".users_information").html(resultDiv);
            },
            error: function (data){
                alert("Что-то пошло не так, повторите попытку, елси проблема не исчезнет повторите через некоторое время");
            }
        }
    )
    return false;
}