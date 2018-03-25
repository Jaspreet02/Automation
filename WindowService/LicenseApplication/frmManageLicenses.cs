using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BLW.Lib.SqliteHelper;
using BLW.Lib.SqliteHelper.Tables;
using BLW.Lib.Log;

namespace BLW.Modules.WindowsService.LicenceApplication
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmManageLicences : Form
    {
        BindingSource bindingLicenses;
        /// <summary>
        /// Initialise Manage Licences form
        /// </summary>
        public frmManageLicences()
        {
            InitializeComponent();
            LogInitializer.InitializeLogger("LicenceApp");
            lblMessage.Visible = true;
            SingletonLogger.Instance.Attach(new ObserverLogToWindows(lblMessage)); // Send log messages to debugger form lbl message (output window).             
        }
        /// <summary>
        /// Save or update Licencess
        /// </summary>
        /// <param name="sender">click</param>
        /// <param name="e">event</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            int iteration = 0;
            foreach (DataGridViewRow gr in dgvLicence.Rows)
            {
               
                Licence tblLicence = new Licence();
                if (!string.IsNullOrEmpty(gr.Cells["ExePath"].Value.ToString()))
                {
                    tblLicence.ExePath = gr.Cells["ExePath"].Value.ToString();
                }
                else
                {

                    SingletonLogger.Instance.Warning("Please Fill exe name in Row number:" + iteration+1 + " or delete row number:" + iteration+1 + "");
                    return;
                }
                tblLicence.OldExePath =iteration>=_ActiveLicences.Count?gr.Cells["ExePath"].Value.ToString(): _ActiveLicences[iteration].OldExePath;
                tblLicence.Type = (LicenceType)Enum.Parse(typeof(LicenceType), gr.Cells["Type"].Value.ToString());
                tblLicence.MaxTime = Convert.ToInt32(gr.Cells["MaxTime"].Value);
                tblLicence.Enabled = Convert.ToBoolean(gr.Cells["Enabled"].Value);
                tblLicence.TimeInterval = Convert.ToInt32(gr.Cells["TimeInterval"].Value);
                tblLicence.ConcurrentLicences = Convert.ToInt32(gr.Cells["ConcurrentLicences"].Value);
                tblLicence.InsertAndUpdate();
                iteration = iteration + 1;
            }
            BindLicences();
            SingletonLogger.Instance.Info("All Licencess has updated or saved");
        }

        private List<Licence> _ActiveLicences;
        /// <summary>
        /// Property get all licencess
        /// </summary>
        public List<Licence> ActiveLicences
        {
            get { return _ActiveLicences; }
        }
        /// <summary>
        /// Load all Licencess in DataGrid
        /// </summary>
        /// <param name="sender">Form Load</param>
        /// <param name="e">event</param>
        private void frmManageLicences_Load(object sender, EventArgs e)
        {
            DataGridViewComboBoxColumn combo;
            combo = CreateComboBoxColumn();
            List<LicenceType> lt = new List<LicenceType>();
            lt.Add(LicenceType.PERIODIC);
            lt.Add(LicenceType.TRIGGERBASE);
            combo.DataSource = lt;
            dgvLicence.Columns.Insert(2, combo);
            dgvLicence.AllowUserToAddRows = false;
            BindLicences();
            SingletonLogger.Instance.Info("Licences successfully loaded.");
        }
        /// <summary>
        /// add Combo box in DataGrid
        /// </summary>
        /// <returns>returns DataGridColumn</returns>
        private DataGridViewComboBoxColumn CreateComboBoxColumn()
        {
            DataGridViewComboBoxColumn column =
                new DataGridViewComboBoxColumn();
            {
                column.DataPropertyName = "Type";
                column.HeaderText = "Type";
                column.Name = "Type";
                column.Width = 100;
                column.FlatStyle = FlatStyle.Flat;
            }
            return column;
        }
        /// <summary>
        /// method to handle each column click
        /// </summary>
        /// <param name="sender">click</param>
        /// <param name="e">event</param>
        private void dgvLicence_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 8)
            {
                if (dgvLicence.Rows[e.RowIndex].Cells[8].Value.ToString() != "0")
                {
                    string exepathName = dgvLicence.Rows[e.RowIndex].Cells[0].Value.ToString();
                    frmTransactions frm = new frmTransactions(System.IO.Path.GetFileName(exepathName));
                    frm.ShowDialog();
                }
            }
            if (e.ColumnIndex == 9)
            {


                if (!string.IsNullOrEmpty(dgvLicence.Rows[e.RowIndex].Cells[0].Value.ToString()))
                {
                    string exepathName = dgvLicence.Rows[e.RowIndex].Cells[0].Value.ToString();
                    var getLicences = Licence.GetInfo(exepathName);
                    if (getLicences != null)
                    {
                        DialogResult x = MessageBox.Show("Are you Sure to Delete?", "Delete Confirmation", MessageBoxButtons.OKCancel);
                        if (x == DialogResult.OK)
                        {
                            bool action = Licence.Delete(exepathName);
                            if (action)
                            {
                                bindingLicenses.RemoveAt(e.RowIndex);
                                bindingLicenses.ResetBindings(false);

                                SingletonLogger.Instance.Info("Licencess of exe:" + exepathName + " has Successfully deleted");
                            }
                            else
                            {
                                SingletonLogger.Instance.Error("Error while Deleting Licences of exe:" + exepathName);

                            }
                        }
                    }
                    else
                    {

                        bindingLicenses.RemoveAt(e.RowIndex);
                        bindingLicenses.ResetBindings(false);
                    }
                }
                else
                {
                    int cc = bindingLicenses.Count - 1;

                    bindingLicenses.RemoveAt(e.RowIndex);
                    bindingLicenses.ResetBindings(false);
                }
            }
        }
        /// <summary>
        /// event occur when move from one cell to another,used to validate cell value
        /// </summary>
        /// <param name="sender">DataGrid cell leave handler</param>
        /// <param name="e">event</param>
        private void dgvLicence_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                switch (e.ColumnIndex)
                {

                    case 0:
                        List<string> allExe = new List<string>();
                        foreach (DataGridViewRow gr in dgvLicence.Rows)
                        {
                            if (gr.Index != e.RowIndex)
                            {
                                allExe.Add(gr.Cells["ExePath"].Value.ToString());
                            }
                        }
                        string exepath = dgvLicence.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString();
                        if (allExe.Contains(exepath))
                        {
                            SingletonLogger.Instance.Error("Duplicate exe name.");
                            return;
                        }
                        _ActiveLicences[e.RowIndex].ExePath = exepath;
                        break;
                    case 1:
                        int timeInterval = 0;
                        if (!int.TryParse(dgvLicence.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString(), out timeInterval))
                        {
                            SingletonLogger.Instance.Error("Not a valid Time Interval.");
                            return;
                        }
                        break;
                    case 2:

                        string typeSelect = dgvLicence.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString();
                        if (typeSelect == "PERIODIC")
                        {
                            dgvLicence.Rows[e.RowIndex].Cells["ConcurrentLicences"].Value = 1;
                            dgvLicence.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "PERIODIC";
                        }
                        break;
                    case 3:

                        int ConcurrentLicences = 0;
                        if (!int.TryParse(dgvLicence.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString(), out ConcurrentLicences))
                        {
                            SingletonLogger.Instance.Error("Not a valid Time Concurrent Licences Value");
                            return;
                        }
                        break;
                    case 4:

                        int maxtime = 0;
                        if (!int.TryParse(dgvLicence.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString(), out maxtime))
                        {
                            SingletonLogger.Instance.Error("Not a valid Maxtime value.");
                            return;
                        }
                        break;
                    default:
                        break;
                }

            }
        }
        /// <summary>
        /// Occur When Save button Clicked.Reload Licencess from Database
        /// </summary>
        /// <param name="sender">Refresh button click</param>
        /// <param name="e">event</param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindLicences();
            SingletonLogger.Instance.Info("Licences Successfully Refreshed.");
        }
        /// <summary>
        /// Occur when Add New Button Clicked.Add new Row in DataGRid
        /// </summary>
        /// <param name="sender">Add New Button Click</param>
        /// <param name="e">event</param>
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            if (validateLastRow())
            {
                Licence licence = new Licence("");
                licence.Enabled = true;
                licence.MaxTime = -1;
                licence.ConcurrentLicences = 1;
                licence.Type = LicenceType.PERIODIC;
                ActiveLicences.Add(licence);
                bindingLicenses.ResetBindings(false);
                SingletonLogger.Instance.Info("New row has been added successfully");
            }
            else
            {
                SingletonLogger.Instance.Warning("Please Enter Exe Name");
                return;
            }
        }
        /// <summary>
        ///Load Licencess from Database into DataGrid
        /// </summary>
        public void BindLicences()
        {

            _ActiveLicences = Licence.GetAll();
            bindingLicenses = new BindingSource();
            bindingLicenses.DataSource = ActiveLicences;
            dgvLicence.AutoGenerateColumns = false;
            dgvLicence.DataSource = bindingLicenses;
        }
        /// <summary>
        /// Validate for Add new button check to add new row or not
        /// </summary>
        /// <returns>Returns true if exepath value of last DataGrid's Row is not empty </returns>
        public bool validateLastRow()
        {
            int LastRowIndex = dgvLicence.Rows.Count;
            if (LastRowIndex > 0)
            {
                DataGridViewRow gr = dgvLicence.Rows[LastRowIndex - 1];
                Licence tblLicence = new Licence();
                if (!string.IsNullOrEmpty(gr.Cells["ExePath"].Value.ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Show all running instancess
        /// </summary>
        /// <param name="sender">All Instancess button Click</param>
        /// <param name="e">event</param>
        private void btnAllInstances_Click(object sender, EventArgs e)
        {
            frmTransactions frm = new frmTransactions();
            frm.ShowDialog();
        }
        /// <summary>
        /// Handing default error message of DataGrid
        /// </summary>
        /// <param name="sender">DataGrid error handler</param>
        /// <param name="e">event</param>
        private void dgvLicence_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void frmManageLicences_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to save changes?", "Save changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Cancel the Closing event from closing the form.              
                btnSave_Click(null, null);
            }
        }

      

    }
}
