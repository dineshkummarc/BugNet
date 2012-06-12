// According to http://msdn2.microsoft.com/en-us/library/system.web.httppostedfile.aspx
// "Files are uploaded in MIME multipart/form-data format. 
// By default, all requests, including form fields and uploaded files, 
// larger than 256 KB are buffered to disk, rather than held in server memory."
// So we can use an HttpHandler to handle uploaded files and not have to worry
// about the server recycling the request do to low memory. 
// don't forget to increase the MaxRequestLength in the web.config.
// If you server is still giving errors, then something else is wrong.
// I've uploaded a 1.3 gig file without any problems. One thing to note, 
// when the SaveAs function is called, it takes time for the server to 
// save the file. The larger the file, the longer it takes.
// So if a progress bar is used in the upload, it may read 100%, but the upload won't
// be complete until the file is saved.  So it may look like it is stalled, but it
// is not.

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using BugNET.BusinessLogicLayer;
using log4net;

namespace BugNET.UserInterfaceLayer
{
    /// <summary>
    /// Upload handler for uploading files.
    /// </summary>
    public class DownloadAttachment : IHttpHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DownloadAttachment));

        /// <summary>
        /// Initializes a new instance of the <see cref="Upload"/> class.
        /// </summary>
        public DownloadAttachment()
        {
        }

        #region IHttpHandler Members

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["mode"] == "project")
            {
                int projectId = Int32.Parse(context.Request.QueryString["id"]);
                ProjectImage ProjectImage = Project.GetProjectImage(projectId);

                if (ProjectImage != null)
                {
                    // Write out the attachment
                    context.Server.ScriptTimeout = 600;
                    context.Response.Buffer = true;
                    context.Response.Clear();
                    context.Response.ContentType = "application/octet-stream";
                    //context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + attachment.FileName + "\";");
                    context.Response.AddHeader("Content-Length", ProjectImage.ImageFileLength.ToString());
                    context.Response.BinaryWrite(ProjectImage.ImageContent);
                }
                else
                {
                    context.Response.WriteFile("~/Images/project.png");
                }

            }
            else
            {
                // Get the attachment
                int attachmentId = Int32.Parse(context.Request.QueryString["id"]);
                IssueAttachment attachment = IssueAttachment.GetIssueAttachmentById(attachmentId);

                if (attachment.Attachment != null)
                {
                    // Write out the attachment
                    context.Server.ScriptTimeout = 600;
                    context.Response.Buffer = true;
                    context.Response.Clear();
                    if (attachment.ContentType.ToLower().StartsWith("image/"))
                    {
                        context.Response.ContentType = attachment.ContentType;
                        context.Response.AddHeader("Content-Disposition", "inline; filename=\"" + attachment.FileName + "\";");
                    }
                    else
                    {
                        context.Response.ContentType = "application/octet-stream";
                        context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + attachment.FileName + "\";");
                    }
                    context.Response.AddHeader("Content-Length", attachment.Attachment.Length.ToString());
                    context.Response.BinaryWrite(attachment.Attachment);
                }
                else
                {

                    Project p = Project.GetProjectById(Issue.GetIssueById(attachment.IssueId).ProjectId);

                    string FileName = attachment.FileName;
                    string ProjectPath = p.UploadPath;
                    string Path;

                    //append a trailing slash if it doesn't exist
                    if (!ProjectPath.EndsWith("\\"))
                        ProjectPath = String.Concat(ProjectPath, "\\");

                    Path = String.Concat("~", Globals.UploadFolder, ProjectPath, FileName);

                    if (System.IO.File.Exists(context.Server.MapPath(Path)))
                    {
                        context.Response.Clear();
                        context.Response.ContentType = attachment.ContentType;
                        if (attachment.ContentType.ToLower().StartsWith("image/")) context.Response.AddHeader("Content-Disposition", "inline; filename=\"" + attachment.FileName + "\";");
                        else context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + attachment.FileName + "\";");
                        context.Response.WriteFile(Path);
                    }
                    else
                    {
                        context.Response.Write("<h1>Attachment Not Found.</h1>  It may have been deleted from the server.");
                    }
                }
            }

            // End the response
            context.Response.End();
        }

        #endregion
    }

}