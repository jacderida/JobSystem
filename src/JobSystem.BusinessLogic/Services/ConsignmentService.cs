using System;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Framework.Messaging;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.Resources.Consignments;
using System.Collections.Generic;

namespace JobSystem.BusinessLogic.Services
{
	public class ConsignmentService : ServiceBase
	{
		private readonly IConsignmentRepository _consignmentRepository;
		private readonly ISupplierRepository _supplierRepository;
		private readonly IEntityIdProvider _entityIdProvider;

		public ConsignmentService(
			IUserContext applicationContext,
			IConsignmentRepository consignmentRepository,
			ISupplierRepository supplierRepository,
			IEntityIdProvider entityIdProvider,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_consignmentRepository = consignmentRepository;
			_supplierRepository = supplierRepository;
			_entityIdProvider = entityIdProvider;
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

		private Supplier GetSupplier(Guid supplierId)
		{
			var supplier = _supplierRepository.GetById(supplierId);
			if (supplier == null)
				throw new ArgumentException("A valid supplier ID must be provided for the consignment.");
			return supplier;
		}
	}
}