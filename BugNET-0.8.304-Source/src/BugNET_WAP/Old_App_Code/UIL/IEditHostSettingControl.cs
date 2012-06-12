using System;
using System.Collections.Generic;
using System.Text;

namespace BugNET.UserInterfaceLayer
{
    public interface IEditHostSettingControl
    {
        /// <summary>
        /// Updates this instance.
        /// </summary>
        /// <returns></returns>
       void Update();

        /// <summary>
        /// Inits this instance.
        /// </summary>
        void Initialize();
    }
}
