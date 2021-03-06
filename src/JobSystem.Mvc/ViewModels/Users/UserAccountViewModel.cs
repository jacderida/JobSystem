﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JobSystem.Mvc.ViewModels.Shared;

namespace JobSystem.Mvc.ViewModels.Users
{
    public class UserAccountViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Job Title")]
        [Required]
        public string JobTitle { get; set; }
        [Display(Name = "Role")]
        public List<CheckboxViewModel> Roles { get; set; }
    }
}