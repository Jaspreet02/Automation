using System;
using System.Data.SQLite;
using System.Data;
using System.Configuration;
using System.IO;
using BLW.Lib.Log;

namespace BLW.Lib.SqliteHelper
{

    public class SqliteDbHelper
    {
        // Connection with SQL Lite
        private static SQLiteConnection _sQLiteConnection;
        private static readonly object MSyncRoot = new object();
        private static string DbFile
        {
            get
            {
                var dbFile = ConfigurationManager.AppSettings["DbFile"];
                //Map Special folders               
                if (string.IsNullOrEmpty(dbFile))
                {
                    SingletonLogger.Instance.Debug("Setting database path to application path. File name is : Licences.sqlite.");
                    dbFile = "Licences.sqlite";
                }
                SingletonLogger.Instance.Debug(dbFile);
                return dbFile;
            }
        }

        // Connection string
        private static string ConnectionString
        {
            get
            {
                return string.Format("Data Source={0};Version=3;", DbFile);
            }
        }

        public static SQLiteConnection CurrentConnection
        {
            get
            {
                // Check that the instance is null
                if (_sQLiteConnection == null)
                {
                    // Lock the object
                    lock (MSyncRoot)
                    {
                        // Check to make sure its null
                        if (_sQLiteConnection == null)
                        {
                            SingletonLogger.Instance.Debug(ConnectionString);
                            _sQLiteConnection = new SQLiteConnection(ConnectionString);
                            if (_sQLiteConnection.State != ConnectionState.Open)
                                _sQLiteConnection.Open();
                        }
                    }
                }
                // Return the non-null instance of Singleton
                return _sQLiteConnection;
            }
        }

