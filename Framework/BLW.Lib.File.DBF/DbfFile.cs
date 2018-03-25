using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;

namespace BLW.Lib.File.DBF
{
    public class DbfFile
    {
        string _connectionString = @"Provider=VFPOLEDB.1;Mode=ReadWrite;Data Type=DBF;Collating Sequence=general;Data Source={0};Extended Properties=dBase III;";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sPath"></param>
        public DbfFile(string sPath)
        {
            DBFPath = sPath;
            DirectoryPath = Path.GetDirectoryName(sPath);
            TableName = Path.GetFileNameWithoutExtension(sPath);
            ConnectionString = string.Format(_connectionString, DirectoryPath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sDir"></param>
        /// <param name="sTable"></param>
        public DbfFile(string sDir, string sTable)
        {
            DirectoryPath = sDir;
            TableName = sTable;
            ConnectionString = string.Format(_connectionString, DirectoryPath);
        }
        public string ConnectionString { get; set; }
        public string DBFPath { get; set; }
        public string DirectoryPath { get; set; }
        public string TableName { get; set; }
        public DataSet Read()
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                connection.Open();
                string query = "select * from {0}.dbf";
                query = string.Format(query, TableName);
                OleDbCommand command = new OleDbCommand(query, connection);
                command.CommandType = CommandType.Text;
                command.CommandText = query;
                OleDbDataAdapter da = new OleDbDataAdapter(command);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }        
    }
}
