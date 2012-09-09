using System;
using System.Collections.Generic;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.Certificates;

namespace JobSystem.BusinessLogic.Services
{
	public class CertificateService : ServiceBase
	{
		private readonly IJobItemRepository _jobItemRepository;
		private readonly ICertificateRepository _certificateRepository;
		private readonly IListItemRepository _listItemRepository;
		private readonly IEntityIdProvider _entityIdProvider;

		public CertificateService(
			IUserContext applicationContext,
			IJobItemRepository jobItemRepository,
			ICertificateRepository certificateRepository,
			IListItemRepository listItemRepository,
			IEntityIdProvider entityIdProvider,
			IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
		{
			_jobItemRepository = jobItemRepository;
			_certificateRepository = certificateRepository;
			_listItemRepository = listItemRepository;
			_entityIdProvider = entityIdProvider;
		}

		public Certificate Create(Guid id, Guid certificateTypeId, Guid jobItemId, string procedureList)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance, "CurrentUser");
			if (id == Guid.Empty)
				throw new ArgumentException("An ID must be supplied for the certificate");
			var certificate = new Certificate();
			certificate.Id = id;
			certificate.CertificateNumber = _entityIdProvider.GetIdFor<Certificate>();
			certificate.DateCreated = AppDateTime.GetUtcNow();
			certificate.CreatedBy = CurrentUser;
			certificate.JobItem = GetJobItem(jobItemId);
			certificate.Type = GetCertificateType(certificateTypeId);
			certificate.ProcedureList = procedureList;
			ValidateAnnotatedObjectThrowOnFailure(certificate);
			_certificateRepository.Create(certificate);
			return certificate;
		}

		public Certificate GetById(Guid id)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance, "CurrentUser");
			return _certificateRepository.GetById(id);
		}

		public IEnumerable<Certificate> GetCertificates()
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance, "CurrentUser");
			return _certificateRepository.GetCertificates();
		}

		public IEnumerable<Certificate> GetCertificatesForJobItem(Guid jobItemId)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance, "CurrentUser");
			return _certificateRepository.GetCertificatesForJobItem(jobItemId);
		}

		public IEnumerable<Certificate> SearchByKeyword(string keyword)
		{
			if (!CurrentUser.HasRole(UserRole.Member))
				throw new DomainValidationException(Messages.InsufficientSecurityClearance, "CurrentUser");
			return _certificateRepository.SearchByKeyword(keyword);
		}

		private JobItem GetJobItem(Guid jobItemId)
		{
			var jobItem = _jobItemRepository.GetById(jobItemId);
			if (jobItem == null)
				throw new ArgumentException("A valid job item ID must be supplied for the certificate");
			return jobItem;
		}

		private ListItem GetCertificateType(Guid certificateTypeId)
		{
			var type = _listItemRepository.GetById(certificateTypeId);
			if (type == null)
				throw new ArgumentException("A valid certificate type ID must be supplied");
			if (type.Category.Type != ListItemCategoryType.Certificate)
				throw new ArgumentException("A certificate type list item must be supplied");
			return type;
		}
	}
}