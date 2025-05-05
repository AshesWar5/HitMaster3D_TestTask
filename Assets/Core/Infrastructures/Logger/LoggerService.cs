using System;
using System.IO;
using UnityEngine;

namespace Core.Infrastructures.Logger
{
    public sealed class LoggerService : ILoggerService
    {
        public void Log(string msg, string memberName = "",
            string fileName = "", int lineNumber = 0)
        {
            Debug.Log($"{Path.GetFileNameWithoutExtension(fileName)}" +
                      $"({lineNumber}):{memberName}:{msg}");
        }
        
        public void LogWarning(string msg, string memberName = "",
            string fileName = "", int lineNumber = 0)
        {
            Debug.LogWarning($"{Path.GetFileNameWithoutExtension(fileName)}" +
                             $"({lineNumber}):{memberName}:{msg}");
        }

        public void LogError(string msg, string memberName = "",
            string fileName = "", int lineNumber = 0)
        {
            Debug.LogError($"{Path.GetFileNameWithoutExtension(fileName)}" +
                           $"({lineNumber}):{memberName}:{msg}");
        }

        public void LogException(Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}