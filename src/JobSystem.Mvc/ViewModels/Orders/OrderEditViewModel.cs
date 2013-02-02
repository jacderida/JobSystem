using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JobSystem.Mvc.ViewModels.Orders
{
    public class OrderEditViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Supplier")]
        public Guid SupplierId { get; set; }
        public string SupplierName { get; set; }
        [StringLength(255, ErrorMessage = "The instructions cannot exceed 255 characters")]
        public string Instructions { get; set; }
        [Display(Name = "Currency")]
        public IEnumerable<SelectListItem> Currencies { get; set; }
        public Guid CurrencyId { get; set; }
    }
}