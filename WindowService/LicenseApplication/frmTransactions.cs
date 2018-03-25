using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BLW.Lib.Log;
namespace BLW.Modules.WindowsService.LicenceApplication
{
    public partial class frmTransactions : Form
    {
        BindingSource bindingTransactions;
        string exepath = string.Empty;
        public frmTransactions(string exe)
        {
            this.exepath = exe;
            InitializeComponent();
        }

        public frmTransactions()
        {
            this.exepath = string.Empty;
            InitializeComponent();
        }

        public void bindGrid()
        {
            var runningTransactions = BLW.Lib.SqliteHelper.Tables.Transaction.GetAllRunning(exepath);
            bindingTransactions = new BindingSource();
            bindingTransactions.DataSource = runningTransactions;
            dgvInstances.AutoGenerateColumns = false;
            dgvInstances.DataSource = bindingTransactions;    
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            bindGrid();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                //Remove process 
                try
                {
                    string processId = dgvInstances.Rows[e.RowIndex].Cells[1].Value.ToString();
                    int processID = Convert.ToInt32(processId);
                    System.Diagnostics.Process process = System.Diagnostics.Process.GetProcesses().Where(T => T.Id == processID).FirstOrDefault();
                    if (process != null)
                        process.Kill();
                    //Remove entry from database    
                    BLW.Lib.SqliteHelper.Tables.Transaction.Delete(processID);
                    bindGrid();
                }
                catch (Exception ex)
                {
                    SingletonLogger.Instance.Error("Error Details: " + ex.ToString());
                }

            }
        }
    }
}
