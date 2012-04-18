$(document).ready(function () {
	$(".primary-action").button();
	$("#manufacturer-autocomplete").autocomplete({
		source: function (request, response) {
			// define a function to call your Action (assuming UserController)
			$.ajax({
				url: 'Instrument/SearchManufacturers', type: "POST", dataType: "json",

				// query will be the param used by your action method
				data: { query: request.term },
				success: function (data) {
					response($.map(data, function (item) {
						return {
							value: item
						}
					}))
				}
			})
		},
		minLength: 1, // require at least one character from the user
		dataType: 'json'
	});

	$("#model-autocomplete").autocomplete({
		source: function (request, response) {
			// define a function to call your Action (assuming UserController)
			$.ajax({
				url: 'Instrument/SearchModelNumber', type: "POST", dataType: "json",

				// query will be the param used by your action method
				data: { modelNo: request.term, manufacturer: $("#manufacturer-autocomplete").val() },
				success: function (data) {
					response($.map(data, function (item) {
						return {
							value: item
						}
					}))
				}
			})
		},
		minLength: 1, // require at least one character from the user
		dataType: 'json'
	});
});