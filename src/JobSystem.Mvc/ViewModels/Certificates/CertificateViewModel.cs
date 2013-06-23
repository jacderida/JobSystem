using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JobSystem.Mvc.ViewModels.Certificates
{
    public class CertificateViewModel
    {
        public Guid Id { get; set; }
        public Guid JobItemId { get; set; }
        public Guid CertificateTypeId { get; set; }
        public Guid CategoryId { get; set; }
        [Display(Name="Type")]
        public IEnumerable<SelectListItem> CertificateTypes { get; set; }
        [Display(Name = "Category")]
        public IEnumerable<SelectListItem> CertificateCategories { get; set; }
        public AttachmentViewModel[] Attachments { get; set; }
    }
}