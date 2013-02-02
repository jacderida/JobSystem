using System;

namespace JobSystem.DataAnnotations
{
    /// <summary>
    /// Attribute for giving Properies human-readable names, used for 
    /// </summary>
    public class ReadableNameAttribute : Attribute
    {
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadableNameAttribute"/> class.
        /// </summary>
        /// <param name="name">The human readable name for the property.</param>
        public ReadableNameAttribute(string name)
        {
            Name = name;
        }
    }
}