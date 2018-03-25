namespace BLW.Modules.WindowsService.LicenceApplication
{
    partial class frmManageLicences
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvLicence = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAllInstances = new System.Windows.Forms.Button();
            this.tbLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.GridPanel = new System.Windows.Forms.Panel();
            this.ExePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeInterval = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConcurrentLicences = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastRunStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastRunEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Enabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Instances = new System.Windows.Forms.DataGridViewLinkColumn();
            this.btnDelete = new System.Windows.Forms.DataGridViewLinkColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLicence)).BeginInit();
            this.tbLayoutPanel.SuspendLayout();
            this.ButtonPanel.SuspendLayout();
            this.GridPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvLicence
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLicence.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLicence.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLicence.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ExePath,
            this.TimeInterval,
            this.ConcurrentLicences,
            this.MaxTime,
            this.LastRunStart,
            this.LastRunEnd,
            this.Enabled,
            this.Instances,
            this.btnDelete});
            this.dgvLicence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLicence.Location = new System.Drawing.Point(0, 0);
            this.dgvLicence.Name = "dgvLicence";
            this.dgvLicence.RowHeadersWidth = 25;
            this.dgvLicence.Size = new System.Drawing.Size(1211, 424);
            this.dgvLicence.TabIndex = 0;
            this.dgvLicence.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLicence_CellContentClick);
            this.dgvLicence.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLicence_CellLeave);
            this.dgvLicence.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvLicence_DataError);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1142, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(66, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(133, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(66, 23);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnAddNew
            // 
            this.btnAddNew.Location = new System.Drawing.Point(1066, 3);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(70, 23);
            this.btnAddNew.TabIndex = 4;
            this.btnAddNew.Text = "Add New";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(13, 395);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 13);
            this.lblMessage.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 375);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 5;
            // 
            // btnAllInstances
            // 
            this.btnAllInstances.Location = new System.Drawing.Point(3, 3);
            this.btnAllInstances.Name = "btnAllInstances";
            this.btnAllInstances.Size = new System.Drawing.Size(124, 23);
            this.btnAllInstances.TabIndex = 6;
            this.btnAllInstances.Text = "All Running Instances";
            this.btnAllInstances.UseVisualStyleBackColor = true;
            this.btnAllInstances.Click += new System.EventHandler(this.btnAllInstances_Click);
            // 
            // tbLayoutPanel
            // 
            this.tbLayoutPanel.ColumnCount = 1;
            this.tbLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbLayoutPanel.Controls.Add(this.ButtonPanel, 0, 1);
            this.tbLayoutPanel.Controls.Add(this.GridPanel, 0, 0);
            this.tbLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tbLayoutPanel.Name = "tbLayoutPanel";
            this.tbLayoutPanel.RowCount = 2;
            this.tbLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.45631F));
            this.tbLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.543688F));
            this.tbLayoutPanel.Size = new System.Drawing.Size(1217, 471);
            this.tbLayoutPanel.TabIndex = 7;
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.Controls.Add(this.btnAllInstances);
            this.ButtonPanel.Controls.Add(this.btnRefresh);
            this.ButtonPanel.Controls.Add(this.btnAddNew);
            this.ButtonPanel.Controls.Add(this.btnSave);
            this.ButtonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonPanel.Location = new System.Drawing.Point(3, 433);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Size = new System.Drawing.Size(1211, 35);
            this.ButtonPanel.TabIndex = 1;
            // 
            // GridPanel
            // 
            this.GridPanel.Controls.Add(this.dgvLicence);
            this.GridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridPanel.Location = new System.Drawing.Point(3, 3);
            this.GridPanel.Name = "GridPanel";
            this.GridPanel.Size = new System.Drawing.Size(1211, 424);
            this.GridPanel.TabIndex = 0;
            // 
            // ExePath
            // 
            this.ExePath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ExePath.DataPropertyName = "ExePath";
            this.ExePath.HeaderText = "Executable/Script File";
            this.ExePath.Name = "ExePath";
            // 
            // TimeInterval
            // 
            this.TimeInterval.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TimeInterval.DataPropertyName = "TimeInterval";
            this.TimeInterval.HeaderText = "Time Interval (Minutes)";
            this.TimeInterval.Name = "TimeInterval";
            // 
            // ConcurrentLicences
            // 
            this.ConcurrentLicences.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ConcurrentLicences.DataPropertyName = "ConcurrentLicences";
            this.ConcurrentLicences.HeaderText = "Concurrent Licences";
            this.ConcurrentLicences.Name = "ConcurrentLicences";
            this.ConcurrentLicences.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ConcurrentLicences.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MaxTime
            // 
            this.MaxTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MaxTime.DataPropertyName = "MaxTime";
            this.MaxTime.HeaderText = "Maximum Time";
            this.MaxTime.Name = "MaxTime";
            // 
            // LastRunStart
            // 
            this.LastRunStart.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LastRunStart.DataPropertyName = "LastRunStart";
            this.LastRunStart.HeaderText = "Last Run Start Time";
            this.LastRunStart.Name = "LastRunStart";
            // 
            // LastRunEnd
            // 
            this.LastRunEnd.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LastRunEnd.DataPropertyName = "LastRunEnd";
            this.LastRunEnd.HeaderText = "Last Run End Time";
            this.LastRunEnd.Name = "LastRunEnd";
            // 
            // Enabled
            // 
            this.Enabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Enabled.DataPropertyName = "Enabled";
            this.Enabled.HeaderText = "Enabled";
            this.Enabled.Name = "Enabled";
            // 
            // Instances
            // 
            this.Instances.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Instances.DataPropertyName = "RunningInstances";
            this.Instances.HeaderText = "Instances";
            this.Instances.Name = "Instances";
            // 
            // btnDelete
            // 
            this.btnDelete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.btnDelete.DataPropertyName = "DeleteText";
            this.btnDelete.HeaderText = " ";
            this.btnDelete.LinkColor = System.Drawing.Color.Red;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnDelete.Text = "Delete";
            this.btnDelete.ToolTipText = "Delete record";
            this.btnDelete.VisitedLinkColor = System.Drawing.Color.Red;
            // 
            // frmManageLicences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1217, 471);
            this.Controls.Add(this.tbLayoutPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmManageLicences";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage Licences";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmManageLicences_FormClosing);
            this.Load += new System.EventHandler(this.frmManageLicences_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLicence)).EndInit();
            this.tbLayoutPanel.ResumeLayout(false);
            this.ButtonPanel.ResumeLayout(false);
            this.GridPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvLicence;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAllInstances;
        private System.Windows.Forms.TableLayoutPanel tbLayoutPanel;
        private System.Windows.Forms.Panel ButtonPanel;
        private System.Windows.Forms.Panel GridPanel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeInterval;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConcurrentLicences;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastRunStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastRunEnd;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Enabled;
        private System.Windows.Forms.DataGridViewLinkColumn Instances;
        private System.Windows.Forms.DataGridViewLinkColumn btnDelete;
    }
}