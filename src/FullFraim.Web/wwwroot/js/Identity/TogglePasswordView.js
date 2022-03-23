// Toggle eye icon on password field and obfuscate/show password text on login screen.
$(document).ready(function () {
    $("#show_hide_password a").on('click', function (event) {
        event.preventDefault();
        if ($('#show_hide_password input').attr("type") == "text") {
            $('#show_hide_password input').attr('type', 'password');
            $('#show_hide_password #opened').removeAttr("hidden");
            $('#show_hide_password #closed').attr("hidden", "");
        } else if ($('#show_hide_password input').attr("type") == "password") {
            $('#show_hide_password input').attr('type', 'text');
            $('#show_hide_password #opened').attr("hidden", "");
            $('#show_hide_password #closed').removeAttr("hidden");
        }
    });
});