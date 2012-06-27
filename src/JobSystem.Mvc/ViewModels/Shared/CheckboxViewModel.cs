using System;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Mvc.ViewModels.Shared
{
	public class CheckboxViewModel
	{
		public int Id { get; set; }
		public bool IsChecked { get; set; }
		public string Name { get; set; }
	}
}