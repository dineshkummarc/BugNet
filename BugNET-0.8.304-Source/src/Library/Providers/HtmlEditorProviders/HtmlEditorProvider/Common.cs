using System;
using System.Collections.Generic;
using System.Web;
using System.Text;

namespace BugNET.Providers.HtmlEditorProviders
{
    public static class Common
    {
        /// <summary>
        /// Returns a site relative HTTP path from a partial path starting out with a ~.
        /// Same syntax that ASP.Net internally supports but this method can be used
        /// outside of the Page framework.
        /// 
        /// Works like Control.ResolveUrl including support for ~ syntax
        /// but returns an absolute URL.
        /// </summary>
        /// <param name="originalUrl">Any Url including those starting with ~</param>
        /// <returns>relative url</returns>
        public static string ResolveUrl(string originalUrl)
        {
            throw new NotImplementedException("Security flaw");
            // This routine will allow remote file inclusions and all kinds of nasty 
            // stuff. 
            //
            // THERE BE DRAGONS
            //
            // Use System.Web.VirtualPathUtility.ToAbsolute() instead and assume your parameter
            // is reltive

            if (originalUrl == null)
                return null;

            // Handle abosulte path.
            //  Check for <protocol>://server.dom/path/file.ext
            //
            // SUPER MEGA BUG!!! the following will bypass checks and expose the
            // relative path in the application or some other unplanned exception
            // It also allows remote file inclusion!
            //
            // The following incorrect URL return the relative path
            // ~/foo/bar?value=--' DELETE FROM BOB; --://
            // // *** Absolute path - just return
            // if (originalUrl.IndexOf("://") != -1)
            //    return originalUrl;
                        
            // Make it near the start.
            // assume the protocol portion is less than 11 characters from the start
            // Test for 
            //if ((originalUrl.IndexOf("://") <= 10) 
            //    && ((originalUrl.IndexOf("://") != -1)))
            //    return originalUrl;

            

            // *** Fix up image path for ~ root app dir directory
            if (originalUrl.StartsWith("~"))
            {
                string newUrl = "";
                if (HttpContext.Current != null)
                    newUrl = HttpContext.Current.Request.ApplicationPath +
                          originalUrl.Substring(1).Replace("//", "/");
                else
                    // *** Not context: assume current directory is the base directory
                    throw new ArgumentException("Invalid URL: Relative URL not allowed.");

                // *** Just to be sure fix up any double slashes
                return newUrl;
            }

            return originalUrl;
        }
    }
}
