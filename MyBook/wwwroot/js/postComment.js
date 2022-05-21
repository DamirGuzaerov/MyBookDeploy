function postComment(bookId){
    var rating = $(".stars:checked").val();
    var comment = $(".comment").val();
    $.ajax(
        {
            type: "GET",
            url: `/Book/PostComment?rating=${rating}&comment=${comment}&bookId=${bookId}`,
            error: function (data) {
                console.error("Что-то пошло не так, повторите попытку, елси проблема не исчезнет повторите через некоторое время");
            }
        }
    )
    return false;
}