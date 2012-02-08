using System;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.Mvc.ViewModels.Consignments;

namespace JobSystem.Mvc.Controllers
{
    public class ConsignmentController : Controller
    {
        private readonly ConsignmentService _consignmentService;
		
		public ConsignmentController(ConsignmentService consignmentService)
		{
			_consignmentService = consignmentService;
		}

        public ActionResult Create(Guid Id)
        {
            var viewmodel = new ConsignmentCreateViewModel(){
				JobItemId = Id
			};
			return PartialView("_Create", viewmodel);
        }

		[HttpPost]
		[Transaction]
		public ActionResult Create(ConsignmentCreateViewModel viewmodel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_consignmentService.Create(System.Guid.NewGuid(), viewmodel.SupplierId);
					return RedirectToAction("Details", "JobItem", new { Id = viewmodel.JobItemId });
				}
				catch (DomainValidationException dex)
				{
					ModelState.UpdateFromDomain(dex.Result);
				}
			}
			return PartialView("_Create", viewmodel);
		}
    }
}
