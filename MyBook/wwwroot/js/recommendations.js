let pageNumber = 1;

function getNextBooks(){
    $.ajax(
        {
            type: "GET",
            url:`/UserProfile/GetRecommendationsPaginated?page=${++pageNumber}`,
            success: function (data) {
                var $response=$(data);
                var resultDiv = $response.find("#list").html();
                $(".recommendations__container").html(resultDiv);
            },
            error: function (data){
                alert("Что-то пошло не так, повторите попытку, елси проблема не исчезнет повторите через некоторое время");
            }
        }
    )
}
