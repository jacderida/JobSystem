﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobSystem.Resources.Orders {
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
    public class OrderItemMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal OrderItemMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("JobSystem.Resources.Orders.OrderItemMessages", typeof(OrderItemMessages).Assembly);
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
        ///   Looks up a localized string similar to A member role is required to perform this operation.
        /// </summary>
        public static string InsufficientSecurity {
            get {
                return ResourceManager.GetString("InsufficientSecurity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The delivery days cannot be less than 0.
        /// </summary>
        public static string InvalidDeliveryDays {
            get {
                return ResourceManager.GetString("InvalidDeliveryDays", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The instructions cannot exceed 2000 characters.
        /// </summary>
        public static string InvalidInstructions {
            get {
                return ResourceManager.GetString("InvalidInstructions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The part number cannot exceed 50 characters.
        /// </summary>
        public static string InvalidPartNo {
            get {
                return ResourceManager.GetString("InvalidPartNo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The price cannot be less than 0.
        /// </summary>
        public static string InvalidPrice {
            get {
                return ResourceManager.GetString("InvalidPrice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The quantity cannot be less than 1.
        /// </summary>
        public static string InvalidQuantity {
            get {
                return ResourceManager.GetString("InvalidQuantity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The job item that this order relates to is still pending approval.
        /// </summary>
        public static string JobPending {
            get {
                return ResourceManager.GetString("JobPending", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The job item is already pending order.
        /// </summary>
        public static string PendingItemExists {
            get {
                return ResourceManager.GetString("PendingItemExists", resourceCulture);
            }
        }
    }
}
