using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BugNET.BusinessLogicLayer;
using BugNET.POP3Reader;

namespace BugNET.UnitTests
{
    [Category("Business Logic Layer")]
    [TestFixture]
    public class MailBoxReaderTest
    {

      

        private int _Id;
        private string _server;
        private string _username;
        private string _password;
        private string _bodyTemplate;
        private string _reportingUserName;
        private bool _inlineAttachedPictures;
        private bool _deleteAllMessages;        
        private Entry _entry;

        /// <summary>
        /// Inits this instance.
        /// </summary>
        [SetUp]
        public void Init()
        {
            _Id = 1;
            _server = "127.0.0.1";
            _username = "";
            _password = "";
            _bodyTemplate = "{0} {1} {2}";
            _inlineAttachedPictures  = false;
            _reportingUserName = "Admin";
            _deleteAllMessages= false;  

            _entry = new Entry();
            _entry.AttachmentFileNames.Clear();
            
            List<Project> p = Project.GetAllProjects();
            List<IssueType> issT = IssueType.GetIssueTypesByProjectId(p[0].Id);

            // _entry.ProjectMailbox = new ProjectMailbox("testmailbox@testserver.com", p[0].Id, "Admin", "", issT[0].Id);
        }


        private MailboxReader CreateMailBoxReader()
        {
            return new MailboxReader(_server,_username, _password, _inlineAttachedPictures
                ,_bodyTemplate , _deleteAllMessages, _reportingUserName);
        }

        /// <summary>
        /// Tests the creation.
        /// </summary>
        [Test]
        public void TestCreation()
        {
            MailboxReader mbr = CreateMailBoxReader();
            Assert.IsNotNull(mbr);
            // not else to really test            
        }

        /// <summary>
        /// Tests if an issue is added.
        /// </summary>
        [Test]
        public void TestSaveEntryNoAttachments()
        {
            MailboxReader mbr = CreateMailBoxReader();
            Assert.IsNotNull(mbr);
            _entry.MailAttachments.Clear();
            _entry.AttachmentFileNames.Clear();

            mbr.SaveEntry(_entry);          
        }


    }
}
