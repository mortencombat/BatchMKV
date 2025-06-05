using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventLogger
{
    public interface IEventLogger
    {
        string SourcePrefix { get; set; }
        string LogName { get; set; }
        bool Enabled { get; set; }

        bool LogEntry(string Source, string Message, EventLogEntryType Type, int ID = 0, short Category = 0, bool CreateEventSource = false);

        bool CreateEventSource(string Source);
    }
}
