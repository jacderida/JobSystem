﻿$(document).ready(function () {
	$(".primary-action").button();

	$('.work-item-list-item').last().addClass('last');
	$('.cert-item-list-item').last().addClass('last');

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

	//Create CERTIFICATE modal form
	$('.createCertificateButton').click(function () {
		var elemId = $(this).attr('id');
		var Url = $('#createCertificateUrl').val() + '/' + elemId;
		$("#create-certificate-container").dialog({
			modal: true,
			width: 335,
			title: 'Create Certificate',
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

	//Edit Job item INFORMATION  form
	$('#editJobItemInformationButton').live('click', function (e) {
		e.preventDefault();

		var editUrl = $('#editJobItemInformationUrl').val();
		//Edit user modal form
		$("#edit-job-item-information").dialog({
			modal: true,
			width: 335,
			title: 'Edit Information',
			position: ['center', 100],
			open: function (event, ui) {
				//Load the Edit action which will return 
				// the partial view _Edit
				$(this).load(editUrl);
			}
		});
	});


    //Edit Job item INSTRUMENT  form
	$('#edit-instrument-button').live('click', function (e) {
	    e.preventDefault();

	    var editUrl = $(this).attr('href');
	    //Edit user modal form
	    $("#edit-job-item-instrument").dialog({
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
});