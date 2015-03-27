using System;
using System.Collections.Generic;
using System.Linq;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel;
using JobSystem.DataModel.Entities;
using JobSystem.DataModel.Repositories;
using JobSystem.Framework;
using JobSystem.Framework.Messaging;
using JobSystem.Resources.Jobs;
using JobSystem.Storage;
using JobSystem.Storage.Jobs;

namespace JobSystem.BusinessLogic.Services
{
    public class JobService : ServiceBase
    {
        private readonly IJobAttachmentDataRepository _jobAttachmentDataRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IListItemRepository _listItemRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEntityIdProvider _entityIdProvider;

        public JobService(
            IUserContext applicationContext,
            IJobAttachmentDataRepository jobAttachmentDataRepository,
            IJobRepository jobRepository,
            IListItemRepository listItemRepository,
            ICustomerRepository customerRepository,
            IEntityIdProvider entityIdProvider,
            IQueueDispatcher<IMessage> dispatcher) : base(applicationContext, dispatcher)
        {
            _jobAttachmentDataRepository = jobAttachmentDataRepository;
            _jobRepository = jobRepository;
            _listItemRepository = listItemRepository;
            _customerRepository = customerRepository;
            _entityIdProvider = entityIdProvider;
        }

        public Job CreateJob(Guid id, string instructions, string orderNo, string adviceNo, Guid typeId, Guid customerId, string notes, string contact)
        {
            var job = new Job();
            job.Id = id;
            job.CreatedBy = CurrentUser;
            job.DateCreated = AppDateTime.GetUtcNow();
            job.Instructions = instructions;
            job.OrderNo = orderNo;
            job.AdviceNo = adviceNo;
            job.Customer = GetCustomer(customerId);
            job.Type = GetType(typeId);
            job.JobNo = _entityIdProvider.GetIdFor<Job>();
            job.Notes = notes;
            job.Contact = contact;
            job.IsPending = true;
            ValidateAnnotatedObjectThrowOnFailure(job);
            _jobRepository.Create(job);
            return job;
        }

        public Job Edit(Guid id, string orderNumber, string adviceNumber, string contact, string notes, string instructions)
        {
            if (!CurrentUser.HasRole(UserRole.Admin | UserRole.Manager))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            var job = GetJob(id);
            job.OrderNo = orderNumber;
            job.AdviceNo = adviceNumber;
            job.Contact = contact;
            job.Notes = notes;
            job.Instructions = instructions;
            ValidateAnnotatedObjectThrowOnFailure(job);
            _jobRepository.Update(job);
            return job;
        }

        public Job AddAttachment(Guid jobId, Guid attachmentId, string fileName)
        {
            var job = GetJob(jobId);
            if (attachmentId == Guid.Empty)
                throw new ArgumentException("An ID must be supplied for the attachment");
            if (String.IsNullOrEmpty(fileName))
                throw new DomainValidationException(Messages.FileNameRequired);
            job.Attachments.Add(new Attachment { Id = attachmentId, Filename = fileName });
            _jobRepository.Update(job);
            return job;
        }

        public Job ApproveJob(Guid jobId)
        {
            if (!CurrentUser.HasRole(UserRole.JobApprover))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            var job = _jobRepository.GetById(jobId);
            if (job == null)
                throw new ArgumentException("A valid ID must be supplied by for job.");
            var jobItemCount = _jobRepository.GetJobItemCount(jobId);
            if (jobItemCount == 0)
                throw new DomainValidationException(Messages.ApprovingJobHasNoItems, "JobId");
            job.IsPending = false;
            _jobRepository.Update(job);
            return job;
        }

        public AttachmentData GetAttachment(Guid jobId, Guid attachmentId)
        {
            var job = GetJob(jobId);
            var attachment = job.Attachments.Where(x => x.Id.Equals(attachmentId)).SingleOrDefault();
            if (attachment == null)
                throw new DomainValidationException(String.Format("No attachment exists for job {0}", job.JobNo));
            return _jobAttachmentDataRepository.GetById(attachmentId);
        }

        public Job GetJob(Guid id)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            var job = _jobRepository.GetById(id);
            if (job == null)
                throw new ArgumentException("An invalid ID was supplied for the job");
            return job;
        }

        public int GetApprovedJobsCount()
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _jobRepository.GetApprovedJobsCount();
        }

        public IEnumerable<Job> GetApprovedJobs()
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _jobRepository.GetApprovedJobs().OrderBy(j => j.JobNo);
        }

        public IEnumerable<Job> GetPendingJobs()
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _jobRepository.GetPendingJobs().OrderBy(j => j.JobNo);
        }

        public IEnumerable<Job> SearchByKeyword(string keyword)
        {
            if (!CurrentUser.HasRole(UserRole.Member))
                throw new DomainValidationException(Messages.InsufficientSecurityClearance);
            return _jobRepository.SearchByKeyword(keyword);
        }

        private Customer GetCustomer(Guid customerId)
        {
            var customer = _customerRepository.GetById(customerId);
            if (customer == null)
                throw new ArgumentException(Messages.InvalidCustomerId);
            return customer;
        }

        private ListItem GetType(Guid typeId)
        {
            var type = _listItemRepository.GetById(typeId);
            if (type == null)
                throw new ArgumentException(Messages.InvalidTypeId);
            return type;
        }
    }
}