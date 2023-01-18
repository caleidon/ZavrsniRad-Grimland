using System.Collections.Generic;
using UnityEngine;

namespace CommandTerminal
{
    public enum TerminalLogType
    {
        Error = LogType.Error,
        Warning = LogType.Warning,
        Message = LogType.Log,
        Input,
        ShellMessage
    }

    public struct LogItem
    {
        public TerminalLogType Type;
        public string Message;
        public string StackTrace;
    }

    public class CommandLog
    {
        private readonly int maxItems;

        public List<LogItem> Logs { get; } = new List<LogItem>();

        public CommandLog(int maxItems)
        {
            this.maxItems = maxItems;
        }

        public void HandleLog(string message, TerminalLogType type)
        {
            HandleLog(message, "", type);
        }

        public void HandleLog(string message, string stackTrace, TerminalLogType type)
        {
            LogItem log = new LogItem
            {
                Message = message,
                StackTrace = stackTrace,
                Type = type
            };

            Logs.Add(log);

            if (Logs.Count > maxItems)
            {
                Logs.RemoveAt(0);
            }
        }

        public void Clear()
        {
            Logs.Clear();
        }
    }
}