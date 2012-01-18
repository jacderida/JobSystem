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

		//Create JOB ITEM modal form
		$('.createJobItemButton').click(function () {
			var elemId = $(this).attr('id');
			var editUrl = $('#editUrl').val() + '/' + elemId;
			//Edit user modal form
			$("#create-job-item-container").dialog({
				modal: true,
				width: 750,
				title: 'Create New Job Item',
				open: function (event, ui) {
					//Load the Edit action which will return 
					// the partial view _Edit
					$(this).load(editUrl);
				}
			});
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
			$.get("../../JobItem/Details/" + $(this).attr('id'),
			   function (data) {
			   	$('#st_horizontal').html(data);
			   });
		});

		$("#invoiceDetails").hide();
		$("#invoiceCheckbox").click(function () {
			if (!$(this).attr('checked')) {
				$("#invoiceDetails").show(300);
			}
			else {
				$("#invoiceDetails").hide(300);
			}
		});

		$("#deliveryDetails").hide();
		$("#deliveryCheckbox").click(function () {
			if (!$(this).attr('checked')) {
				$("#deliveryDetails").show(300);
			}
			else {
				$("#deliveryDetails").hide(300);
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

		// Horizontal Sliding Tabs demo
		$('div#st_horizontal').slideTabs({
			// Options  			
			contentAnim: 'slideH',
			contentAnimTime: 200,
			contentEasing: 'easeInOutExpo',
			tabsAnimTime: 100,
			tabsScroll: false,
			autoHeight: true,
			totalWidth: '774'
		});

		//Populate customer delivery/invoice address with main address details
		function populateAddressFields(fieldsToPopulate, selected) {
			var addressLine1 = $('#Address1').val();
			var addressLine2 = $('#Address2').val();
			var addressLine3 = $('#Address3').val();
			var townCity = $('#Address4').val();
			var postcode = $('#Address5').val();

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
			}
		}

		$('#deliveryCheckbox').click(function () {
			populateAddressFields('delivery', $(this).is(':checked'));
		});

		$('#invoiceCheckbox').click(function () {
			populateAddressFields('invoice', $(this).is(':checked'));
		});
	});
});