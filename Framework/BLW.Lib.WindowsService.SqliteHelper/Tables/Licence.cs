using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLW.Lib.Log;

namespace BLW.Lib.SqliteHelper.Tables
{
    /// <summary>
    /// Define the management of licences of a executable file
    /// </summary>
    public class Licence
    {
        public const string TABLE_NAME = "Licences";
        public Licence()
        {
            this._ExePath = string.Empty;
        }
        public Licence(string exePath)
        {
            this._OldExePath = exePath;
            this._ExePath = exePath;
        }

        #region Private Members
        private string _ExePath;
        private string _OldExePath;
        private LicenceType _Type;
        private int _TimeInterval;
        private int _MaxTime;
        private int _ConcurrentLicences;
        private DateTime? _LastRunStart;
        private DateTime? _LastRunEnd;
        private bool _Enabled;
        private bool _Saved;
        private bool _Dirty;
        public int RunningInstances
        {
            get
            {
                return Transaction.NoOfRunningInstances(System.IO.Path.GetFileName(ExePath));
            }
        }
        public string DeleteText
        {
            get
            {
                return "Delete";
            }
        }
        #endregion

        #region Properties
        public string ExePath
        {
            get { return _ExePath; }
            set { _ExePath = value; }
        }
        public string OldExePath
        {
            get { return _OldExePath; }
            set { _OldExePath = value; }
        }
        public LicenceType Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public int TimeInterval
        {
            get { return _TimeInterval; }
            set { _TimeInterval = value; }
        }
        public int MaxTime
        {
            get { return _MaxTime; }
            set { _MaxTime = value; }
        }
        public int ConcurrentLicences
        {
            get { return _ConcurrentLicences; }
            set { _ConcurrentLicences = value; }
        }
        public bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }
        public bool Saved
        {
            get { return _Saved; }
        }
        public bool Dirty
        {
            get { return _Dirty; }
        }
        public DateTime? LastRunStart
        {
            get { return _LastRunStart; }
            set { _LastRunStart = value; }
        }
        public DateTime? LastRunEnd
        {
            get { return _LastRunEnd; }
            set { _LastRunEnd = value; }
        }
        #endregion


