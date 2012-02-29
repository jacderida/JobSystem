using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel.Storage;

namespace JobSystem.Mvc.Controllers
{
	public class AttachmentsController : Controller
	{
		private readonly AttachmentService _attachmentService;

		public AttachmentsController(AttachmentService attachmentService)
		{
			_attachmentService = attachmentService;
		}

		public virtual ActionResult AddAttachment(HttpPostedFileBase attachment, System.Guid entityId)
		{
			// touching attachment.InputStream here causes the ASP.Net framework to read the entire upload,
			// which is buffered to disk, see http://msdn.microsoft.com/en-us/library/system.web.httppostedfile.aspx
			var attachmentData = new AttachmentData
			{
				Id = Guid.NewGuid(),
				Content = attachment.InputStream,
				ContentType = attachment.ContentType,
				Filename = attachment.FileName
			};
			_attachmentService.Create(attachmentData);
			return Json(new { Id = attachmentData.Id, Filename = attachmentData.Filename });
		}
	}
}