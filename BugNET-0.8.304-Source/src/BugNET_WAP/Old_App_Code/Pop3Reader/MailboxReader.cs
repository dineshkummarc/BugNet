using System;
using System.Configuration;
using System.Web;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using BugNET.BusinessLogicLayer;
//using IssueAttachment = BugNET.BusinessLogicLayer.IssueAttachment;
using Lesnikowski.Client;
using Lesnikowski.Mail;
using MailAttachment = Lesnikowski.Mail.MimeData;
using log4net;
using log4net.Config;

namespace BugNET.POP3Reader
{
    /// <summary>
    /// This is the handler class for the Mail-To-Weblog functionality.
    /// </summary>
    public class MailboxReader
    {
        string Pop3Server;
        string Pop3Username;
        string Pop3Password;
        bool Pop3InlineAttachedPictures;
        bool Pop3DeleteAllMessages;
        string ReportingUserName;
        string BodyTemplate;

        private static readonly ILog Log = LogManager.GetLogger(typeof(MailboxReader));

        /// <summary>
        /// Processes the exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns></returns>
        private ApplicationException ProcessException(Exception ex)
        {
            //set user to log4net context, so we can use %X{user} in the appenders
            if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                MDC.Set("user", System.Web.HttpContext.Current.User.Identity.Name);

            if (Log.IsErrorEnabled)
                Log.Error("Mailbox Reader Error", ex);

            return new ApplicationException(Logging.GetErrorMessageResource("MailboxReaderError"), ex);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MailboxReader"/> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="inlineAttachedPictures">if set to <c>true</c> [inline attached pictures].</param>
        /// <param name="bodyTemplate">The body template.</param>
        /// <param name="deleteAllMessages">if set to <c>true</c> [delete all messages].</param>
        /// <param name="reportingUserName">Name of the reporting user.</param>
        public MailboxReader(string server, string userName, string password,
             bool inlineAttachedPictures, string bodyTemplate,
            bool deleteAllMessages, string reportingUserName)
        {
            Pop3Server = server;
            Pop3Username = userName;
            Pop3Password = password;

            Pop3InlineAttachedPictures = inlineAttachedPictures;
            BodyTemplate = bodyTemplate;
            Pop3DeleteAllMessages = deleteAllMessages;
            ReportingUserName = reportingUserName;
        }

        /// <summary>
        /// Compares two binary buffers up to a certain length.
        /// </summary>
        /// <param name="buf1">First buffer</param>
        /// <param name="buf2">Second buffer</param>
        /// <param name="len">Length</param>
        /// <returns>true or false indicator about the equality of the buffers</returns>
        private bool EqualBuffers(byte[] buf1, byte[] buf2, int len)
        {
            if (buf1.Length >= len && buf2.Length >= len)
            {
                for (int l = 0; l < len; l++)
                {
                    if (buf1[l] != buf2[l])
                        return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Stores an attachment to disk.
        /// </summary>
        /// <param name="attachment"></param>
        /// <param name="binariesPath"></param>
        /// <returns></returns>
        public string StoreAttachment(MailAttachment attachment, string binariesPath)
        {
            bool alreadyUploaded = false;
            string baseFileName = attachment.FileName;
            string targetFileName = Path.Combine(binariesPath, baseFileName);
            int numSuffix = 1;

            // if the target filename already exists, we check whether we already 
            // have that file stored by comparing the first 2048 bytes of the incoming
            // date to the target file (creating a hash would be better, but this is 
            // "good enough" for the time being)
            while (File.Exists(targetFileName))
            {
                byte[] targetBuffer = new byte[Math.Min(2048, attachment.Data.Length)];
                int targetBytesRead;

                using (FileStream targetFile = new FileStream(targetFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    long numBytes = targetFile.Length;
                    if (numBytes == (long)attachment.Data.Length)
                    {
                        targetBytesRead = targetFile.Read(targetBuffer, 0, targetBuffer.Length);
                        if (targetBytesRead == targetBuffer.Length)
                        {
                            if (EqualBuffers(targetBuffer, attachment.Data, targetBuffer.Length))
                            {
                                alreadyUploaded = true;
                            }
                        }
                    }
                }

                // If the file names are equal, but it's not considered the same file,
                // we append an incrementing numeric suffix to the file name and retry.
                if (!alreadyUploaded)
                {
                    string ext = Path.GetExtension(baseFileName);
                    string file = Path.GetFileNameWithoutExtension(baseFileName);
                    string newFileName = file + (numSuffix++).ToString();
                    baseFileName = newFileName + ext;
                    targetFileName = Path.Combine(binariesPath, baseFileName);
                }
                else
                {
                    break;
                }
            }

            // now we've got a unique file name or the file is already stored. If it's
            // not stored, write it to disk.
            if (!alreadyUploaded)
            {
                using (FileStream fileStream = new FileStream(targetFileName, FileMode.Create))
                {
                    fileStream.Write(attachment.Data, 0, attachment.Data.Length);
                    fileStream.Flush();
                }
            }
            return baseFileName;
        }

        /// <summary>
        /// Saves the entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public void SaveEntry(Entry entry)
        {
            try
            {
                string body = string.Format(this.BodyTemplate, entry.Content.ToString().Trim(), entry.From, entry.Date.ToString());
                int curPID = entry.ProjectMailbox.ProjectId;

                Issue MailIssue = Issue.GetDefaultIssueByProjectId(curPID, entry.Title.Trim(), body.Trim(), entry.ProjectMailbox.AssignToName, ReportingUserName);

                if (MailIssue.Save())
                {
                    //If there is an attached file present then add it to the database 
                    //and copy it to the directory specified in the web.config file
                    for (int i = 0; i < entry.AttachmentFileNames.Count; i++)
                    {
                        MailAttachment attMail = entry.MailAttachments[i] as MailAttachment;
                        IssueAttachment att = new IssueAttachment(0,
                            MailIssue.Id,
                            ReportingUserName,
                            ReportingUserName,
                            DateTime.Now,
                            Path.GetFileName(entry.AttachmentFileNames[i].ToString()),
                            attMail.ContentType.ToString(),
                            attMail.Data,
                            attMail.Data.Length, "Attached via email"
                            );
                        att.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// ReadMail reads the pop3 mailbox
        /// </summary>
        public void ReadMail()
        {
            Pop3 pop3 = new Pop3();

            try
            {
                try
                {
                    pop3.User = Pop3Username;
                    pop3.Password = Pop3Password;


                    pop3.Connect(Pop3Server);

                    if (pop3.HasTimeStamp == true)
                        pop3.Login();//APOPLogin();
                    else
                        pop3.Login();

                    pop3.GetAccountStat();

                    int j = 1;

                    bool messageWasProcessed = false;

                    // Read each message to find out the recipient. This is for the project.
                    for (; j <= pop3.MessageCount; j++)
                    {
                        SimpleMailMessage message = SimpleMailMessage.Parse(pop3.GetMessage(j));

                        string messageFrom = "";

                        if (message.From.Count > 0)
                        {
                            MailBox from = message.From[0];
                            messageFrom = from.Address;
                        }

                        // E-Mail addresses look usually like this:
                        // My Name <myname@example.com> or simply
                        // myname@example.com. This block handles 
                        // both variants.

                        // Need to check the TO, CC and BCC message fields for the bugnet email address
                        List<MailBox[]> recipients = new List<MailBox[]>();
                        recipients.Add((MailBox[])message.To.ToArray());
                        recipients.Add((MailBox[])message.Cc.ToArray());
                        recipients.Add((MailBox[])message.Bcc.ToArray());

                        foreach (MailBox[] toAr in recipients)
                        {
                            foreach (MailBox to in toAr)
                            {
                                string mailbox = to.Address;

                                ProjectMailbox pmbox = ProjectMailbox.GetProjectByMailbox(mailbox);

                                // Only if the mailbox appears in project's mailbox aliases
                                // we accept the message
                                if (pmbox != null)
                                {
                                    string binariesBaseUri;
                                    string binariesPath;

                                    string uploadPath = Project.GetProjectById(pmbox.ProjectId).UploadPath.TrimEnd('/');

                                    //string appPath = HostSetting.GetHostSetting("DefaultUrl");
                                    string appPath = HttpContext.Current.Request.PhysicalApplicationPath;
                                    if (!System.IO.Directory.Exists(appPath))
                                        throw new Exception("Upload path does not exist.");

                                    if (uploadPath.StartsWith("~"))
                                    {
                                        binariesPath = uploadPath.Replace("~", appPath.TrimEnd('\\'));

                                        if (!System.IO.Directory.Exists(appPath))
                                            throw new Exception("Upload path does not exist.");

                                        binariesBaseUri = uploadPath.Replace("~\\", Globals.DefaultUrl).Replace("\\", "/");
                                    }
                                    else
                                    {
                                        binariesPath = string.Concat(appPath, "Uploads\\");

                                        if (!System.IO.Directory.Exists(appPath))
                                            throw new Exception("Upload path does not exist.");

                                        binariesBaseUri = string.Concat(Globals.DefaultUrl, "Uploads/");
                                    }

                                    Entry entry = new Entry();

                                    entry.Title = message.Subject.Trim();
                                    entry.From = messageFrom;
                                    entry.ProjectMailbox = pmbox;

                                    entry.Date = message.Date;

                                    // plain text?
                                    if (message.HtmlDataString.Length == 0)
                                    {
                                        entry.Content.Append(message.TextDataString);
                                    }
                                    else
                                    {
                                        // Strip the <body> out of the message (using code from below)
                                        Regex bodyExtractor = new Regex("<body.*?>(?<content>.*)</body>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                                        Match match = bodyExtractor.Match(message.HtmlDataString);
                                        if (match != null && match.Success && match.Groups["content"] != null)
                                        {
                                            entry.Content.Append(match.Groups["content"].Value);
                                        }
                                        else
                                        {
                                            entry.Content.Append(message.HtmlDataString);
                                        }
                                    }

                                    Hashtable embeddedFiles = new Hashtable();
                                    ArrayList attachedFiles = new ArrayList();

                                    foreach (MailAttachment attachment in message.Attachments)
                                    {
                                        if (attachment.Data != null && attachment.FileName != null && attachment.FileName.Length > 0)
                                        {
                                            string fn = StoreAttachment(attachment, binariesPath);

                                            entry.MailAttachments.Add(attachment);
                                            entry.AttachmentFileNames.Add(fn);

                                            attachedFiles.Add(fn);
                                        }
                                    }

                                    // TODO Should use XSLT templates BGN-1591

                                    if (entry.MailAttachments.Count > 0)
                                    {
                                        entry.Content.Append("<p>");

                                        for (int i = 0; i < entry.MailAttachments.Count; i++)
                                        {
                                            MailAttachment attachment = (MailAttachment)entry.MailAttachments[i];

                                            string absoluteUri = string.Concat(binariesBaseUri, entry.AttachmentFileNames[i]);
                                            if (Pop3InlineAttachedPictures && attachment.ContentType.MimeType == MimeType.Image)
                                            {
                                                entry.Content.Append(String.Format("<br><img src=\"{0}\" />", absoluteUri));
                                            }
                                            else
                                            {
                                                entry.Content.Append(String.Format("<br><a href=\"{0}\" />{1}</a>", absoluteUri, attachment.FileName));
                                            }
                                        }

                                        entry.Content.Append("</p>");
                                    }

                                    messageWasProcessed = true;

                                    SaveEntry(entry);
                                }
                                else
                                {
                                    if (Log.IsWarnEnabled)
                                        Log.WarnFormat("Project Mailbox Not Found: {0}", message.To.ToString());
                                }
                            }
                        }
                        // luke@jurasource.co.uk (01-MAR-04)
                        if (Pop3DeleteAllMessages || messageWasProcessed)
                            pop3.DeleteMessage(j);
                    }
                }
                catch (Exception ex)
                {
                    throw ProcessException(ex);
                }
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
            finally
            {
                try
                {
                    pop3.Close();
                }
                catch
                {
                }
            }
        }
    }
}
