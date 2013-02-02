using System;
using System.IO;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataModel.Entities;
using JobSystem.Storage;
using JobSystem.Storage.Jobs;
using JobSystem.TestHelpers;
using JobSystem.TestHelpers.Context;
using NUnit.Framework;
using Rhino.Mocks;

namespace JobSystem.BusinessLogic.UnitTests
{
    public class AttachmentServiceTests
    {
        private DomainValidationException _domainValidationException;
        private JobAttachmentService _attachmentService;

        [SetUp]
        public void Setup()
        {
            _domainValidationException = null;
        }

        [Test]
        public void AddAttachment_ValidAttachmentDetails_AttachmentAdded()
        {
            var attachmentId = Guid.NewGuid();
            var fileName = "attachment.pdf";
            var contentType = "image";
            var content = new byte[] { 1, 2, 3, 4, 5 };

            var attachmentDataRepositoryMock = MockRepository.GenerateMock<IJobAttachmentDataRepository>();
            attachmentDataRepositoryMock.Expect(x => x.Put(null)).IgnoreArguments();
            _attachmentService = AttachmentServiceFactory.Create(attachmentDataRepositoryMock);
            AddAttachment(attachmentId, fileName, contentType, content);
            attachmentDataRepositoryMock.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddAttachment_FilenameGreaterThan2000Characters_ArgumentExceptionThrow()
        {
            var attachmentId = Guid.NewGuid();
            var fileName = new string('a', 2001);
            var contentType = "image";
            var content = new byte[] { 1, 2, 3, 4, 5 };

            _attachmentService = AttachmentServiceFactory.Create(MockRepository.GenerateStub<IJobAttachmentDataRepository>());
            AddAttachment(attachmentId, fileName, contentType, content);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.FileNameTooLarge));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddAttachment_FilenameNotSupplied_ArgumentExceptionThrow()
        {
            var attachmentId = Guid.NewGuid();
            var fileName = String.Empty;
            var contentType = "image";
            var content = new byte[] { 1, 2, 3, 4, 5 };

            _attachmentService = AttachmentServiceFactory.Create(MockRepository.GenerateStub<IJobAttachmentDataRepository>());
            AddAttachment(attachmentId, fileName, contentType, content);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.FileNameRequired));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddAttachment_ContentTypeNotSupplied_ArgumentExceptionThrow()
        {
            var attachmentId = Guid.NewGuid();
            var fileName = "attachment.pdf";
            var contentType = String.Empty;
            var content = new byte[] { 1, 2, 3, 4, 5 };

            _attachmentService = AttachmentServiceFactory.Create(MockRepository.GenerateStub<IJobAttachmentDataRepository>());
            AddAttachment(attachmentId, fileName, contentType, content);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.ContentTypeNotSupplied));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddAttachment_ContentNotSupplied_ArgumentExceptionThrow()
        {
            var attachmentId = Guid.NewGuid();
            var fileName = "attachment.pdf";
            var contentType = "image";
            byte[] content = null;

            _attachmentService = AttachmentServiceFactory.Create(MockRepository.GenerateStub<IJobAttachmentDataRepository>());
            AddAttachment(attachmentId, fileName, contentType, content);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.ContentNotSupplied));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddAttachment_AttachmentIdNotSupplied_ArgumentExceptionThrow()
        {
            var attachmentId = Guid.Empty;
            var fileName = "attachment.pdf";
            var contentType = "image";
            byte[] content = new byte[] { 1, 2, 3, 4, 5 };

            _attachmentService = AttachmentServiceFactory.Create(MockRepository.GenerateStub<IJobAttachmentDataRepository>());
            AddAttachment(attachmentId, fileName, contentType, content);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddAttachment_UserDoesNotHaveMemberRole_ArgumentExceptionThrow()
        {
            var attachmentId = Guid.NewGuid();
            var fileName = "attachment.pdf";
            var contentType = "image";
            byte[] content = new byte[] { 1, 2, 3, 4, 5 };

            _attachmentService = AttachmentServiceFactory.Create(MockRepository.GenerateStub<IJobAttachmentDataRepository>(),
                TestUserContext.Create("graham.robertson@intertek.com", "Graham Robertson", "Operations Manager", UserRole.Public));
            AddAttachment(attachmentId, fileName, contentType, content);
            Assert.IsTrue(_domainValidationException.ResultContainsMessage(JobSystem.Resources.Jobs.Messages.InsufficientSecurityClearance));
        }

        private void AddAttachment(Guid attachmentId, string fileName, string contentType, byte[] content)
        {
            try
            {
                MemoryStream ms = null;
                if (content != null)
                    ms = new MemoryStream(content);
                _attachmentService.Create(new AttachmentData { Id = attachmentId, Content = ms, ContentType = contentType, Filename = fileName });
            }
            catch (DomainValidationException dex)
            {
                _domainValidationException = dex;
            }
        }
    }
}