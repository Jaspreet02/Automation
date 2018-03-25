using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace BLW.Lib.SqliteHelper.Tables
{

    public class Transaction
    {

        public Transaction(string guid)
        {
            this._Guid = guid;
        }

        #region Private Members
        private string _Guid;
        private string _ExePath;
        private int _ProcessId;
        private string _Application;
        private DateTime? _StartedAt;
        private DateTime? _EndedAt;
        private bool _Enabled;
        private bool _Saved;
        private bool _Dirty;


        public string KillProcess
        {
            get { return "Kill Process"; }
        }
        public bool Saved
        {
            get { return _Saved; }
        }
        public bool Dirty
        {
            get { return _Dirty; }
        }
        #endregion

        #region Properties
        public string Guid
        {
            get { return _Guid; }
        }
        public int ProcessId
        {
            get { return _ProcessId; }
            set { _ProcessId = value; }
        }
        public string Application
        {
            get { return _Application; }
            set { _Application = value; }
        }
        public string ExeName
        {
            get { return _ExePath; }
            set { _ExePath = value; }
        }
        public DateTime? StartedAt
        {
            get { return _StartedAt; }
            set { _StartedAt = value; }
        }
        public string StartedTime
        {
            get { return _StartedAt.HasValue ? _StartedAt.Value.ToString("dd-MM-yyyy hh:mm:ss tt") : string.Empty; }
        }
        public DateTime? EndedAt
        {
            get { return _EndedAt; }
            set { _EndedAt = value; }
        }
        public bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }
        #endregion



        #region Db Methods
        /// <summary>
        /// Create schema for Transaction table if it is not exist
        /// </summary>
        /// <returns>return true if created, else false</returns>
        public static void CreateSchema()
        {
            string sql = "Create Table IF NOT EXISTS Transactions (Guid VARCHAR(100), ProcessId INT, Application VARCHAR(100),ExePath VARCHAR(100), StartedAt  VARCHAR(30), EndedAt  VARCHAR(30), Enabled  BOOLEAN)";
            SqliteDbHelper.ExecuteQuery(sql);
        }

        /// <summary>
        /// Save the transaction,
        /// <para>If GUID not exists then insert new </para>
        ///  <para> Otherwise Update existing</para>
        /// </summary>
        /// <param name="forceInsert">Insert a new transaction force fully</param>
        /// <returns>Return true if saved, else false</returns>  
        public bool Save(bool forceInsert = false)
        {
            string sql = string.Empty;
            if (forceInsert)
                sql = string.Format("INSERT INTO Transactions (Guid, ProcessId, Application, ExePath, StartedAt, EndedAt, Enabled) Values ({0}, {1}, {2}, {3}, {4}, {5}, {6})", SqliteDbHelper.SQLString(this.Guid), SqliteDbHelper.SQLNumber(this.ProcessId), SqliteDbHelper.SQLString(this.Application), SqliteDbHelper.SQLString(this.ExeName), SqliteDbHelper.SQLDateTime(this.StartedAt), SqliteDbHelper.SQLDateTime(this.EndedAt), SqliteDbHelper.SQLBoolean(this.Enabled));
            else
            {
                Transaction transaction = Transaction.Get(this.Guid);
                if (transaction != null) //ByJASPREET(09/jan/2015):it insert row sometime after delete by exe successfully.                    
                    sql = string.Format("UPDATE Transactions Set ProcessId = {0}, Application = {1},ExePath = {2}, StartedAt = {3}, EndedAt = {4}, Enabled = {5} where Guid = {6} ", SqliteDbHelper.SQLNumber(this.ProcessId), SqliteDbHelper.SQLString(this.Application), SqliteDbHelper.SQLString(this.ExeName), SqliteDbHelper.SQLDateTime(this.StartedAt), SqliteDbHelper.SQLDateTime(this.EndedAt), SqliteDbHelper.SQLBoolean(this.Enabled), SqliteDbHelper.SQLString(this.Guid));
            }
            if (string.IsNullOrEmpty(SqliteDbHelper.ExecuteQuery(sql)))
            {
                _Saved = true;
                _Dirty = false;
            }
            return _Saved;
        }

        /// <summary>
        /// Delete transaction having GUID
        /// </summary>
        /// <param name="guid">Unique identity</param>
        /// <returns>Return true if saved, else false</returns>
        public static bool Delete(string guid)
        {
            string sql = "DELETE FROM Transactions WHERE Guid = {0}";
            sql = string.Format(sql, SqliteDbHelper.SQLString(guid));
            if (string.IsNullOrEmpty(SqliteDbHelper.ExecuteQuery(sql)))
                return true;
            return false;
        }
        public static bool Delete(int processId)
        {
            string sql = "DELETE FROM Transactions WHERE ProcessId = {0}";
            sql = string.Format(sql, SqliteDbHelper.SQLNumber(processId));
            if (string.IsNullOrEmpty(SqliteDbHelper.ExecuteQuery(sql)))
                return true;
            return false;
        }
        public static Transaction Get(string guid)
        {
            string sql = "SELECT Guid, ExePath,Application,ProcessId, StartedAt, EndedAt, Enabled FROM Transactions WHERE Guid = {0} AND Enabled =" + SqliteDbHelper.SQLBoolean(true);
            sql = string.Format(sql, SqliteDbHelper.SQLString(guid));
            System.Data.DataSet transData = SqliteDbHelper.GetData(sql);
            if (transData != null)
                if (transData.Tables.Count > 0)
                    if (transData.Tables[0].Rows.Count == 1)
                    {
                        System.Data.DataRow dr = transData.Tables[0].Rows[0];
                        Transaction transaction = new Transaction(SqliteDbHelper.ToString(dr["Guid"]));
                        transaction.ProcessId = SqliteDbHelper.ToInt(dr["ProcessId"]);
                        transaction.Application = SqliteDbHelper.ToString(dr["Application"]);
                        transaction.ExeName = SqliteDbHelper.ToString(dr["ExePath"]);
                        transaction.StartedAt = SqliteDbHelper.ToNullableDateTime(dr["StartedAt"]);
                        transaction.EndedAt = SqliteDbHelper.ToNullableDateTime(dr["EndedAt"]);
                        transaction.Enabled = SqliteDbHelper.ToBoolean(dr["Enabled"]);
                        return transaction;
                    }
            return null;
        }

        #endregion
        public static List<Transaction> GetAllRunning(string exePath)
        {
            List<Transaction> list = new List<Transaction>();
            string sql = "SELECT Guid, ExePath,ProcessId, StartedAt, Application, EndedAt, Enabled FROM Transactions WHERE Enabled =" + SqliteDbHelper.SQLBoolean(true);
            if (!string.IsNullOrEmpty(exePath))
                sql += " AND ExePath =" + SqliteDbHelper.SQLString(exePath);

            System.Data.DataSet transData = SqliteDbHelper.GetData(sql);
            if (transData != null)
                if (transData.Tables.Count > 0)
                    if (transData.Tables[0].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow dr in transData.Tables[0].Rows)
                        {
                            Transaction transaction = new Transaction(SqliteDbHelper.ToString(dr["Guid"]));
                            transaction.ProcessId = SqliteDbHelper.ToInt(dr["ProcessId"]);
                            transaction.Application = SqliteDbHelper.ToString(dr["Application"]);
                            transaction.ExeName = SqliteDbHelper.ToString(dr["ExePath"]);
                            transaction.StartedAt = SqliteDbHelper.ToNullableDateTime(dr["StartedAt"]);
                            transaction.EndedAt = SqliteDbHelper.ToNullableDateTime(dr["EndedAt"]);
                            transaction.Enabled = SqliteDbHelper.ToBoolean(dr["Enabled"]);
                            list.Add(transaction);
                        }
                    }
            return list;
        }
        /// <summary>
        /// Get number of instances
        /// </summary>
        /// <param name="exePath"></param>
        /// <returns></returns>
        public static int NoOfRunningInstances(string exePath)
        {
            List<Transaction> list = new List<Transaction>();
            string sql = "SELECT Count(*) NoOfInstances FROM Transactions WHERE ExePath like '{0}' AND Enabled =" + SqliteDbHelper.SQLBoolean(true);
            sql = string.Format(sql, exePath);
            System.Data.DataSet transData = SqliteDbHelper.GetData(sql);
            if (transData != null)
                if (transData.Tables.Count > 0)
                    if (transData.Tables[0].Rows.Count > 0)
                    {
                        System.Data.DataRow dr = transData.Tables[0].Rows[0];
                        return SqliteDbHelper.ToInt(dr["NoOfInstances"]);
                    }
            return 0;
        }
    }
}
