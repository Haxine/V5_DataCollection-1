using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using V5_Model;
using V5_DataCollection._Class.Common;
using System.IO;
using V5_DataCollection._Class.Gather;
using V5_DataCollection.Forms.Task.TaskData;
using V5_DataCollection.Forms.Task.Tools;
using V5_DataCollection._Class.DAL;
using V5_WinLibs.Core;
using V5_Utility.Utility;
using V5_WinLibs.DBUtility;
using V5_DataCollection._Class;
using V5_DataPlugins;
using System.Diagnostics;
namespace V5_DataCollection.Forms.Task
{

    public partial class FrmTaskMain : BaseContent {

        #region ��������
        Dictionary<string, SpiderHelper> listGatherTask = new Dictionary<string, SpiderHelper>();

        public MainEventHandler.OutPutWindowHandler OutPutWindowDelegate;

        public int ClassID { get; set; } = 0;
        #endregion

        #region ���������
        public FrmTaskMain() {
            InitializeComponent();
            this.dataGridView_TaskList.AutoGenerateColumns = false;
            this.dataGridView_TaskList.AllowUserToAddRows = false;
        }

        private void FrmTaskMain_Load(object sender, EventArgs e) {
            Bind_DataList();
            Bind_Plugin();
            tsmExportData.DropDownItems.Clear();
            foreach (IOutPutFormat t in PluginUtility.ListIOutputFormatPlugin)
            {
                var item = new ToolStripMenuItem(t.Format);               
                item.MouseEnter += (object s, EventArgs e1) => {
                    var id = Get_DataViewID();
                    var Model = new DALTask().GetModel(id);
                    if (Model.IsSaveLocal2 != null && Model.IsSaveLocal2.Value == 1)
                    {
                        item.Enabled = true;
                    }
                    else
                    {
                        item.Enabled = false;
                    }
                };
                item.Click += (object s, EventArgs e1) =>
                  {
                      var id = Get_DataViewID();
                      var Model = new DALTask().GetModel(id);
                      string LocalSQLiteName = "Data\\Collection\\" + Model.TaskName + "\\SpiderResult.db";
                      DataTable dtData = DbHelper.Query(LocalSQLiteName, "Select * From Content").Tables[0];
                      if (!Directory.Exists(Model.SaveDirectory2))
                          Directory.CreateDirectory(Model.SaveDirectory2);
                      if (File.Exists(LocalSQLiteName))
                      {
                          var r = t.RunSave(dtData, Model);
                          MainEvents.OutPutWindowEventArgs ev = new MainEvents.OutPutWindowEventArgs();
                          if (r.IsOk)
                          {
                              ev.Message = r.Message;
                              OutPutWindowDelegate?.Invoke(this, ev);
                          }
                          else
                          {
                              ev.Message = "������̳��ִ���" + r.Message;
                              OutPutWindowDelegate?.Invoke(this, ev);
                          } }
                  };
                tsmExportData.DropDownItems.Add(item);
            }
        }

        /// <summary>
        /// �������б�
        /// </summary>
        public void Bind_DataList() {
            Bind_DataList(string.Empty);
        }
        private void Bind_Plugin()
        {
            PluginUtility.LoadAllDlls();
        }
        public void Bind_DataList(string strWhere) {
            if (this.ClassID > 0) {
                strWhere += " And TaskClassID=" + this.ClassID + " ";
            }
            strWhere += " Order by Id Desc ";
            DALTask dal = new DALTask();
            DataTable dt = dal.GetList(strWhere).Tables[0];
            this.dataGridView_TaskList.DataSource = dt.DefaultView;

            for (int i = 0; i < this.dataGridView_TaskList.Rows.Count; i++) {
                var row = this.dataGridView_TaskList.Rows[i];
                row.Selected = false;
                if (i == m_RowIndex) {
                    row.Selected = true;
                }
            }
        }


        /// <summary>
        /// ���ݸ�ʽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_TaskList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (this.dataGridView_TaskList.Columns[e.ColumnIndex].Name.ToLower() == "status") {
                string s = e.Value.ToString();
                if (s == "1") {
                    e.Value = "����";
                }
                else {
                    e.Value = "��ͣ";
                }
            }

