using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Core.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.MongoDB
{
    [TestClass]
    public class MongoDBRepositoryTests
    {
        private readonly string _pathToDbFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                                "\\id-of-core";

        [TestInitialize]
        public void TestInit()
        {
            string[] testFiles = Directory.GetFiles(Path.GetDirectoryName(_pathToDbFile), "*.invalid").ToArray();

            Array.ForEach(testFiles, File.Delete);
        }

        [TestMethod]
        public void Initialize_creates_new_db_file_if_doesnt_exist()
        {
            InMemoryLogWriter logWriter = new InMemoryLogWriter(100);

            string pathToDbFile = _pathToDbFile + DateTime.UtcNow.Ticks + ".log";

            TestMongoDBRepository testRepository = new TestMongoDBRepository(pathToDbFile, logWriter);

            Assert.IsFalse(File.Exists(pathToDbFile), pathToDbFile + " not deleted.");

            testRepository.Initialize();

            Assert.IsTrue(File.Exists(pathToDbFile), pathToDbFile + " not created.");

            Assert.AreEqual(1, logWriter.Entries.Count, "Missing log entry.");
        }

        [TestMethod]
        public void Initialize_replaces_corrupt_file_and_logs_warning()
        {
            string pathToDbFile = _pathToDbFile + DateTime.UtcNow.Ticks + ".log";

            using (StreamWriter fileWriter = new StreamWriter(pathToDbFile))
            {
                fileWriter.WriteLine("this is not a sqlite file");

                fileWriter.Flush();
            }

            InMemoryLogWriter logWriter = new InMemoryLogWriter(100);

            TestMongoDBRepository testRepository = new TestMongoDBRepository(pathToDbFile, logWriter);

            string pathToBackupFile =
                // ReSharper disable once AssignNullToNotNullAttribute
                Directory.GetFiles(Path.GetDirectoryName(pathToDbFile), "*.invalid").FirstOrDefault();

            if (pathToBackupFile != null)
            {
                File.Delete(pathToBackupFile);
            }

            testRepository.Initialize();

            // ReSharper disable once AssignNullToNotNullAttribute
            pathToBackupFile = Directory.GetFiles(Path.GetDirectoryName(pathToDbFile), "*.invalid").FirstOrDefault();

            Assert.IsNotNull(pathToBackupFile, "Backup file not created.");

            Assert.IsTrue(logWriter.Entries[0].Level == MessageLevel.Warning, "Application log entry not created.");

            File.Delete(pathToBackupFile);

            testRepository.Initialize();

            Assert.IsTrue(logWriter.Entries.Count == 1, "No new log entries should exist.");
        }
    }
}
