using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobSystem.Reporting.Models
{
    public abstract class ReportModelBase
    {
        public virtual string CompanyName { get; set; }
        public virtual string CompanyAddress { get; set; }
        public virtual string CompanyAddress1 { get; set; }
        public virtual string CompanyAddress2 { get; set; }
        public virtual string CompanyAddress3 { get; set; }
        public virtual string CompanyAddress4 { get; set; }
        public virtual string CompanyAddress5 { get; set; }
        public virtual string CompanyContactInfo { get; set; }
        public virtual string CompanyTelephone { get; set; }
        public virtual string CompanyFax { get; set; }
        public virtual string CompanyRegNo { get; set; }
        public virtual string CompanyVatRegNo { get; set; }
        public virtual string CompanyEmail { get; set; }
        public virtual string CompanyWww { get; set; }
        public virtual string CompanyTermsAndConditions { get; set; }
    }
}