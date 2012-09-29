using System;
using System.Collections.Generic;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.Consignments;

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
			ValidateAnnotatedObjectThrowOnFailure(consignmentItem);
			_jobItemRepository.EmitItemHistory(CurrentUser, jobItemId, 0, 0, String.Format("Item consigned on {0}", consignment.ConsignmentNo), ListItemType.StatusConsigned, ListItemType.WorkTypeAdministration);
			_consignmentItemRepository.Create(consignmentItem);
			_jobItemRepository.Update(jobItem);
			return consignmentItem;
		}

		public PendingConsignmentItem CreatePending(Guid id, Guid jobItemId, Guid supplierId, string instructions)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			if (_consignmentItemRepository.JobItemHasPendingConsignmentItem(jobItemId))
				throw new DomainValidationException(Messages.PendingItemAlreadyExists);
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

		public PendingConsignmentItem EditPending(Guid id, Guid jobItemId, Guid supplierId, string instructions)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			var pendingItem = _consignmentItemRepository.GetPendingItem(id);
			if (pendingItem == null)
				throw new ArgumentException("A valid ID must be supplied for the pending consignment.");
			var jobItem = _jobItemRepository.GetById(jobItemId);
			if (jobItem == null)
				throw new ArgumentException("A valid ID must be supplied for the job item.");
			var supplier = _supplierRepository.GetById(supplierId);
			if (supplier == null)
				throw new ArgumentException("A valid ID must be supplied for the supplier.");
			pendingItem.JobItem = jobItem;
			pendingItem.Supplier = supplier;
			pendingItem.Instructions = instructions;
			ValidateAnnotatedObjectThrowOnFailure(pendingItem);
			_consignmentItemRepository.UpdatePendingItem(pendingItem);
			return pendingItem;
		}

		public ConsignmentItem Edit(Guid consignmentItemId, string instructions)
		{
			var consignmentItem = GetById(consignmentItemId);
			if (consignmentItem == null)
				throw new ArgumentException("A valid ID must be supplied for the consignment item");
			if (consignmentItem.Consignment.IsOrdered)
				throw new DomainValidationException(Messages.ConsignmentIsOrdered);
			consignmentItem.Instructions = instructions;
			ValidateAnnotatedObjectThrowOnFailure(consignmentItem);
			_consignmentItemRepository.Update(consignmentItem);
			return consignmentItem;
		}

		public ConsignmentItem GetById(Guid id)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance);
			var consignmentItem = _consignmentItemRepository.GetById(id);
			return consignmentItem;
		}

		public PendingConsignmentItem GetPendingItem(Guid id)
		{
			return _consignmentItemRepository.GetPendingItem(id);
		}

		public void DeletePendingItem(Guid id)
		{
			_consignmentItemRepository.DeletePendingItem(id);
		}

		public IEnumerable<ConsignmentItem> GetConsignmentItems(Guid consignmentId)
		{
			return _consignmentItemRepository.GetConsignmentItems(consignmentId);
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