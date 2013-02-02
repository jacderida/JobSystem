using System;
using System.Collections.Generic;
using JobSystem.Reporting.Models;

namespace JobSystem.Reporting.Data
{
    public interface IQuoteReportDataProvider
    {
        List<QuoteReportModel> GetQuoteReportData(Guid quoteId);
    }
}