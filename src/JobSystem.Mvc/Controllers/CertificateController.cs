using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JobSystem.BusinessLogic.Services;
using JobSystem.BusinessLogic.Validation.Core;
using JobSystem.DataAccess.NHibernate;
using JobSystem.DataModel.Entities;
using JobSystem.Mvc.Core.UIValidation;
using JobSystem.Mvc.Core.Utilities;
using JobSystem.Mvc.ViewModels.Certificates;

namespace JobSystem.Mvc.Controllers
{
    public class CertificateController : Controller
    {
        private readonly CertificateService _certificateService;
        private readonly ListItemService _listItemService;

        public CertificateController(CertificateService certificateService, ListItemService listItemService)
        {
            _certificateService = certificateService;
            _listItemService = listItemService;
        }

        public ActionResult Index()
        {
            var items = _certificateService.GetCertificates().Select(i => new CertificateIndexViewModel
            {
                Id = i.Id,
                CertificateNo = i.CertificateNumber,
                DateCreated = i.DateCreated.ToLongDateString() + ' ' + i.DateCreated.ToShortTimeString(),
                CreatedBy = i.CreatedBy.Name,
                JobNo = i.JobItem.Job.JobNo,
                JobItemNo = i.JobItem.ItemNo.ToString(),
                TypeName = i.Type.Name
            }).ToList();
            return View(items);
        }

        [HttpGet]
        public ActionResult Create(Guid id)
        {
            var viewmodel = new CertificateViewModel()
            {
                CertificateTypes = _listItemService.GetAllByCategory(ListItemCategoryType.Certificate).ToSelectList(),
                JobItemId = id
            };
            return PartialView("_Create", viewmodel);
        }

        [HttpPost]
        public ActionResult Create(CertificateViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    NHibernateSession.Current.BeginTransaction();
                    _certificateService.Create(Guid.NewGuid(), viewmodel.CertificateTypeId, Guid.Empty, viewmodel.JobItemId, null);
                    NHibernateSession.Current.Transaction.Commit();
                    return RedirectToAction("Details", "JobItem", new { id = viewmodel.JobItemId });
                }
                catch (DomainValidationException dex)
                {
                    NHibernateSession.Current.Transaction.Rollback();
                    ModelState.UpdateFromDomain(dex.Result);
                }
                finally
                {
                    NHibernateSession.Current.Transaction.Dispose();
                }
            }
            return PartialView("_Create", viewmodel);
        }

        [HttpPost]
        public ActionResult SearchByKeyword(string keyword)
        {
            var certificates = _certificateService.SearchByKeyword(keyword);
            var certificateViewModels = new List<CertificateIndexViewModel>();
            foreach (var cert in certificates)
            {
                certificateViewModels.Add(new CertificateIndexViewModel(){
                    CertificateNo = cert.CertificateNumber,
                    CreatedBy = cert.CreatedBy.Name,
                    DateCreated = cert.DateCreated.ToLongDateString() + ' ' + cert.DateCreated.ToShortTimeString(),
                    Id = cert.Id,
                    JobItemNo = cert.JobItem.ItemNo.ToString(),
                    JobNo = cert.JobItem.Job.JobNo,
                    TypeName = cert.Type.Name
                });
            }
            return PartialView("_SearchResults", certificateViewModels);
        }
    }
}