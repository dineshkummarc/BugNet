using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BugNET.BusinessLogicLayer;
using BugNET.UserInterfaceLayer;
using System.IO;
using log4net;

namespace BugNET.Issues.UserControls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Attachments : System.Web.UI.UserControl, IIssueTab
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Attachments));
        private int _IssueId = 0;
        private int _ProjectId = 0;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ////if (!Page.IsPostBack) { 
            //    PostBackTrigger trigger = new PostBackTrigger();
            //    trigger.ControlID = "ctl00$Content$ctlIssueTabs$ctlContent$UploadButton";
            //    UpdatePanel panel = this.Parent.FindControl("IssueTabsUpdatePanel") as UpdatePanel;
            //    if(panel != null)
            //        panel.Triggers.Add(trigger);

            ////}

                ScriptManager sman = ScriptManager.GetCurrent(Page);
                sman.RegisterPostBackControl(UploadButton);
        }

        #region IIssueTab Members

        /// <summary>
        /// Gets or sets the bug id.
        /// </summary>
        /// <value>The bug id.</value>
        public int IssueId
        {
            get { return _IssueId; }
            set { _IssueId = value; }
        }

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
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            AttachmentsDataGrid.Columns[0].HeaderText = GetLocalResourceObject("AttachmentsGrid.FileNameHeader.Text").ToString();
            AttachmentsDataGrid.Columns[1].HeaderText = GetLocalResourceObject("AttachmentsGrid.SizeHeader.Text").ToString();
            AttachmentsDataGrid.Columns[2].HeaderText = GetLocalResourceObject("AttachmentsGrid.Description.Text").ToString();

            BindAttachments();

            //check users role permission for adding an attachment
            if (!Page.User.Identity.IsAuthenticated || !ITUser.HasPermission(ProjectId, Globals.Permission.ADD_ATTACHMENT.ToString()))
                pnlAddAttachment.Visible = false;

            if (!Page.User.Identity.IsAuthenticated || !ITUser.HasPermission(ProjectId, Globals.Permission.DELETE_ATTACHMENT.ToString()))
                AttachmentsDataGrid.Columns[5].Visible = false;
        }

        #endregion

        /// <summary>
        /// Binds the attachments.
        /// </summary>
        private void BindAttachments()
        {
            List<IssueAttachment> attachments = IssueAttachment.GetIssueAttachmentsByIssueId(_IssueId);

            if (attachments.Count == 0)
            {
                lblAttachments.Text = GetLocalResourceObject("NoAttachments").ToString();
                lblAttachments.Visible = true;
                AttachmentsDataGrid.Visible = false;
            }
            else
            {
                lblAttachments.Visible = false;
                AttachmentsDataGrid.DataSource = attachments;
                AttachmentsDataGrid.DataBind();
                AttachmentsDataGrid.Visible = true;
            }
        }

        /// <summary>
        /// Uploads the document.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void UploadDocument(object sender, EventArgs e)
        {
            // get the current file
            HttpPostedFile uploadFile = this.AspUploadFile.PostedFile;
            HttpContext context = HttpContext.Current;

            IssueAttachment.UploadFile(IssueId, uploadFile, context, AttachmentDescription.Text.Trim());
            this.BindAttachments();

            // if there was a file uploaded
            //if (uploadFile.ContentLength > 0)
            //{
            //    bool isFileOk = false;
            //    string[] AllowedFileTypes = HostSetting.GetHostSetting("AllowedFileExtensions").Split(new char[';']);
            //    string fileExt = System.IO.Path.GetExtension(uploadFile.FileName);
            //    string uploadedFileName = string.Empty;

            //    uploadedFileName = Path.GetFileName(uploadFile.FileName);

            //    if (AllowedFileTypes.Length > 0 && AllowedFileTypes[0].CompareTo("*.*") == 0)
            //    {
            //        isFileOk = true;
            //    }
            //    else
            //    {

            //        foreach (string fileType in AllowedFileTypes)
            //        {
            //            string newfileType = fileType.Substring(fileType.LastIndexOf("."));
            //            if (newfileType.CompareTo(fileExt) == 0)
            //                isFileOk = true;

            //        }
            //    }

            //    //file type is not valid
            //    if (!isFileOk)
            //    {
            //        if (Log.IsErrorEnabled) Log.Error(string.Format(Logging.GetErrorMessageResource("InvalidFileType"), uploadedFileName));
            //        return;
            //    }

            //    //check for illegal filename characters
            //    if (uploadedFileName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
            //    {
            //        if (Log.IsErrorEnabled) Log.Error(string.Format(Logging.GetErrorMessageResource("InvalidFileName"), uploadedFileName));
            //        return;
            //    }

            //    //if the file is ok save it.
            //    if (isFileOk)
            //    {
            //        // save the file to the upload directory
            //        int projectId = Issue.GetIssueById(IssueId).ProjectId;
            //        Project p = Project.GetProjectById(projectId);

            //        if (p.AllowAttachments)
            //        {
            //            IssueAttachment attachment;

            //            if (p.AttachmentStorageType == IssueAttachmentStorageType.Database)
            //            {
            //                int fileSize = uploadFile.ContentLength;
            //                byte[] fileBytes = new byte[fileSize];
            //                System.IO.Stream myStream = uploadFile.InputStream;
            //                myStream.Read(fileBytes, 0, fileSize);
            //                attachment = new IssueAttachment(
            //                    IssueId, 
            //                    HttpContext.Current.User.Identity.Name,
            //                    uploadedFileName, 
            //                    uploadFile.ContentType, 
            //                    fileBytes, 
            //                    fileSize);
            //                attachment.Save();
            //            }
            //            else
            //            {
            //                string ProjectPath = p.UploadPath;

            //                try
            //                {
            //                    if (ProjectPath.Length == 0)
            //                        throw new ApplicationException(string.Format(Logging.GetErrorMessageResource("UploadPathNotDefined"), p.Name));

            //                    string UploadedFileName = String.Format("{0:0000}_", IssueId) + System.IO.Path.GetFileName(uploadedFileName);
            //                    string UploadedFilePath = context.Server.MapPath("~" + Globals.UploadFolder + ProjectPath) + "\\" + UploadedFileName;
            //                    attachment = new IssueAttachment(IssueId, context.User.Identity.Name, UploadedFileName, uploadFile.ContentType, null, uploadFile.ContentLength);
            //                    if (attachment.Save())
            //                    {
            //                        uploadFile.SaveAs(UploadedFilePath);
            //                    }

            //                }
            //                catch (DirectoryNotFoundException ex)
            //                {
            //                    if (Log.IsErrorEnabled) Log.Error(string.Format(Logging.GetErrorMessageResource("UploadPathNotFound"), ProjectPath), ex);
            //                    throw;
            //                }
            //                catch (Exception ex)
            //                {
            //                    if (Log.IsErrorEnabled) Log.Error(ex.Message, ex);
            //                    throw;
            //                }
            //            }

            //            // rebind to show the new file.
            //            this.BindAttachments();
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Handles the ItemDataBound event of the AttachmentsDataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridItemEventArgs"/> instance containing the event data.</param>
        protected void AttachmentsDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                IssueAttachment currentAttachment = (IssueAttachment)e.Item.DataItem;
                ((HtmlAnchor)e.Item.FindControl("lnkAttachment")).InnerText = currentAttachment.FileName;
                ((HtmlAnchor)e.Item.FindControl("lnkAttachment")).HRef = "DownloadAttachment.axd?id=" + currentAttachment.Id.ToString();
                ImageButton lnkDeleteAttachment = (ImageButton)e.Item.FindControl("lnkDeleteAttachment");
                lnkDeleteAttachment.OnClientClick = string.Format("return confirm('{0}');", GetLocalResourceObject("DeleteAttachment").ToString());
                LinkButton cmdDeleteAttachment = (LinkButton)e.Item.FindControl("cmdDeleteAttachment");
                cmdDeleteAttachment.OnClientClick = string.Format("return confirm('{0}');", GetLocalResourceObject("DeleteAttachment").ToString());

                float size;
                string label;
                if (currentAttachment.Size > 1000)
                {
                    size = currentAttachment.Size / 1000f;
                    label = string.Format("{0} kb", size.ToString("##,##"));
                }
                else
                {
                    size = currentAttachment.Size;
                    label = string.Format("{0} b", size.ToString("##,##"));
                }
                ((Label)e.Item.FindControl("lblSize")).Text = label;
            }
        }

        /// <summary>
        /// Handles the ItemCommand event of the dtgAttachment control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
        protected void AttachmentsDataGrid_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Delete":
                    IssueAttachment.DeleteIssueAttachment(Convert.ToInt32(e.CommandArgument));
                    break;
            }
            BindAttachments();
        }

    }
}