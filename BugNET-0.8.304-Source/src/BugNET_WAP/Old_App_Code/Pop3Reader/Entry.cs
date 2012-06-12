using System;
using System.Collections;
using System.Text;
using BugNET.BusinessLogicLayer;

namespace BugNET.POP3Reader
{
	/// <summary>
	/// Summary description for Entry.
	/// </summary>
	public class Entry
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Entry"/> class.
        /// </summary>
		public Entry()
		{}

        /// <summary>
        /// 
        /// </summary>
		public StringBuilder Content = new StringBuilder();
        /// <summary>
        /// 
        /// </summary>
		public DateTime Date;
        /// <summary>
        /// 
        /// </summary>
		public string Title;
        /// <summary>
        /// 
        /// </summary>
		public string From;
        /// <summary>
        /// 
        /// </summary>
		public ProjectMailbox ProjectMailbox;
        /// <summary>
        /// 
        /// </summary>
		public ArrayList MailAttachments = new ArrayList();
		/// <summary>
		/// 
		/// </summary>
        public ArrayList AttachmentFileNames = new ArrayList();
	}
}
