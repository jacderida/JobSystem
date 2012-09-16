using System;
using System.Linq;
using System.Web.Mvc;
using JobSystem.Admin.Mvc.Data;
using JobSystem.Admin.Mvc.Models;
using JobSystem.Admin.DbWireup;
using JobSystem.DataModel.Entities;
using System.Drawing;
using System.Reflection;
using System.IO;
using JobSystem.Framework.Configuration;

namespace JobSystem.Admin.Mvc.Controllers
{
	public class TenantController : Controller
	{
		private readonly ITenantRepository _tenantRepository;
		private readonly IConnectionStringProviderRepository _connectionStringRepository;
		private Guid _defaultBankDetailsId;
		private Guid _defaultCurrencyId;
		private Guid _defaultTaxCodeId;
		private Guid _defaultPaymentTermId;

		public TenantController(
			ITenantRepository tenantRepository,
			IConnectionStringProviderRepository connectionStringRepository)
		{
			_tenantRepository = tenantRepository;
			_connectionStringRepository = connectionStringRepository;
		}

		public ActionResult Success()
		{
			return View();
		}

		public ActionResult Error()
		{
			return View();
		}

		public ActionResult Index()
		{
			return View(_tenantRepository.GetTenants().Select(
				t => new TenantIndexModel { Id = t.Item1, Name = t.Item2, CompanyName = t.Item3 }));
		}

		public ActionResult Create()
		{
			var model = new TenantModel
			{
				JobSeed = 2000,
				ConsignmentSeed = 2000,
				QuoteSeed = 2000,
				OrderSeed = 2000,
				CertificateSeed = 2000,
				DeliverySeed = 2000,
				InvoiceSeed = 2000
			};
			return View(model);
		}

		[HttpPost]
		public ActionResult Create(TenantModel model)
		{
			if (model.TenantName.Contains(' '))
			{
				ModelState.AddModelError("TenantName", "The Tenant name cannot contain a space");
				return View(model);
			}
			if (_tenantRepository.TenantNameExists(model.TenantName))
			{
				ModelState.AddModelError("TenantName", "A tenant with this name already exists");
				return View(model);
			}
			model.Id = Guid.NewGuid();
			model.CompanyId = Guid.NewGuid();
			model.BankDetailsId = Guid.NewGuid();
			model.TenantName = model.TenantName.ToLower();
			_defaultBankDetailsId = model.BankDetailsId;
			var databaseService = new JobSystemDatabaseCreationService(model.TenantName);
			try
			{
				databaseService.CreateDatabase(true);
				databaseService.CreateJobSystemSchemaFromMigrations(Server.MapPath("~/bin/JobSystem.Migrations.dll"));
				databaseService.InitHibernate(Server.MapPath("~/bin/JobSystem.DataAccess.NHibernate.dll"));
				databaseService.BeginHibernateTransaction();
				databaseService.InsertDefaultData(BuildDefaultData(model));
				databaseService.InsertCompanyDetails(GetCompanyDetails(databaseService, model));
				databaseService.InsertAdminUser(
					model.AdminUserEmailAddress, model.AdminName, model.AdminJobTitle, model.AdminPassword);
				databaseService.CommitHibernateTransaction();
				_tenantRepository.InsertTenant(model.Id, model.TenantName, model.CompanyName);
				_tenantRepository.InsertTenantConnectionString(model.TenantName, databaseService.GetTenantConnectionString());
				_connectionStringRepository.Put(model.TenantName, databaseService.GetTenantConnectionString());
			}
			catch (Exception)
			{
				return RedirectToAction("Error");
			}
			return RedirectToAction("Success");
		}

		private JobSystemDefaultData BuildDefaultData(TenantModel model)
		{
			var builder = new JobSystemDefaultDataBuilder();
			AddListItemDefaultData(builder);
			AddBankDetailsDefaultData(model, builder);
			AddPaymentTermsDefaultData(builder);
			AddCurrenciesDefaultData(builder);
			AddTaxCodesDefaultData(builder);
			AddEntitySeedsDefaultData(model, builder);
			return builder.Build();
		}

		private CompanyDetails GetCompanyDetails(JobSystemDatabaseCreationService databaseService, TenantModel model)
		{
			var logoImage = Image.FromStream(
				Assembly.GetExecutingAssembly().GetManifestResourceStream("JobSystem.Admin.Mvc.intertek_logon.gif"));
			var ms = new MemoryStream();
			logoImage.Save(ms, logoImage.RawFormat);
			return new CompanyDetails
			{
				Id = model.CompanyId,
				Name = model.CompanyName,
				Address1 = model.Address1,
				Address2 = model.Address2,
				Address3 = model.Address3,
				Address4 = model.Address4,
				Address5 = model.Address5,
				Telephone = model.Telephone,
				Fax = model.Fax,
				Email = model.Email,
				RegNo = model.RegNo,
				VatRegNo = model.VatRegNo,
				TermsAndConditions = model.TermsAndConditions,
				Www = model.Www,
				DefaultBankDetails = databaseService.GetBankDetails(_defaultBankDetailsId),
				DefaultCurrency = databaseService.GetCurrency(_defaultCurrencyId),
				DefaultPaymentTerm = databaseService.GetPaymentTerm(_defaultPaymentTermId),
				DefaultTaxCode = databaseService.GetTaxCode(_defaultTaxCodeId),
				DefaultCultureCode = "en-GB",
				MainLogo = ms.ToArray()
			};
		}

