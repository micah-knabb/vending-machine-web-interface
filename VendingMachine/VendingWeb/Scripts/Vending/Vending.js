
$(document).ready(function ()
{
    $(".active").removeClass("active");
    $(".homeNav").addClass("active");
    getBalance();
    getInventory();
    feedBillEvents();

    $("#change").on("click", getChange);
});

let yourBalance = 0;

function feedBillEvents() {
    $("#1bill").on("click", function (e) {
        sendMoney(1);
    })
    $("#5bill").on("click", function (e) {
        sendMoney(5);
    })
    $("#10bill").on("click", function (e) {
        sendMoney(10);
    })
}

function getChange() {
    if (yourBalance > 0) {
        $.ajax({
            url: vndrServerUrl + "api/change",
            type: "POST"
        }).done(function (response) {
            $("#feedBackStatus").text(response.Status.Status);

            let message = "Your change: ";

            if (response.Change.Dollars > 0) {
                message += `Dollars: ${response.Change.Dollars} `;
            }

            if (response.Change.Quarters > 0) {
                message += `Quarters: ${response.Change.Quarters} `;
            }

            if (response.Change.Dimes > 0) {
                message += `Dimes: ${response.Change.Dimes} `;
            }

            if (response.Change.Nickels > 0) {
                message += `Nickels: ${response.Change.Nickels} `;
            }

            if (response.Change.Pennies > 0) {
                message += `Pennies: ${response.Change.Pennies} `;
            }

            $("#feedBackImage").removeClass();
            $("#feedBackImage").addClass("fas far fa-coins fa-5x text-center card-img-top debug");
            $("#feedBackMessage").text(message);

            getBalance();
            getInventory();
        })
    }
    else {
        $("#feedBackStatus").text("Woops")
        $("#feedBackImage").removeClass();
        $("#feedBackImage").addClass("fas far fa-coins fa-5x text-center card-img-top debug");
        $("#feedBackMessage").text("You had no money")
    }
        
}

function sendMoney(amt) {
    console.log("start load inventory")
    console.time("inventory");
    $.ajax({
        url: vndrServerUrl + "api/feedmoney",
        type: "POST",
        data: {
            amount: amt
        }
    }).done(function (response) {
        getBalance();
        feedMessage(amt, response.Status);
        getInventory();
    })
}

function getInventory() {
    $.ajax({
        url: vndrServerUrl + "api/inventory",
        type: "GET",
        dataType: "json"
    }).done(function (data) {
        //console.log("Inventory Loaded");
        $("#itemContainer").empty();
        let itemCounter = 0;
        
        for (let i = 1; i <= data[data.length - 1].Inventory.Column; i++) {
           
            column = $("<div>");
            col = i.toString();

            for (let i = 0; i< data[data.length - 1].Inventory.Row; i++) {

                key = data[itemCounter].Inventory.Key;
                row = data[itemCounter].Inventory.Row.toString();
                col = data[itemCounter].Inventory.Column.toString();

                //Create objects to hold new item
                card = $("<div>").addClass("card");
                title = $("<h5>").addClass("card-title");
                header = $("<h5>").addClass("card-header");
                image = $("<img>").addClass("card-img-top ");
                price = $("<div>").addClass("card-footer");
                itemSoldOut = $("<p>").addClass("card-text text-center align-middle cardss");

                title.text(data[itemCounter].Product.Name);
                header.text(data[itemCounter].Product.Name);
                card.append(header);

                
                //Add attributes
                price.text(`$${data[itemCounter].Product.Price.toFixed(2)}`);
                if (data[itemCounter].Inventory.Qty > 0) {
                    image.attr("src", data[itemCounter].Product.Image);
                    image.addClass("itemImage");
                    card.append(image);
                }
                else {
                    itemSoldOut.text("Sold Out");
                    card.append(itemSoldOut);
                }

                card.attr("id", key);
                card.css({ "grid-row": row });
                card.data("Row", row);
                card.data("Col", col)

                card.append(price);

                //Events
                if (data[itemCounter].Inventory.Qty == 0) {
                    card.addClass("cantClick");
                    card.on("click", soldOut);
                }
                else if (data[itemCounter].Product.Price > yourBalance) {
                    card.addClass("cantClick");
                    card.on("click", notEnough);
                }
                else {
                    card.on("click", purchaseProduct);
                    card.on("click", function () {
                        card.effect("bounce", { times: 3 }, 300);
                    });
                }
                card.addClass("vendingItem")
                column.append(card);
                itemCounter++;
            }

            $("#itemContainer").append(column);
        }
    });
}

function notEnough(e) {
    updateStatus("fa-times-circle", "Not enough money", "Error");
    for (var x = 1; x <= 3; x++) {
        $(this).animate({ "left": "+=20px" }, 50).animate({ "left": "-=20px" }, 50).animate({ "left": "-=20px" }, 50).animate({ "left": "+=20px" }, 50)
    }
}

function soldOut(e) {
    updateStatus("fa-times-circle", "Item is sold out", "Error");
    for (var x = 1; x <= 3; x++) {
        $(this).animate({ "left": "+=20px" }, 50).animate({ "left": "-=20px" }, 50).animate({ "left": "-=20px" }, 50).animate({ "left": "+=20px" }, 50)
    }
}

function purchaseProduct() {
    let prodCol = $(this).data("Col");
    let prodRow = $(this).data("Row");

    $.ajax({
        url: vndrServerUrl + "api/purchase",
        type: "POST",
        dataType: "json",
        data: {
            row: prodRow,
            col: prodCol
        }
    }).done(function (data) {
        console.log(data);
        getBalance();
        let image = "fa-thumbs-up";
        if (data.Status === "Error") {
            image = "fa-times-circle";
        }
        updateStatus(image, data.Message, data.Status);
        getInventory();
    });
}

function feedMessage(amount, status) {
    updateStatus("fa-money-bill-wave", `$${amount} added to balance`, status);
}

function updateStatus(image, message, status) {
    $("#feedBackImage").removeClass();
    $("#feedBackImage").addClass(`fas far ${image} fa-5x text-center card-img-top debug`);
    $("#feedBackMessage").text(message);
    $("#feedBackStatus").text(status);
}

function getBalance() {
    $.ajax({
        url: vndrServerUrl + "api/balance",
        type: "GET",
        dataType: "json",
    }).done(function (data)
    {
        yourBalance = data;
        let balance = formatter.format(data)
        $("#balance").text(balance)
    });
}

const formatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
    minimumFractionDigits: 2
});