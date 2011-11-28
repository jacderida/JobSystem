using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobSystem.DataModel.Dto
{
	public class Address
	{
		public string Line1 { get; set; }
		public string Line2 { get; set; }
		public string Line3 { get; set; }
		public string Line4 { get; set; }
		public string Line5 { get; set; }

		public static Address GetAddressFromLineDetails(
			string line1, string line2, string line3, string line4, string line5)
		{
			return new Address
			{
				Line1 = !String.IsNullOrEmpty(line1) ? line1 : String.Empty,
				Line2 = !String.IsNullOrEmpty(line2) ? line2 : String.Empty,
				Line3 = !String.IsNullOrEmpty(line3) ? line3 : String.Empty,
				Line4 = !String.IsNullOrEmpty(line4) ? line4 : String.Empty,
				Line5 = !String.IsNullOrEmpty(line5) ? line5 : String.Empty
			};
		}
	}
}