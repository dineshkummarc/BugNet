﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BugNET.UserInterfaceLayer;
using BugNET.BusinessLogicLayer;
using BugNET.UserControls;

namespace BugNET.Administration.Projects.UserControls
{
    public partial class ProjectIssueTypes : System.Web.UI.UserControl, IEditProjectControl
    {
        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Trace.Warn("Initializing");
            this.grdIssueTypes.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DeleteIssueType);
            this.grdIssueTypes.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdIssueTypes_ItemDataBound);

        }
        #endregion


        private int _ProjectId = -1;


        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        /// <value>The project id.</value>
        public int ProjectId
        {
            get { return _ProjectId; }
            set { _ProjectId = value; }
        }


        /// <summary>
        /// Updates this instance.
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            if (Page.IsValid)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets a value indicating whether [show save button].
        /// </summary>
        /// <value><c>true</c> if [show save button]; otherwise, <c>false</c>.</value>
        public bool ShowSaveButton
        {
            get { return false; }
        }


        /// <summary>
        /// Inits this instance.
        /// </summary>
        public void Initialize()
        {
            BindIssueType();
            lstImages.Initialize();
        }


        /// <summary>
        /// Binds the status.
        /// </summary>
        void BindIssueType()
        {
            grdIssueTypes.Columns[1].HeaderText = GetGlobalResourceObject("CommonTerms", "IssueType").ToString();
            grdIssueTypes.Columns[2].HeaderText = GetGlobalResourceObject("CommonTerms", "Image").ToString();
            grdIssueTypes.Columns[3].HeaderText = GetGlobalResourceObject("CommonTerms", "Order").ToString();

            grdIssueTypes.DataSource = IssueType.GetIssueTypesByProjectId(ProjectId);
            grdIssueTypes.DataKeyField = "Id";
            grdIssueTypes.DataBind();

            if (grdIssueTypes.Items.Count == 0)
                grdIssueTypes.Visible = false;
            else
                grdIssueTypes.Visible = true;
        }


        /// <summary>
        /// Handles the ItemCommand event of the grdIssueTypes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
        protected void grdIssueTypes_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            IssueType s;
            int itemIndex = e.Item.ItemIndex;
            switch (e.CommandName)
            {
                case "up":
                    //move row up
                    if (itemIndex == 0)
                        return;
                    s = IssueType.GetIssueTypeById(Convert.ToInt32(grdIssueTypes.DataKeys[e.Item.ItemIndex]));
                    s.SortOrder -= 1;
                    s.Save();
                    break;
                case "down":
                    //move row down
                    if (itemIndex == grdIssueTypes.Items.Count - 1)
                        return;
                    s = IssueType.GetIssueTypeById(Convert.ToInt32(grdIssueTypes.DataKeys[e.Item.ItemIndex]));
                    s.SortOrder += 1;
                    s.Save();
                    break;
            }
            BindIssueType();
        }

        /// <summary>
        /// Adds the status.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void AddIssueType(Object s, EventArgs e)
        {
            string newName = txtName.Text.Trim();

            if (newName == String.Empty)
                return;

            IssueType newIssueType = new IssueType(ProjectId, newName, lstImages.SelectedValue);
            if (newIssueType.Save())
            {
                txtName.Text = "";
                BindIssueType();
                lstImages.SelectedValue = String.Empty;
            }
            else
            {
                lblError.Text = Logging.GetErrorMessageResource("SaveIssueTypeError");
            }
        }


        /// <summary>
        /// Deletes the status.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
        void DeleteIssueType(Object s, DataGridCommandEventArgs e)
        {
            int statusId = (int)grdIssueTypes.DataKeys[e.Item.ItemIndex];

            if (!IssueType.DeleteIssueType(statusId))
                lblError.Text = Logging.GetErrorMessageResource("DeleteIssueTypeError");
            else
                BindIssueType();

        }

        /// <summary>
        /// Handles the Edit event of the grdIssueTypes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
        protected void grdIssueTypes_Edit(object sender, DataGridCommandEventArgs e)
        {
            grdIssueTypes.EditItemIndex = e.Item.ItemIndex;
            grdIssueTypes.DataBind();
        }

        /// <summary>
        /// Handles the Update event of the grdIssueTypes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
        protected void grdIssueTypes_Update(object sender, DataGridCommandEventArgs e)
        {
            
            TextBox txtIssueTypeName = (TextBox)e.Item.FindControl("txtIssueTypeName");
            PickImage pickimg = (PickImage)e.Item.FindControl("lstEditImages");

            if (txtIssueTypeName.Text.Trim() == "")
            {
                throw new ArgumentNullException("Issue Type name empty");
            }

            IssueType s = IssueType.GetIssueTypeById(Convert.ToInt32(grdIssueTypes.DataKeys[e.Item.ItemIndex]));
            s.Name = txtIssueTypeName.Text.Trim();
            s.ImageUrl = pickimg.SelectedValue;
            s.Save();

            grdIssueTypes.EditItemIndex = -1;
            BindIssueType();

        }

        /// <summary>
        /// Handles the Cancel event of the grdIssueTypes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
        protected void grdIssueTypes_Cancel(object sender, DataGridCommandEventArgs e)
        {
            grdIssueTypes.EditItemIndex = -1;
            grdIssueTypes.DataBind();
        }


        /// <summary>
        /// Handles the ItemDataBound event of the grdIssueTypes control.
        /// </summary>
        /// <param name="s">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridItemEventArgs"/> instance containing the event data.</param>
        void grdIssueTypes_ItemDataBound(Object s, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                IssueType currentIssueType = (IssueType)e.Item.DataItem;

                Label lblIssueTypeName = (Label)e.Item.FindControl("lblIssueTypeName");
                lblIssueTypeName.Text = currentIssueType.Name;

                Image imgIssueType = (Image)e.Item.FindControl("imgIssueType");
                if (currentIssueType.ImageUrl == String.Empty)
                {
                    imgIssueType.Visible = false;
                }
                else
                {
                    imgIssueType.ImageUrl = "~/Images/IssueType/" + currentIssueType.ImageUrl;
                    imgIssueType.AlternateText = currentIssueType.Name;
                }

                Button btnDelete = (Button)e.Item.FindControl("btnDelete");
                string message = string.Format(GetLocalResourceObject("ConfirmDelete").ToString(), currentIssueType.Name);
                btnDelete.Attributes.Add("onclick", String.Format("return confirm('{0}');", message));
            }
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                IssueType currentIssueType = (IssueType)e.Item.DataItem;
                TextBox txtIssueTypeName = (TextBox)e.Item.FindControl("txtIssueTypeName");
                PickImage pickimg = (PickImage)e.Item.FindControl("lstEditImages");

                txtIssueTypeName.Text = currentIssueType.Name;
                pickimg.Initialize();
                pickimg.SelectedValue = currentIssueType.ImageUrl;
            }
        }


        /// <summary>
        /// Validates the status.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.</param>
        protected void ValidateIssueType(Object s, ServerValidateEventArgs e)
        {
            if (grdIssueTypes.Items.Count > 0)
                e.IsValid = true;
            else
                e.IsValid = false;
        }
    }
}