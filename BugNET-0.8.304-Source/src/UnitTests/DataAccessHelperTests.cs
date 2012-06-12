﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using BugNET.DataAccessLayer;
using BugNET.BusinessLogicLayer;

namespace BugNET.UnitTests
{
    /// <summary>
    /// Data Access Helper Tests
    /// </summary>
    [Category("Data Access Layer")]
    [TestFixture]
    public class DataAccessHelperTests
    {
        /// <summary>
        /// Tests the data access layer is single instance.
        /// </summary>
        [Test]
        public void TestDataAccessLayerIsSingleInstance()
        {
            Assert.AreSame(DataProviderManager.Provider, DataProviderManager.Provider);
        }       
    }
}
