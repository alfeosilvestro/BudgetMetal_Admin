

$("#btnNewRequirement").click(function () {
    var detailRequirementLastId = $("#detailRequirementLastId").val();

    var newRow = "<tr>" +
        "<td></td><td> " +
        "<input type='text' name='Requirement[" + detailRequirementLastId + "].ServiceName' class='form-control requirement' />" +
        "</td> " +
        "<td> " +
        "<input type='text' name='Requirement[" + detailRequirementLastId + "].Description' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<button type='button' class='btn btn-danger waves-effect' onclick='RemoveRequirementRow(this)'> Remove</button >" +
        "</td> " +
        "</tr> ";

    $("#requirementsTable > tbody").append(newRow);
    detailRequirementLastId = parseInt(detailRequirementLastId) + 1;
    $("#detailRequirementLastId").val(detailRequirementLastId);
    FillSerialNumber("requirementsTable");

});

function RemoveRequirementRow(item) {
    $(item).parent().parent().remove();    
    FillSerialNumber("requirementsTable");
}

// Add and Remove SLA
$("#btnNewSLA").click(function () {
    var detailSLALastId = $("#detailSLALastId").val();

    var newRow = "<tr>" +
        "<td></td><td> " +
        "<input type='text' name='Sla[" + detailSLALastId + "].Requirement' class='form-control support' />" +
        "</td> " +
        "<td> " +
        "<input type='text' name='Sla[" + detailSLALastId + "].Description' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<button type='button' class='btn btn-danger waves-effect' onclick='RemoveSLARow(this)'> Remove</button >" +
        "</td> " +
        "</tr> ";

    $("#slaTable > tbody").append(newRow);
    detailSLALastId = parseInt(detailSLALastId) + 1;
    $("#detailSLALastId").val(detailSLALastId);
    FillSerialNumber("slaTable");
});

function RemoveSLARow(item) {
    $(item).parent().parent().remove();
    FillSerialNumber("slaTable");
}

// Add and Remove Penalty
$("#btnNewPenalty").click(function () {
    var PenaltyLastId = $("#PenaltyLastId").val();

    var newRow = "<tr>" +
        "<td></td><td> " +
        "<input type='text' name='Penalty[" + PenaltyLastId + "].BreachOfServiceDefinition' class='form-control commercial' />" +
        "</td> " +
        //"<td> " +
        //"<input type='text' name='Penalty[" + PenaltyLastId + "].PenaltyAmount' class='form-control' />" +
        //"</td> " +
        "<td> " +
        "<input type='text' name='Penalty[" + PenaltyLastId + "].Description' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<button type='button' class='btn btn-danger waves-effect' onclick='RemovePenaltyRow(this)'> Remove</button >" +
        "</td> " +
        "</tr> ";

    $("#penaltyTable > tbody").append(newRow);
    PenaltyLastId = parseInt(PenaltyLastId) + 1;
    $("#PenaltyLastId").val(PenaltyLastId);
    FillSerialNumber("penaltyTable");
});

function RemovePenaltyRow(item) {
    $(item).parent().parent().remove();
    FillSerialNumber("penaltyTable");
}


// Add and Remove Pricing
$("#btnNewPricing").click(function () {
    var PricingLastId = $("#PricingLastId").val();
    var newRow = "<tr>" +
        "<td></td><td> " +
        "<input type='text' name='RfqPriceSchedule[" + PricingLastId + "].ItemName' class='form-control pricing' />" +
        "</td> " +
        "<td> " +
        "<input type='text' name='RfqPriceSchedule[" + PricingLastId + "].ItemDescription' class='form-control' />" +
        "</td> " +
        //"<td> " +
        //"<input type='text' name='RfqPriceSchedule[" + PricingLastId + "].InternalRefrenceCode' class='form-control' />" +
        //"</td> " +
        "<td> " +
        "<input type='text' name='RfqPriceSchedule[" + PricingLastId + "].QuantityRequired' class='form-control pricingQty' /> <input type='hidden' name='RfqPriceSchedule[" + PricingLastId + "].CategoryId' value='100081' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<button type='button' class='btn btn-danger waves-effect' onclick='RemovePricingRow(this)'> Remove</button >" +
        "</td> " +
        "</tr> ";

    $("#pricingTable > tbody").append(newRow);
    PricingLastId = parseInt(PricingLastId) + 1;
    $("#PricingLastId").val(PricingLastId);
    FillSerialNumber("pricingTable");
});

function RemovePricingRow(item) {
    $(item).parent().parent().remove();
    FillSerialNumber("pricingTable");
    FillSerialNumber("pricingServiceTable");
    FillSerialNumber("pricingWarrantyTable");
}

