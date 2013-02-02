using System;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.DataAnnotations
{
    /// <summary>
    /// Data annotation to apply to a property for an email address that isn't required.
    /// </summary>
    public class EmailAddressOptionalAttribute : ValidationAttribute
    {
        /// <summary>
        /// Determines whether the applied property is valid.
        /// </summary>
        /// <param name="value">The value of the property to validate.</param>
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
            return true;
        }
    }
}