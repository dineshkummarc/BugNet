using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Threading;
using System.Globalization;
using System.Web.Security;
using BugNET.BusinessLogicLayer;
using log4net;
using log4net.Config;
using BugNET.POP3Reader;
using System.Configuration;
using System.Web.Configuration;
using BugNET;
using BugNET.Install;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Config/Log4Net.config", Watch = true)]

namespace BugNET
{
    /// <summary>
    /// Global Application Class
    /// </summary>
    public class Global : System.Web.HttpApplication
    {        
        private static readonly ILog Log = LogManager.GetLogger(typeof(Global));

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Global"/> class.
        /// </summary>
        public Global()
        {}

      
        /// <summary>
        /// Handles the Start event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void Application_Start(object sender, EventArgs e)
        {
			
        }

        /// <summary>
        /// Handles the End event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void Application_End(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void Application_Error(Object sender, EventArgs e)
        {
            //set user to log4net context, so we can use %X{user} in the appenders
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                MDC.Set("user", HttpContext.Current.User.Identity.Name);

            Log.Error("Application Error", Server.GetLastError());
        }

        /// <summary>
        /// Handles the End event of the Session control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void Session_End(Object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;

            // Attempt to perform first request initialization
            Initialization.Init(context);
        }

        /// <summary>
        /// Updates the cookie.
        /// </summary>
        /// <param name="cookie_name">The cookie_name.</param>
        /// <param name="cookie_value">The cookie_value.</param>
        void UpdateCookie(string cookie_name, string cookie_value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);
            if (cookie == null)
            {
                cookie = new HttpCookie(cookie_name);
                HttpContext.Current.Request.Cookies.Add(cookie);
            }
            cookie.Value = cookie_value;
            HttpContext.Current.Request.Cookies.Set(cookie);
        }
    }

    /// <summary>
    /// Initialization class for IIS7 integrated mode
    /// </summary>
    class Initialization
    {
        private static bool s_InitializedAlready = false;
        private static Object s_lock = new Object();
        private static readonly ILog Log = LogManager.GetLogger(typeof(Initialization));

        /// <summary>
        /// Initializes only on the first request
        /// </summary>
        /// <param name="context">The context.</param>
        public static void Init(HttpContext context)
        {
            if (s_InitializedAlready)
            {
                return;
            }

            lock (s_lock)
            {
                if (s_InitializedAlready)
                {
                    return;
                }

                // First check if we are upgrading/installing
                if (HttpContext.Current.Request.Url.LocalPath.ToLower().EndsWith("install.aspx"))
                    return;

                if (Upgrade.isInstalled())
                {
                    //log4net.Util.LogLog.InternalDebugging = true;
                    //load the host settings into the application cache
                    HostSetting.GetHostSettings();
                    //configure logging
                    Logging.ConfigureLogging();
                    Log.Info("Application Start");
                }
                else
                {
                    // Not installed, we just quit.
                    // We wont be intialized either.
                    return;
                    // context.Response.Redirect("~/Install/Install.aspx");
                }

                // Perform first-request initialization here ...
                // We are now properly Initialized
                s_InitializedAlready = true;

            }

        }

    }
}
