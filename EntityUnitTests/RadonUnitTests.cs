using System;
using System.Linq;
using Core.Application;
using Core.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RA.MongoDB;
using RA.RadonRepository;

namespace RadonUnitTests
{

    /// <summary>
    /// Tests the creation of the CMS entities
    /// </summary>
    [TestClass]
    public class RadonTests : Initializer
    {
        //public static RadonRepo _mainRepo;
     

        public RadonTests()
        {
            ExceptionHandler exceptionHandler = new ExceptionHandler();

            try
            {
                AppInitialize();
          
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
        }

        private TestContext testContextInstance;

        [TestMethod]
        public void DetermineIfRadonCollectionExists()
        {
            new NotImplementedException();

            //RadonRepository repo = new RadonRepository(coreIdentity);

            //database.GetCollection("blah").Exists();
        }
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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        
    }
}
