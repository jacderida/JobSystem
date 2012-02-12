using System;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.Consignments;
using System.Collections.Generic;

namespace JobSystem.BusinessLogic.Services
{
	public class ConsignmentItemService : ServiceBase
	{
		private readonly IConsignmentRepository _consignmentRepository;
		private readonly IConsignmentItemRepository _consignmentItemRepository;
		private readonly IJobItemRepository _jobItemRepository;
		private readonly IListItemRepository _listItemRepository;
		private readonly ISupplierRepository _supplierRepository;

		public ConsignmentItemService(
			IUserContext applicationContext,
			IConsignmentRepository consignmentRepository,
			IConsignmentItemRepository consignmentItemRepository,
			IJobItemRepository jobItemRepository,
			IListItemRepository listItemRepository,
			ISupplierRepository supplierRepository,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_consignmentRepository = consignmentRepository;
			_consignmentItemRepository = consignmentItemRepository;
			_jobItemRepository = jobItemRepository;
			_listItemRepository = listItemRepository;
			_supplierRepository = supplierRepository;
		}

		public ConsignmentItem Create(Guid id, Guid jobItemId, Guid consignmentId, string instructions)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			if (id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied for the consignment item.");
			var jobItem = _jobItemRepository.GetById(jobItemId);
			if (jobItem == null)
				throw new ArgumentException("A valid ID must be supplied for the job item.");
			if (jobItem.Job.IsPending)
				throw new DomainValidationException(Messages.PendingJob);
			var consignment = _consignmentRepository.GetById(consignmentId);
			if (consignment == null)
				throw new ArgumentException("A valid ID must be supplied for the parent consignment.");
			var consignmentItem = new ConsignmentItem
			{
				Id = id,
				Consignment = consignment,
				ItemNo = _consignmentRepository.GetConsignmentItemCount(consignmentId) + 1,
				JobItem = jobItem,
				Instructions = instructions
			};
			jobItem.Status = _listItemRepository.GetByType(ListItemType.StatusConsigned);
			jobItem.Location = _listItemRepository.GetByType(ListItemType.WorkLocationSubContract);
			ValidateAnnotatedObjectThrowOnFailure(consignmentItem);
			_jobItemRepository.EmitItemHistory(CurrentUser, jobItemId, 0, 0, String.Empty, ListItemType.StatusConsigned, ListItemType.WorkTypeAdministration, ListItemType.WorkLocationSubContract);
			_consignmentItemRepository.Create(consignmentItem);
			_jobItemRepository.Update(jobItem);
			return consignmentItem;
		}

		public PendingConsignmentItem CreatePending(Guid id, Guid jobItemId, Guid supplierId, string instructions)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			var jobItem = _jobItemRepository.GetById(jobItemId);
			if (jobItem == null)
				throw new ArgumentException("An ID must be supplied for the job item.");
			if (jobItem.Job.IsPending)
				throw new DomainValidationException(Messages.PendingJob);
			var supplier = _supplierRepository.GetById(supplierId);
			if (supplier == null)
				throw new ArgumentException("An ID must be supplied for the supplier.");
			var pendingItem = new PendingConsignmentItem
			{
				Id = id,
				JobItem = jobItem,
				Supplier = supplier,
				Instructions = instructions
			};
			ValidateAnnotatedObjectThrowOnFailure(pendingItem);
			_consignmentItemRepository.CreatePendingItem(pendingItem);
			return pendingItem;
		}

		public IEnumerable<PendingConsignmentItem> GetPendingItems()
		{
			return _consignmentItemRepository.GetPendingConsignmentItems();
		}

		public IEnumerable<PendingConsignmentItem> GetPendingItems(IList<Guid> pendingItemIds)
		{
			return _consignmentItemRepository.GetPendingConsignmentItems(pendingItemIds);
		}
	}
}