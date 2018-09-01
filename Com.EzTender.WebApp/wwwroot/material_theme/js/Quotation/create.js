
// Add and Remove Attachment
$("#btnNewFile").click(function () {
    var newRow = "<tr>" +
        "<td> " +
        "<input type='file' name='attachmentQuotation[]' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<input type='text' name='fileDescriptionQuotation[]' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<button type='button' class='btn btn-danger waves-effect' onclick='RemoveAttachmentRow(this)'> Remove</button >" +
        "</td> " +
        "</tr> ";

    $("#attachmentTable > tbody").append(newRow);
});

function RemoveAttachmentRow(item) {
    $(item).parent().parent().remove();
}