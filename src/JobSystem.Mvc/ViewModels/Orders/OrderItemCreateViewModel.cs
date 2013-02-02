using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace JobSystem.Mvc.ViewModels.Orders
{
    public class OrderItemCreateViewModel
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        [Display(Name = "Supplier")]
        [Required]
        public Guid SupplierId { get; set; }
        public string Instructions { get; set; }
        public string Description { get; set; }
        [Display(Name = "Part Number")]
        public string PartNo { get; set; }
        [Display(Name = "Delivery Days")]
        [Required]
        public int DeliveryDays { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Integer]
        [Min(1, ErrorMessage = "Please enter a quantity of at least 1.")]
        [Required]
        public int Quantity { get; set; }
    }
}