        /// <summary>
        /// It creates a new database file 
        /// <para>Create schema of all the tables, That I need to register</para>
        /// </summary>
        /// <returns></returns>
        public static bool Initialize()
        {
            // Create database file, if not exists.
            if (CreateDatabase(DbFile))
            {
                //Create Tables(Register tables here)
                Tables.Transaction.CreateSchema();
                Tables.Licence.CreateSchema();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Create Database
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static bool CreateDatabase(string filePath)
        {
            if (!File.Exists(filePath))
                SQLiteConnection.CreateFile(filePath);
            return true;
        }

        /// <summary>
        /// Get Data 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet GetData(string sql)
        {
            try
            {
                SingletonLogger.Instance.Debug(sql);
                SQLiteCommand command = new SQLiteCommand(sql, CurrentConnection);
                // Create the DataAdapter & DataSet
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(command))
                {
                    DataSet ds = new DataSet();
                    // Fill the DataSet using default values for DataTable names, etc
                    da.Fill(ds);

                    // Return the dataset
                    return ds;
                }
            }
            catch (SQLiteException sqlEx)
            {
                if (sqlEx.ResultCode == SQLiteErrorCode.Locked || sqlEx.ResultCode == SQLiteErrorCode.Busy)
                {
                    System.Threading.Thread.Sleep(500);
                    return GetData(sql);
                }
                throw new Exception("Sqlite error while executing query.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new SQLiteException("Error while getting data.", ex);
            }
        }

        /// <summary>
        /// Execute Query
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string ExecuteQuery(string sql)
        {
            try
            {
                // Create a command and prepare it for execution
                SQLiteCommand cmd = new SQLiteCommand(sql, CurrentConnection);
                // Execute the command & return the results
                object retval = cmd.ExecuteScalar();
                if (retval != null)
                    return retval.ToString();
                return string.Empty;
            }
            catch (SQLiteException sqlEx)
            {
                if (sqlEx.ResultCode == SQLiteErrorCode.Locked || sqlEx.ResultCode == SQLiteErrorCode.Busy)
                {
                    System.Threading.Thread.Sleep(500);
                    return ExecuteQuery(sql);
                }
                throw new Exception("Sqlite error while executing query.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while executing query.", ex);
            }
        }

        #region Helper method for SQL queries
        public static string SQLString(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                sql = "";
            return " '" + SafeSQL(sql.Trim()) + "' ";
        }
        public static string SQLBoolean(bool bSql)
        {
            string sql = " 0 ";
            if (bSql)
                sql = " 1 ";
            return sql.Trim();
        }
        public static string SQLNumber(decimal dSql)
        {
            return dSql.ToString();
        }
        public static string SQLNumber(int dSql)
        {
            return dSql.ToString();
        }
        public static string SQLDateTime(DateTime sqlDate)
        {
            string sql = string.Empty;
            if (sqlDate == null)
                sql = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            else
                sql = sqlDate.ToString("yyyy-MM-dd hh:mm:ss tt");
            return " '" + SafeSQL(sql.Trim()) + "' ";
        }
        public static string SQLDateTime(DateTime? sqlDate)
        {
            string sql = "NULL";
            if (sqlDate.HasValue)
            {
                sql = sqlDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt");
                sql = " '" + SafeSQL(sql.Trim()) + "' ";
            }
            return sql;

        }
        private static string SafeSQL(string sql)
        {
            return sql.Replace("'", "''");
        }
        #endregion

        #region Convert object
        public static DateTime ToDateTime(object objValue)
        {
            DateTime returnDate = new DateTime();
            try
            {
                if (objValue != null)
                {
                    string strValue = string.Empty;
                    strValue = objValue.ToString().Trim();
                    if (!string.IsNullOrEmpty(strValue))
                    {
                        if (DateTime.TryParse(strValue, out returnDate))
                        {
                            return returnDate;
                        }
                    }
                }
                else
                {
                    return returnDate;
                }
                return returnDate;
            }
            catch (Exception ex)
            {
                return returnDate;
            }
        }
        public static DateTime? ToNullableDateTime(object objValue)
        {
            DateTime? returnDate = null;
            try
            {
                if (objValue != null)
                {
                    string strValue = string.Empty;
                    strValue = objValue.ToString().Trim();
                    if (!string.IsNullOrEmpty(strValue))
                        try
                        {
                            returnDate = DateTime.Parse(strValue);
                            return returnDate;
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                }
            }
            catch (Exception)
            {
            }
            return returnDate;
        }
        public static decimal ToDecimal(object objValue)
        {
            decimal returnDecimal = 0;
            try
            {
                if (objValue != null)
                {
                    string strValue = string.Empty;
                    strValue = objValue.ToString().Trim();
                    if (!string.IsNullOrEmpty(strValue))
                    {
                        if (decimal.TryParse(strValue, out returnDecimal))
                        {
                            return returnDecimal;
                        }
                    }
                }
                else
                {
                    return returnDecimal;
                }
                return returnDecimal;
            }
            catch (Exception)
            {
                return returnDecimal;
            }
        }
        public static int ToInt(object objValue)
        {
            int returnInt = 0;
            try
            {
                if (objValue != null)
                {
                    string strValue = string.Empty;
                    strValue = objValue.ToString().Trim();
                    if (!string.IsNullOrEmpty(strValue))
                    {
                        if (int.TryParse(strValue, out returnInt))
                        {
                            return returnInt;
                        }
                    }
                }
                else
                {
                    return returnInt;
                }
                return returnInt;
            }
            catch (Exception)
            {
                return returnInt;
            }
        }
        public static string ToString(object objValue)
        {
            string returnString = string.Empty;
            try
            {
                if (objValue != null)
                {
                    string strValue = objValue.ToString().Trim();
                    if (!string.IsNullOrEmpty(strValue))
                    {
                        returnString = strValue;
                        return returnString;
                    }
                }
                else
                {
                    return returnString;
                }
                return returnString;
            }
            catch (Exception)
            {
                return returnString;
            }
        }
        public static bool ToBoolean(object objValue)
        {
            bool returnBool = false;
            try
            {
                if (objValue != null)
                {
                    string strValue = string.Empty;
                    strValue = objValue.ToString().Trim();
                    if (!string.IsNullOrEmpty(strValue))
                    {
                        if (bool.TryParse(strValue, out returnBool))
                        {
                            return returnBool;
                        }
                    }
                }
                else
                {
                    return returnBool;
                }
                return returnBool;
            }
            catch (Exception)
            {
                return returnBool;
            }
        }
        #endregion
    }
}
