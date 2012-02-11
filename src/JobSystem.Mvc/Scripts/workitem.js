$(document).ready(function () {
	$(".primary-action").button();

	var supplierId = null;
	$("#supplier-autocomplete").autocomplete({
		source: function (request, response) {
			// define a function to call your Action (assuming UserController)
			$.ajax({
				url: '../../Supplier/SearchSuppliers', type: "POST", dataType: "json",

				// query will be the param used by your action method
				data: { query: request.term },
				success: function (data) {
					response($.map(data, function (item) {
						supplierId = item.Id;
						return item.Name + " - " + item.Address4 + " - " + item.Address5;
					}))
				}
			})
		},
		select: function (e, ui) {
			$("#SupplierId").val(supplierId);
		},
		minLength: 1, // require at least one character from the user
		dataType: 'json'
	});
});