using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.Consignments;

namespace JobSystem.BusinessLogic.Services
{
	public class ConsignmentService : ServiceBase
	{
		private readonly IConsignmentRepository _consignmentRepository;
		private readonly ISupplierRepository _supplierRepository;
		private readonly IEntityIdProvider _entityIdProvider;
		private readonly ConsignmentItemService _consignmentItemService;

		public ConsignmentService(
			IUserContext applicationContext,
			IConsignmentRepository consignmentRepository,
			ISupplierRepository supplierRepository,
			IEntityIdProvider entityIdProvider,
			ConsignmentItemService consignmentItemService,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_consignmentRepository = consignmentRepository;
			_supplierRepository = supplierRepository;
			_entityIdProvider = entityIdProvider;
			_consignmentItemService = consignmentItemService;
		}

		public Consignment Create(Guid id, Guid supplierId)
		{
			if (id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied for the consignment.");
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			var consignment = new Consignment();
			consignment.Id = id;
			consignment.DateCreated = AppDateTime.GetUtcNow();
			consignment.Supplier = GetSupplier(supplierId);
			consignment.CreatedBy = CurrentUser;
			consignment.ConsignmentNo = _entityIdProvider.GetIdFor<Consignment>();
			_consignmentRepository.Create(consignment);
			return consignment;
		}

		public void CreateConsignmentsFromPendingItems()
		{
			DoCreateConsignmentItems(_consignmentItemService.GetPendingItems());
		}

		public void CreateConsignmentsFromPendingItems(IList<Guid> pendingItemIds)
		{
			DoCreateConsignmentItems(_consignmentItemService.GetPendingItems(pendingItemIds));
		}

		public Consignment Edit(Guid consignmentId, Guid supplierId)
		{
			if (!CurrentUser.HasRole(UserRole.Admin | UserRole.Manager))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			var consignment = GetById(consignmentId);
			if (consignment == null)
				throw new ArgumentException("A valid consignment ID must be supplied");
			if (consignment.IsOrdered)
				throw new DomainValidationException(Messages.ConsignmentIsOrdered);
			consignment.Supplier = GetSupplier(supplierId);
			_consignmentRepository.Update(consignment);
			return consignment;
		}

		public Consignment GetById(Guid id)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			return _consignmentRepository.GetById(id);
		}

		public IEnumerable<Consignment> GetConsignments()
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			return _consignmentRepository.GetConsignments();
		}

		public int GetConsignmentsCount()
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			return _consignmentRepository.GetConsigmentsCount();
		}

		private void DoCreateConsignmentItems(IEnumerable<PendingConsignmentItem> pendingItems)
		{
			var groupedByPendingItems = pendingItems.GroupBy(p => p.Supplier.Name, p => p).OrderBy(p => p.Key);
			foreach (var group in groupedByPendingItems)
			{
				var i = 0;
				var consignmentId = Guid.NewGuid();
				foreach (var item in group.OrderBy(p => p.JobItem.Job.JobNo).ThenBy(p => p.JobItem.ItemNo))
				{
					if (i++ == 0)	// Ahh, horribly hack-ish, but it works! :/
						Create(consignmentId, item.Supplier.Id);
					_consignmentItemService.Create(Guid.NewGuid(), item.JobItem.Id, consignmentId, !String.IsNullOrEmpty(item.Instructions) ? item.Instructions : String.Empty);
					_consignmentItemService.DeletePendingItem(item.Id);
				}
			}
		}

		private Supplier GetSupplier(Guid supplierId)
		{
			var supplier = _supplierRepository.GetById(supplierId);
			if (supplier == null)
				throw new ArgumentException("A valid supplier ID must be provided for the consignment.");
			return supplier;
		}
	}
}