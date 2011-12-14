﻿$(document).ready(function () {
	$(function () {
		// $("input:submit, a.primary-action, button, .nav-link").button();
		$(".primary-action").button();

		//Create user modal form
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
			var userId = $(this).attr('id');
			var editUrl = $('#editUrl').val() + '/' + userId;
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
	});
});