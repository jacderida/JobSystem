using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace JobSystem.Mvc.Controllers
{
    public class JobController : Controller
    {
        //
        // GET: /Job/

        public ActionResult Index()
        {
            //Placeholder admin role check to see whether user should be shown pending or approved jobs by default
			var isAdmin = true;

			if (isAdmin) 
			{
				return RedirectToAction("PendingJobs");
			}

			return RedirectToAction("ApprovedJobs");
        }

		public ActionResult Details()
		{
			return View();
		}

		public ActionResult PendingJobs()
		{
			return View();
		}

		public ActionResult ApprovedJobs()
		{
			return View();
		}

		public ActionResult HtmlToPdf()
		{
			//var url = Request.Url.GetLeftPart(UriPartial.Authority) + "/CPCDownload.aspx?IsPDF=False?UserID=" + this.CurrentUser.UserID.ToString();
			var url = Request.Url.AbsoluteUri;
			var file = WKHtmlToPdf(url);
			if (file != null)
			{
				Response.ContentType = "Application/pdf";
				Response.BinaryWrite(file);
				Response.End();
			}

			return RedirectToAction("PendingJobs");
		}

		public byte[] WKHtmlToPdf(string url)
		{
			var fileName = " - ";
			var wkhtmlDir = "C:\\Program Files (x86)\\wkhtmltopdf\\";
			var wkhtml = "C:\\Program Files (x86)\\wkhtmltopdf\\wkhtmltopdf.exe";
			var p = new Process();

			p.StartInfo.CreateNoWindow = true;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.RedirectStandardInput = true;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.FileName = wkhtml;
			p.StartInfo.WorkingDirectory = wkhtmlDir;

			string switches = "";
			switches += "--print-media-type ";
			switches += "--margin-top 10mm --margin-bottom 10mm --margin-right 10mm --margin-left 10mm ";
			switches += "--page-size Letter ";
			p.StartInfo.Arguments = switches + " " + url + " " + fileName;
			p.Start();

			//read output
			byte[] buffer = new byte[32768];
			byte[] file;
			using (var ms = new MemoryStream())
			{
				while (true)
				{
					int read = p.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length);

					if (read <= 0)
					{
						break;
					}
					ms.Write(buffer, 0, read);
				}
				file = ms.ToArray();
			}

			// wait or exit
			p.WaitForExit(60000);

			// read the exit code, close process
			int returnCode = p.ExitCode;
			p.Close();

			return returnCode == 0 ? file : null;
		}
    }
}
