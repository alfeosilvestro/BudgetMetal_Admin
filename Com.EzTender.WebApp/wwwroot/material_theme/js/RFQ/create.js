

$("#btnNewRequirement").click(function () {
    var detailRequirementLastId = $("#detailRequirementLastId").val();

    var newRow = "<tr>" +
        "<td> " +
        "<input type='text' name='Requirement[" + detailRequirementLastId + "].ServiceName' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<input type='text' name='Requirement[" + detailRequirementLastId + "].Description' class='form-control' />" +
        "</td> " +
        "<td>" +
        "<input type='text' name='Requirement[" + detailRequirementLastId + "].Quantity' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<button type='button' class='btn btn-danger waves-effect' onclick='RemoveRequirementRow(this)'> Remove</button >" +
        "</td> " +
        "</tr> ";

    $("#requirementsTable > tbody").append(newRow);
    detailRequirementLastId = parseInt(detailRequirementLastId) + 1;
    $("#detailRequirementLastId").val(detailRequirementLastId);
});

function RemoveRequirementRow(item) {
    $(item).parent().parent().remove();
}

// Add and Remove SLA
$("#btnNewSLA").click(function () {
    var detailSLALastId = $("#detailSLALastId").val();

    var newRow = "<tr>" +
        "<td> " +
        "<input type='text' name='Sla[" + detailSLALastId + "].Requirement' class='form-control' />" +
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
});

function RemoveSLARow(item) {
    $(item).parent().parent().remove();
}

// Add and Remove Penalty
$("#btnNewPenalty").click(function () {
    var PenaltyLastId = $("#PenaltyLastId").val();

    var newRow = "<tr>" +
        "<td> " +
        "<input type='text' name='Penalty[" + PenaltyLastId + "].BreachOfServiceDefinition' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<input type='text' name='Penalty[" + PenaltyLastId + "].PenaltyAmount' class='form-control' />" +
        "</td> " +
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
});

function RemovePenaltyRow(item) {
    $(item).parent().parent().remove();
}


// Add and Remove Pricing
$("#btnNewPricing").click(function () {
    var PricingLastId = $("#PricingLastId").val();
    var newRow = "<tr>" +
        "<td> " +
        "<input type='text' name='RfqPriceSchedule[" + PricingLastId + "].ItemName' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<input type='text' name='RfqPriceSchedule[" + PricingLastId + "].ItemDescription' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<input type='text' name='RfqPriceSchedule[" + PricingLastId + "].InternalRefrenceCode' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<input type='text' name='RfqPriceSchedule[" + PricingLastId + "].QuantityRequired' class='form-control' />" +
        "</td> " +
        "<td> " +
        "<button type='button' class='btn btn-danger waves-effect' onclick='RemovePricingRow(this)'> Remove</button >" +
        "</td> " +
        "</tr> ";

    $("#pricingTable > tbody").append(newRow);
    PricingLastId = parseInt(PricingLastId) + 1;
    $("#PricingLastId").val(PricingLastId);
});

function RemovePricingRow(item) {
    $(item).parent().parent().remove();
}