            if (this.dataGridView_TaskList.Columns[e.ColumnIndex].Name.ToLower() == "col_taskid") {
                string s = e.Value.ToString();
                if (s == "1") {
                    e.Value = "������";
                }
                else {
                    e.Value = "ֹͣ";
                }
            }
            if (this.dataGridView_TaskList.Columns[e.ColumnIndex].Name.ToLower() == "col_classid") {
                string s = e.Value.ToString();
                if (s == string.Empty) {
                    e.Value = "δ����";
                }
            }
        }

        private void dataGridView_TaskList_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            var viewIds = Get_DataViewIDs();

            var FormTaskEdit = new FrmTaskEdit();
            FormTaskEdit.TaskOpDelegate = OutTaskOpDelegate;
            FormTaskEdit.ID = int.Parse(viewIds[0]);
            FormTaskEdit.TaskIndex = this.dataGridView_TaskList.SelectedRows[0].Index;
            FormTaskEdit.ShowDialog(this);
        }

        int m_RowIndex = 0;
        private void dataGridView_TaskList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Left) {
                if (e.RowIndex >= 0) {
                    dataGridView_TaskList.ClearSelection();
                    dataGridView_TaskList.Rows[e.RowIndex].Selected = true;

                    m_RowIndex = e.RowIndex;
                }
            }
        }
        #endregion

        #region �½� �༭ ɾ��
        /// <summary>
        /// �½�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_TaskNew_Click(object sender, EventArgs e) {
            FrmTaskEdit FormTaskEdit = new FrmTaskEdit();
            FormTaskEdit.TaskOpDelegate = OutTaskOpDelegate;
            FormTaskEdit.ShowDialog(this);
        }
        /// <summary>
        /// �༭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_TaskEdit_Click(object sender, EventArgs e) {
            FrmTaskEdit FormTaskEdit = new FrmTaskEdit();
            FormTaskEdit.TaskOpDelegate = OutTaskOpDelegate;
            FormTaskEdit.ID = Get_DataViewID();
            FormTaskEdit.TaskIndex = this.dataGridView_TaskList.SelectedRows[0].Index;
            FormTaskEdit.ShowDialog(this);
        }
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_TaskDel_Click(object sender, EventArgs e) {
            if (MessageBox.Show("��ȷ��Ҫɾ����?ɾ�����ɻָ�!", "����!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
                DALTask dal = new DALTask();
                ModelTask model = dal.GetModel(Get_DataViewID());
                string TaskName = model.TaskName;
                string RootPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\Collection\\";
                string TaskPath = RootPath + TaskName;
                if (Directory.Exists(TaskPath)) {
                    Directory.Delete(TaskPath, true);
                }
                dal.Delete(Get_DataViewID());
                this.Bind_DataList();
            }
        }
        /// <summary>
        /// ��ȡ����Id
        /// </summary>
        /// <returns></returns>
        public int Get_DataViewID() {
            DataGridViewSelectedRowCollection row = this.dataGridView_TaskList.SelectedRows;
            if (row.Count > 0) {
                return StringHelper.Instance.SetNumber(row[0].Cells[0].Value.ToString());
            }
            return 0;
        }
        /// <summary>
        /// ��ȡ����ѡ�е�����Id
        /// </summary>
        /// <returns></returns>
        public string[] Get_DataViewIDs() {
            StringBuilder sb = new StringBuilder();
            DataGridViewSelectedRowCollection row = this.dataGridView_TaskList.SelectedRows;
            foreach (DataGridViewRow r in row) {
                sb.Append(r.Cells[0].Value.ToString() + ",");
            }
            if (sb.Length > 0) {
                sb = sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries); ;
        }
        /// <summary>
        /// ��� �༭ ���� ί��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OutTaskOpDelegate(object sender, TaskEvents.TaskOpEvents e) {
            if (e.OpType == 0 || e.OpType == 1) {
                Bind_DataList();
            }
            else {
                this.dataGridView_TaskList.Rows[0].Selected = false;
                this.dataGridView_TaskList.Rows[e.TaskIndex].Selected = true;
            }
        }
        #endregion

        #region  ����ʼ ��ͣ ����
        /// <summary>
        /// ����ʼ
        /// </summary>
        private void ToolStripMenuItem_TaskStart_Click(object sender, EventArgs e) {
            try {
                DALTask dal = new DALTask();
                ModelTask model = new ModelTask();
                int ID = Get_DataViewID();

                if (listGatherTask.ContainsKey(ID.ToString())) {
                    var Spider = listGatherTask.FirstOrDefault().Value;
                    if (Spider.Stopped) {
                        Spider.Start();
                    }
                }
                else {
                    model = dal.GetModelSingleTask(ID);
                    if (model.Status != 1) {
                        MainEvents.OutPutWindowEventArgs ev = new MainEvents.OutPutWindowEventArgs();
                        ev.Message = "����ر���.���ɲɼ�...";
                        OutPutWindowDelegate?.Invoke(this, ev);
                        return;
                    }
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory + "Data\\Collection\\";
                    string SQLiteName = baseDir + model.TaskName + "\\SpiderResult.db";
                    if (!File.Exists(SQLiteName)) {
                        CreateDataFile(model.TaskName, ID);
                    }
                    var Spider = new SpiderHelper();
                    Spider.modelTask = model;
                    Spider.GatherWorkDelegate = OutUrl;
                    Spider.GatherComplateDelegate = GatherOverDelegate;
                    Spider.OutPutTaskProgressBarDelegate = OutPutTaskProgressBarDelegate;
                    Spider.TaskIndex = this.dataGridView_TaskList.SelectedRows[0].Index;
                    Spider.Start();
                    lock (listGatherTask) {
                        if (!listGatherTask.ContainsKey(ID.ToString())) {
                            listGatherTask.Add(ID.ToString(), Spider);
                        }
                    }
                }
            }
            catch (Exception ex) {
                Log4Helper.Write(LogLevel.Error, ex.StackTrace, ex);
            }

        }

        /// <summary>
        /// ���������ί����Ϣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutPutTaskProgressBarDelegate(object sender, MainEvents.OutPutTaskProgressBarEventArgs e) {
            int TaskIndex = e.TaskIndex;
            int ProgressNum, RecordNum;
            ProgressNum = e.ProgressNum;
            RecordNum = e.RecordNum;
            double fPerNum = double.Parse(ProgressNum.ToString()) / double.Parse(RecordNum.ToString());
            double perNumF = double.Parse(fPerNum.ToString("f2")) * 100;
            int perNum = Convert.ToInt32(perNumF);
            this.dataGridView_TaskList.Rows[TaskIndex].Cells["ProgressBar"].Value = perNum;
        }


        /// <summary>
        /// ���������
        /// </summary>
        public void OutUrl(object sender, GatherEvents.GatherLinkEvents e) {
            MainEvents.OutPutWindowEventArgs ev = new MainEvents.OutPutWindowEventArgs();
            ev.Message = e.Message;
            OutPutWindowDelegate?.Invoke(this, ev);
        }
        /// <summary>
        /// ������ͣ
        /// </summary>
        private void ToolStripMenuItem_TaskPause_Click(object sender, EventArgs e) {
            MessageBox.Show("��ͣ������ʱ������!", "����!");
        }
        /// <summary>
        /// �������
        /// </summary>
        private void ToolStripMenuItem_TaskStop_Click(object sender, EventArgs e) {
            int ID = Get_DataViewID();
            if (listGatherTask.ContainsKey(ID.ToString())) {
                var Spider = listGatherTask.FirstOrDefault().Value;
                listGatherTask.Remove(ID.ToString());
                Spider.Stop();

                OutPutWindowDelegate?.Invoke(this, new MainEvents.OutPutWindowEventArgs() {
                    Message = "�����Ѿ�ֹͣ!",
                    TaskId = ID
                });
            }
        }
        /// <summary>
        /// �ɼ�����
        /// </summary>
        /// <param name="model"></param>
        private void GatherOverDelegate(ModelTask model) {
            if (model == null)
                return;
            if (listGatherTask.ContainsKey(model.ID.ToString())) {
                var Spider = listGatherTask.FirstOrDefault().Value;
                listGatherTask.Remove(model.ID.ToString());
                Spider.Start();
            }
        }
        #endregion

        #region ��� ���¹��� �鿴
        /// <summary>
        /// ���������������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_ClearTaskAllData_Click(object sender, EventArgs e) {
            if (MessageBox.Show("��ȷ��Ҫ���������?������ɻָ�!", "����!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
                ModelTask model = new ModelTask();
                int ID = Get_DataViewID();
                DALTask dal = new DALTask();
                model = dal.GetModelSingleTask(ID);
                string LocalSQLiteName = "Data\\Collection\\" + model.TaskName + "\\SpiderResult.db";
                string sql = " DELETE FROM Content";
                DbHelper.Execute(LocalSQLiteName, sql);
            }

        }

        /// <summary>
        /// ���½������ݽṹ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_ReCreateTable_Click(object sender, EventArgs e) {
            if (MessageBox.Show("��ȷ��Ҫ���½������ݽṹ��?������ɻָ�!", "����!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
                DataGridViewSelectedRowCollection row = this.dataGridView_TaskList.SelectedRows;
                string TaskName = "";
                if (row.Count > 0) {
                    TaskName = row[0].Cells[3].Value.ToString();
                }
                int ID = Get_DataViewID();
                CreateDataFile(TaskName, ID);
                MessageBox.Show("�ؽ��ɹ�");
            }
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="taskName"></param>
        private void CreateDataFile(string taskName, int taskID) {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory + "Data\\Collection\\";
            string SQLiteName = baseDir + taskName + "\\SpiderResult.db";
            string LocalSQLiteName = "Data\\Collection\\" + taskName + "\\SpiderResult.db";
            string SQL = string.Empty;
            if (!Directory.Exists(baseDir + taskName + "\\")) {
                Directory.CreateDirectory(baseDir + taskName + "\\");
            }
            if (!File.Exists(SQLiteName)) {
                DbHelper.CreateDataBase(SQLiteName);
                DALTask dal = new DALTask();
                string createSQL = string.Empty;
                DataTable dt = new DALTaskLabel().GetList(" TaskID=" + taskID).Tables[0];
                foreach (DataRow dr in dt.Rows) {
                    createSQL += @"
                     " + dr["LabelName"] + @" varchar,";
                }
                if (createSQL.Trim() != "") {
                    createSQL = createSQL.Remove(createSQL.Length - 1);
                }
                SQL = @"
                create table Content(
                    ID integer primary key autoincrement,
                    [HrefSource] varchar,
                    [�Ѳ�] int,
                    [�ѷ�] int,
                    " + createSQL + @"
                );
            ";
                DbHelper.Execute(LocalSQLiteName, SQL);
            }
            else {
                DataTable dt = new DALTaskLabel().GetList(" TaskID=" + taskID).Tables[0];
                //20190522 ����ԭ��ԭ��ɾ��������ֶ�ʱ���������ݿ���ԭ���в���ɾ��������
                var columns = "ID,[HrefSource],[�Ѳ�],[�ѷ�],";
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        //20190522 ����ԭ��ԭ��ɾ��������ֶ�ʱ���������ݿ���ԭ���в���ɾ��������
                        columns += "[" + dr["LabelName"] + "],";
                        DbHelper.Execute(LocalSQLiteName, " ALTER TABLE Content ADD COLUMN [" + dr["LabelName"] + "] VARCHAR; ");

                    }
                    catch
                    {
                        continue;
                    }
                }
                //20190522 ����ԭ��ԭ��ɾ��������ֶ�ʱ���������ݿ���ԭ���в���ɾ��������
                columns = columns.TrimEnd(',');
                if (!string.IsNullOrEmpty(columns))
                {
                    DbHelper.Execute(LocalSQLiteName, $"create table tempContent as select {columns} from Content ");
                    DbHelper.Execute(LocalSQLiteName, $"drop table if exists Content ");
                    DbHelper.Execute(LocalSQLiteName, $"alter table tempContent rename to Content ");
                }

            }
        }

        private void ToolStripMenuItem_ViewTaskData_Click(object sender, EventArgs e) {
            ModelTask model = new ModelTask();
            int ID = Get_DataViewID();
            DALTask dal = new DALTask();
            model = dal.GetModelSingleTask(ID);

            frmTaskDataList FromTaskDataList = new frmTaskDataList();
            FromTaskDataList.TaskName = model.TaskName;
            FromTaskDataList.ShowDialog(this);
        }

        private void ��������ToolStripMenuItem_Click(object sender, EventArgs e) {
            int ID = Get_DataViewID();
            DALTask dal = new DALTask();

            int currentMaxId = dal.GetMaxId();
            ModelTask model = dal.GetModelSingleTask(ID);
            string currentTaskName = model.TaskName + "����" + DateTime.Now.Millisecond;


            model.ID = currentMaxId;
            model.TaskName = currentTaskName;
            dal.Add(model);
            DALTaskLabel dalLable = new DALTaskLabel();
            DataTable dt = dalLable.GetList(" TaskId=" + ID).Tables[0];
            if (dt != null && dt.Rows.Count > 0) {
                foreach (DataRow dr in dt.Rows) {
                    ModelTaskLabel modelLabel = dalLable.GetModel(StringHelper.Instance.SetNumber(dr["Id"].ToString()));
                    modelLabel.TaskID = currentMaxId;
                    dalLable.Add(modelLabel);
                }
            }
            CreateDataFile(currentTaskName, currentMaxId);
            this.ClassID = model.TaskClassID.Value;
            Bind_DataList();
        }
        #endregion

        #region �ƻ�����

        private void �ƻ�����ToolStripMenuItem_Click(object sender, EventArgs e) {
            int ID = Get_DataViewID();
            var f = new frmTaskPlanSet();
            f.ShowDialog(this);
        }

        private void �����ɼ�����ToolStripMenuItem_Click(object sender, EventArgs e) {
            var id = this.Get_DataViewID();
            if (id > 0) {
                var dal = new DALTask();
                var model = dal.GetModelSingleTask(id);
                var saveDialog = new SaveFileDialog();
                saveDialog.Filter = "(*.xml)|*.xml";
                saveDialog.FileName = model.TaskName+DateTime.Now.ToString("yyyy-MM-dd");
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    if (SerializeHelper.SerializeObject<ModelTask>(model, saveDialog.FileName))
                    {
                        MessageBox.Show("�����ɹ�");
                    }
                }


            }
        }
        #endregion

        private void tsmRefreshTaskList_Click(object sender, EventArgs e)
        {
            Bind_DataList();
        }

        private void tsmOpenTaskDir_Click(object sender, EventArgs e)
        {
            ModelTask model = new ModelTask();
            int ID = Get_DataViewID();
            DALTask dal = new DALTask();
            model = dal.GetModelSingleTask(ID);
           // if (!string.IsNullOrEmpty(model.TaskName))
          //  {
                Process p = new Process();
                p.StartInfo.FileName = "explorer.exe";
                p.StartInfo.Arguments = AppDomain.CurrentDomain.BaseDirectory + "Data\\Collection\\" + model.TaskName;
                p.Start();
              
           // }
        }

        private void tsmTaskOpenResourceDir_Click(object sender, EventArgs e)
        {
            ModelTask model = new ModelTask();
            int ID = Get_DataViewID();
            DALTask dal = new DALTask();
            model = dal.GetModelSingleTask(ID);
            if (!string.IsNullOrEmpty(model.ResourceSavePath))
            {
                Process p = new Process();
                p.StartInfo.FileName = "explorer.exe";
                p.StartInfo.Arguments = model.ResourceSavePath; 
                p.Start();
            }
            else
            {
                Process p = new Process();
                p.StartInfo.FileName = "explorer.exe";
                p.StartInfo.Arguments =  AppDomain.CurrentDomain.BaseDirectory + "Data\\Collection\\" + model.TaskName + "\\Images\\";
                p.Start();
            }
        }

        private void tsmExportData_MouseEnter(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in tsmExportData.DropDownItems)
            {
                item.Enabled = true;
            }
        }
    }
}
