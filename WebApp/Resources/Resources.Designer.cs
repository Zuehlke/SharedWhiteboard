﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
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
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Connection successfull.
        /// </summary>
        public static string ConnectionSuccessfull {
            get {
                return ResourceManager.GetString("ConnectionSuccessfull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to input.
        /// </summary>
        public static string InputFolder {
            get {
                return ResourceManager.GetString("InputFolder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is no active session with the given pin..
        /// </summary>
        public static string NoSessionWithTheGivenPin {
            get {
                return ResourceManager.GetString("NoSessionWithTheGivenPin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not all participants joined the session..
        /// </summary>
        public static string NotAllParticipantsJoinedSession {
            get {
                return ResourceManager.GetString("NotAllParticipantsJoinedSession", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to output.
        /// </summary>
        public static string OutputFolder {
            get {
                return ResourceManager.GetString("OutputFolder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Storage.
        /// </summary>
        public static string StorageFolder {
            get {
                return ResourceManager.GetString("StorageFolder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to templates.
        /// </summary>
        public static string TemplatesFolder {
            get {
                return ResourceManager.GetString("TemplatesFolder", resourceCulture);
            }
        }
    }
}