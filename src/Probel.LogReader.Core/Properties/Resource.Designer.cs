﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Probel.LogReader.Core.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Probel.LogReader.Core.Properties.Resource", typeof(Resource).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Show all logs..
        /// </summary>
        internal static string Filter_AllLogs {
            get {
                return ResourceManager.GetString("Filter_AllLogs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to When log category is {0} &apos;{1}&apos;.
        /// </summary>
        internal static string Filter_Category {
            get {
                return ResourceManager.GetString("Filter_Category", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to exactly.
        /// </summary>
        internal static string Filter_Equals {
            get {
                return ResourceManager.GetString("Filter_Equals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to more than.
        /// </summary>
        internal static string Filter_GreaterThan {
            get {
                return ResourceManager.GetString("Filter_GreaterThan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to more than or .
        /// </summary>
        internal static string Filter_GreaterThanOrEquals {
            get {
                return ResourceManager.GetString("Filter_GreaterThanOrEquals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to in.
        /// </summary>
        internal static string Filter_In {
            get {
                return ResourceManager.GetString("Filter_In", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to less than.
        /// </summary>
        internal static string Filter_LessThan {
            get {
                return ResourceManager.GetString("Filter_LessThan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to less than or .
        /// </summary>
        internal static string Filter_LessThanOrEquals {
            get {
                return ResourceManager.GetString("Filter_LessThanOrEquals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to When logs level is {0}  &apos;{1}&apos;.
        /// </summary>
        internal static string Filter_Level {
            get {
                return ResourceManager.GetString("Filter_Level", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to not.
        /// </summary>
        internal static string Filter_NotEquals {
            get {
                return ResourceManager.GetString("Filter_NotEquals", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to not in.
        /// </summary>
        internal static string Filter_NotIn {
            get {
                return ResourceManager.GetString("Filter_NotIn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to When logs occured {0} {1} minute(s) ago.
        /// </summary>
        internal static string Filter_Time {
            get {
                return ResourceManager.GetString("Filter_Time", resourceCulture);
            }
        }
    }
}