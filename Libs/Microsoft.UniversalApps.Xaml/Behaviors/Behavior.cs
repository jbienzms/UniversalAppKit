#region License
/******************************************************************************
 * COPYRIGHT © MICROSOFT CORP. 
 * MICROSOFT LIMITED PERMISSIVE LICENSE (MS-LPL)
 * This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.
 * 1. Definitions
 * The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.
 * A "contribution" is the original software, or any additions or changes to the software.
 * A "contributor" is any person that distributes its contribution under this license.
 * "Licensed patents" are a contributor's patent claims that read directly on its contribution.
 * 2. Grant of Rights
 * (A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
 * (B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
 * 3. Conditions and Limitations
 * (A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
 * (B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
 * (C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
 * (D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
 * (E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.
 * (F) Platform Limitation- The licenses granted in sections 2(A) & 2(B) extend only to the software or derivative works that you create that run on a Microsoft Windows operating system product.
 ******************************************************************************/
#endregion // License
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace Microsoft.UniversalApps.Behaviors
{
    /// <summary>
    /// The base class for a behavior.
    /// </summary>
    /// <typeparam name="T">
    /// The type of object the behavior can be attached to. This type must inherit from <see cref="DependencyObject"/>.
    /// </typeparam>
    public class Behavior<T> : DependencyObject, IBehavior where T : DependencyObject
    {
        #region Overridables / Event Triggers
        /// <summary>
        /// Allows derived classes to handle being attached to an object.
        /// </summary>
        protected virtual void OnAttached() { }

        /// <summary>
        /// Allows derived classes to handle being detached from an object.
        /// </summary>
        protected virtual void OnDetaching() { }
        #endregion // Overridables / Event Triggers

        #region Public Methods
        /// <summary>
        /// Attaches the behavior to the specified dependency object.
        /// </summary>
        /// <param name="associatedObject">
        /// The dependency object that the behavior should be attached to.
        /// </param>
        public void Attach(DependencyObject associatedObject)
        {
            // Make sure we're not already attached
            if (this.AssociatedObject != null) { throw new InvalidOperationException("The behavior is already attached."); }

            // Validate
            if (associatedObject == null) throw new ArgumentNullException("associatedObject");

            // Validate type
            if (!(associatedObject is T)) { throw new InvalidOperationException(string.Format("This behavior can only be attached to an object of type {0}", typeof(T).Name)); }

            // Store
            AssociatedObject = (T)associatedObject;

            // Notify
            OnAttached();
        }

        /// <summary>
        /// Detaches the behavior from its attached object.
        /// </summary>
        public void Detach()
        {
            // Validate
            if (AssociatedObject == null) { throw new InvalidOperationException("The behavior is not attached."); }

            // Notify
            OnDetaching();

            // Clear
            AssociatedObject = null;
        }
        #endregion // Public Methods

        #region Public Properties
        /// <summary>
        /// Gets the dependency object that the behavior is attached to.
        /// </summary>
        public T AssociatedObject { get; private set; }
        #endregion // Public Properties

        #region IBehavior Members
        DependencyObject IBehavior.AssociatedObject
        {
            get
            {
                return this.AssociatedObject;
            }
        }
        #endregion // IBehavior Members
    }

    /// <summary>
    /// The base class for a behavior that can be attached to any <see cref="DependencyObject"/>.
    /// </summary>
    public class Behavior : Behavior<DependencyObject> { }
}
