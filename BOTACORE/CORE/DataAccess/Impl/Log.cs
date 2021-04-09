using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

namespace BOTACORE.CORE.Impl
{
    public static class Log
    {
        private static Dictionary<Type, ILog> _loggers = new Dictionary<Type, ILog>();
        private static bool _logInitialized = false;
        private static object _lock = new object();

        public static string SerializeException(Exception e)
        {
            return SerializeException(e, string.Empty);
        }

        private static string SerializeException(Exception e, string exceptionMessage)
        {
            if (e == null) return string.Empty;

            exceptionMessage = string.Format(
                "{0}{1}{2}\n{3}",
                exceptionMessage,
                (exceptionMessage == string.Empty) ? string.Empty : "\n\n",
                e.Message,
                e.StackTrace);

            if (e.InnerException != null)
                exceptionMessage = SerializeException(e.InnerException, exceptionMessage);

            return exceptionMessage;
        }

        private static ILog getLogger(Type source)
        {
            lock (_lock)
            {
                if (_loggers.ContainsKey(source))
                {
                    return _loggers[source];
                }
                else
                {
                    ILog logger = LogManager.GetLogger(source);
                    _loggers.Add(source, logger);
                    return logger;
                }
            }
        }

        /* Log a message object */

        public static void Debug(object source, object message)
        {
            Debug(source.GetType(), message);
        }

        public static void Debug(Type source, object message)
        {
            getLogger(source).Debug(message);
        }

        public static void Info(object source, object message)
        {
            Info(source.GetType(), message);
        }

        public static void Info(Type source, object message)
        {
            getLogger(source).Info(message);
        }

        public static void Warn(object source, object message)
        {
            Warn(source.GetType(), message);
        }

        public static void Warn(Type source, object message)
        {
            getLogger(source).Warn(message);
        }

        public static void Error(object source, object message)
        {
            Error(source.GetType(), message);
        }

        public static void Error(Type source, object message)
        {
            getLogger(source).Error(message);
        }

        public static void Fatal(object source, object message)
        {
            Fatal(source.GetType(), message);
        }

        public static void Fatal(Type source, object message)
        {
            getLogger(source).Fatal(message);
        }

        /* Log a message object and exception */

        public static void Debug(object source, object message, Exception exception)
        {
            Debug(source.GetType(), message, exception);
        }

        public static void Debug(Type source, object message, Exception exception)
        {
            getLogger(source).Debug(message, exception);
        }

        public static void Info(object source, object message, Exception exception)
        {
            Info(source.GetType(), message, exception);
        }

        public static void Info(Type source, object message, Exception exception)
        {
            getLogger(source).Info(message, exception);
        }

        public static void Warn(object source, object message, Exception exception)
        {
            Warn(source.GetType(), message, exception);
        }

        public static void Warn(Type source, object message, Exception exception)
        {
            getLogger(source).Warn(message, exception);
        }

        public static void Error(object source, object message, Exception exception)
        {
            Error(source.GetType(), message, exception);
        }

        public static void Error(Type source, object message, Exception exception)
        {
            getLogger(source).Error(message, exception);
        }

        public static void Fatal(object source, object message, Exception exception)
        {
            Fatal(source.GetType(), message, exception);
        }

        public static void Fatal(Type source, object message, Exception exception)
        {
            getLogger(source).Fatal(message, exception);
        }

        private static void initialize()
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4Net.config");
            if (!File.Exists(logFilePath))
            {
                logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\Log4Net.config");
            }

            XmlConfigurator.ConfigureAndWatch(new FileInfo(logFilePath));
        }

        public static void EnsureInitialized()
        {
            if (!_logInitialized)
            {
                initialize();
                _logInitialized = true;
            }
        }
    }
}