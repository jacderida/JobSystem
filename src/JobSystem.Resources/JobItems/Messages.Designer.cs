﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobSystem.Resources.JobItems {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("JobSystem.Resources.JobItems.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The accessories cannot exceed 255 characters.
        /// </summary>
        public static string AccessoriesTooLarge {
            get {
                return ResourceManager.GetString("AccessoriesTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The asset number cannot exceed 50 characters.
        /// </summary>
        public static string AssetNoTooLarge {
            get {
                return ResourceManager.GetString("AssetNoTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The comments cannot exceed 255 characters.
        /// </summary>
        public static string CommentsTooLarge {
            get {
                return ResourceManager.GetString("CommentsTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The instructions cannot exceed 255 characters.
        /// </summary>
        public static string InstructionsTooLarge {
            get {
                return ResourceManager.GetString("InstructionsTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The member role is required for this operation.
        /// </summary>
        public static string InsufficientSecurityClearance {
            get {
                return ResourceManager.GetString("InsufficientSecurityClearance", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The cal period must be greater than 0.
        /// </summary>
        public static string InvalidCalPeriod {
            get {
                return ResourceManager.GetString("InvalidCalPeriod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The list item type used must be a job item status.
        /// </summary>
        public static string InvalidListItemType {
            get {
                return ResourceManager.GetString("InvalidListItemType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You must have a member role to perform this action.
        /// </summary>
        public static string ItemHistoryInsufficientSecurityClearance {
            get {
                return ResourceManager.GetString("ItemHistoryInsufficientSecurityClearance", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The over time must be greater than or equal to 0.
        /// </summary>
        public static string ItemHistoryInvalidOverTime {
            get {
                return ResourceManager.GetString("ItemHistoryInvalidOverTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The work time must be greater than or equal to 0.
        /// </summary>
        public static string ItemHistoryInvalidWorkTime {
            get {
                return ResourceManager.GetString("ItemHistoryInvalidWorkTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The report cannot exceed 255 characters.
        /// </summary>
        public static string ItemHistoryReportTooLarge {
            get {
                return ResourceManager.GetString("ItemHistoryReportTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are currently no job items..
        /// </summary>
        public static string NoJobItems {
            get {
                return ResourceManager.GetString("NoJobItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The return reason cannot exceed 255 characters.
        /// </summary>
        public static string ReturnReasonTooLarge {
            get {
                return ResourceManager.GetString("ReturnReasonTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A serial number is required for the job item.
        /// </summary>
        public static string SerialNoRequired {
            get {
                return ResourceManager.GetString("SerialNoRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The serial number cannot exceed 50 characters.
        /// </summary>
        public static string SerialNoTooLarge {
            get {
                return ResourceManager.GetString("SerialNoTooLarge", resourceCulture);
            }
        }
    }
}
