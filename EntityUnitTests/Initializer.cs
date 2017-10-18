using System;
using System.Collections.Generic;
using System.IO;
using Core.Application;
using Core.Logging;
using Core.Exceptions;
using Core.Logging.MongoDB;
using RA.RadonRepository;

namespace RadonUnitTests
{
    public abstract class Initializer
    {
        public ILogWriter LogWriter { get; private set; }
        public static FileStream ostrm;
        public static StreamWriter writer;
        public static TextWriter oldOut = Console.Out;
        public static ICoreIdentity _coreIdentity;
        public static ILogWriter _logWriter;

        public static void AppInitialize()
        {
            // This will get called on startup
            ExceptionHandler exceptionHandler = new ExceptionHandler();

            try
            {
                //TODO: turn off when done
                EnableDebugLogging();

                InitCore(exceptionHandler);
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
        }

        private static ICoreIdentity InitcoreIdentity(ILogWriter applicationLog)
        {
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "HWWD",
                "HappyWorksWebDesign");

            return _coreIdentity = CoreIdentity.Load(appDataPath);
        }


        public static void InitCore(IExceptionHandler exceptionHandler)
        {
            EnableDebugLogging();

            ApplicationLog applicationLog = ApplicationLog.GetDefault(exceptionHandler);

            ExceptionHandler.DefaultStrategies.Strategies.Add(new LoggingStrategy(applicationLog.AddEntry));

            CoreApplication.Init(
                applicationLog,
                //log => new NullWatchdog(),
                log => InitExceptionHandler(exceptionHandler),
                InitcoreIdentity,
                //InitSettingsProvider,
                InitApplicationLog,
                //LoadServiceBus,
                OnAppShutdown);
        }

        private static IExceptionHandler InitExceptionHandler(IExceptionHandler exceptionHandler)
        {
            ExceptionHandler.HandleUnhandledAppDomainExceptions(new NullStrategy());

            //          LoggingStrategy.LogEntryModifiers.Add(new ExceptionAsWarningLogModifier<EndpointNotFoundException>());

            IList<IExceptionStrategy> defaultStrategies = ExceptionHandler.DefaultStrategies.Strategies;

            defaultStrategies.Insert(0, new UnwrapTargetInvocationExceptionsStrategy());

            //#if !DEBUG
            GeneralExceptionFloodStrategy generalFloodStrategy =
                new GeneralExceptionFloodStrategy(TimeSpan.FromMinutes(30), TimeSpan.FromHours(1));

            //         generalFloodStrategy.FloodWatchers.Add(typeof (EndpointNotFoundException),
            //                            new ExceptionFloodWatcher(TimeSpan.FromHours(1), TimeSpan.FromHours(1)));

            defaultStrategies.Insert(0, generalFloodStrategy);
            //#endif

            return exceptionHandler;
        }

        //private static ISettingsProvider InitSettingsProvider(ILogWriter logWriter, ICoreIdentity coreIdentity)
        //{
        //    SqliteSettingsProvider sqliteSettingsProvider = new SqliteSettingsProvider(coreIdentity, logWriter);

        //    sqliteSettingsProvider.Initialize();

        //    return sqliteSettingsProvider;
        //}

        private static void InitApplicationLog(
            //ISettingsProvider settingsProvider,
            IApplicationLog applicationLog,
            ICoreIdentity coreIdentity)
        {
            string appDataFolder = coreIdentity.PathToCoreDataDirectory;

            Directory.CreateDirectory(appDataFolder);

            applicationLog.LogWriters.Add(_logWriter = new TextFileLogWriter(Path.Combine(appDataFolder, "coreLog.txt")));

            MongoDBLogRepository logRepository = new MongoDBLogRepository(coreIdentity, applicationLog);
            logRepository.Initialize();


            ApplicationLog.InitDefault(applicationLog);
        }

        private static void OnAppShutdown()
        {
            if (writer != null)
            {
                writer.Close();
            }

            if (ostrm != null)
            {
                ostrm.Close();

                Console.SetOut(oldOut);
            }
        }

        private static void EnableDebugLogging()
        {
            try
            {

                ostrm = new FileStream(@"C:\ProgramData\HWWD\HappyWorksWebDesign\Redirect.txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);

            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open Redirect.txt for writing");
                Console.WriteLine(e.Message);
                return;
            }

            if (Directory.Exists(@"C:\ProgramData\HWWD\HappyWorksWebDesign\"))
            {
                Console.SetOut(writer);
            }
        }

        //private static IApplicationServiceBus LoadServiceBus(
        //    IExceptionHandler exceptionHandler,
        //    ILogWriter logWriter,
        //    ISettingsProvider settingsProvider,
        //    ICoreIdentity coreIdentity,
        //    IApplicationWatchdog watchdog)
        //{
        //    UnityContainer unityDiContainer = new UnityContainer();

        //    IApplicationServiceBus serviceBus =
        //        new ApplicationServiceBus(
        //            () => LoadApplicationServices(
        //                exceptionHandler,
        //                logWriter,
        //                settingsProvider,
        //                watchdog,
        //                coreIdentity,
        //                unityDiContainer),
        //            ExceptionHandler.DefaultStrategies,
        //            logWriter);
        
        //    unityDiContainer.RegisterInstance(serviceBus);

        //    return serviceBus;
        //}
        

        //private static IList<IApplicationService> LoadApplicationServices(
        //    IExceptionHandler exceptionHandler,
        //    ILogWriter logWriter,
        //    ISettingsProvider settingsProvider,
        //    IApplicationWatchdog watchdog,
        //    ICoreIdentity coreIdentity,
        //    UnityContainer unityDiContainer)
        //{

        //   http://stackoverflow.com/questions/39086690/unity-container-resolving-with-unitydependencyresolver

        //    IApplicationService agentDependencyResolver = new UnityDependencyResolver(unityDiContainer);

        //    List<IApplicationService> services = new List<IApplicationService>();

        //    services.Add(agentDependencyResolver);

        //    https://msdn.microsoft.com/en-us/library/ff648211.aspx

        //    unityDiContainer.RegisterInstance(settingsProvider);

        //    SqliteSensorHostRepository sensorHostRepository = new SqliteSensorHostRepository(coreIdentity, logWriter);
        //    sensorHostRepository.Initialize();
        //    unityDiContainer.RegisterInstance<ISensorHostInfoRepository>(sensorHostRepository);

        

        //    return services;
        //}
    }
}