		private void AddListItemDefaultData(JobSystemDefaultDataBuilder builder)
		{
			builder
				.WithListItemCategories(
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.JobTypeId, "Job Type", ListItemCategoryType.JobType),
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.CategoryId, "Category", ListItemCategoryType.JobItemCategory),
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.InitialStatusId, "Initial Status", ListItemCategoryType.JobItemInitialStatus),
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.StatusId, "Status", ListItemCategoryType.JobItemStatus),
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.WorkTypeId, "Work Type", ListItemCategoryType.JobItemWorkType),
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.WorkStatusId, "Status", ListItemCategoryType.JobItemWorkStatus),
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.PaymentTermId, "Payment Term", ListItemCategoryType.PaymentTerm),
					Tuple.Create<Guid, string, ListItemCategoryType>(ListCategoryIds.CertificateId, "Certificate", ListItemCategoryType.Certificate))
				.WithJobTypes(
					Tuple.Create<string, ListItemType, Guid>("Lab Service", ListItemType.JobTypeField, ListCategoryIds.JobTypeId),
					Tuple.Create<string, ListItemType, Guid>("Site Service", ListItemType.JobTypeService, ListCategoryIds.JobTypeId))
				.WithCertificateTypes(
					Tuple.Create<string, ListItemType, Guid>("House", ListItemType.CertificateTypeHouse, ListCategoryIds.CertificateId),
					Tuple.Create<string, ListItemType, Guid>("UKAS", ListItemType.CertificateTypeUkas, ListCategoryIds.CertificateId))
				.WithJobItemCategories(
					Tuple.Create<string, ListItemType, Guid>("T - Temperature", ListItemType.CategoryTemperature, ListCategoryIds.CategoryId),
					Tuple.Create<string, ListItemType, Guid>("E - Electrical", ListItemType.CategoryElectrical, ListCategoryIds.CategoryId),
					Tuple.Create<string, ListItemType, Guid>("D - Density", ListItemType.CategoryDensity, ListCategoryIds.CategoryId),
					Tuple.Create<string, ListItemType, Guid>("M - Mechanical", ListItemType.CategoryMechanical, ListCategoryIds.CategoryId),
					Tuple.Create<string, ListItemType, Guid>("O - Outsource", ListItemType.CategorySubContract, ListCategoryIds.CategoryId),
					Tuple.Create<string, ListItemType, Guid>("H - Hire", ListItemType.CategoryHire, ListCategoryIds.CategoryId),
					Tuple.Create<string, ListItemType, Guid>("R - Resale", ListItemType.CategoryResale, ListCategoryIds.CategoryId),
					Tuple.Create<string, ListItemType, Guid>("S - Site Services", ListItemType.CategoryField, ListCategoryIds.CategoryId),
					Tuple.Create<string, ListItemType, Guid>("P - Pressure", ListItemType.CategoryPressure, ListCategoryIds.CategoryId))
				.WithJobItemInitialStatusItems(
					Tuple.Create<string, ListItemType, Guid>("UKAS Calibration", ListItemType.InitialStatusUkasCalibration, ListCategoryIds.InitialStatusId),
					Tuple.Create<string, ListItemType, Guid>("House Calibration", ListItemType.InitialStatusHouseCalibration, ListCategoryIds.InitialStatusId),
					Tuple.Create<string, ListItemType, Guid>("Sub Contract", ListItemType.InitialStatusSubContract, ListCategoryIds.InitialStatusId),
					Tuple.Create<string, ListItemType, Guid>("Repair", ListItemType.InitialStatusRepair, ListCategoryIds.InitialStatusId))
				.WithJobItemWorkStatusItems(
					Tuple.Create<string, ListItemType, Guid>("Calibrated", ListItemType.WorkStatusCalibrated, ListCategoryIds.WorkStatusId),
					Tuple.Create<string, ListItemType, Guid>("Repaired", ListItemType.WorkStatusRepaired, ListCategoryIds.WorkStatusId),
					Tuple.Create<string, ListItemType, Guid>("Failed", ListItemType.WorkStatusFailed, ListCategoryIds.WorkStatusId),
					Tuple.Create<string, ListItemType, Guid>("Investigated", ListItemType.WorkStatusInvestigated, ListCategoryIds.WorkStatusId),
					Tuple.Create<string, ListItemType, Guid>("Returned", ListItemType.WorkStatusReturned, ListCategoryIds.WorkStatusId),
					Tuple.Create<string, ListItemType, Guid>("Completed", ListItemType.WorkStatusCompleted, ListCategoryIds.WorkStatusId),
					Tuple.Create<string, ListItemType, Guid>("Reviewed", ListItemType.WorkStatusReviewed, ListCategoryIds.WorkStatusId))
				.WithJobItemStatusItems(
					Tuple.Create<string, ListItemType, Guid>("Quote Prepared", ListItemType.StatusQuotedPrepared, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Quote Accepted", ListItemType.StatusQuoteAccepted, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Quote Rejected", ListItemType.StatusQuoteRejected, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Consigned", ListItemType.StatusConsigned, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Order Reviewed", ListItemType.StatusOrderReviewed, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Ordered", ListItemType.StatusOrdered, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Item with Sub Contractor", ListItemType.StatusItemWithSubContractor, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Awaiting Parts", ListItemType.StatusAwaitingParts, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Delivery Note Produced", ListItemType.StatusDeliveryNoteProduced, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Booked In", ListItemType.StatusBookedIn, ListCategoryIds.StatusId),
					Tuple.Create<string, ListItemType, Guid>("Invoiced", ListItemType.StatusInvoiced, ListCategoryIds.StatusId))
				.WithJobItemWorkTypes(
					Tuple.Create<string, ListItemType, Guid>("Calibration", ListItemType.WorkTypeCalibration, ListCategoryIds.WorkTypeId),
					Tuple.Create<string, ListItemType, Guid>("Repair", ListItemType.WorkTypeRepair, ListCategoryIds.WorkTypeId),
					Tuple.Create<string, ListItemType, Guid>("Investigation", ListItemType.WorkTypeInvestigation, ListCategoryIds.WorkTypeId),
					Tuple.Create<string, ListItemType, Guid>("Administration", ListItemType.WorkTypeAdministration, ListCategoryIds.WorkTypeId));
		}

		private void AddBankDetailsDefaultData(TenantModel model, JobSystemDefaultDataBuilder builder)
		{
			builder.WithBankDetails(
				new BankDetails
				{
					Id = _defaultBankDetailsId,
					Name = model.BankName,
					ShortName = model.BankShortName,
					AccountNo = model.AccountNo,
					SortCode = model.SortCode,
					Address1 = model.BankAddress1,
					Address2 = model.BankAddress2,
					Address3 = model.BankAddress3,
					Address4 = model.BankAddress4,
					Address5 = model.BankAddress5,
					Iban = model.Iban
				});
		}

		private void AddPaymentTermsDefaultData(JobSystemDefaultDataBuilder builder)
		{
			_defaultPaymentTermId = Guid.NewGuid();
			builder.WithPaymentTerms(
				Tuple.Create<Guid, string, ListItemType, Guid>(_defaultPaymentTermId, "30 Days", ListItemType.PaymentTerm30Days, ListCategoryIds.PaymentTermId),
				Tuple.Create<Guid, string, ListItemType, Guid>(Guid.NewGuid(), "As Agreed", ListItemType.PaymentTermAsAgreed, ListCategoryIds.PaymentTermId));
		}

		private void AddCurrenciesDefaultData(JobSystemDefaultDataBuilder builder)
		{
			_defaultCurrencyId = Guid.NewGuid();
			builder.WithCurrencies(
				new Currency { Id = _defaultCurrencyId, Name = "GBP", DisplayMessage = "All prices in GBP" },
				new Currency { Id = Guid.NewGuid(), Name = "EUR", DisplayMessage = "All prices in Euros" },
				new Currency { Id = Guid.NewGuid(), Name = "USD", DisplayMessage = "All prices in Dollars" });
		}

		private void AddTaxCodesDefaultData(JobSystemDefaultDataBuilder builder)
		{
			_defaultTaxCodeId = Guid.NewGuid();
			builder.WithTaxCodes(
				new TaxCode { Id = Guid.NewGuid(), TaxCodeName = "T0", Rate = 0, Description = "No VAT" },
				new TaxCode { Id = _defaultTaxCodeId, TaxCodeName = "T1", Rate = 0.20, Description = "VAT at 20%" });
		}

		private void AddEntitySeedsDefaultData(TenantModel model, JobSystemDefaultDataBuilder builder)
		{
			builder.WithEntitySeeds(
				Tuple.Create<Type, int, string>(typeof(Job), model.JobSeed, "JR"),
				Tuple.Create<Type, int, string>(typeof(Consignment), model.ConsignmentSeed, "CR"),
				Tuple.Create<Type, int, string>(typeof(Quote), model.QuoteSeed, "QR"),
				Tuple.Create<Type, int, string>(typeof(Order), model.OrderSeed, "OR"),
				Tuple.Create<Type, int, string>(typeof(Certificate), model.CertificateSeed, "CERT"),
				Tuple.Create<Type, int, string>(typeof(Delivery), model.DeliverySeed, "DR"),
				Tuple.Create<Type, int, string>(typeof(Invoice), model.InvoiceSeed, "IR"));
		}
	}
}