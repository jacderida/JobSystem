$(document).ready(function () {
	$(".primary-action").button();

	$(".standardMultiSelect").chosen();

	$("#supplier-autocomplete").autocomplete({
		source: function (request, response) {
			// define a function to call your Action (assuming UserController)
			$.ajax({
				url: '../../Supplier/SearchSuppliers', type: "POST", dataType: "json",

				// query will be the param used by your action method
				data: { query: request.term },
				success: function (data) {
					response($.map(data, function (item) {
						return {
							value: item.Name + " - " + item.Address4 + " - " + item.Address5,
							key: item.Id
						}
					}))
				}
			})
		},
		select: function (e, ui) {
			$("#SupplierId").val(ui.item.key);
		},
		minLength: 1, // require at least one character from the user
		dataType: 'json'
	});

	var uploader = new qq.FileUploader({
		// pass the dom node (ex. $(selector)[0] for jQuery users)
		element: document.getElementById('file-uploader'),
		// path to server-side upload script
		action: "../Attachments/AddAttachment",
		onComplete: function (id, fileName, responseJSON) {
			var idHidden = '<input type="hidden" name="AttachmentId" value="' + responseJSON.Id + '"/>';
			var nameHidden = '<input type="hidden" name="AttachmentName" value="' + responseJSON.Filename + '"/>';
			$('#createJobForm').append(idHidden).append(nameHidden);
		}
	});
});