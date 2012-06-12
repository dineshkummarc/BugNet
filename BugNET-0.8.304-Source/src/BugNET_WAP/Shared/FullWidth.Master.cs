using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace BugNET
{
    public partial class FullWidth : System.Web.UI.MasterPage
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlGenericControl Include = new HtmlGenericControl("script");
            Include.Attributes.Add("type", "text/javascript");
            Include.Attributes.Add("src", Page.ResolveUrl("~/js/jquery-1.4.2.min.js"));
            Page.Header.Controls.Add(Include);

            Include = new HtmlGenericControl("script");
            Include.Attributes.Add("type", "text/javascript");
            Include.Attributes.Add("src", Page.ResolveUrl("~/js/jquery.datepick.min.js"));
            Page.Header.Controls.Add(Include);
        }
    }
}