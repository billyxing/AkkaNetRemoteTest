using MessageClassLibrary;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestServer.Control
{
    public class OracleHelper
    {


        public static DBInternalMessages DBQuery(string conStr, string sqlstr,out string result)
        {
            OracleConnection _dbconnection = new OracleConnection(conStr);
            
            _dbconnection.Open();

            try
            {
                if (_dbconnection.State == ConnectionState.Open)
                {
                    OracleCommand _command = new OracleCommand(sqlstr, _dbconnection);
                    OracleDataAdapter _dbadapter = new OracleDataAdapter(_command);
                    DataSet _dataset = new DataSet();
                    _dbadapter.Fill(_dataset);
                    _dbconnection.Close();
                    if (_dataset.Tables.Count > 0)
                    {
                        result = JsonConvert.SerializeObject(_dataset);
                        return DBInternalMessages.DB_QuerySuccess;
                    }
                    else
                    {
                        result = ""; //no data
                        return DBInternalMessages.DB_NODATA;
                    }
                }
                else
                {
                    result = ""; //db not open
                    return DBInternalMessages.DB_NOT_OPEN;
                }

            }
            catch (Exception ex)
            {
                _dbconnection.Close();
                LoggingHelper.WriteLog("AkkaTestServer", "DBQuery Ex:" + ex.Message);
                result = ""; //ex
                return DBInternalMessages.DB_Exception;
            }

            
            

        }

        public static DBInternalMessages DBNonQuery(string conStr, string sqlstr)
        {
            OracleConnection _dbconnection = new OracleConnection(conStr);
           
            _dbconnection.Open();

            try
            {
                if (_dbconnection.State == ConnectionState.Open)
                {
                    OracleCommand _command = new OracleCommand(sqlstr, _dbconnection);
                    int r = _command.ExecuteNonQuery();
                    _dbconnection.Close();
                    if (r > 0)
                    {
                       return DBInternalMessages.DB_NonQuerySuccess;// "Success";
                    }
                    else
                    {
                        return  DBInternalMessages.DB_NonQueryFailed;
                    }
                }
                else
                {
                    return DBInternalMessages.DB_NOT_OPEN; //db not open
                }

            }
            catch (Exception ex)
            {
                _dbconnection.Close();
                LoggingHelper.WriteLog("AkkaTestServer", "DBNonQuery Ex:" + ex.Message);
                return DBInternalMessages.DB_Exception; //ex
            }

            
          

        }




    }
}