$("#btnServicePricing").click(function () {
    var PricingLastId = $("#PricingLastId").val();
    var newRow = "<tr>" +
        "<td></td><td> " +
        "<input type='text' name='RfqPriceSchedule[" + PricingLastId + "].ItemName' class='form-control pricing' />" +
        "</td> " +
        "<td> " +
        "<input type='text' name='RfqPriceSchedule[" + PricingLastId + "].ItemDescription' class='form-control' />" +
        "</td> " +
        //"<td> " +
        //"<input type='text' name='RfqPriceSchedule[" + PricingLastId + "].InternalRefrenceCode' class='form-control' />" +
        //"</td> " +
        "<td> " +
        "<input type='text' name='RfqPriceSchedule[" + PricingLastId + "].QuantityRequired' class='form-control pricingQty' /><input type='hidden' name='RfqPriceSchedule[" + PricingLastId + "].CategoryId' value='100082' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<button type='button' class='btn btn-danger waves-effect' onclick='RemovePricingRow(this)'> Remove</button >" +
        "</td> " +
        "</tr> ";

    $("#pricingServiceTable > tbody").append(newRow);
    PricingLastId = parseInt(PricingLastId) + 1;
    $("#PricingLastId").val(PricingLastId);
    FillSerialNumber("pricingServiceTable");
});

$("#btnWarrantyPricing").click(function () {
    var PricingLastId = $("#PricingLastId").val();
    var newRow = "<tr>" +
        "<td></td><td> " +
        "<input type='text' name='RfqPriceSchedule[" + PricingLastId + "].ItemName' class='form-control pricing' />" +
        "</td> " +
        "<td> " +
        "<input type='text' name='RfqPriceSchedule[" + PricingLastId + "].ItemDescription' class='form-control' />" +
        "</td> " +
        //"<td> " +
        //"<input type='text' name='RfqPriceSchedule[" + PricingLastId + "].InternalRefrenceCode' class='form-control' />" +
        //"</td> " +
        "<td> " +
        "<input type='text' name='RfqPriceSchedule[" + PricingLastId + "].QuantityRequired' class='form-control pricingQty' /> <input type='hidden' name='RfqPriceSchedule[" + PricingLastId + "].CategoryId' value='100083' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<button type='button' class='btn btn-danger waves-effect' onclick='RemovePricingRow(this)'> Remove</button >" +
        "</td> " +
        "</tr> ";

    $("#pricingWarrantyTable > tbody").append(newRow);
    PricingLastId = parseInt(PricingLastId) + 1;
    $("#PricingLastId").val(PricingLastId);
    FillSerialNumber("pricingWarrantyTable");
});

// Add and Remove Attachment
$("#btnNewFile").click(function () {
    var newRow = "<tr>" +
        "<td></td><td> " +
        "<input type='file' name='attachmentRFQ[]' class='form-control' onchange='getFilename(this)' />" +
        "</td> " +
        "<td> " +
        "<input type='text' name='fileDescriptionRFQ[]' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<button type='button' class='btn btn-danger waves-effect' onclick='RemoveAttachmentRFQRow(this)'> Remove</button >" +
        "</td> " +
        "</tr> ";

    $("#attachmentTable > tbody").append(newRow);
});

function RemoveAttachmentRFQRow(item) {
    $(item).parent().parent().remove();
}


$("input:file").change(function () {
    var fileName = $(this).val();
    alert(fileName);
});

function getFileName(elm) {
    var fn = $(elm).val();
    var filename = fn.match(/[^\\/]*$/)[0]; // remove C:\fakename
    alert(filename);
}

$("#btnRfqEmailsInvites").click(function () {
    var detailEmalsInviteLastId = $("#detailEmalsInviteLastId").val();

    var newRow = "<tr>" +
        "<td></td><td> " +
        "<input type='text' name='RfqEmailInvites[" + detailEmalsInviteLastId + "].Name' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<input type='email' placeholder='Enter your email' name='RfqEmailInvites[" + detailEmalsInviteLastId + "].EmailAddress' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<button type='button' class='btn btn-danger waves-effect' onclick='RemoveRfqEmailInvites(this)'> Remove</button >" +
        "</td> " +
        "</tr> ";

    $("#reqEmailInvitesTable > tbody").append(newRow);
    detailRequirementLastId = parseInt(detailEmalsInviteLastId) + 1;
    $("#detailEmalsInviteLastId").val(detailEmalsInviteLastId);
});

function RemoveRfqEmailInvites(item) {
    $(item).parent().parent().remove();
}


function FillSerialNumber(tbName) {
    var i = 1;
    $('#' + tbName + ' > tbody  > tr > td:first-child').each(function () {
        $(this).html(i);
        i++;
    });
}