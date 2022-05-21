$(document).ready(function () {
    $('#containerProfile').show();
    $('#containerSubscr').hide();
    $('#containerHistory').hide();
    $('#containerRecommends').hide();

    $('#subscr').click(function () {
        $('#containerProfile').hide();
        $('#containerSubscr').show();
        $('#containerHistory').hide();
        $('#containerRecommends').hide();
        return false;
    });

    $('#profile').click(function () {
        $('#containerProfile').show();
        $('#containerSubscr').hide();
        $('#containerHistory').hide();
        $('#containerRecommends').hide();
    });


    $('#history').click(function () {
        $('#containerProfile').hide();
        $('#containerSubscr').hide();
        $('#containerHistory').show();
        $('#containerRecommends').hide();
    });

    $('#recommends').click(function () {
        $('#containerProfile').hide();
        $('#containerSubscr').hide();
        $('#containerHistory').hide();
        $('#containerRecommends').show();
    });
});