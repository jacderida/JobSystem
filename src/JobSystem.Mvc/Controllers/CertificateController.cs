using System;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.DataModel.Entities;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.Mvc.ViewModels.Certificates;
using System.Linq;

namespace JobSystem.Mvc.Controllers
{
    public class CertificateController : Controller
    {
		private readonly CertificateService _certificateService;
		private readonly ListItemService _listItemService;
		private readonly TestStandardsService _testStandardsService;

		public CertificateController(CertificateService certificateService, ListItemService listItemService, TestStandardsService testStandardsService)
		{
			_certificateService = certificateService;
			_listItemService = listItemService;
			_testStandardsService = testStandardsService;
		}

        public ActionResult Index()
        {
			return View();
        }

		[HttpGet]
		public ActionResult Create(Guid id)
		{
			var viewmodel = new CertificateViewModel(){
				CertificateTypes = _listItemService.GetAllByCategory(ListItemCategoryType.Certificate).ToSelectList(),
				TestStandards = _testStandardsService.GetTestStandards().ToList().Select(x => new SelectListItem{
						Text = x.CertificateNo,
						Value = x.Id.ToString()
					}),
				JobItemId = id
			};

			return PartialView("_Create", viewmodel);
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(CertificateViewModel viewmodel)
		{
			_certificateService.Create(Guid.NewGuid(),
				viewmodel.CertificateTypeId,
				viewmodel.JobItemId,
				null,
				viewmodel.SelectedTestStandardIds);
			return RedirectToAction("Details", "JobItem", new { id = viewmodel.JobItemId });
		}
    }
}
