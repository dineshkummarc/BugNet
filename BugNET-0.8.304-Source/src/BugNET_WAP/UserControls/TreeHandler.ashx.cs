using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using BugNET.UserInterfaceLayer;
using System.Web.Script.Serialization;
using BugNET.BusinessLogicLayer;

namespace BugNET.UserControls
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class TreeHandler : IHttpHandler
    {

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        { 
            if (context.Request.QueryString["id"] == null || context.Request.QueryString["id"].Length == 0)
                throw new ArgumentNullException("id");

            int ProjectId = int.Parse(context.Request.QueryString["id"]);
            if (context.User.Identity == null || !context.User.Identity.IsAuthenticated || ( !ITUser.HasPermission(context.User.Identity.Name,ProjectId, Globals.Permission.ADMIN_EDIT_PROJECT.ToString()) && !ITUser.IsInRole(context.User.Identity.Name, 0, Globals.SuperUserRole)))
                throw new System.Security.SecurityException("Access Denied");

            context.Response.ContentType = "text/plain";
            List<ExtTreeNode> nodes = new List<ExtTreeNode>();
            List<Category> comps = Category.GetRootCategoriesByProjectId(ProjectId);
            PopulateNodes(comps, nodes);
            JavaScriptSerializer ser = new JavaScriptSerializer();
            context.Response.Write(ser.Serialize(nodes));
        }

        /// <summary>
        /// Populates the nodes.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="nodes">The nodes.</param>
        private void PopulateNodes(List<Category> list, List<ExtTreeNode> nodes)
        {

            foreach (Category c in list)
            {
                ExtTreeNode no = new ExtTreeNode();
                no.draggable = true;
                no.id = c.Id.ToString();
                no.text = c.Name;
                no.leaf = false;
                no.expanded = true;
                no.cls = "category-node";
                nodes.Add(no);

                if (c.ChildCount > 0)
                {
                    PopulateSubLevel(c.Id, no);
                }
            }
        }

        /// <summary>
        /// Populates the sub level.
        /// </summary>
        /// <param name="parentid">The parentid.</param>
        /// <param name="parentNode">The parent node.</param>
        private void PopulateSubLevel(int parentid, ExtTreeNode parentNode)
        {
            PopulateNodes(Category.GetChildCategoriesByCategoryId(parentid), parentNode.children);
        }
        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
