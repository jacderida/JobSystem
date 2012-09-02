using System;
using System.Web.Mvc;
using JobSystem.Storage;
using JobSystem.Storage.Jobs;

namespace JobSystem.Mvc.Controllers
{
	public class AttachmentsController : Controller
	{
		private readonly JobAttachmentService _jobAttachmentService;

		public AttachmentsController(JobAttachmentService attachmentService)
		{
			_jobAttachmentService = attachmentService;
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
			_jobAttachmentService.Create(attachmentData);
			return Json(new { success = true, Id = attachmentData.Id, Filename = attachmentData.Filename }, "text/html");
		}
	}
}