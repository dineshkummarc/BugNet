using System;
using System.Web.Compilation;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Collections.Specialized;

namespace BugNET.Providers.ResourceProviders
{
    /// <summary>
    /// Resource provider accessing resources from the database.
    /// This type is thread safe.
    /// </summary>
    public class DBResourceProvider : DisposableBaseType, IResourceProvider
    {
        private string m_classKey;
        private StringResourcesDALC m_dalc;

        // resource cache
        private Dictionary<string, Dictionary<string, string>> m_resourceCache = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// Constructs this instance of the provider 
        /// supplying a resource type for the instance. 
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        public DBResourceProvider(string classKey)
        {
            Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "DBResourceProvider.DBResourceProvider({0}", classKey));

            this.m_classKey = classKey;
            m_dalc = new StringResourcesDALC(classKey);

        }

        #region IResourceProvider Members

        /// <summary>
        /// Retrieves a resource entry based on the specified culture and 
        /// resource key. The resource type is based on this instance of the
        /// DBResourceProvider as passed to the constructor.
        /// To optimize performance, this function caches values in a dictionary
        /// per culture.
        /// </summary>
        /// <param name="resourceKey">The resource key to find.</param>
        /// <param name="culture">The culture to search with.</param>
        /// <returns>If found, the resource string is returned. 
        /// Otherwise an empty string is returned.</returns>
        public object GetObject(string resourceKey, System.Globalization.CultureInfo culture)
        {
            Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "DBResourceProvider.GetObject({0}, {1}) - type:{2}", resourceKey, culture, this.m_classKey));

            if (Disposed)
            {
                throw new ObjectDisposedException("DBResourceProvider object is already disposed.");
            }

            if (string.IsNullOrEmpty(resourceKey))
            {
                throw new ArgumentNullException("resourceKey");
            }
            
            if (culture == null)
            {
                culture = CultureInfo.CurrentUICulture;
            }

            string resourceValue = null;
            Dictionary<string, string> resCacheByCulture = null;
            // check the cache first
            // find the dictionary for this culture
            // check for the inner dictionary entry for this key
            if (m_resourceCache.ContainsKey(culture.Name))
            {
                resCacheByCulture = m_resourceCache[culture.Name];
                if (resCacheByCulture.ContainsKey(resourceKey))
                {
                    resourceValue = resCacheByCulture[resourceKey];
                }
            }

            // if not in the cache, go to the database
            if (resourceValue == null)
            {
                resourceValue = m_dalc.GetResourceByCultureAndKey(culture, resourceKey);

                // add this result to the cache
                // find the dictionary for this culture
                // add this key/value pair to the inner dictionary
                lock (this)
                {
                    if (resCacheByCulture == null)
                    {
                        resCacheByCulture = new Dictionary<string, string>();
                        m_resourceCache.Add(culture.Name, resCacheByCulture);
                    }
                    resCacheByCulture.Add(resourceKey, resourceValue);
                }
            }
            return resourceValue;
        }

        /// <summary>
        /// Returns a resource reader.
        /// </summary>
        public System.Resources.IResourceReader ResourceReader
        {
            get
            {
                Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "DBResourceProvider.get_ResourceReader - type:{0}", this.m_classKey));

                if (Disposed)
                {
                    throw new ObjectDisposedException("DBResourceProvider object is already disposed.");
                }

                // this is required for implicit resources 
                // this is also used for the expression editor sheet 

                ListDictionary resourceDictionary = this.m_dalc.GetResourcesByCulture(CultureInfo.InvariantCulture);

                return new DBResourceReader(resourceDictionary);
            }

        }

        #endregion

        protected override void Cleanup()
        {
            try
            {
                this.m_dalc.Dispose();
                this.m_resourceCache.Clear();
            }
            finally
            {
                base.Cleanup();
            }
        }

    }
}