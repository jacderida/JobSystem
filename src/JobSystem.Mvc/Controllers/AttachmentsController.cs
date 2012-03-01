using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataModel.Storage;
using System.IO;

namespace JobSystem.Mvc.Controllers
{
	public class AttachmentsController : Controller
	{
		private readonly AttachmentService _attachmentService;

		public AttachmentsController(AttachmentService attachmentService)
		{
			_attachmentService = attachmentService;
		}

		public ActionResult AddAttachment(string qqfile)
		{
			var runningInIE = String.IsNullOrEmpty(Request["qqfile"]);
			var attachmentData = new AttachmentData
			{
				Id = Guid.NewGuid(),
				ContentType = Request.Headers["Content-Type"] ?? null,
				Filename = runningInIE ? Request.Files[0].FileName : qqfile,
				Content = Request.InputStream
			};
			_attachmentService.Create(attachmentData);
			return Json(new { success = true, Id = attachmentData.Id, Filename = attachmentData.Filename });
		}
	}
}