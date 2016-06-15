using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aladdin.HASP;

namespace MotionMetrics.Common
{
    /// <summary>
    ///     HASP software license class
    /// </summary>
    public static class SoftwareLicense
    {
        private static string getVendorCode()
        {


            string vendorCode =
                "keep this code safe";

            return vendorCode;

        }



        /// <summary>
        ///     Uses pre-defined vendor code for validating whether the HASP software license installed on the server is valid or not
        /// </summary>
        /// <remarks>
        ///     The HASP dlls (hasp_net_windows.dll, hasp_windows_x64_102489.dll, haspvlib_102489.dll, and hasp_windows_102489.dll) 
        ///     must be placed in the windows system directory in order for IIS to access those external dlls
        /// </remarks>
        public static bool validateLicense()
        {

            // Sentinel LDK API
            HaspFeature feature = HaspFeature.FromFeature(1001);  // Using feature 1 defined in EMS-Server; Actual product should be using 1001

            Hasp hasp = new Hasp(feature);
            HaspStatus status = hasp.Login(getVendorCode());

            if (HaspStatus.StatusOk != status)
            {
                return false;
            }
            else
                return true;
        }


        /// <summary>
        ///     Uses pre-defined vendor code for validating whether the HASP software license installed on the server is valid or not
        ///     Returns the detailed hasp pass/fail message only
        /// </summary>
        /// <remarks>
        ///     The HASP dlls (hasp_net_windows.dll, hasp_windows_x64_102489.dll, haspvlib_102489.dll, and hasp_windows_102489.dll) 
        ///     must be placed in the windows system directory in order for IIS to access those external dlls
        /// </remarks>
        public static string validateLicenseDetails()
        {

            // Sentinel LDK API
            HaspFeature feature = HaspFeature.FromFeature(1001);  // Using feature 1 defined in EMS-Server; Actual product should be using 1001

            Hasp hasp = new Hasp(feature);
            HaspStatus status = hasp.Login(getVendorCode());

            return status.ToString();

        }



    }
}
