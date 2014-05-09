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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Microsoft.UniversalApps.Behaviors
{
    /// <summary>
    /// A behavior that inheirts visual state changes from a higher source in the visual tree.
    /// </summary>
    /// <remarks>
    /// This behavior allows controls that are nested deep in the user interface to respond 
    /// to visual state changes at a higher level (for example on the page).
    /// 
    /// When this behavior is attached it walks up the visual tree looking for a behavior that 
    /// implements <see cref="INotifyVisualStateChanged"/>. If found, it subscribes to the 
    /// <see cref="INotifyVisualStateChanged.VisualStateChanged">VisualStateChanged</see> event 
    /// and updates the state of the attached object any time the parent state changes.
    /// </remarks>
    public class LinkedStateBehavior : VisualStateBehavior
    {
        #region Static Version
        #region Internal Methods
        /// <summary>
        /// Attempts to find a visual state change provider in the parent tree.
        /// </summary>
        /// <param name="obj">
        /// The object to begin searching in.
        /// </param>
        /// <returns>
        /// The provider, if found; otherwise <see langword="null"/>.
        /// </returns>
        static private INotifyVisualStateChanged FindParentStateProvider(DependencyObject obj)
        {
            // See if this one has it
            var chg = (from b in Interaction.GetBehaviors(obj)
                       where b is INotifyVisualStateChanged
                       select b as INotifyVisualStateChanged).FirstOrDefault();

            // If found, return it
            if (chg != null) { return chg; }

            // Not found. Try parent?
            var fe = obj as FrameworkElement;
            if ((fe != null) && (fe.Parent != null))
            {
                return FindParentStateProvider(fe.Parent);
            }

            // Not found
            return null;
        }
        #endregion // Internal Methods
        #endregion // Static Version

        #region Instance Version
        #region Member Variables
        private string currentStateName = "";
        private INotifyVisualStateChanged stateProvider;
        #endregion // Member Variables

        #region Overrides / Event Handlers
        protected override void OnAttached()
        {
            base.OnAttached();

            // Find provider
            if (AssociatedObject.Parent != null)
            {
                stateProvider = FindParentStateProvider(AssociatedObject.Parent);

                // If found, subscribe to events
                if (stateProvider != null)
                {
                    stateProvider.VisualStateChanged += StateProvider_VisualStateChanged;
                }
            }
        }

        protected override void OnDetaching()
        {
            if (stateProvider != null)
            {
                stateProvider.VisualStateChanged -= StateProvider_VisualStateChanged;
            }
            base.OnDetaching();
        }

        private void StateProvider_VisualStateChanged(object sender, VisualStateEventArgs e)
        {
            // Store in case public version of Apply is called
            currentStateName = e.StateName;

            // Apply using shortcut version
            ApplyState(currentStateName, e.UseTransitions, e.ForceUpdate);
        }

        protected override bool TryCalculateStateName(out string stateName)
        {
            if (!string.IsNullOrEmpty(currentStateName))
            {
                stateName = currentStateName;
                return true;
            }
            else
            {
                stateName = null;
                return false;
            }
            
        }
        #endregion // Overrides / Event Handlers
        #endregion // Instance Version
    }
}
