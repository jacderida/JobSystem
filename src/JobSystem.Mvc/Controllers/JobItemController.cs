using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobSystem.DataAccess.NHibernate.Web;

namespace JobSystem.Mvc.Controllers
{
    public class JobItemController : Controller
    {
		[HttpPost]
		[Transaction]
		public ActionResult Create()
        {
            return PartialView("_Create");
        }

    }
}
