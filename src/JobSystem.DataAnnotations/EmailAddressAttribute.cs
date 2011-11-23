using System;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.DataAnnotations
{
	/// <summary>
	/// Data annotation to apply to a property for an email address that is required.
	/// </summary>
	public class EmailAddressAttribute : ValidationAttribute
	{
		/// <summary>
		/// override of isvalid
		/// </summary>
		/// <param name="value">the object to check</param>
		/// <returns>True if the string contains an @ symbol.</returns>
		public override bool IsValid(object value)
		{
			var str = value as string;
			if (!String.IsNullOrEmpty(str))
			{
				int index = str.IndexOf("@");
				bool found = index != -1;
				bool start = index == 0;
				bool end = index == str.Length - 1;
				return found && !(start || end);
			}
			return false;
		}
	}
}