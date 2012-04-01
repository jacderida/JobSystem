$(document).ready(function () {
	$(".primary-action").button();

	$('.work-item-list-item').last().addClass('last');

	//Create WORK ITEM modal form
	$('.createWorkButton').click(function () {
		var elemId = $(this).attr('id');
		var Url = $('#createWorkItemUrl').val() + '/' + elemId;
		$("#create-work-item-container").dialog({
			modal: true,
			width: 335,
			title: 'Create Work Item',
			position: ['center', 100],
			open: function (event, ui) {
				$(this).load(Url);
			}
		});
		return false;
	});

	//Create CONSIGNMENT ITEM modal form
	$('.createConsignmentButton').click(function () {
		var elemId = $(this).attr('id');
		var Url = $('#createConsignmentUrl').val() + '/' + elemId;
		$("#create-consignment-container").dialog({
			modal: true,
			width: 335,
			title: 'Raise Consignment',
			position: ['center', 100],
			open: function (event, ui) {
				$(this).load(Url);
			}
		});
		return false;
	});

	//Create DELIVERY ITEM modal form
	$('.createDeliveryButton').live('click', function () {
		var elemId = $(this).attr('id');
		var Url = $('#createDeliveryUrl').val() + '/' + elemId;
		$("#create-delivery-container").dialog({
			modal: true,
			width: 335,
			title: 'Raise Delivery',
			position: ['center', 100],
			open: function (event, ui) {
				$(this).load(Url);
			}
		});
		return false;
	});
});