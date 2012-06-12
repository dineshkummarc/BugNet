using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Threading;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;

namespace BugNET.Providers.ResourceProviders
{

   /// <summary>
   /// Implementation of IResourceReader required to retrieve a dictionary
   /// of resource values for implicit localization. 
   /// </summary>
   public class DBResourceReader : DisposableBaseType, IResourceReader, IEnumerable<KeyValuePair<string, object>> 
   {
       private ListDictionary m_resourceDictionary;

       /// <summary>
       /// Initializes a new instance of the <see cref="DBResourceReader"/> class.
       /// </summary>
       /// <param name="resourceDictionary">The resource dictionary.</param>
       public DBResourceReader(ListDictionary resourceDictionary)
       {
           Debug.WriteLine("DBResourceReader()");

           this.m_resourceDictionary = resourceDictionary;
       }

       /// <summary>
       /// Cleanups this instance.
       /// </summary>
       protected override void Cleanup()
       {
           try
           {
               this.m_resourceDictionary = null;
           }
           finally
           {
               base.Cleanup();
           }
       }

       #region IResourceReader Members

       /// <summary>
       /// Closes the resource reader after releasing any resources associated with it.
       /// </summary>
       public void Close()
       {
           this.Dispose();
       }

       /// <summary>
       /// Returns an <see cref="T:System.Collections.IDictionaryEnumerator"/> of the resources for this reader.
       /// </summary>
       /// <returns>
       /// A dictionary enumerator for the resources for this reader.
       /// </returns>
       public IDictionaryEnumerator GetEnumerator()
       {
           Debug.WriteLine("DBResourceReader.GetEnumerator()");
           
           // NOTE: this is the only enumerator called by the runtime for 
           // implicit expressions

           if (Disposed)
           {
               throw new ObjectDisposedException("DBResourceReader object is already disposed.");
           }

           return this.m_resourceDictionary.GetEnumerator();
       }

       #endregion

       #region IEnumerable Members

       /// <summary>
       /// Returns an enumerator that iterates through a collection.
       /// </summary>
       /// <returns>
       /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
       /// </returns>
       IEnumerator IEnumerable.GetEnumerator()
       {
           if (Disposed)
           {
               throw new ObjectDisposedException("DBResourceReader object is already disposed.");
           }

           return this.m_resourceDictionary.GetEnumerator();
       }

       #endregion

       #region IEnumerable<KeyValuePair<string,object>> Members

       /// <summary>
       /// Returns an enumerator that iterates through the collection.
       /// </summary>
       /// <returns>
       /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
       /// </returns>
       IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
       {
           if (Disposed)
           {
               throw new ObjectDisposedException("DBResourceReader object is already disposed.");
           }

           return this.m_resourceDictionary.GetEnumerator() as IEnumerator<KeyValuePair<string, object>>;
       }

       #endregion
   }

}