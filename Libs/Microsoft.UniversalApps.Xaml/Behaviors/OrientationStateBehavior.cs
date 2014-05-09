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
    /// A behavior that switches visual state based on the orientation of the layout.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    /// <term>State Name</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>
    /// Landscape
    /// </term>
    /// <description>
    /// The layout is wider than it is tall.
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// Portrait
    /// </term>
    /// <description>
    /// The layout is taller than it is wide.
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// Square
    /// </term>
    /// <description>
    /// The layout is as wide as it is tall (or is within the SquareThreshold).
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    public class OrientationStateBehavior : LayoutStateBehavior
    {
        #region Constants
        private const string LandscapeStateName = "Landscape";
        private const string PortraitStateName = "Portrait";
        private const string SquareStateName = "Square";
        #endregion // Constants

        #region Dependency Property Definitions
        /// <summary>
        /// Identifies the <see cref="SquareThreshold"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty SquareThresholdProperty = DependencyProperty.Register("SquareThreshold", typeof(double), typeof(OrientationStateBehavior), new PropertyMetadata(10d));
        #endregion // Dependency Property Definitions

        #region Overrides / Event Handlers
        protected override bool TryCalculateStateName(LayoutState layout, out string stateName)
        {
            // Figure out difference between width and height
            var diff = Math.Abs(layout.ActualWidth - layout.ActualHeight);

            // If it's within the square threshold, call it square
            if (diff <= SquareThreshold)
            {
                stateName = SquareStateName;
            }
            // Wider than tall?
            else if (layout.ActualWidth > layout.ActualHeight)
            {
                stateName = LandscapeStateName;
            }
            else
            {
                stateName = PortraitStateName;
            }

            // Success
            return true;
        }
        #endregion // Overrides / Event Handlers

        #region Public Properties
        /// <summary>
        /// Gets or sets the amount that width can vary from height and still be considered square. This is a dependency property.
        /// </summary>
        /// <value>
        /// The amount that width can vary from height and still be considered square. The default is 10.
        /// </value>
        public double SquareThreshold
        {
            get
            {
                return (double)GetValue(SquareThresholdProperty);
            }
            set
            {
                SetValue(SquareThresholdProperty, value);
            }
        }
        #endregion // Public Properties
    }
}
