using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Core.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core
{
    [TestClass]
    class CoreIdentityTests
    {
        private static readonly string _pathToDataDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [TestInitialize]
        public void TestInit()
        {
            string[] testFiles = Directory.GetFiles(_pathToDataDirectory, "*.id").ToArray();

            Array.ForEach(testFiles, File.Delete);
        }

        [TestMethod]
        public void Load_created_identity_file_if_none_exists()
        {
            string pathToIdentityFile = Path.Combine(_pathToDataDirectory, "agent.id");

            Assert.IsFalse(File.Exists(pathToIdentityFile), "Previous test file not deleted.");

            ICoreIdentity coreIdentity = CoreIdentity.Load(_pathToDataDirectory);

            Assert.IsTrue(File.Exists(pathToIdentityFile), "New id file not created.");

            Assert.IsNotNull(coreIdentity.ID, "ID property not assigned.");
        }

        [TestMethod]
        public void Load_uses_existing_identity_file_if_created()
        {
            string pathToIdentityFile = Path.Combine(_pathToDataDirectory, "agent.id");

            Assert.IsFalse(File.Exists(pathToIdentityFile), "Previous test file not deleted.");

            ICoreIdentity coreIdentity = CoreIdentity.Load(_pathToDataDirectory);

            Assert.IsTrue(File.Exists(pathToIdentityFile), "New id file not created.");

            string newcoreId = coreIdentity.ID;

            coreIdentity = CoreIdentity.Load(_pathToDataDirectory);

            Assert.AreEqual(newcoreId, coreIdentity.ID, "Pre-existing agent identity not loaded.");
        }
    }
}
