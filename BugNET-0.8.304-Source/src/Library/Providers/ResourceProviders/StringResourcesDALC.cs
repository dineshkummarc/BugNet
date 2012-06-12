using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading;
using System.Data;
using System.Runtime.Remoting.Contexts;
using System.Runtime.CompilerServices;

namespace BugNET.Providers.ResourceProviders
{
    /// <summary>
    /// Data access component for the StringResources table. 
    /// This type is thread safe.
    /// </summary>
    public class StringResourcesDALC: IDisposable
    {
        private string m_defaultResourceCulture = "en";
        private string m_resourceType = "";

        private SqlConnection m_connection;
        private SqlCommand m_cmdGetResourceByCultureAndKey;
        private SqlCommand m_cmdGetResourcesByCulture;

        /// <summary>
        /// Constructs this instance of the data access 
        /// component supplying a resource type for the instance. 
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        public StringResourcesDALC(string resourceType)
        {
            // save the resource type for this instance
            this.m_resourceType = resourceType;

            // grab the connection string
            m_connection = new SqlConnection(ConfigurationManager.ConnectionStrings[0].ConnectionString);

            // command to retrieve the resource the matches 
            // a specific type, culture and key
            m_cmdGetResourceByCultureAndKey = new SqlCommand("SELECT resourceType, cultureCode, resourceKey, resourceValue FROM BugNet_StringResources WHERE (resourceType=@resourceType) AND (cultureCode=@cultureCode) AND (resourceKey=@resourceKey)");
            m_cmdGetResourceByCultureAndKey.Connection = m_connection;
            m_cmdGetResourceByCultureAndKey.Parameters.AddWithValue("resourceType", this.m_resourceType);
            m_cmdGetResourceByCultureAndKey.Parameters.AddWithValue("cultureCode", "");
            m_cmdGetResourceByCultureAndKey.Parameters.AddWithValue("resourceKey", "");

            // command to retrieve all resources for a particular culture
            m_cmdGetResourcesByCulture = new SqlCommand("SELECT resourceType, cultureCode, resourceKey, resourceValue FROM BugNet_StringResources WHERE (resourceType=@resourceType) AND (cultureCode=@cultureCode)");
            m_cmdGetResourcesByCulture.Connection = m_connection;
            m_cmdGetResourcesByCulture.Parameters.AddWithValue("resourceType", this.m_resourceType);
            m_cmdGetResourcesByCulture.Parameters.AddWithValue("cultureCode", "");

        }

        /// <summary>
        /// Uses an open database connection to recurse 
        /// looking for the resource.
        /// Retrieves a resource entry based on the 
        /// specified culture and resource 
        /// key. The resource type is based on this instance of the
        /// StringResourceDALC as passed to the constructor.
        /// Resource fallback follows the same mechanism 
        /// of the .NET 
        /// ResourceManager. Ultimately falling back to the 
        /// default resource
        /// specified in this class.
        /// </summary>
        /// <param name="culture">The culture to search with.</param>
        /// <param name="resourceKey">The resource key to find.</param>
        /// <returns>If found, the resource string is returned. 
        /// Otherwise an empty string is returned.</returns>
        private string GetResourceByCultureAndKeyInternal(CultureInfo culture, string resourceKey)
        {

            // we should only get one back, but just in case, we'll iterate reader results
            StringCollection resources = new StringCollection();
            string resourceValue = null;

            // set up the dynamic query params
            this.m_cmdGetResourceByCultureAndKey.Parameters["cultureCode"].Value = culture.Name;
            this.m_cmdGetResourceByCultureAndKey.Parameters["resourceKey"].Value = resourceKey;

            // get resources from the database
            using (SqlDataReader reader = this.m_cmdGetResourceByCultureAndKey.ExecuteReader())
            {
                while (reader.Read())
                {
                    resources.Add(reader.GetString(reader.GetOrdinal("resourceValue")));
                }
            }

            // we should only get 1 back, this is just to verify the tables aren't incorrect
            if (resources.Count == 0)
            {
                // is this already fallback location?
                if (culture.Name == this.m_defaultResourceCulture)
                {
                    throw new InvalidOperationException(String.Format(Thread.CurrentThread.CurrentUICulture, Properties.Resources.RM_DefaultResourceNotFound, resourceKey));
                }

                // try to get parent culture
                culture = culture.Parent;
                if (culture.Name.Length == 0)
                {
                    // there isn't a parent culture, change to neutral
                    culture = new CultureInfo(this.m_defaultResourceCulture);
                }
                resourceValue = this.GetResourceByCultureAndKeyInternal(culture, resourceKey);
            }
            else if (resources.Count == 1)
            {
                resourceValue = resources[0];
            }
            else
            {
                // if > 1 row returned, log an error, we shouldn't have > 1 value for a resourceKey!
                throw new DataException(String.Format(Thread.CurrentThread.CurrentUICulture, Properties.Resources.RM_DuplicateResourceFound, resourceKey));
            }

            return resourceValue;
        }

