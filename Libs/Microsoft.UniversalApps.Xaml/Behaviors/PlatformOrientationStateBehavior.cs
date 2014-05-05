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
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.UniversalApps.Behaviors
{
    /// <summary>
    /// A behavior that switches the visual state based on the current platform and orientation.
    /// </summary>
    public class PlatformOrientationStateBehavior : Behavior<Control>
    {
        #region Dependency Property Definitions
        /// <summary>
        /// Identifies the <see cref="WindowsLandscapeStateName"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty WindowsLandscapeStateNameProperty = DependencyProperty.Register("WindowsLandscapeStateName", typeof(string), typeof(PlatformOrientationStateBehavior), new PropertyMetadata("WindowsLandscape", (d, e) => { ((PlatformOrientationStateBehavior)d).ApplyState(); }));

        /// <summary>
        /// Identifies the <see cref="WindowsPortraitStateName"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty WindowsPortraitStateNameProperty = DependencyProperty.Register("WindowsPortraitStateName", typeof(string), typeof(PlatformOrientationStateBehavior), new PropertyMetadata("WindowsPortrait", (d, e) => { ((PlatformOrientationStateBehavior)d).ApplyState(); }));

        /// <summary>
        /// Identifies the <see cref="WindowsPhoneLandscapeStateName"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty WindowsPhoneLandscapeStateNameProperty = DependencyProperty.Register("WindowsPhoneLandscapeStateName", typeof(string), typeof(PlatformOrientationStateBehavior), new PropertyMetadata("WindowsPhoneLandscape", (d, e) => { ((PlatformOrientationStateBehavior)d).ApplyState(); }));

        /// <summary>
        /// Identifies the <see cref="WindowsPhonePortraitStateName"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty WindowsPhonePortraitStateNameProperty = DependencyProperty.Register("WindowsPhonePortraitStateName", typeof(string), typeof(PlatformOrientationStateBehavior), new PropertyMetadata("WindowsPhonePortrait", (d, e) => { ((PlatformOrientationStateBehavior)d).ApplyState(); }));

        /// <summary>
        /// Identifies the <see cref="UseAnimations"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty UseAnimationsProperty = DependencyProperty.Register("UseAnimations", typeof(bool), typeof(PlatformOrientationStateBehavior), new PropertyMetadata(false));
        #endregion // Dependency Property Definitions

        #region Internal Methods
        /// <summary>
        /// Attempts to apply the correct state.
        /// </summary>
        private void ApplyState()
        {
            // In case actual width or height can't be accessed
            try
            {
                // If width and height are the same, we'll use landscape
                if (AssociatedObject.ActualWidth >= AssociatedObject.ActualHeight)
                {
                    if (IsWindowsPhone)
                    {
                        VisualStateManager.GoToState((Control)AssociatedObject, WindowsPhoneLandscapeStateName, UseAnimations);
                    }
                    else
                    {
                        VisualStateManager.GoToState((Control)AssociatedObject, WindowsLandscapeStateName, UseAnimations);
                    }
                }
                else
                {
                    if (IsWindowsPhone)
                    {
                        VisualStateManager.GoToState((Control)AssociatedObject, WindowsPhonePortraitStateName, UseAnimations);
                    }
                    else
                    {
                        VisualStateManager.GoToState((Control)AssociatedObject, WindowsPortraitStateName, UseAnimations);
                    }
                }

            }
            catch (Exception) { }
        }
        #endregion // Internal Methods

        #region Overrides / Event Handlers
        /// <summary>
        /// Occurs when the behavior is attached.
        /// </summary>
        protected override void OnAttached()
        {
            // Pass to base first
            base.OnAttached();

            // Attempt to apply now
            ApplyState();

            // Handle loaded in case the control isn't yet loaded
            AssociatedObject.Loaded += OnAttachedLoaded;

            // Handle size changed
            AssociatedObject.SizeChanged += OnAttachedSizeChanged;
        }

        protected virtual void OnAttachedLoaded(object sender, RoutedEventArgs e)
        {
            // Make sure applied
            ApplyState();
        }

        protected virtual void OnAttachedSizeChanged(object sender, RoutedEventArgs e)
        {
            // Make sure applied
            ApplyState();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            // Get as control
            var control = (Control)AssociatedObject;
            
            // Unsubscribe so we can release
            AssociatedObject.Loaded -= OnAttachedLoaded;
            AssociatedObject.SizeChanged -= OnAttachedSizeChanged;
        }
        #endregion // Overrides / Event Handlers

        #region Public Properties
        /// <summary>
        /// Gets a value that indicates if the current platform is Windows WindowsPhone.
        /// </summary>
        public bool IsWindowsPhone
        {
            get
            {
                #if WINDOWS_PHONE_APP
                    return true;
                #else
                    return false;
                #endif
            }
        }

        /// <summary>
        /// Gets or sets the name of the visual state that should be used on phones. This is a dependency property.
        /// </summary>
        /// <value>
        /// The name of the visual state that should be used on phones.
        /// </value>
        public string WindowsLandscapeStateName
        {
            get
            {
                return (string)GetValue(WindowsLandscapeStateNameProperty);
            }
            set
            {
                SetValue(WindowsLandscapeStateNameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the visual state that should be used on tablets or desktops. This is a dependency property.
        /// </summary>
        /// <value>
        /// The name of the visual state that should be used on tablets or desktops.
        /// </value>
        public string WindowsPortraitStateName
        {
            get
            {
                return (string)GetValue(WindowsPortraitStateNameProperty);
            }
            set
            {
                SetValue(WindowsPortraitStateNameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the visual state that should be used on phones. This is a dependency property.
        /// </summary>
        /// <value>
        /// The name of the visual state that should be used on phones.
        /// </value>
        public string WindowsPhoneLandscapeStateName
        {
            get
            {
                return (string)GetValue(WindowsPhoneLandscapeStateNameProperty);
            }
            set
            {
                SetValue(WindowsPhoneLandscapeStateNameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the visual state that should be used on tablets or desktops. This is a dependency property.
        /// </summary>
        /// <value>
        /// The name of the visual state that should be used on tablets or desktops.
        /// </value>
        public string WindowsPhonePortraitStateName
        {
            get
            {
                return (string)GetValue(WindowsPhonePortraitStateNameProperty);
            }
            set
            {
                SetValue(WindowsPhonePortraitStateNameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates if animations should be used during state changes. This is a dependency property.
        /// </summary>
        /// <value>
        /// <c>true</c> if animations should be used during state changes; otherwise <c>false</c>.
        /// </value>
        public bool UseAnimations
        {
            get
            {
                return (bool)GetValue(UseAnimationsProperty);
            }
            set
            {
                SetValue(UseAnimationsProperty, value);
            }
        }
        #endregion // Public Properties
    }
}
