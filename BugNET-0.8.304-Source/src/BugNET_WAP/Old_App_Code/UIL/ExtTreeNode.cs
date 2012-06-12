using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace BugNET.UserInterfaceLayer
{
   
    [Serializable()]
    public class ExtTreeNode
    {
       

        private string _text;
        /// <summary>
        /// Text of the node
        /// </summary>
        public string text
        {
            get { return _text; }
            set { _text = value; }
        }

        private string _href;
        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        /// <value>The href.</value>
        private string href
        {
            get { return _href; }
            set { _href = value; }
        }

        private bool _expanded;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ExtTreeNode"/> is expanded.
        /// </summary>
        /// <value><c>true</c> if expanded; otherwise, <c>false</c>.</value>
        public bool expanded
        {
            get { return _expanded; }
            set { _expanded = value; }
        }

        private bool _draggable;
        /// <summary>
        /// If the node is draggabe
        /// </summary>
        public bool draggable
        {
            get { return _draggable; }
            set { _draggable = value; }
        }
        private string _id;
        /// <summary>
        /// Node id
        /// </summary>
        public string id
        {
            get { return _id; }
            set { _id = value; }
        }
        private bool _leaf;
        /// <summary>
        /// If it has children then leaf=false
        /// </summary>
        public bool leaf
        {
            get { return _leaf; }
            set { _leaf = value; }
        }
        private string _cls;
        /// <summary>
        /// Css class to render a different icon to a node
        /// </summary>
        public string cls
        {
            get { return _cls; }
            set { _cls = value; }
        }

        //private string _iconCls;
        ///// <summary>
        ///// Gets or sets the icon CLS.
        ///// </summary>
        ///// <value>The icon CLS.</value>
        //public string iconCls
        //{
        //    get { return _iconCls; }
        //    set { _iconCls = value; }
        //}

        //private bool _isRoot;
        ///// <summary>
        ///// If this is the root node
        ///// </summary>
        //public bool IsRoot
        //{
        //    get { return _isRoot; }
        //    set { _isRoot = value; }
        //}

        public List<ExtTreeNode> children = new List<ExtTreeNode>();
    }
}
