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
    }
}
