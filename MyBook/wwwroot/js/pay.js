$(document).ready(function () {
    $('#one').click(function () {
        $('#period').val(1);
        $('#monthsNumb').html("Кол-во месяцев: 1");
        var total = $('#price').val() * 1;
        $('#total').html(total + " руб.");
    });
    $('#three').click(function () {
        $('#period').val(3);
        $('#monthsNumb').html("Кол-во месяцев: 3");
        var total = $('#price').val() * 3;
        $('#total').html(total + " руб.");
    });
    $('#twelve').click(function () {
        $('#period').val(12);
        $('#monthsNumb').html("Кол-во месяцев: 12");
        var total = $('#price').val() * 12;
        $('#total').html(total + " руб.");
    });
});