        #region Db Methods
        /// <summary>
        /// Create schema 
        /// </summary>
        /// <returns>return true if created, else false</returns>
        public static void CreateSchema()
        {
            string sql = "Create Table IF NOT EXISTS " + TABLE_NAME + " (ExePath VARCHAR(100), Type VARCHAR(20), TimeInterval INT, MaxTime INT, ConcurrentLicences INT,LastRunStart datetime, LastRunEnd datetime, Enabled  BOOLEAN)";
            SqliteDbHelper.ExecuteQuery(sql);
        }
        /// <summary>
        /// Save the licences,
        /// <para>If Guid not exists then insert new </para>
        ///  <para> Otherwise Update existing</para>
        /// </summary>
        /// <returns>Return true if saved, else false</returns>
        public bool Save()
        {
            string sql = string.Empty;
            Licence transaction = Licence.GetInfo(this.ExePath);
            if (transaction == null)
                sql = string.Format("INSERT INTO " + TABLE_NAME + "  (ExePath, Type, TimeInterval,MaxTime,ConcurrentLicences,LastRunStart,LastRunEnd  , Enabled) Values ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", SqliteDbHelper.SQLString(this.ExePath), SqliteDbHelper.SQLString(this.Type.ToString()), SqliteDbHelper.SQLNumber(this.TimeInterval), SqliteDbHelper.SQLNumber(this.MaxTime), SqliteDbHelper.SQLNumber(this.ConcurrentLicences), SqliteDbHelper.SQLDateTime(this.LastRunStart), SqliteDbHelper.SQLDateTime(this.LastRunEnd), SqliteDbHelper.SQLBoolean(this.Enabled));
            else
            {
                this.LastRunEnd = transaction.LastRunEnd;
                this.LastRunStart = transaction.LastRunStart;
                sql = string.Format("UPDATE " + TABLE_NAME + "  Set Type = {1}, TimeInterval = {2}, MaxTime ={3}, ConcurrentLicences = {4}, LastRunStart={5}, LastRunEnd={6}, Enabled = {7} WHERE ExePath = {0} ", SqliteDbHelper.SQLString(this.ExePath), SqliteDbHelper.SQLString(this.Type.ToString()), SqliteDbHelper.SQLNumber(this.TimeInterval), SqliteDbHelper.SQLNumber(this.MaxTime), SqliteDbHelper.SQLNumber(this.ConcurrentLicences), SqliteDbHelper.SQLDateTime(this.LastRunStart), SqliteDbHelper.SQLDateTime(this.LastRunEnd), SqliteDbHelper.SQLBoolean(this.Enabled));
            }

            if (string.IsNullOrEmpty(SqliteDbHelper.ExecuteQuery(sql)))
            {
                _Saved = true;
                _Dirty = false;
            }
            return _Saved;
        }
        public bool InsertAndUpdate()
        {
            string sql = string.Empty;
            Licence transaction = Licence.GetInfo(this.OldExePath);
            if (transaction == null)
                sql = string.Format("INSERT INTO " + TABLE_NAME + "  (ExePath, Type, TimeInterval,MaxTime,ConcurrentLicences,LastRunStart,LastRunEnd  , Enabled) Values ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", SqliteDbHelper.SQLString(this.ExePath), SqliteDbHelper.SQLString(this.Type.ToString()), SqliteDbHelper.SQLNumber(this.TimeInterval), SqliteDbHelper.SQLNumber(this.MaxTime), SqliteDbHelper.SQLNumber(this.ConcurrentLicences), SqliteDbHelper.SQLDateTime(this.LastRunStart), SqliteDbHelper.SQLDateTime(this.LastRunEnd), SqliteDbHelper.SQLBoolean(this.Enabled));
            else
            {
                this.LastRunEnd = transaction.LastRunEnd;
                this.LastRunStart = transaction.LastRunStart;
                sql = string.Format("UPDATE " + TABLE_NAME + "  Set Type = {1}, TimeInterval = {2}, MaxTime ={3}, ConcurrentLicences = {4}, LastRunStart={5}, LastRunEnd={6}, Enabled = {7},ExePath = {8} WHERE ExePath = {0} ", SqliteDbHelper.SQLString(this.OldExePath), SqliteDbHelper.SQLString(this.Type.ToString()), SqliteDbHelper.SQLNumber(this.TimeInterval), SqliteDbHelper.SQLNumber(this.MaxTime), SqliteDbHelper.SQLNumber(this.ConcurrentLicences), SqliteDbHelper.SQLDateTime(this.LastRunStart), SqliteDbHelper.SQLDateTime(this.LastRunEnd), SqliteDbHelper.SQLBoolean(this.Enabled), SqliteDbHelper.SQLString(this.ExePath));
            }

            if (string.IsNullOrEmpty(SqliteDbHelper.ExecuteQuery(sql)))
            {
                _Saved = true;
                _Dirty = false;
            }
            return _Saved;
        }