        /// <summary>
        /// Returns a dictionary type containing all resources for a 
        /// particular resource type and culture.
        /// The resource type is based on this instance of the
        /// StringResourceDALC as passed to the constructor.
        /// </summary>
        /// <param name="culture">The culture to search for.</param>
        /// <param name="resourceKey">The resource key to 
        /// search for.</param>
        /// <returns>If found, the dictionary contains key/value 
        /// pairs for each 
        /// resource for the specified culture.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public ListDictionary GetResourcesByCulture(CultureInfo culture)
        {
            // make sure we have a default culture at least
            if (culture == null || culture.Name.Length == 0)
            {
                culture = new CultureInfo(this.m_defaultResourceCulture);
            }

            // set up dynamic query string parameters
            this.m_cmdGetResourcesByCulture.Parameters["cultureCode"].Value = culture.Name;

            // create the dictionary
            ListDictionary resourceDictionary = new ListDictionary();

            // open a connection to gather resource and create the dictionary
            try
            {
                m_connection.Open();

                // get resources from the database
                using (SqlDataReader reader = this.m_cmdGetResourcesByCulture.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string k = reader.GetString(reader.GetOrdinal("resourceKey"));
                        string v = reader.GetString(reader.GetOrdinal("resourceValue"));

                        resourceDictionary.Add(k, v);

                    }
                }

            }
            finally
            {
                m_connection.Close();
            }
            return resourceDictionary;
        }

        /// <summary>
        /// Retrieves a resource entry based on the specified culture and 
        /// resource key. The resource type is based on this instance of the
        /// StringResourceDALC as passed to the constructor.
        /// To optimize performance, this function opens the database connection 
        /// before calling the internal recursive function. 
        /// </summary>
        /// <param name="culture">The culture to search with.</param>
        /// <param name="resourceKey">The resource key to find.</param>
        /// <returns>If found, the resource string is returned. Otherwise an empty string is returned.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetResourceByCultureAndKey(CultureInfo culture, string resourceKey)
        {
            string resourceValue = string.Empty;

            try
            {

                // make sure we have a default culture at least
                if (culture == null || culture.Name.Length == 0)
                {
                    culture = new CultureInfo(this.m_defaultResourceCulture);
                }

                // open the connection before we call the recursive reading function
                this.m_connection.Open();

                // recurse to find resource, includes fallback behavior
                resourceValue = this.GetResourceByCultureAndKeyInternal(culture, resourceKey);
            }
            finally
            {
                // cleanup the connection, reader won't do that if it was open prior to calling in, and that's what we wanted
                this.m_connection.Close();
            }
            return resourceValue;
        }

        #region IDisposable Members

            public void  Dispose()
            {
                try
                {
                    this.m_cmdGetResourceByCultureAndKey.Dispose();
                    this.m_cmdGetResourcesByCulture.Dispose();
                    this.m_connection.Dispose();
                }
                catch { }
            }

        #endregion
    }

    public class ResourceRecord
    {
        private string m_resourceType;

        public string ResourceType
        {
            get { return m_resourceType; }
            set { m_resourceType = value; }
        }
        private string m_cultureCode;

        public string CultureCode
        {
            get { return m_cultureCode; }
            set { m_cultureCode = value; }
        }
        private string m_resourceKey;

        public string ResourceKey
        {
            get { return m_resourceKey; }
            set { m_resourceKey = value; }
        }
        private string m_resourceValue;

        public string ResourceValue
        {
            get { return m_resourceValue; }
            set { m_resourceValue = value; }
        }


    }
}
