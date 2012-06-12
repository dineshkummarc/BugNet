//====================================================================
// This file is generated as part of Web project conversion.
// The extra class 'Tab' in the code behind file in 'usercontrols\TabMenu.ascx.cs' is moved to this file.
//====================================================================




namespace BugNET.UserInterfaceLayer
 {

	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Collections;

    /// <summary>
    /// Menu Tab Class
    /// </summary>
	public class Tab : HyperLink
	{

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tab"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="url">The URL.</param>
		public Tab(string name, string url) 
		{
			this.Text  = String.Format("<span>{0}</span>",name);
			this.NavigateUrl = url;
		}
		
	}

}