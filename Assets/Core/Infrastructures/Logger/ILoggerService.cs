using System;

namespace Core.Infrastructures.Logger
{
    public interface ILoggerService
    {
        void Log(string msg, string memberName = "",
            string fileName = "", int lineNumber = 0);

        void LogWarning(string msg, string memberName = "",
            string fileName = "", int lineNumber = 0);

        void LogError(string msg, string memberName = "",
            string fileName = "", int lineNumber = 0);

        void LogException(Exception exception);
    }
}