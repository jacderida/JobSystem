$(document).ready(function () {
	$(".primary-action").button();

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

});