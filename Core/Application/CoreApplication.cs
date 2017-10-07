using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Application;
using Core.Exceptions;
using Core.Logging;
using Core.Services;
using Core.Settings;

namespace Core.Application
{
    
    public static class CoreApplication
    {
        private const string ThisClassName = "Core.Application.CoreApplication";

        //        private const string Shutdownkey = "Core Last Clean Shutdown";
        private const string RestartApplicationNowSettingName = "RestartApplicationNow";

        //private static IApplicationServiceBus _serviceBus;
        private static IExceptionHandler _exceptionHandler;
        //private static ISettingsProvider _settingsProvider;
        private static Action _onShutDown;
        private static IApplicationLog _applicationLog;
        private static ICoreIdentity _coreIdentity;


        public static bool RestartRequested { get; private set; }

        public static void ShutDown()
        {

            if (_onShutDown != null) _onShutDown();
            //            WriteShutdownTimeToRegistry(DateTime.Now.ToString());
            if (_applicationLog != null)
            {
                string coreId = _coreIdentity == null ? "(unknown)" : _coreIdentity.ID;

                _applicationLog.AddEntry(
                    new LogEntry(
                        MessageLevel.AppLifecycle,
                        ThisClassName,
                        string.Format("Core {0} shutting down.", coreId),
                        DateTime.Now));
            }

            //StopAllServices();
        }

        //private static void StopAllServices()
        //{
        //    try
        //    {
        //        if (_serviceBus != null) _serviceBus.Dispose();
        //    }
        //    catch (Exception e)
        //    {
        //        if (_exceptionHandler != null) _exceptionHandler.HandleException(e);
        //    }
        //}

        //private static bool RestartApplicationNow
        //{
        //    get
        //    {
        //        string boolean = _settingsProvider.GetSetting(
        //            ThisClassName,
        //            RestartApplicationNowSettingName,
        //            false.ToString());

        //        return Convert.ToBoolean(boolean);
        //    }
        //    set { _settingsProvider.UpsertSetting(ThisClassName, RestartApplicationNowSettingName, value.ToString()); }
        //}

        public static void Init(
            IApplicationLog applicationLog,
            //Func<ILogWriter, IApplicationWatchdog> initWatchdog,
            Func<ILogWriter, IExceptionHandler> initExceptionHandler,
            Func<ILogWriter, ICoreIdentity> initCoreIdentity,
            //Func<ILogWriter, ICoreIdentity, ISettingsProvider> initSettingsProvider,
            Action<IApplicationLog, ICoreIdentity> initApplicationLog,
            //Func
            //    <IExceptionHandler, ILogWriter, ISettingsProvider, ICoreIdentity, IApplicationWatchdog,
            //        IApplicationServiceBus> loadServiceBus,
            Action onShutDown)
        {
            if (applicationLog == null) throw new ArgumentNullException("applicationLog");
            //if (initWatchdog == null) throw new ArgumentNullException("initWatchdog");
            if (initApplicationLog == null) throw new ArgumentNullException("initApplicationLog");
            if (initExceptionHandler == null) throw new ArgumentNullException("initExceptionHandler");
            //if (initSettingsProvider == null) throw new ArgumentNullException("initSettingsProvider");
            if (initCoreIdentity == null) throw new ArgumentNullException("initCoreIdentity");
            if (onShutDown == null) throw new ArgumentNullException("onShutDown");
            //if (loadServiceBus == null) throw new ArgumentNullException("loadServiceBus");

            _onShutDown = onShutDown;

            //IApplicationWatchdog watchdog = initWatchdog(applicationLog);

            //watchdog.BeatDog();

            _exceptionHandler = initExceptionHandler(applicationLog);

            _coreIdentity = initCoreIdentity(applicationLog);

            initApplicationLog(applicationLog, _coreIdentity);

            _applicationLog = applicationLog;

            applicationLog.AddEntry(
                new LogEntry(
                    MessageLevel.Detail,
                    ThisClassName,
                    string.Format("Core {0} started.", _coreIdentity.ID),
                    DateTime.Now));

            //_settingsProvider = initSettingsProvider(applicationLog, _coreIdentity);

            //_serviceBus = loadServiceBus(_exceptionHandler, applicationLog, _settingsProvider, _coreIdentity, watchdog);

            //_settingsProvider.SettingsChanged += SettingsProvider_SettingsChanged;
        }

        //private static void SettingsProvider_SettingsChanged()
        //{
        //    if (RestartApplicationNow)
        //    {
        //        RestartApplicationNow = false;

        //        ShutDown();

        //        RestartRequested = true;
        //    }
        //}
    }
}
