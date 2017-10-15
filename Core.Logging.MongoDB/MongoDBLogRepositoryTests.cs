using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Core.Application;
using Core.Logging;
using Core.Logging.MongoDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Logging.MongoDB
{
    [TestClass]
    public class MongoDBLogRepositoryTests
    {
        private static readonly string _pathToDbDirectory =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            string[] testFiles = Directory.GetFiles(_pathToDbDirectory, "*.log").ToArray();

            Array.ForEach(testFiles, File.Delete);
        }

        [TestMethod]
        public void Initialize_adds_log_table_to_db()
        {
            //ICoreIdentity coreIdentity = new CoreIdentity
            //{
            //    ID = "coreId" + DateTime.UtcNow.Ticks,
            //    PathToCoreDataDirectory = _pathToDbDirectory
            //};



            //MongoDBLogRepositoryDecorator logRepository = new MongoDBLogRepositoryDecorator(
            //    coreIdentity,
            //    new InMemoryLogWriter(100));

            //logRepository.Initialize();

            //logRepository.Add(new LogEntry(MessageLevel.Detail, "unit testing", "testing", DateTime.Now));

            //logRepository.

            //    bool logTableExists;

            //    string sql = "SELECT count(name) FROM sqlite_master WHERE name = 'log'";


            //        logTableExists = (long)testCommand.ExecuteScalar() > 0;
                

            //    Assert.IsTrue(logTableExists, "Log table not created.");

            //    sql = "select * from log limit 1";


            //            string[] columnNames =
            //                Enumerable.Range(0, dataReader.FieldCount).Select(dataReader.GetName).ToArray();

            //            Assert.IsTrue(columnNames.Contains("ID"), "ID column missing from Log table.");
            //            Assert.IsTrue(columnNames.Contains("Time"), "Time column missing from Log table.");
            //            Assert.IsTrue(columnNames.Contains("Level"), "Level column missing from Log table.");
            //            Assert.IsTrue(columnNames.Contains("Source"), "Source column missing from Log table.");
            //            Assert.IsTrue(columnNames.Contains("Message"), "Message column missing from Log table.");

            //}
        }

        [TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        public void Add_throws_exception_if_called_before_Initialize()
        {
            //ICoreIdentity coreIdentity = new CoreIdentity
            //{
            //    ID = "coreId" + DateTime.UtcNow.Ticks,
            //    PathToCoreDataDirectory = _pathToDbDirectory
            //};

            //MongoDBLogRepository logRepository = new MongoDBLogRepository(coreIdentity, new InMemoryLogWriter(100));

            //logRepository.Add(new LogEntry(MessageLevel.Verbose, "add test", "should throw exception"));
        }

        [TestMethod]
        public void Add_inserts_new_record_in_log_table()
        {
            //ICoreIdentity coreIdentity = new CoreIdentity
            //{
            //    ID = "coreId" + DateTime.UtcNow.Ticks,
            //    PathToCoreDataDirectory = _pathToDbDirectory
            //};

            //MongoDBLogRepositoryDecorator logRepository = new MongoDBLogRepositoryDecorator(
            //    coreIdentity,
            //    new InMemoryLogWriter(100));

            //logRepository.Initialize();

            //logRepository.Add(new LogEntry(MessageLevel.Verbose, "add test", "should add this row to table", DateTime.Now));

            //using (SQLiteConnection dbConnection = logRepository.GetNewDbConnection())
            //{
            //    dbConnection.Open();

            //    string sql = "SELECT * FROM Log";


            //            Assert.IsTrue(dataReader.HasRows, "Row not added to Log table.");

            //            dataReader.Read();

            //            object[] columnValues =
            //                Enumerable.Range(0, dataReader.FieldCount).Select(dataReader.GetValue).ToArray();

            //            Assert.IsTrue(columnValues[1] is DateTime, "'Time' column value incorrect.");
            //            Assert.AreEqual(
            //                (int)MessageLevel.Verbose,
            //                Convert.ToInt32(columnValues[2]),
            //                "'Level' column value incorrect.");
            //            Assert.AreEqual("add test", columnValues[3], "'Source' column value incorrect.");
            //            Assert.AreEqual(
            //                "should add this row to table",
            //                columnValues[4],
            //                "'Message' column value incorrect.");

            //}
        }

        [TestMethod]
        public void Add_enforces_max_table_size()
        {
            //int maxEntryCount = 100;

            //ICoreIdentity coreIdentity = new CoreIdentity
            //{
            //    ID = "coreId" + DateTime.UtcNow.Ticks,
            //    PathToCoreDataDirectory = _pathToDbDirectory
            //};

            //MongoDBLogRepository logRepository = new MongoDBLogRepositoryDecorator(
            //    coreIdentity,
            //    new InMemoryLogWriter(100),
            //    maxEntryCount);

            //logRepository.Initialize();

            ////default Add operations per trim execution is 100
            //for (int entryNumber = 0; entryNumber < maxEntryCount + 1; entryNumber++)
            //{
            //    logRepository.Add(new LogEntry(MessageLevel.Verbose, "add test", entryNumber.ToString(), DateTime.Now));

            //    Thread.Sleep(1);
            //}

            //(MongoDBLogRepositoryDecorator)logRepository).GetNewDbConnection()
            
            

            //   new SQLiteCommand("SELECT count(*) FROM Log", dbConnection))
               
            //        long rowCount = (long)dbCommand.ExecuteScalar();

            //        Assert.AreEqual(maxEntryCount, rowCount, "Incorrect number of rows trimmed from Log table.");
               
            //    new SQLiteCommand("SELECT Message FROM Log ORDER BY Time LIMIT 1"

            //        string oldestMessage = (string)dbCommand.ExecuteScalar();

            //        Assert.AreEqual("1", oldestMessage, "Incorrect rows trimmed from table.");
                

            //    using (
            //        SQLiteCommand dbCommand = new SQLiteCommand(
            //            "SELECT Message FROM Log ORDER BY Time DESC LIMIT 1",
            //            dbConnection))
            //    {
            //        string newestMessage = (string)dbCommand.ExecuteScalar();

            //        Assert.AreEqual("100", newestMessage, "Incorrect rows trimmed from table.");
            //    }
            //}
        }

        [TestMethod]
        public void All_locks_on_db_file_release_after_being_accessed()
        {
            //ICoreIdentity coreIdentity = new CoreIdentity
            //{
            //    ID = DateTime.UtcNow.Ticks.ToString(),
            //    PathToCoreDataDirectory = _pathToDbDirectory
            //};

            //MongoDBLogRepository logRepository = new MongoDBLogRepository(
            //    coreIdentity,
            //    new InMemoryLogWriter(100));

            //logRepository.Initialize();

            //string[] testFiles = Directory.GetFiles(_pathToDbDirectory, "*.log").ToArray();

            //Array.ForEach(testFiles, File.Delete);
        }
    }
}
