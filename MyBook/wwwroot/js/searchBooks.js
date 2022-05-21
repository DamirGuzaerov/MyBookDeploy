let pageNumber = 1;
function showMoreBooks(){
    $.ajax(
        {
            type: "GET",
            url:`/Search/SearchBooks?page=${++pageNumber}`,
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
    return false;
}


