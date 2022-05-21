
$('body').on('click', '.password-control', function () {
    if ($('#surnameForm').attr('type') == 'password') {
        $(this).addClass('view');
        $('#surnameForm').attr('type', 'text');
    } else {
        $(this).removeClass('view');
        $('#surnameForm').attr('type', 'password');
    }
    return false;
});