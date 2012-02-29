var app = {
	closeModal: function (element) {
		switch (element) {
			case 'work-item':
				$("#create-work-item-container").dialog('close');
				//And update the job item tab status
				var label = $('#st_vertical .st_tab_active .jiStatus');
				label.text($('#StatusId option:selected').text());
				break;
			case 'consignment-item':
				$("#create-consignment-container").dialog('close');
				break;
		}
	}
}

$(document).ready(function () {
	//Show/hide returned reason box on job item create
	//Initial hide
	$('#return-reason-group').hide();
	$('#IsReturned').click(function () {
		if ($(this).is(':checked')) {
			$('#return-reason-group').slideDown();
		} else {
			$('#return-reason-group').slideUp();
			$('#ReturnReason').val('');
		}
	});

	$(function () {
		// $("input:submit, a.primary-action, button, .nav-link").button();
		$(".primary-action").button();

		//Create USER modal form
		$("#create-user-container").dialog({
			autoOpen: false,
			modal: true,
			width: 335,
			title: 'Create New User'
		});

		$('#createUserButton').click(function () {
			$("#create-user-container").dialog('open');
		});

		$('.editUserButton').click(function () {
			var elemId = $(this).attr('id');
			var editUrl = $('#editUrl').val() + '/' + elemId;
			//Edit user modal form
			$("#edit-user-container").dialog({
				modal: true,
				width: 335,
				title: 'Edit User',
				position: ['center', 100],
				open: function (event, ui) {
					//Load the Edit action which will return 
					// the partial view _Edit
					$(this).load(editUrl);
				}
			});
		});

		//Create JOB modal form
		$("#create-job-container").dialog({
			autoOpen: false,
			modal: true,
			width: 335,
			title: 'Create New Job'
		});

		$('#createJobButton').click(function () {
			$("#create-job-container").dialog('open');
		});

		//Create INSTRUMENT modal form
		$("#create-instrument-container").dialog({
			autoOpen: false,
			modal: true,
			width: 335,
			title: 'Create New Instrument'
		});

		$('#createInstrumentButton').click(function () {
			$("#create-instrument-container").dialog('open');
		});

		$('.editInstrumentButton').click(function () {
			var elemId = $(this).attr('id');
			var editUrl = $('#editUrl').val() + '/' + elemId;
			//Edit user modal form
			$("#edit-instrument-container").dialog({
				modal: true,
				width: 335,
				title: 'Edit Instrument',
				position: ['center', 100],
				open: function (event, ui) {
					//Load the Edit action which will return 
					// the partial view _Edit
					$(this).load(editUrl);
				}
			});
		});

		var instrumentId = null;
		$("#instrument-autocomplete").autocomplete({
			source: function (request, response) {
				// define a function to call your Action (assuming UserController)
				$.ajax({
					url: '../../Instrument/SearchInstruments', type: "POST", dataType: "json",

					// query will be the param used by your action method
					data: { query: request.term },
					success: function (data) {
						response($.map(data, function (item) {
							instrumentId = item.Id;
							return item.Manufacturer + " - " + item.ModelNo + " - " + item.Range;
						}))
					}
				})
			},
			select: function (e, ui) {
				$("#InstrumentId").val(instrumentId);
			},
			minLength: 1, // require at least one character from the user
			dataType: 'json'
		});

		// Vertical Sliding Tabs demo
		$('div#st_vertical').slideTabs({
			// Options
			contentAnim: 'slideH',
			contentAnimTime: 200,
			contentEasing: 'easeInOutExpo',
			orientation: 'vertical',
			tabsAnimTime: 100,
			autoHeight: true,
			totalWidth: '168'
		});

		//Get Job Item Details
		$('.getJobItem').click(function () {
			//			$('html, body').animate({
			//				scrollTop: $("#horz_tabs_container").offset().top
			//			}, 50);
			$.get("../../JobItem/Details/" + $(this).attr('id'),
			   function (data) {
			   	$('#st_horizontal').hide();
			   	$('#st_horizontal').fadeIn(300);
			   	$('#st_horizontal').html(data);
			   });
		});

		$("#invoiceCheckbox").click(function () {
			if (!$(this).attr('checked')) {
				$("#invoiceDetails").show(300);
			}
			else {
				$("#invoiceDetails").hide(300);
			}
		});

		$("#deliveryCheckbox").click(function () {
			if (!$(this).attr('checked')) {
				$("#deliveryDetails").show(300);
			}
			else {
				$("#deliveryDetails").hide(300);
			}
		});

		$("#salesCheckbox").click(function () {
			if (!$(this).attr('checked')) {
				$("#salesDetails").show(300);
			}
			else {
				$("#salesDetails").hide(300);
			}
		});

		$('#settings').hover(function () {
			$('#admin-menu').removeClass('hidden');
		});

		$('#settings').mouseleave(function () {
			var nav_timeout = setTimeout(function () {
				$('#admin-menu').addClass('hidden');
			}, 300);

			var menu_timeout;

			$('#admin-menu').mouseenter(function () {
				clearTimeout(nav_timeout);
				clearTimeout(menu_timeout);
				$('#admin-menu').removeClass('hidden');

				$('#admin-menu').mouseleave(function () {
					menu_timeout = setTimeout(function () {
						$('#admin-menu').addClass('hidden');
					}, 300);
				});
			});
		});

		var uploader = new qq.FileUploader({
			// pass the dom node (ex. $(selector)[0] for jQuery users)
			element: document.getElementById('file-uploader'),
			// path to server-side upload script
			action: "../../Attachment/Upload",
			onComplete: function (id, fileName, responseJSON) {
			}
		});

		//Populate customer delivery/invoice address with main address details
		function populateAddressFields(fieldsToPopulate, selected) {
			var addressLine1 = $('#Address1').val();
			var addressLine2 = $('#Address2').val();
			var addressLine3 = $('#Address3').val();
			var townCity = $('#Address4').val();
			var postcode = $('#Address5').val();

			var telephone = $('#Telephone').val();
			var fax = $('#Fax').val();
			var email = $('#Email').val();
			var contact1 = $('#Contact1').val();
			var contact2 = $('#Contact2').val();

			switch (fieldsToPopulate) {
				case 'invoice':
					if (selected) {
						$('#InvoiceAddress1').val(addressLine1);
						$('#InvoiceAddress2').val(addressLine2);
						$('#InvoiceAddress3').val(addressLine3);
						$('#InvoiceAddress4').val(townCity);
						$('#InvoiceAddress5').val(postcode);
					}
					else {
						$('#InvoiceAddress1').val("");
						$('#InvoiceAddress2').val("");
						$('#InvoiceAddress3').val("");
						$('#InvoiceAddress4').val("");
						$('#InvoiceAddress5').val("");
					}
					break;
				case 'delivery':
					if (selected) {
						$('#DeliveryAddress1').val(addressLine1);
						$('#DeliveryAddress2').val(addressLine2);
						$('#DeliveryAddress3').val(addressLine3);
						$('#DeliveryAddress4').val(townCity);
						$('#DeliveryAddress5').val(postcode);
					}
					else {
						$('#DeliveryAddress1').val("");
						$('#DeliveryAddress2').val("");
						$('#DeliveryAddress3').val("");
						$('#DeliveryAddress4').val("");
						$('#DeliveryAddress5').val("");
					}
					break;
				case 'sales':
					if (selected) {
						$('#SalesAddress1').val(addressLine1);
						$('#SalesAddress2').val(addressLine2);
						$('#SalesAddress3').val(addressLine3);
						$('#SalesAddress4').val(townCity);
						$('#SalesAddress5').val(postcode);
						$('#SalesTelephone').val(telephone);
						$('#SalesFax').val(fax);
						$('#SalesEmail').val(email);
						$('#SalesContact1').val(contact1);
						$('#SalesContact2').val(contact2);
					}
					else {
						$('#SalesAddress1').val("");
						$('#SalesAddress2').val("");
						$('#SalesAddress3').val("");
						$('#SalesAddress4').val("");
						$('#SalesAddress5').val("");
						$('#SalesTelephone').val("");
						$('#SalesFax').val("");
						$('#SalesEmail').val("");
						$('#SalesContact1').val("");
						$('#SalesContact2').val("");
					}
					break;
			}
		}

		$('#deliveryCheckbox').click(function () {
			populateAddressFields('delivery', $(this).is(':checked'));
		});

		$('#invoiceCheckbox').click(function () {
			populateAddressFields('invoice', $(this).is(':checked'));
		});

		$('#salesCheckbox').click(function () {
			populateAddressFields('sales', $(this).is(':checked'));
		});
	});

	$('.work-item-list-item').live('mouseenter', function () {
		var elem = $(this);
		var elemToShow = $('#work-item-' + elem.attr('id'));
		var container = $('#item-tooltip');

		container.html(elemToShow.clone());
		container.css('top', (elem.offset().top - 366));
		container.children('.work-item-details').css('top', '20px');
		container.stop(true, true).fadeIn(200);

		$('.work-item-list-item').live('mouseleave', function () {
			container.stop(true, true).fadeOut(200);
		});
	});

});