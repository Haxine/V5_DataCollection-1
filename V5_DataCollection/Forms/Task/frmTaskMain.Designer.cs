using V5_WinControls;
using V5_WinControls.DataGrid;

namespace V5_DataCollection.Forms.Task {
    partial class FrmTaskMain {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip_TaskList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_TaskNew = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_TaskEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_TaskDel = new System.Windows.Forms.ToolStripMenuItem();
            this.��������ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_TaskStart = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_TaskPause = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_TaskStop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_ClearTaskAllData = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_ReCreateTable = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_ViewTaskData = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmExportData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.�ƻ�����ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.�����ɼ�����ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmRefreshTaskList = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView_TaskList = new V5_WinControls.V5DataGridView(this.components);
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_ClassId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskProps = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_TaskId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProgressBar = new V5_WinControls.DataGrid.DataGridViewProgressBarColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsmOpenTaskDir = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmTaskOpenResourceDir = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip_TaskList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_TaskList)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip_TaskList
            // 
            this.contextMenuStrip_TaskList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_TaskNew,
            this.ToolStripMenuItem_TaskEdit,
            this.ToolStripMenuItem_TaskDel,
            this.��������ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.ToolStripMenuItem_TaskStart,
            this.ToolStripMenuItem_TaskPause,
            this.ToolStripMenuItem_TaskStop,
            this.toolStripMenuItem2,
            this.ToolStripMenuItem_ClearTaskAllData,
            this.ToolStripMenuItem_ReCreateTable,
            this.ToolStripMenuItem_ViewTaskData,
            this.tsmExportData,
            this.tsmOpenTaskDir,
            this.tsmTaskOpenResourceDir,
            this.toolStripMenuItem3,
            this.�ƻ�����ToolStripMenuItem,
            this.�����ɼ�����ToolStripMenuItem,
            this.tsmRefreshTaskList});
            this.contextMenuStrip_TaskList.Name = "contextMenuStrip_TaskList";
            this.contextMenuStrip_TaskList.Size = new System.Drawing.Size(185, 396);
            // 
            // ToolStripMenuItem_TaskNew
            // 
            this.ToolStripMenuItem_TaskNew.Name = "ToolStripMenuItem_TaskNew";
            this.ToolStripMenuItem_TaskNew.Size = new System.Drawing.Size(184, 22);
            this.ToolStripMenuItem_TaskNew.Text = "�½�����";
            this.ToolStripMenuItem_TaskNew.Click += new System.EventHandler(this.ToolStripMenuItem_TaskNew_Click);
            // 
            // ToolStripMenuItem_TaskEdit
            // 
            this.ToolStripMenuItem_TaskEdit.Name = "ToolStripMenuItem_TaskEdit";
            this.ToolStripMenuItem_TaskEdit.Size = new System.Drawing.Size(184, 22);
            this.ToolStripMenuItem_TaskEdit.Text = "�༭����";
            this.ToolStripMenuItem_TaskEdit.Click += new System.EventHandler(this.ToolStripMenuItem_TaskEdit_Click);
            // 
            // ToolStripMenuItem_TaskDel
            // 
            this.ToolStripMenuItem_TaskDel.Name = "ToolStripMenuItem_TaskDel";
            this.ToolStripMenuItem_TaskDel.Size = new System.Drawing.Size(184, 22);
            this.ToolStripMenuItem_TaskDel.Text = "ɾ������";
            this.ToolStripMenuItem_TaskDel.Click += new System.EventHandler(this.ToolStripMenuItem_TaskDel_Click);
            // 
            // ��������ToolStripMenuItem
            // 
            this.��������ToolStripMenuItem.Name = "��������ToolStripMenuItem";
            this.��������ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.��������ToolStripMenuItem.Text = "��������";
            this.��������ToolStripMenuItem.Click += new System.EventHandler(this.��������ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(181, 6);
            // 
            // ToolStripMenuItem_TaskStart
            // 
            this.ToolStripMenuItem_TaskStart.Name = "ToolStripMenuItem_TaskStart";
            this.ToolStripMenuItem_TaskStart.Size = new System.Drawing.Size(184, 22);
            this.ToolStripMenuItem_TaskStart.Text = "��ʼ����";
            this.ToolStripMenuItem_TaskStart.Click += new System.EventHandler(this.ToolStripMenuItem_TaskStart_Click);
            // 
            // ToolStripMenuItem_TaskPause
            // 
            this.ToolStripMenuItem_TaskPause.Name = "ToolStripMenuItem_TaskPause";
            this.ToolStripMenuItem_TaskPause.Size = new System.Drawing.Size(184, 22);
            this.ToolStripMenuItem_TaskPause.Text = "��ͣ����";
            this.ToolStripMenuItem_TaskPause.Click += new System.EventHandler(this.ToolStripMenuItem_TaskPause_Click);
            // 
            // ToolStripMenuItem_TaskStop
            // 
            this.ToolStripMenuItem_TaskStop.Name = "ToolStripMenuItem_TaskStop";
            this.ToolStripMenuItem_TaskStop.Size = new System.Drawing.Size(184, 22);
            this.ToolStripMenuItem_TaskStop.Text = "ֹͣ����";
            this.ToolStripMenuItem_TaskStop.Click += new System.EventHandler(this.ToolStripMenuItem_TaskStop_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(181, 6);
            // 
            // ToolStripMenuItem_ClearTaskAllData
            // 
            this.ToolStripMenuItem_ClearTaskAllData.Name = "ToolStripMenuItem_ClearTaskAllData";
            this.ToolStripMenuItem_ClearTaskAllData.Size = new System.Drawing.Size(184, 22);
            this.ToolStripMenuItem_ClearTaskAllData.Text = "�����������������";
            this.ToolStripMenuItem_ClearTaskAllData.Click += new System.EventHandler(this.ToolStripMenuItem_ClearTaskAllData_Click);
            // 
            // ToolStripMenuItem_ReCreateTable
            // 
            this.ToolStripMenuItem_ReCreateTable.Name = "ToolStripMenuItem_ReCreateTable";
            this.ToolStripMenuItem_ReCreateTable.Size = new System.Drawing.Size(184, 22);
            this.ToolStripMenuItem_ReCreateTable.Text = "���½������ݿ�ṹ";
            this.ToolStripMenuItem_ReCreateTable.Click += new System.EventHandler(this.ToolStripMenuItem_ReCreateTable_Click);
            // 
            // ToolStripMenuItem_ViewTaskData
            // 
            this.ToolStripMenuItem_ViewTaskData.Name = "ToolStripMenuItem_ViewTaskData";
            this.ToolStripMenuItem_ViewTaskData.Size = new System.Drawing.Size(184, 22);
            this.ToolStripMenuItem_ViewTaskData.Text = "�鿴����������";
            this.ToolStripMenuItem_ViewTaskData.Click += new System.EventHandler(this.ToolStripMenuItem_ViewTaskData_Click);
            // 
            // tsmExportData
            // 
            this.tsmExportData.Name = "tsmExportData";
            this.tsmExportData.Size = new System.Drawing.Size(184, 22);
            this.tsmExportData.Text = "��������";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(181, 6);
            // 
            // �ƻ�����ToolStripMenuItem
            // 
            this.�ƻ�����ToolStripMenuItem.Name = "�ƻ�����ToolStripMenuItem";
            this.�ƻ�����ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.�ƻ�����ToolStripMenuItem.Text = "�ƻ�����";
            this.�ƻ�����ToolStripMenuItem.Click += new System.EventHandler(this.�ƻ�����ToolStripMenuItem_Click);
            // 
            // �����ɼ�����ToolStripMenuItem
            // 
            this.�����ɼ�����ToolStripMenuItem.Name = "�����ɼ�����ToolStripMenuItem";
            this.�����ɼ�����ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.�����ɼ�����ToolStripMenuItem.Text = "�����ɼ�����";
            this.�����ɼ�����ToolStripMenuItem.Click += new System.EventHandler(this.�����ɼ�����ToolStripMenuItem_Click);
            // 
            // tsmRefreshTaskList
            // 
            this.tsmRefreshTaskList.Name = "tsmRefreshTaskList";
            this.tsmRefreshTaskList.Size = new System.Drawing.Size(184, 22);
            this.tsmRefreshTaskList.Text = "ˢ�������б�";
            this.tsmRefreshTaskList.Click += new System.EventHandler(this.tsmRefreshTaskList_Click);
            // 
            // dataGridView_TaskList
            // 
            this.dataGridView_TaskList.AllowUserToAddRows = false;
            this.dataGridView_TaskList.AllowUserToDeleteRows = false;
            this.dataGridView_TaskList.AllowUserToResizeRows = false;
            this.dataGridView_TaskList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_TaskList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_TaskList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Col_ClassId,
            this.Status,
            this.Column2,
            this.TaskProps,
            this.Col_TaskId,
            this.ProgressBar,
            this.Column1});
            this.dataGridView_TaskList.ContextMenuStrip = this.contextMenuStrip_TaskList;
            this.dataGridView_TaskList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dataGridView_TaskList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_TaskList.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_TaskList.Name = "dataGridView_TaskList";
            this.dataGridView_TaskList.ReadOnly = true;
            this.dataGridView_TaskList.RowHeadersVisible = false;
            this.dataGridView_TaskList.RowTemplate.Height = 23;
            this.dataGridView_TaskList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_TaskList.Size = new System.Drawing.Size(738, 465);
            this.dataGridView_TaskList.TabIndex = 1;
            this.dataGridView_TaskList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_TaskList_CellDoubleClick);
            this.dataGridView_TaskList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView_TaskList_CellFormatting);
            this.dataGridView_TaskList.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_TaskList_CellMouseDown);
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 50;
            // 
            // Col_ClassId
            // 
            this.Col_ClassId.DataPropertyName = "ClassName";
            this.Col_ClassId.HeaderText = "��������";
            this.Col_ClassId.Name = "Col_ClassId";
            this.Col_ClassId.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "Status";
            this.Status.HeaderText = "״̬";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "TaskName";
            this.Column2.HeaderText = "��������";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 300;
            // 
            // TaskProps
            // 
            this.TaskProps.HeaderText = "��������";
            this.TaskProps.Name = "TaskProps";
            this.TaskProps.ReadOnly = true;
            // 
            // Col_TaskId
            // 
            this.Col_TaskId.DataPropertyName = "IsPlan";
            this.Col_TaskId.HeaderText = "�ƻ�����";
            this.Col_TaskId.Name = "Col_TaskId";
            this.Col_TaskId.ReadOnly = true;
            // 
            // ProgressBar
            // 
            this.ProgressBar.HeaderText = "����";
            this.ProgressBar.Maximum = 100;
            this.ProgressBar.Mimimum = 0;
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.ReadOnly = true;
            this.ProgressBar.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "ID";
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "״̬";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "TaskName";
            this.dataGridViewTextBoxColumn3.HeaderText = "��������";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // tsmOpenTaskDir
            // 
            this.tsmOpenTaskDir.Name = "tsmOpenTaskDir";
            this.tsmOpenTaskDir.Size = new System.Drawing.Size(184, 22);
            this.tsmOpenTaskDir.Text = "������Ŀ¼";
            this.tsmOpenTaskDir.Click += new System.EventHandler(this.tsmOpenTaskDir_Click);
            // 
            // tsmTaskOpenResourceDir
            // 
            this.tsmTaskOpenResourceDir.Name = "tsmTaskOpenResourceDir";
            this.tsmTaskOpenResourceDir.Size = new System.Drawing.Size(184, 22);
            this.tsmTaskOpenResourceDir.Text = "��������ԴĿ¼";
            this.tsmTaskOpenResourceDir.Click += new System.EventHandler(this.tsmTaskOpenResourceDir_Click);
            // 
            // FrmTaskMain
            // 
            this.AllowEndUserDocking = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 465);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.dataGridView_TaskList);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Name = "FrmTaskMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "�ɼ�����";
            this.Load += new System.EventHandler(this.FrmTaskMain_Load);
            this.contextMenuStrip_TaskList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_TaskList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_TaskList;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_TaskNew;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_TaskEdit;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_TaskDel;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_TaskStart;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_TaskPause;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_TaskStop;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private V5DataGridView dataGridView_TaskList;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_ClearTaskAllData;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_ReCreateTable;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_ViewTaskData;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem �ƻ�����ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ��������ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem �����ɼ�����ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_ClassId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskProps;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_TaskId;
        private DataGridViewProgressBarColumn ProgressBar;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.ToolStripMenuItem tsmRefreshTaskList;
        private System.Windows.Forms.ToolStripMenuItem tsmExportData;
        private System.Windows.Forms.ToolStripMenuItem tsmOpenTaskDir;
        private System.Windows.Forms.ToolStripMenuItem tsmTaskOpenResourceDir;
    }
}