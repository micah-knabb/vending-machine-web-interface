$(function () {
    $.ajax({
        url: vndrServerUrl + "api/users",
        dataType: "json",
        method: 'GET'
    }).done(function (data) {

        // Clear the user table
        $("#userData").empty();
        for (var i = 0; i < data.length; i++) {
            var tableRow = $("<tr>");
            var id = $(`<td>${data[i].Id}</td>`);
            var firstName = $(`<td>${data[i].FirstName}</td>`);
            var lastName = $(`<td>${data[i].LastName}</td>`);
            var username = $(`<td>${data[i].Username}</td>`);
            var email = $(`<td>${data[i].Email}</td>`);
            var deleteUser = $(`<td><a data-id="${data[i].Id}">delete</a></td>`);
            //deleteUser.on("click", onDeleteUser);

            // Append all the cells
            tableRow.append([id, firstName, lastName, username, email, deleteUser]);

            // Wire the event handler onRowClick
            //tableRow.on("click", onRowClick);

            $("#userData").append(tableRow);
        }
    });
});