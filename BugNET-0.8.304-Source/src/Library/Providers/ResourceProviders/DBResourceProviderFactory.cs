using System;
using System.Web.Compilation;
using System.Web;
using System.Diagnostics;
using System.Globalization;

namespace BugNET.Providers.ResourceProviders
{

    public class DBResourceProviderFactory : ResourceProviderFactory
    {

        public override IResourceProvider CreateGlobalResourceProvider(string classKey)
        {
            Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "DBResourceProviderFactory.CreateGlobalResourceProvider({0})", classKey));
            return new DBResourceProvider(classKey); 
        }

        public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
        {
            Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "DBResourceProviderFactory.CreateLocalResourceProvider({0}", virtualPath));

            // we should always get a path from the runtime
            string classKey = virtualPath;
            if (!string.IsNullOrEmpty(virtualPath))
            {
                classKey = VirtualPathUtility.ToAppRelative(virtualPath).Substring(2);
            }

            return new DBResourceProvider(classKey);
        }
    }
}