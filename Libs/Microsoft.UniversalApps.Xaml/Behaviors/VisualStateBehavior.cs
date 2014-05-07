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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.UniversalApps.Behaviors
{
    /// <summary>
    /// Provides data for events about visual states.
    /// </summary>
    public class VisualStateEventArgs : EventArgs
    {
        /// <summary>
        /// Iniatializes a new <see cref="VisualStateEventArgs"/> instance.
        /// </summary>
        /// <param name="stateName">
        /// The name of the current visual state.
        /// </param>
        /// <param name="useTransitions">
        /// <c>true</c> if transitions should be used; otherwise <c>false</c>.
        /// </param>
        /// <param name="forceUpdate">
        /// <c>true</c> if the state should be updated even if the state name has not changed; otherwise <c>false</c>.
        /// </param>
        public VisualStateEventArgs(string stateName, bool useTransitions, bool forceUpdate)
        {
            // Validate
            if (string.IsNullOrEmpty(stateName)) throw new ArgumentException("stateName");

            // Store
            this.ForceUpdate = forceUpdate;
            this.StateName = stateName;
            this.UseTransitions = useTransitions;
        }

        /// <summary>
        /// Gets or sets a value that indicates if the state should be updated even if the state name has not changed.
        /// </summary>
        /// <value>
        /// <c>true</c> if the state should be updated even if the state name has not changed; otherwise <c>false</c>.
        /// </value>
        public bool ForceUpdate { get; private set; }

        /// <summary>
        /// Gets the name of the visual state.
        /// </summary>
        public string StateName { get; private set;}

        /// <summary>
        /// Gets a value that indicates if transitions should be used.
        /// </summary>
        /// <value>
        /// <c>true</c> if transitions should be used; otherwise <c>false</c>.
        /// </value>
        public bool UseTransitions { get; private set; }
    }

    /// <summary>
    /// The interface for a class that provides visual state change notifications.
    /// </summary>
    public interface INotifyVisualStateChanged
    {
        /// <summary>
        /// Raised when the value of the current visual state changes.
        /// </summary>
        event EventHandler<VisualStateEventArgs> VisualStateChanged;
    }

    /// <summary>
    /// A behavior that calculates and applies visual states.
    /// </summary>
    public abstract class VisualStateBehavior : Behavior<Control>, INotifyVisualStateChanged
    {
        #region Static Version
        #region Dependency Property Definitions
        /// <summary>
        /// Identifies the <see cref="StateNamePrefix"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty StateNamePrefixProperty = DependencyProperty.Register("StateNamePrefix", typeof(string), typeof(OrientationStateBehavior), new PropertyMetadata("", (d, e) => { ((LayoutStateBehavior)d).ApplyState(false); }));

        /// <summary>
        /// Identifies the <see cref="UseTransitions"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty UseTransitionsProperty = DependencyProperty.Register("UseTransitions", typeof(bool), typeof(PlatformOrientationStateBehavior), new PropertyMetadata(false));
        #endregion // Dependency Property Definitions
        #endregion // Static Version

        #region Instance Version
        #region Member Variables
        private string lastStateName;
        #endregion // Member Variables

        /// <summary>
        /// Applies a visual state.
        /// </summary>
        /// <param name="stateName">
        /// The name of the state to apply.
        /// </param>
        /// <param name="useTransitions">
        /// <c>true</c> to use transitions; otherwise <c>false</c>.
        /// </param>
        /// <param name="forceUpdate">
        /// <c>true</c> if an updated should be forced; otherwise <c>false</c>. If false 
        /// VisualStateManager.GoToState will not be called if the calculated state name is 
        /// unchanged from the last time it was calculated.
        /// </param>
        protected void ApplyState(string stateName, bool useTransitions, bool forceUpdate)
        {
            // If it has changed (or we're forcing the update) apply
            if (lastStateName != stateName)
            {
                // Calculate the state name with prefix
                var preStateName = StateNamePrefix + stateName;

                // Change it
                VisualStateManager.GoToState(AssociatedObject, preStateName, useTransitions);

                // Notify
                if (VisualStateChanged != null)
                {
                    VisualStateChanged(this, new VisualStateEventArgs(stateName, useTransitions, forceUpdate));
                }
            }

            // Update last state name
            lastStateName = stateName;
        }

        #region Overrides / Event Handlers
        /// <summary>
        /// Occurs when the behavior is attached.
        /// </summary>
        protected override void OnAttached()
        {
            // Pass to base first
            base.OnAttached();

            // Attempt to apply now
            ApplyState(true);

            // Handle loaded in case the control isn't yet loaded
            AssociatedObject.Loaded += OnAttachedLoaded;
        }

        protected virtual void OnAttachedLoaded(object sender, RoutedEventArgs e)
        {
            // Make sure applied
            ApplyState(true);
        }

        protected override void OnDetaching()
        {
            // Pass to base first
            base.OnDetaching();

            // Unsubscribe so we can release
            AssociatedObject.Loaded -= OnAttachedLoaded;
        }
        #endregion // Overrides / Event Handlers

        #region Overridables / Event Triggers
        /// <summary>
        /// Calculates the name of the current state for the associated object.
        /// </summary>
        /// <returns>
        /// The name of the current state for the associated object.
        /// </returns>
        protected abstract string CalculateStateName();
        #endregion // Overridables / Event Triggers

        #region Public Methods
        /// <summary>
        /// Calculates the new visual state name and applies it to the associated object.
        /// </summary>
        /// <param name="forceUpdate">
        /// <c>true</c> if an updated should be forced; otherwise <c>false</c>. If false 
        /// VisualStateManager.GoToState will not be called if the calculated state name is 
        /// unchanged from the last time it was calculated.
        /// </param>
        public void ApplyState(bool forceUpdate)
        {
            // Placeholder
            string stateName = null;

            // In case there is a problem caclulating the name
            try
            {
                var view = ApplicationView.GetForCurrentView();
                var bounds = (Window.Current != null ? Window.Current.Bounds : Rect.Empty);

                // Calculate the state name
                // TODO: Pass in bounds, etc.
                stateName = CalculateStateName();

                // Apply
                ApplyState(stateName, UseTransitions, forceUpdate);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to transition to state '{0}': {1}", stateName, ex.Message);
            }
        }
        #endregion // Public Methods

        #region Public Properties
        /// <summary>
        /// Gets or sets a prefix to be added to state names before they are applied. This is a dependency property.
        /// </summary>
        /// <value>
        /// A prefix to be added to state names before they are applied. The default is empty string.
        /// </value>
        public string StateNamePrefix
        {
            get
            {
                return (string)GetValue(StateNamePrefixProperty);
            }
            set
            {
                SetValue(StateNamePrefixProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates if animations should be used during state changes. This is a dependency property.
        /// </summary>
        /// <value>
        /// <c>true</c> if animations should be used during state changes; otherwise <c>false</c>.
        /// </value>
        public bool UseTransitions
        {
            get
            {
                return (bool)GetValue(UseTransitionsProperty);
            }
            set
            {
                SetValue(UseTransitionsProperty, value);
            }
        }
        #endregion // Public Properties

        #region Public Events
        /// <summary>
        /// Raised when the value of the current visual state changes.
        /// </summary>
        public event EventHandler<VisualStateEventArgs> VisualStateChanged;
        #endregion // Public Events

        #endregion // Instance Version
    }
}
