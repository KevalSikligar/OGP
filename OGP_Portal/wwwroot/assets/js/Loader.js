

$(document).ajaxStart(function () {
    // Show image container
    $("#AjaxLoader").show();
});
$(document).ajaxComplete(function () {
    // Hide image container
    $("#AjaxLoader").hide();
});