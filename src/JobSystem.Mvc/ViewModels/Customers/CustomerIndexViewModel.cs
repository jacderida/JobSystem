using System;

namespace JobSystem.Mvc.ViewModels.Customers
{
    public class CustomerIndexViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AssetLine { get; set; }
        public string Email { get; set; }
        public string Contact1 { get; set; }
    }
}