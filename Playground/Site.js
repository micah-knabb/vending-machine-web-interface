$( "#Test" ).on( "click", function() {

    let cvm = {};
    cvm.GuestName = "Chris";
    cvm.AvailableFunds = "15.50";
    cvm.Items = [];

    let VendingItemViewModel = {};
    VendingItemViewModel.ProductName = "Ball";
    VendingItemViewModel.ProductPrice = 1.25;
    VendingItemViewModel.CategoryName = "Toy";
    VendingItemViewModel.Col = 1;
    VendingItemViewModel.Row = 1;
    
    cvm.Items.push(VendingItemViewModel);

    $.ajax({
        url: "http://localhost:55801/Vending/CustomIndex",
        data: cvm,
      }).done(function() {
        alert("done");
      });
});
