using System;
using System.Runtime.CompilerServices;

namespace Core.Infrastructures.Logger
{
    public interface ILoggerService
    {
        void Log(string msg, [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0);

        void LogWarning(string msg, [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0);

        void LogError(string msg, [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0);

        void LogException(Exception exception);
    }
}