        /// <summary>
        /// Update Last run start time, will keep track of when component start
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public static bool UpdateLastRunStartTime(string exePath, DateTime startTime)
        {                      
            string sql = string.Format("UPDATE " + TABLE_NAME + "  Set LastRunStart = {0} where ExePath = {1} ", SqliteDbHelper.SQLDateTime(startTime), SqliteDbHelper.SQLString(exePath));
            SingletonLogger.Instance.Debug(sql);
            if (string.IsNullOrEmpty(SqliteDbHelper.ExecuteQuery(sql)))
                return true;
            return false;
        }
        /// <summary>
        /// Update Last run end time, will keep track of when component end
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static bool UpdateLastRunEndTime(string exeName, DateTime endTime)
        {
            //Get exepath from exeName
            exeName = GetExePathByExeName(exeName);
            string sql = string.Format("UPDATE " + TABLE_NAME + "  Set LastRunEnd = {0} where ExePath = {1} ", SqliteDbHelper.SQLDateTime(endTime), SqliteDbHelper.SQLString(exeName));
            SingletonLogger.Instance.Debug(sql);
            if (string.IsNullOrEmpty(SqliteDbHelper.ExecuteQuery(sql)))
                return true;
            return false;
        }
        /// <summary>
        /// Delete Licence having guid
        /// </summary>
        /// <param name="guid">Unique identity</param>
        /// <returns>Return true if saved, else false</returns>
        public static bool Delete(string exePath)
        {
            string sql = "DELETE FROM " + TABLE_NAME + "  WHERE ExePath = {0}";
            sql = string.Format(sql, SqliteDbHelper.SQLString(exePath));
            if (string.IsNullOrEmpty(SqliteDbHelper.ExecuteQuery(sql)))
                return true;
            return false;
        }
        /// <summary>
        /// Get a licence by id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public static Licence Get(string exePath)
        {
            string sql = "SELECT ExePath, Type, TimeInterval,MaxTime, ConcurrentLicences ,LastRunStart,LastRunEnd  , Enabled FROM " + TABLE_NAME + "  WHERE ExePath = '{0}'";            
            sql = string.Format(sql, exePath);
            SingletonLogger.Instance.Debug(sql);
            System.Data.DataSet transData = SqliteDbHelper.GetData(sql);
            if (transData != null)
                if (transData.Tables.Count > 0)
                    if (transData.Tables[0].Rows.Count > 0)
                    {
                        System.Data.DataRow dr = transData.Tables[0].Rows[0];
                        Licence Licence = new Licence(SqliteDbHelper.ToString(dr["ExePath"]));
                        Licence.Type = (LicenceType)Enum.Parse(typeof(LicenceType), SqliteDbHelper.ToString(dr["Type"]));
                        Licence.TimeInterval = SqliteDbHelper.ToInt(dr["TimeInterval"]);
                        Licence.MaxTime = SqliteDbHelper.ToInt(dr["MaxTime"]);
                        Licence.ConcurrentLicences = SqliteDbHelper.ToInt(dr["ConcurrentLicences"]);
                        Licence.LastRunStart = SqliteDbHelper.ToNullableDateTime(dr["LastRunStart"]);
                        Licence.LastRunEnd = SqliteDbHelper.ToNullableDateTime(dr["LastRunEnd"]);
                        Licence.Enabled = SqliteDbHelper.ToBoolean(dr["Enabled"]);
                        return Licence;
                    }
            return null;
        }
        /// <summary>
        /// Get  licence info of a licences by id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public static Licence GetInfo(string exePath)
        {
            string sql = "SELECT ExePath, Type, TimeInterval,MaxTime, ConcurrentLicences ,LastRunStart,LastRunEnd  , Enabled FROM " + TABLE_NAME + "  WHERE ExePath = {0} ";
            sql = string.Format(sql, SqliteDbHelper.SQLString(exePath));
            System.Data.DataSet transData = SqliteDbHelper.GetData(sql);
            if (transData != null)
                if (transData.Tables.Count > 0)
                    if (transData.Tables[0].Rows.Count > 0)
                    {
                        System.Data.DataRow dr = transData.Tables[0].Rows[0];
                        Licence Licence = new Licence(SqliteDbHelper.ToString(dr["ExePath"]));
                        Licence.Type = (LicenceType)Enum.Parse(typeof(LicenceType), SqliteDbHelper.ToString(dr["Type"]));
                        Licence.TimeInterval = SqliteDbHelper.ToInt(dr["TimeInterval"]);
                        Licence.MaxTime = SqliteDbHelper.ToInt(dr["MaxTime"]);
                        Licence.ConcurrentLicences = SqliteDbHelper.ToInt(dr["ConcurrentLicences"]);
                        Licence.LastRunStart = SqliteDbHelper.ToNullableDateTime(dr["LastRunStart"]);
                        Licence.LastRunEnd = SqliteDbHelper.ToNullableDateTime(dr["LastRunEnd"]);
                        Licence.Enabled = SqliteDbHelper.ToBoolean(dr["Enabled"]);
                        return Licence;
                    }
            return null;
        }
        /// <summary>
        /// Get all Licences
        /// </summary>
        /// <returns></returns>
        private static List<Licence> GetAllLicences(LicenceType type, bool? enabled = null)
        {
            List<Licence> Licences = new List<Licence>();
            string sWHERE = " WHERE ";
            string sAND = string.Empty;
            string sql = "SELECT ExePath, Type, TimeInterval,MaxTime, ConcurrentLicences ,LastRunStart,LastRunEnd  , Enabled FROM " + TABLE_NAME;
            switch (type)
            {
                case LicenceType.TRIGGERBASE:
                    sql = sql + sWHERE + sAND + " Type = 'TRIGGERBASE' ";
                    sWHERE = string.Empty;
                    sAND = " AND ";
                    break;
                case LicenceType.PERIODIC:
                    sql = sql + sWHERE + sAND + " Type = 'PERIODIC' ";
                    sWHERE = string.Empty;
                    sAND = " AND ";
                    break;
                case LicenceType.NONE:
                    break;
            }
            if (enabled.HasValue)
            {
                if (enabled.Value)
                    sql = sql + sWHERE + sAND + " Enabled = '1'";
                else
                    sql = sql + sWHERE + sAND + " Enabled = '0'";

                sWHERE = string.Empty;
                sAND = " AND ";
            }
            System.Data.DataSet transData = SqliteDbHelper.GetData(sql);
            if (transData != null)
                if (transData.Tables.Count > 0)
                    if (transData.Tables[0].Rows.Count > 0)
                        foreach (System.Data.DataRow dr in transData.Tables[0].Rows)
                        {
                            Licence Licence = new Licence(SqliteDbHelper.ToString(dr["ExePath"]));
                            
                            Licence.Type = (LicenceType)Enum.Parse(typeof(LicenceType), SqliteDbHelper.ToString(dr["Type"]));
                            Licence.TimeInterval = SqliteDbHelper.ToInt(dr["TimeInterval"]);
                            Licence.MaxTime = SqliteDbHelper.ToInt(dr["MaxTime"]);
                            Licence.ConcurrentLicences = SqliteDbHelper.ToInt(dr["ConcurrentLicences"]);
                            Licence.LastRunStart = SqliteDbHelper.ToNullableDateTime(dr["LastRunStart"]);
                            Licence.LastRunEnd = SqliteDbHelper.ToNullableDateTime(dr["LastRunEnd"]);
                            Licence.Enabled = SqliteDbHelper.ToBoolean(dr["Enabled"]);
                            Licences.Add(Licence);
                        }
            return Licences;
        }
        /// <summary>
        /// Get all licences
        /// </summary>
        /// <returns></returns>
        public static List<Licence> GetAll()
        {
            return GetAllLicences(LicenceType.NONE);
        }
        /// <summary>
        /// Get all timebase Licences
        /// </summary>
        /// <returns></returns>
        public static List<Licence> GetAllTimebase()
        {
            return GetAllLicences(LicenceType.PERIODIC, true);
        }
        public static List<Licence> GetAllTriggerbase()
        {
            return GetAllLicences(LicenceType.TRIGGERBASE, true);
        }
        private static string GetExePathByExeName(string exeName)
        {
            string sql = "SELECT ExePath FROM " + TABLE_NAME + " WHERE ExePath LIKE '%" + exeName + "%' AND Enabled = '1'";
            System.Data.DataSet transData = SqliteDbHelper.GetData(sql);
            if (transData != null)
                if (transData.Tables.Count > 0)
                    if (transData.Tables[0].Rows.Count > 0)
                        foreach (System.Data.DataRow dr in transData.Tables[0].Rows)
                        {
                            string exePath = SqliteDbHelper.ToString(dr["ExePath"]);
                            if (!string.IsNullOrEmpty(exePath))
                                if (System.IO.Path.GetFileName(exePath).Equals(exeName))
                                    return exePath;
                            throw new Exception("Error while getting exe Name.");
                        }
            return string.Empty;
        }
        #endregion
    }
}
