using System;
using System.Collections.Generic;
using System.Text;

namespace BugNET.BusinessLogicLayer
{
    /// <summary>
    /// 
    /// </summary>
    public interface INotificationContext
    {
        /// <summary>
        /// Gets or sets the body text.
        /// </summary>
        /// <value>The body text.</value>
        string BodyText { get; }

        /// <summary>
        /// Gets or sets the send to address.
        /// </summary>
        /// <value>The send to address.</value>
        string Username { get;  }

        /// <summary>
        /// Gets or sets the users display name.
        /// </summary>
        /// <value>The user display name.</value>
        string UserDisplayName { get;  }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        string Subject { get;  }

        /// <summary>
        /// Gets o
        /// </summary>
        EmailFormatTypes EmailFormatType { get;  }
    }
}
