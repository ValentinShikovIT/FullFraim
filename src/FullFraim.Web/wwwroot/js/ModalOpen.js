$(document).ready(function () {
    // Ajax GET to open modal for creating project.
    showInPopup = (url, title) => {
        $.ajax({
            type: 'GET',
            url: url,
            success: function (res) {
                $('.modal-body').html(res);
                $('.modal-title').html(title);
            }
        });
    }
});