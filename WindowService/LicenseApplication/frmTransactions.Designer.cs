namespace BLW.Modules.WindowsService.LicenceApplication
{
    partial class frmTransactions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvInstances = new System.Windows.Forms.DataGridView();
            this.Guid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProcessID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Application = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KillProcess = new System.Windows.Forms.DataGridViewLinkColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstances)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvInstances
            // 
            this.dgvInstances.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInstances.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Guid,
            this.ProcessID,
            this.Application,
            this.StartTime,
            this.KillProcess});
            this.dgvInstances.Location = new System.Drawing.Point(1, 3);
            this.dgvInstances.Name = "dgvInstances";
            this.dgvInstances.Size = new System.Drawing.Size(873, 356);
            this.dgvInstances.TabIndex = 0;
            this.dgvInstances.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Guid
            // 
            this.Guid.DataPropertyName = "Guid";
            this.Guid.HeaderText = "Guid";
            this.Guid.Name = "Guid";
            this.Guid.Width = 260;
            // 
            // ProcessID
            // 
            this.ProcessID.DataPropertyName = "ProcessId";
            this.ProcessID.HeaderText = "System Process ID";
            this.ProcessID.Name = "ProcessID";
            // 
            // Application
            // 
            this.Application.DataPropertyName = "Application";
            this.Application.HeaderText = "Application";
            this.Application.Name = "Application";
            this.Application.Width = 150;
            // 
            // StartTime
            // 
            this.StartTime.DataPropertyName = "StartedTime";
            this.StartTime.HeaderText = "Start Time";
            this.StartTime.Name = "StartTime";
            this.StartTime.Width = 170;
            // 
            // KillProcess
            // 
            this.KillProcess.DataPropertyName = "KillProcess";
            this.KillProcess.HeaderText = "KillProcess";
            this.KillProcess.LinkColor = System.Drawing.Color.Red;
            this.KillProcess.Name = "KillProcess";
            this.KillProcess.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.KillProcess.VisitedLinkColor = System.Drawing.Color.Red;
            this.KillProcess.Width = 120;
            // 
            // frmTransactions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 372);
            this.Controls.Add(this.dgvInstances);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTransactions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Running Instances";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstances)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvInstances;
        private System.Windows.Forms.DataGridViewTextBoxColumn Guid;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProcessID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Application;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartTime;
        private System.Windows.Forms.DataGridViewLinkColumn KillProcess;
    }
}