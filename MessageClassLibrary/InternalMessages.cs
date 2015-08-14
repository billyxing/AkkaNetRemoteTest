using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageClassLibrary
{
    public enum DBInternalMessages
    {
        //db
        DB_NOT_OPEN = 1002,
        DB_Exception = 1009,
        DB_NODATA = 1001,
        DB_NonQuerySuccess = 1000,
        DB_NonQueryFailed = 1003,
        DB_QuerySuccess = 1000

    }

    public enum ActorInternalMessages
    {
        //db
        DB_NOT_OPEN = 1002,
        DB_Exception = 1009,
        DB_NODATA = 1001,
        DB_NonQuerySuccess = 1000,
        DB_NonQueryFailed = 1003

    }
}
