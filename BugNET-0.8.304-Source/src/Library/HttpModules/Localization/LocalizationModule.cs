﻿using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;
//using BugNET.BusinessLogicLayer;
//using log4net;
//using log4net.Config;
using System.Threading;
using System.Globalization;

namespace BugNET.HttpModules
{
    public class LocalizationModule : IHttpModule
    {
        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public string ModuleName
        {
            get { return "LocalizationModule"; }
        }

        #region IHttpModule Members

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute+=new EventHandler(context_PreRequestHandlerExecute);
        }

        #endregion

        /// <summary>
        /// Handles the PreRequestHandlerExecute event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (HttpContext.Current.User == null || HttpContext.Current.Profile == null 
                || HttpContext.Current.Request.Url.LocalPath.ToLower().EndsWith("install.aspx") || !HttpContext.Current.User.Identity.IsAuthenticated)
                return;
          
         
            if (HttpContext.Current.Profile["PreferredLocale"] != null)
            {
                //retrieve culture
                string culture = HttpContext.Current.Profile["PreferredLocale"].ToString();

                //set culture
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            }
            
        }

    }
}
