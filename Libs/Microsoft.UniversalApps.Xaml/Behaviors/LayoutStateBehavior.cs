﻿#region License
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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.UniversalApps.Behaviors
{
    /// <summary>
    /// A behavior that calculates and applies visual states when layout changes.
    /// </summary>
    /// <remarks>
    /// The functionality in this and derived behaviors is based loosely on a talk by Peter Torr from the 
    /// //build 2014 conference titled <see cref="http://channel9.msdn.com/Events/Build/2014/3-541">
    /// From 4 to 40 inches: Developing Windows Applications across Multiple Form Factors</see>.
    /// 
    /// Key concepts include:
    /// 
    /// <list type="table">
    /// <listheader>
    /// <term>Term</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>
    /// Effective Resolution
    /// </term>
    /// <description>
    /// A virtual resolution system that takes viewing distance into account. In this system a 4" phone 
    /// held at 1' has the same Effective Resolution as a 40" TV viewed at 10'. This topic is discussed 
    /// at 6:40 in the video.
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// Detail Level
    /// </term>
    /// <description>
    /// A method for determining the amount of information to display based on space available. This topic is discussed at 25:15 in the video. 
    /// </description>
    /// </item>
    /// </list>
    /// 
    /// Visual States proposed by Peter incldued: 
    /// <list type="table">
    /// <listheader>
    /// <term>State</term>
    /// <description>Usage</description>
    /// </listheader>
    /// <item>
    /// <term>Wide</term>
    /// <description>
    /// Traditionally thought of as 'landscape', this state implies that the window is wider than it is tall. 
    /// In the sample project, a minimum width of 1200 Effective Pixels trigered this state.
    /// </description>
    /// </item>
    /// <item>
    /// <term>Square</term>
    /// <description>
    /// This state implies that the window has roughly equal width and height. 
    /// In the sample project, a minimum width of 700 Effective Pixels trigered this state.
    /// </description>
    /// </item>
    /// <item>
    /// <term>Thin</term>
    /// <description>
    /// Traditionally thought of as 'portrait', this state implies that the window is taller than it is wide. 
    /// In the sample project, a minimum width of 400 Effective Pixels trigered this state.
    /// </description>
    /// </item>
    /// <item>
    /// <term>Rail</term>
    /// <description>
    /// Formerly thought of as 'snapped', this state implies that the window is significantly taller than it is wide. 
    /// In the sample project, anything less than 400 Effective Pixels would triger this state.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    public abstract class LayoutStateBehavior : VisualStateBehavior
    {
        // TODO: Get code from http://aka.ms/WpSLLarge

        #region Overrides / Event Handlers
        /// <summary>
        /// Occurs when the behavior is attached.
        /// </summary>
        protected override void OnAttached()
        {
            // Pass to base first
            base.OnAttached();

            // Handle size changed
            AssociatedObject.SizeChanged += OnAttachedSizeChanged;
        }

        protected virtual void OnAttachedSizeChanged(object sender, RoutedEventArgs e)
        {
            // Make sure applied
            ApplyState(false);
        }

        protected override void OnDetaching()
        {
            // Pass to base first
            base.OnDetaching();

            // Unsubscribe so we can release
            AssociatedObject.SizeChanged -= OnAttachedSizeChanged;
        }
        #endregion // Overrides / Event Handlers
    }
}
