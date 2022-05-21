$(document).ready(function () {
    function notifyAll() {
        $.ajax({
            url: '/Home/NotifyAll', //TODO change url if we need this func
            type: "GET",
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Error" + errorThrown)
            },
            success: function (data) {
            }
        });
    }

    var connection = new signalR.HubConnectionBuilder().withUrl("/NotificationHub").build();
    connection.on("sendToUser", (notificationHeading, notificationContent) => {
        $('.modal-title').html(notificationHeading);
        $('#modal-text').html(notificationContent);
        $('#modal').modal('show');
    });

    connection.start().catch(function (err) {
        return console.error(err.toString());
    }).then(function () {
        //  notifyAll();
    });
});