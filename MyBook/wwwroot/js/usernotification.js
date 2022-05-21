$(document).ready(function () {
    function notifyClient() {
        $.ajax({
            url: '/Home/CheckUserSubscr',
            type: "GET",
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Error" + errorThrown)
            },
            success: function (data) {
            }
        });
    }

    var userId = $('#userIdNot').val();
    if (userId != null) {
        var connection = new signalR.HubConnectionBuilder().withUrl("/NotificationUserHub?userId=" + userId).build();
        connection.on("sendToUser", (notificationHeading, notificationContent) => {
            $('.modal-title').html(notificationHeading);
            $('#modal-text').html(notificationContent);
            $('#modal').modal('show');
        });

        connection.start().catch(function (err) {
            return console.error(err.toString());
        }).then(function () {
            connection.invoke('GetConnectionId')
            notifyClient();
        });
    }
});