﻿using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace BugNET.Providers.HtmlEditorProviders
{

    public abstract class HtmlEditorProvider : ProviderBase
    {
        public abstract Control HtmlEditor { get; }
        public abstract Unit Width { get; set; }
        public abstract Unit Height { get; set; }
        public abstract string Text { get; set; }
        public abstract string ControlId { get; set; }
        public abstract string ProviderPath { get; }
    }
}
