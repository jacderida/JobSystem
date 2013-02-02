using System;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Mvc.ViewModels.Deliveries
{
    public class DeliveryCreateViewModel
    {
        public Guid Id { get; set; }
        public Guid JobItemId { get; set; }

        [Required]
        public string Notes { get; set; }

        [Display(Name="FAO")]
        public string Fao { get; set; }

        [Display(Name="Deliver Individually")]
        public bool IsIndividual { get; set; }
    }
}