$(document).ready(function() {
    $('#containerAuthor').hide();
    $('#containerSubscription').hide();
    $('#containerBook').hide();
    $('#containerBookCenter').hide();
    $('#containerUser').hide();
    switch ($('#current').val()) {
        case "Author":
            $('#containerAuthor').show();
            break;
        case "Book":
            $('#containerBook').show();
            break;
        case "BookCenter":
            $('#containerBookCenter').show();
            break;
        case "User":
            $('#containerUser').show();
            break;
        case "Subscription":
            $('#containerSubscription').show();
            break;
        default:
            $('#containerSubscription').show();
    }
    $('#book').click(function() {
        $('#containerAuthor').hide();
        $('#containerSubscription').hide();
        $('#containerBook').show();
        $('#containerBookCenter').hide();
        $('#containerUser').hide();
        return false;
    });

    $('#author').click(function() {
        $('#containerAuthor').show();
        $('#containerSubscription').hide();
        $('#containerBook').hide();
        $('#containerBookCenter').hide();
        $('#containerUser').hide();
        return false;
    });


    $('#subscr').click(function() {
        $('#containerAuthor').hide();
        $('#containerSubscription').show();
        $('#containerBook').hide();
        $('#containerBookCenter').hide();
        $('#containerUser').hide();
        return false;
    });


    $('#bookCenter').click(function() {
        $('#containerAuthor').hide();
        $('#containerSubscription').hide();
        $('#containerBook').hide();
        $('#containerBookCenter').show();
        $('#containerUser').hide();
        return false;
    });


    $('#user').click(function() {
        $('#containerAuthor').hide();
        $('#containerSubscription').hide();
        $('#containerBook').hide();
        $('#containerBookCenter').hide();
        $('#containerUser').show();
        return false;
    });
});