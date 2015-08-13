﻿using NUnit.Framework;
using Google.GData.Client.UnitTests;
using Google.GData.YouTube;

namespace Google.GData.Client.UnitTests.YouTube

{
    
    
    /// <summary>
    ///This is a test class for FriendsEntryTest and is intended
    ///to contain all FriendsEntryTest Unit Tests
    ///</summary>
    [TestFixture][Category("YouTube")]
    public class FriendsEntryTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for UserName
        ///</summary>
        [Test]
        public void UserNameTest()
        {
            FriendsEntry target = new FriendsEntry(); // TODO: Initialize to an appropriate value
            string expected = "secret test string";
            string actual;
            target.UserName = expected;
            actual = target.UserName;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Status
        ///</summary>
        [Test]
        public void StatusTest()
        {
            FriendsEntry target = new FriendsEntry(); // TODO: Initialize to an appropriate value
            string expected = "secret test string";
            string actual;
            target.Status = expected;
            actual = target.Status;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for FriendsEntry Constructor
        ///</summary>
        [Test]
        public void FriendsEntryConstructorTest()
        {
            FriendsEntry target = new FriendsEntry();
            Assert.IsNotNull(target, "object better not be null");
        }
    }
}
