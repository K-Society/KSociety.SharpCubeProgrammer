// Copyright (c) K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer
{
    using System;
    using Microsoft.Extensions.Logging;

    internal static class LoggerHostLoggerExtensions
    {

        private static readonly Func<EventId> GetApplicationTraceEvent = () => new EventId(LogEvents.ApplicationTrace, nameof(LogEvents.ApplicationTrace));
        private static readonly Func<EventId> GetApplicationDebugEvent = () => new EventId(LogEvents.ApplicationDebug, nameof(LogEvents.ApplicationDebug));
        private static readonly Func<EventId> GetApplicationInformationEvent = () => new EventId(LogEvents.ApplicationInformation, nameof(LogEvents.ApplicationInformation));
        private static readonly Func<EventId> GetApplicationWarningEvent = () => new EventId(LogEvents.ApplicationWarning, nameof(LogEvents.ApplicationWarning));
        private static readonly Func<EventId> GetApplicationErrorEvent = () => new EventId(LogEvents.ApplicationError, nameof(LogEvents.ApplicationError));
        private static readonly Func<EventId> GetApplicationCriticalEvent = () => new EventId(LogEvents.ApplicationCritical, nameof(LogEvents.ApplicationCritical));

        private readonly static Func<ILogger, string, IDisposable> AppendMethod = LoggerMessage.DefineScope<string>("{Method}");
        private readonly static Func<ILogger, int, IDisposable> AppendNumber = LoggerMessage.DefineScope<int>("{Number}");
        private readonly static Func<ILogger, string, IDisposable> AppendState = LoggerMessage.DefineScope<string>("{State}");

        private static readonly Action<ILogger, string, Exception> Trace =
            LoggerMessage.Define<string>(
                LogLevel.Trace,
                GetApplicationTraceEvent(),
                "{Message}");

        private static readonly Action<ILogger, string, Exception> Debug =
            LoggerMessage.Define<string>(
                LogLevel.Debug,
                GetApplicationDebugEvent(),
                "{Message}");

        private static readonly Action<ILogger, string, Exception> Information =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                GetApplicationInformationEvent(),
                "{Message}");

        private static readonly Action<ILogger, string, Exception> Warning =
            LoggerMessage.Define<string>(
                LogLevel.Warning,
                GetApplicationWarningEvent(),
                "{Message}");

        private static readonly Action<ILogger, string, Exception> Error =
            LoggerMessage.Define<string>(
                LogLevel.Error,
                GetApplicationErrorEvent(),
                "{Message}");

        private static readonly Action<ILogger, string, Exception> Critical =
            LoggerMessage.Define<string>(
                LogLevel.Critical,
                GetApplicationCriticalEvent(),
                "{Message}");

        public static IDisposable AppendMethodScope(this ILogger logger, string method)
        {
            return AppendMethod(logger, method);
        }

        public static IDisposable AppendNumberScope(this ILogger logger, int number)
        {
            return AppendNumber(logger, number);
        }

        public static IDisposable AppendStateScope(this ILogger logger, string state)
        {
            return AppendState(logger, state);
        }

        public static void TraceMessage(this ILogger logger, string message)
        {
            Trace(logger, message, default);
        }

        public static void DebugMessage(this ILogger logger, string message)
        {
            Debug(logger, message, default);
        }

        public static void InformationMessage(this ILogger logger, string message)
        {
            Information(logger, message, default);
        }

        public static void WarningMessage(this ILogger logger, string message)
        {
            Warning(logger, message, default);
        }

        public static void ErrorMessage(this ILogger logger, string message, Exception ex)
        {
            Error(logger, message, ex);
        }

        public static void CriticalMessage(this ILogger logger, string message, Exception ex)
        {
            Critical(logger, message, ex);
        }
    }
}
