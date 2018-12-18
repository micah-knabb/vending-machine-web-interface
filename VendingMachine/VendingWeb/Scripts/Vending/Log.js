$(document).ready(function () {
    let ajaxURL = vndrServerUrl + "api/log"; // <--- changes depending on port used
    ajaxCall();
    $(".active").removeClass("active");
    $(".logNav").addClass("active");

    $("#logType").change(function () {
        $(".headerRow").nextAll("div").remove();
        if ($("#logType").val() == "feed") {
            ajaxCall(1);
        }
        else if ($("#logType").val() == "giveChange") {
            ajaxCall(2);
        }
        else if ($("#logType").val() == "purchaseItem") {
            ajaxCall(3);
        }
        else {
            ajaxCall();
        }
        
    });
    
    function ajaxCall(typeToFilter) {
        $.ajax({
            url: ajaxURL,
            type: "GET",
            dataType: "json",
        }).done(function (data) {
            for (let i = 0; i < data.length; i++) {
                if (typeToFilter == undefined || typeToFilter == data[i].OperationType) {
                    let logBlock = $("<div>").addClass("row");
                    logBlock.append($("<div>").addClass("col-sm-1"));

                    let date = $("<div>").html(data[i].TimeStampStr).addClass("col-3");
                    let transaction = $("<div>").addClass("col-4");
                    if (data[i].OperationType == 1) {
                        transaction.html("FEED MONEY:");
                    }
                    else if (data[i].OperationType == 2) {
                        transaction.html("GIVE CHANGE:");
                    }
                    else {
                        transaction.html(data[i].ProductName);
                    }
                    let price = $("<div>").html("$" + data[i].Price.toFixed(2)).addClass("col-2");
                    let amount = $("<div>").html("$" + data[i].RunningTotal.toFixed(2)).addClass("col-1");


                    logBlock.append(date);
                    logBlock.append(transaction);
                    logBlock.append(price);
                    logBlock.append(amount);
                    logBlock.append($("<div>").addClass("col-sm-1"));

                      
                    $("#logBox").append(logBlock);
                        
                }
            }
            
        });
    }
});