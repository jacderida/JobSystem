using System.Collections.Generic;
using JobSystem.Reporting.Models;
using System;

namespace JobSystem.Reporting.Data
{
    public interface IEquipmentProgressReportDataProvider
    {
        List<EquipmentProgressReportModel> GetEquipmentProgressReportData(Guid customerId);
    }
}