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
			var path = @"C:\\Temp\\100\\";
			var file = string.Empty;

			try
			{
				var stream = Request.InputStream;
				if (String.IsNullOrEmpty(Request["qqfile"]))
				{
					// IE
					HttpPostedFileBase postedFile = Request.Files[0];
					stream = postedFile.InputStream;
					file = Path.Combine(path, System.IO.Path.GetFileName(Request.Files[0].FileName));
				}
				else
				{
					//Webkit, Mozilla
					file = Path.Combine(path, qqfile);
				}

				var buffer = new byte[stream.Length];
				stream.Read(buffer, 0, buffer.Length);
				System.IO.File.WriteAllBytes(file, buffer);
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message }, "application/json");
			}

			return Json(new { success = true }, "text/html");

			//// touching attachment.InputStream here causes the ASP.Net framework to read the entire upload,
			//// which is buffered to disk, see http://msdn.microsoft.com/en-us/library/system.web.httppostedfile.aspx
			//var attachmentData = new AttachmentData
			//{
			//    Id = Guid.NewGuid(),
			//    Content = attachment.InputStream,
			//    ContentType = attachment.ContentType,
			//    Filename = attachment.FileName
			//};
			//_attachmentService.Create(attachmentData);
			//return Json(new { Id = attachmentData.Id, Filename = attachmentData.Filename });
		}
	}
}