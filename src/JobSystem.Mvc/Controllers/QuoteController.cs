using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobSystem.Mvc.ViewModels.Quotes;

namespace JobSystem.Mvc.Controllers
{
	public class QuoteController : Controller
	{
		//private readonly QuoteService _quoteService;

		// public QuoteController(QuoteService quoteService)
		// {
		//     _quoteService = quoteService;
		// }

		public ActionResult Index()
		{
			IList<QuoteIndexViewModel> viewmodels = new List<QuoteIndexViewModel>();

			for (int i = 0; i < 10; i++)
			{
				viewmodels.Add(new QuoteIndexViewModel()
				{
					Id = i.ToString(),
					AdviceNo = "ADV" + i.ToString(),
					OrderNo = "ORDN" + i.ToString()
				});
			}

			return View("PendingQuotes", viewmodels);
		}

		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

	}
}
