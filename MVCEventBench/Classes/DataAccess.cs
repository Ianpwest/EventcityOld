using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Net.Mail;


namespace MVCEventBench.Classes
{
    public class DataAccess
    {
        public static string getDBConnection()
        {
            SqlConnectionStringBuilder builderConnection = new SqlConnectionStringBuilder();

            
                builderConnection.DataSource = "Devserver";
                builderConnection.UserID = "Administrator";
                builderConnection.Password = "Margaritas!";


                builderConnection.InitialCatalog = "Events";
            string strConnection = builderConnection.ToString();

            return strConnection;
        }

        public static bool RunSQL(string strSQL, string strUserName)
        {
            bool bSuccess = true;
            using (SqlConnection connection = new SqlConnection(getDBConnection()))
            {
                using (SqlCommand cmdSQL = new SqlCommand(strSQL, connection))
                {
                    try
                    {
                        connection.Open();
                        cmdSQL.ExecuteNonQuery();
                    }
                    catch (Exception sqlExcept)
                    {
                        bSuccess = false;
                        string strErrorMessage = "An error has occured in the TeamSim Web application for either a data UPDATE/INSERT " +
                                                 "using the following sql string : " + strSQL.ToString() + ".  The following user was logged " +
                                                 "in at the time : " + strUserName + ".  The error message provided was: " + sqlExcept.ToString();
                        //LogError(strErrorMessage, "TeamSim.DataAccess::RunSQL", strSQL, strUserName);
                    }
                }
                return bSuccess;
            }
        }

        public static DataTable FillDT(string strSQL, ref DataTable dt, string strUserName)
        {
            dt.Clear();

            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                using (SqlConnection connection = new SqlConnection(getDBConnection()))
                {
                    using (SqlCommand cmdSQL = new SqlCommand(strSQL, connection))
                    {
                        adapter.SelectCommand = cmdSQL;
                        try
                        {
                            adapter.Fill(dt);
                        }
                        catch (Exception sqlExcept)
                        {
                            string strErrorMessage = "An error has occured in the TeamSim Web application.  A datatable was not filled " +
                                                     "using the following sql string : " + strSQL + ".  The following user was logged in " +
                                                     "at the time : " + strUserName + ".  The error message provided was: " + sqlExcept.ToString();
                            //LogError(strErrorMessage, "TeamSim.DataAccess::FillDT", "FillDT", strUserName);
                        }
                    }

                }
            }
            return dt;
        }

        internal static bool RunSQLWithParams(SqlConnection conn, SqlCommand cmd, List<SqlParameter> lstSqlParam, string strUserName)
        {
            bool bSuccess = false;
            using (conn)
            {
                using (cmd)
                {
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        bSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        //LogError(ex.ToString(), "DataAccess:RunSQLWithParams", "Trying to run SQL with Params.  Error occured : " + ex.ToString(), strUserName);
                    }
                       
                }
            }
            return bSuccess;
        }

      
    }
}
