using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace BugNET.BusinessLogicLayer
{
    /// <summary>
    /// This class manages the security modes for interfacing with 
    /// the business logic.
    /// 
    /// Please Note: Many of the methods are deliberately "one-way"
    /// example you can turn on HighSecurityMode, but not turn it off again.
    /// 
    /// This is by design needs to be careful considered in future modifications.
    /// </summary>
    public class SecurityModes
    {

        /// <summary>
        /// This is a one way method which turns on high security mode.
        /// 
        /// It seems a bit convoluted but its design is a bit more resiliant to
        /// in-memory attacks on a very important setting. Well worth the time.
        /// </summary>
        public static void TurnOnHighSecurityMode()
        {
            WebConfigurationManager.AppSettings["HighSecurityMode"] = "on";
        }


        /// <summary>
        /// This checks to see if web.config has an AppSetting of 
        /// HighSecurityMode=on 
        /// </summary>
        /// <returns>true=HighSecurityMode is enabled</returns>
        public static bool isHighSecurityMode()
        {
            // Stewart Moss
            //
            // Warning: This "HighSecurityMode" detection logic is vunerable to attack via any kind of hack which can
            // edit the web.config file. The HighSecurityMode should instead be "on" by default and
            // only "off"  if explicitly told. This still does not really 
            //
            // Disable unsafe settings in HighSecurityMode
            //
            // Check we are not in HighSecurityMode
            string HighSecurityMode = "on";
            try
            {
                // there is no way to turn HighSecurityMode "on" except with a 
                // valid read from web.config
                //                
                // This can lead to a poisoning attack. HighSecurityMode setting in Web.config 
                // can be made anything
                // 
                HighSecurityMode = WebConfigurationManager.AppSettings["HighSecurityMode"].ToLower();
            }
            catch
            {
                // any exception means it was off.
                // very insecure.
                return false;
            }

            // ONLY NOW IS HighSecurityMode ACTIVE
            if (HighSecurityMode == "on")
            {
                // it is now turned on
                return true;
            }
            return false;
        }
    }
}