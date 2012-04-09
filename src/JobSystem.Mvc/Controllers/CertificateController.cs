using System;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.DataAccess.NHibernate.Web;
using JobSystem.DataModel.Entities;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.Mvc.ViewModels.Certificates;
using System.Linq;
using JobSystem.Mvc.ViewModels.TestStandards;

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
			var items = _certificateService.GetCertificates().Select(i => new CertificateIndexViewModel(){
				Id = i.Id,
				CertificateNo = i.CertificateNumber,
				DateCreated = i.DateCreated.ToLongDateString() + ' ' + i.DateCreated.ToShortTimeString(),
				CreatedBy = i.CreatedBy.Name,
				JobNo = i.JobItem.Job.JobNo,
				JobItemNo = i.JobItem.ItemNo.ToString(),
				TypeName = i.Type.Name,
				TestStandards = i.TestStandards.Select(ts => new TestStandardViewModel(){
					CertificateNo = ts.CertificateNo,
					Description = ts.Description,
					SerialNo = ts.SerialNo
				}).ToList()
			}).ToList();
			
			return View(items);
        }

		[HttpGet]
		public ActionResult Create(Guid id)
		{
			var viewmodel = new CertificateViewModel(){
				CertificateTypes = _listItemService.GetAllByCategory(ListItemCategoryType.Certificate).ToSelectList(),
				TestStandards = _testStandardsService.GetTestStandards().ToList().Select(x => new SelectListItem{
						Text = x.Description,
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
