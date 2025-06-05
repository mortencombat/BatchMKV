using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EventLogger
{
    public class WindowsEventLogger : IEventLogger
    {
        private string sanitizeMessage(string message)
        {
            // Clean message to remove %1, %2 to prevent Windows Event Viewer from doing weird string formatting tricks...
            return Regex.Replace(message, "%([0-9]+)", delegate (Match m) {
                return "{" + m.Groups[1].Value + "}";
            });
        }

        private string sourcePrefix;
        public string SourcePrefix
        {
            get { return sourcePrefix; }
            set { sourcePrefix = value; }
        }

        private string logName;
        public string LogName
        {
            get { return logName; }
            set { logName = value; }
        }

        private bool enabled;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public bool LogEntry(string Source, string Message, EventLogEntryType Type, int ID = 0, short Category = 0, bool CreateEventSource = false)
        {
            if (!enabled) return false;

            string src = (sourcePrefix != null ? (sourcePrefix + "\\") : "") + Source;

            if (CreateEventSource)
            {
                if (!this.CreateEventSource(Source))
                    return false;
            }

            try
            {
                EventLog.WriteEntry(src, sanitizeMessage(Message), Type, ID, Category);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CreateEventSource(string Source)
        {
            string src = (sourcePrefix != null ? (sourcePrefix + "\\") : "") + Source;

            try
            {
                if (!EventLog.SourceExists(src))
                    EventLog.CreateEventSource(src, logName);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
