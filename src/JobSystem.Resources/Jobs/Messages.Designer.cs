﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.544
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobSystem.Resources.Jobs {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("JobSystem.Resources.Jobs.Messages", typeof(Messages).Assembly);
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
        ///   Looks up a localized string similar to The advice number cannot exceed 50 characters.
        /// </summary>
        public static string AdviceNoTooLarge {
            get {
                return ResourceManager.GetString("AdviceNoTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A job must have at least 1 item before it can be approved.
        /// </summary>
        public static string ApprovingJobHasNoItems {
            get {
                return ResourceManager.GetString("ApprovingJobHasNoItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The contact cannot exceed 50 characters.
        /// </summary>
        public static string ContactTooLarge {
            get {
                return ResourceManager.GetString("ContactTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The attachment must have content.
        /// </summary>
        public static string ContentNotSupplied {
            get {
                return ResourceManager.GetString("ContentNotSupplied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The content type must be supplied for the attachment.
        /// </summary>
        public static string ContentTypeNotSupplied {
            get {
                return ResourceManager.GetString("ContentTypeNotSupplied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The content supplied for the attachment must be greater than 0 bytes.
        /// </summary>
        public static string ContentZeroLength {
            get {
                return ResourceManager.GetString("ContentZeroLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A file name is required for the attachment.
        /// </summary>
        public static string FileNameRequired {
            get {
                return ResourceManager.GetString("FileNameRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file name cannot exceed 2000 characters.
        /// </summary>
        public static string FileNameTooLarge {
            get {
                return ResourceManager.GetString("FileNameTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The instructions cannot exceed 2000 characters.
        /// </summary>
        public static string InstructionsTooLarge {
            get {
                return ResourceManager.GetString("InstructionsTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An internal member role is required for this view.
        /// </summary>
        public static string InsufficientSecurityClearance {
            get {
                return ResourceManager.GetString("InsufficientSecurityClearance", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An invalid customer ID was supplied.
        /// </summary>
        public static string InvalidCustomerId {
            get {
                return ResourceManager.GetString("InvalidCustomerId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An invalid type ID was supplied.
        /// </summary>
        public static string InvalidTypeId {
            get {
                return ResourceManager.GetString("InvalidTypeId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This action is not available until the job has been approved.
        /// </summary>
        public static string JobNotApproved {
            get {
                return ResourceManager.GetString("JobNotApproved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are currently no approved jobs.
        /// </summary>
        public static string NoApprovedJobs {
            get {
                return ResourceManager.GetString("NoApprovedJobs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are currently no pending jobs.
        /// </summary>
        public static string NoPendingJobs {
            get {
                return ResourceManager.GetString("NoPendingJobs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The notes cannot exceed 2000 characters.
        /// </summary>
        public static string NotesTooLarge {
            get {
                return ResourceManager.GetString("NotesTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The order number cannot exceed 50 characters.
        /// </summary>
        public static string OrderNoTooLarge {
            get {
                return ResourceManager.GetString("OrderNoTooLarge", resourceCulture);
            }
        }
    }
}
