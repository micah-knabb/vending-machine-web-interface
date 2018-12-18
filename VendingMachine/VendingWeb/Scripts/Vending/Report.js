$(document).ready(function () {
    let ajaxURL = vndrServerUrl + "api/report"; // <--- changes depending on port used
    ajaxCall();
    $(".active").removeClass("active");
    $(".reportNav").addClass("active");
    $("#reportYear").change(function () {
        //alert($("#reportYear").val());
        $(".headerRow").nextAll("div").remove();
        ajaxCall();

    });
    $("#users").change(function () {
        ajaxCall();
    })
    function ajaxCall() {
        $(".headerRow").nextAll("div").remove();
        let userSelection = null
        if ($("#users").val() != 'all') {
            userSelection = $('#users').val();
        }
        $.ajax({
            url: ajaxURL,
            type: "GET",
            dataType: "json",
            data: {
                year: Number($("#reportYear").val()),
                userId: userSelection
            }

        }).done(function (data) {
            for (let i = 0; i < data.ReportItems.length; i++) {
                let reportBlock = $("<div>").addClass("row");
                reportBlock.append($("<div>").addClass("col-sm-3"));
                let product = $("<div>").text(data.ReportItems[i].Name).addClass("col").addClass("content");
                let quantity = $("<div>").text(data.ReportItems[i].Qty).addClass("col").addClass("qtySold").addClass("content");

                reportBlock.append(product);
                reportBlock.append(quantity);
                reportBlock.append($("<div>").addClass("col-sm-3"));

                $("#reportBox").append(reportBlock);
            }
            let salesRow = $("<div>").addClass("row").addClass("salesRow");
            salesRow.append($("<div>").addClass("col-sm-3"));
            let totalSalesTxt = $("<div>").text("Total Sales").addClass("col");
            let totalSales = $("<div>").text("$" + data.TotalSales.toFixed(2)).addClass("col").addClass("qtySold");
            salesRow.append(totalSalesTxt);
            salesRow.append(totalSales);
            salesRow.append($("<div>").addClass("col-sm-3"));
            $("#reportBox").append(salesRow);

        }).fail(function (xhr, status, error) {
            console.log(error);
        });
    }

    

}); 