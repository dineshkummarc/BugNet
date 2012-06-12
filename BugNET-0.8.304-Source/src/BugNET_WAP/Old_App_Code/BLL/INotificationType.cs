using System;
using System.Collections.Generic;
using System.Text;

namespace BugNET.BusinessLogicLayer
{
    public interface INotificationType
    {
        string Name{get;}
        bool Enabled{get;}
        void SendNotification(INotificationContext context);
    }
}
