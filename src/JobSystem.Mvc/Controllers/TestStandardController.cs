using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.Mvc.ViewModels.TestStandards;
using System;
using System.Linq;

namespace JobSystem.Mvc.Controllers
{
    public class TestStandardController : Controller
    {
        private readonly TestStandardsService _testStandardService;

		public TestStandardController(TestStandardsService testStandardService)
		{
			_testStandardService = testStandardService;
		}

		public ActionResult Index()
		{
			var standards = _testStandardService.GetTestStandards().Select(
				 i => new TestStandardViewModel
				 {
					 Id = i.Id,
					 CertificateNo = i.CertificateNo,
					 SerialNo= i.SerialNo,
					 Description = i.Description
				 }).ToList();
			
			var viewmodel = new TestStandardListViewModel(){
				Standards = standards,
				CreateViewModel = new TestStandardViewModel()
			};
			return View(viewmodel);
		}

		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[Transaction]
		public ActionResult Create(TestStandardViewModel viewmodel)
		{
			_testStandardService.Create(Guid.NewGuid(),
				viewmodel.Description,
				viewmodel.SerialNo,
				viewmodel.CertificateNo);

			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Edit(Guid id)
		{
			var standard = _testStandardService.GetById(id);
			return PartialView("_Edit", new TestStandardViewModel
			{
				Id = id,
				CertificateNo = standard.CertificateNo,
				SerialNo = standard.SerialNo,
				Description = standard.Description
			});
		}

		[HttpPost]
		[Transaction]
		public ActionResult Edit(TestStandardViewModel viewmodel)
		{
			_testStandardService.Edit(viewmodel.Id,
				viewmodel.Description,
				viewmodel.SerialNo,
				viewmodel.CertificateNo);
			return RedirectToAction("Index");
		}
	}
}
