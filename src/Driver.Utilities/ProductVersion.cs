// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Reflection;
using System.Diagnostics;
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// /********************************************************
// *                                                       *
// *   Copyright (C) Microsoft. All rights reserved.       *
// *                                                       *
// ********************************************************/

namespace Microsoft.CodeAnalysis.Driver
{
    /// <summary>
    /// Provides access to a tool's "display version", used in many places in the UI of the SDL
    /// tools.
    /// </summary>
    public static class ProductVersion
    {
        /// <summary>
        /// Gets or sets the tool's display version.
        /// </summary>
        public static string DisplayVersion
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(s_displayVersion))
                {
                    return s_displayVersion;
                }

                Assembly entry = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
                BrandedVersionAttribute brandedVersion = entry.GetCustomAttribute<BrandedVersionAttribute>();
                if (brandedVersion == null)
                {
                    Version version = entry.GetName().Version;
                    return String.Format(CultureInfo.InvariantCulture, "for SDL {0}.{1}", version.Major, version.Minor);
                }
                else
                {
                    return brandedVersion.BrandedVersion;
                }
            }

            set
            {
                s_displayVersion = value;
            }
        }

        /// <summary>
        /// Gets or sets the tool's display version qualifier (Beta / RC / etc.).
        /// Uses the product version string.
        /// Build numbers ending in 999 are beta (999, 1999, 2999, etc.)
        /// Build numbers in even thousands are RC (1000, 2000, 3000, etc)
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        private static string GetDisplayVersionQualifier()
        {
            int buildNumber = 0;
            const int BuildNumberElement = 2;

            //If we already have a version qualifier, just return that.
            if (!String.IsNullOrWhiteSpace(s_displayVersionQualifier))
            {
                return s_displayVersionQualifier;
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            string productVersion = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            if (String.IsNullOrWhiteSpace(productVersion))
            {
                s_displayVersionQualifier = String.Empty;
                return s_displayVersionQualifier;
            }
            string[] versionComponents = productVersion.Split('.');
            if (versionComponents.Length < 4)
            {
                s_displayVersionQualifier = String.Empty;
                return s_displayVersionQualifier;
            }

            bool result = int.TryParse(versionComponents[BuildNumberElement], out buildNumber);
            if (!result)
            {
                s_displayVersionQualifier = String.Empty;
                return s_displayVersionQualifier;
            }

            //CBT always sets desktop builds to 0
            if (buildNumber == 0)
            {
                return Resources.VersionQualifier_Desktop;
            }
            //numbers ending in 999 are beta (999, 1999, 2999, etc.)
            else if (buildNumber % 1000 != 0)
            {
                s_displayVersionQualifier = Resources.VersionQualifier_Beta;
            }
            //thousands are Release (1000, 2000, 3000, etc) we shouldn't show
            //anything per the spec
            else
            {
                s_displayVersionQualifier = String.Empty;
            }

            return s_displayVersionQualifier;
        }

        public static string CompanyName { get; set; }

        /// <summary>Constructs the full display version for use in banners and title bars e.g. "Microsoft
        /// BinScope for SDL 7.0 (BETA)".</summary>
        /// <param name="productName">Name of the product for which the display version shall be generated.</param>
        /// <returns>The qualified display version.</returns>
        public static string GetQualifiedDisplayVersion(string productName)
        {
            return CompanyName + " " + GetQualifiedCompanylessDisplayVersion(productName);
        }

        /// <summary>Constructs the full display version for use in banners and title bars without
        /// "Microsoft", e.g. "BinScope for SDL 7.0 (BETA)".</summary>
        /// <param name="productName">Name of the product for which the display version shall be generated.</param>
        /// <returns>The qualified display version.</returns>
        public static string GetQualifiedCompanylessDisplayVersion(string productName)
        {
            string productQualifier = GetDisplayVersionQualifier();
            string productQualifierDisplay = (!String.IsNullOrWhiteSpace(productQualifier)) ? " (" + productQualifier + ")" : String.Empty;

            return productName + " " + DisplayVersion + productQualifierDisplay;
        }

        private static string s_displayVersion;
        private static string s_displayVersionQualifier;
    }
}
