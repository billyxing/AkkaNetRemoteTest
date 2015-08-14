using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TestServer.Control
{
    public class LoggingHelper
    {

        public static void WriteLog(string sSource,string sLog)
        {
            EventLog m_EventLog = new EventLog("");
            m_EventLog.Source = sSource;
            m_EventLog.WriteEntry(sLog,
                EventLogEntryType.FailureAudit);
          
        }


    }
}
