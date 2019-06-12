using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using V5_DataCollection._Class.DAL;
using V5_DataCollection._Class.Gather;
using V5_DataCollection.Forms.Publish;
using V5_Model;
using V5_Utility.Utility;
using V5_WinLibs.Core;
using V5_WinLibs.DBUtility;

namespace V5_DataCollection._Class.Publish
{

    public class PublishContentHelper {

        string modulePath = AppDomain.CurrentDomain.BaseDirectory + "/Modules/";

        public ModelTask Model;

        private GatherEvents.GatherLinkEvents gatherEv = new GatherEvents.GatherLinkEvents();
        public GatherEventHandler.GatherWorkHandler PublishCompalteDelegate;

        public void Start() {
            if (Model.IsWebOnlinePublish1 != null && Model.IsWebOnlinePublish1.Value == 1) {
                gatherEv.Message = "��ʼ����Web����!";
                PublishCompalteDelegate?.Invoke(this, gatherEv);
                StartWeb();
            }

            if (Model.IsSaveLocal2 != null && Model.IsSaveLocal2.Value == 1) {
                gatherEv.Message = "��ʼ������������!";
                PublishCompalteDelegate?.Invoke(this, gatherEv);
                StartLocal();
            }

            if (Model.IsSaveDataBase3!=null&& Model.IsSaveDataBase3.Value == 1) {
                gatherEv.Message = "��ʼ���浽���ݿ�!";
                PublishCompalteDelegate?.Invoke(this, gatherEv);
                StartDataBase();
            }

            if (Model.IsSaveSQL4 != null && Model.IsSaveSQL4.Value == 1) {
                gatherEv.Message = "��ʼ�����Զ�����վ!";
                PublishCompalteDelegate?.Invoke(this, gatherEv);
                StartDiyWeb();
            }

            gatherEv.Message = "���ݷ����ɹ�!";
            PublishCompalteDelegate?.Invoke(this, gatherEv);

        }

        public void MessageOut(string msg) {
            gatherEv.Message = msg;
            PublishCompalteDelegate?.Invoke(this, gatherEv);
        }

        #region ����Web����
        private void StartWeb() {
            foreach (ModelWebPublishModule m in Model.ListModelWebPublishModule) {
                var mPublishModuleItem = GetModelXml(m.ModuleNameFile);
                if (m.IsCookiesLogin == 1) {
                    var LoginedCookies = m.CookiesValue;
                    var LoginPostData = mPublishModuleItem.ViewPostData;
                    string result = SimulationHelper.PostPage(m.SiteUrl + mPublishModuleItem.ViewUrl,
                        "",
                        m.SiteUrl + mPublishModuleItem.ViewRefUrl,
                        mPublishModuleItem.PageEncode,
                        ref LoginedCookies);
                }
            }
        }

        private cPublishModuleItem GetModelXml(string pathName) {
            cPublishModuleItem model = new cPublishModuleItem();
            XmlSerializer serializer = new XmlSerializer(typeof(cPublishModuleItem));
            try {
                string fileName = modulePath + pathName;
                using (FileStream fs = new FileStream(fileName, FileMode.Open)) {
                    model = (cPublishModuleItem)serializer.Deserialize(fs);
                    fs.Close();
                }
            }
            catch {

            }
            return model;
        }
        #endregion

        #region ������������
        private void StartLocal() {
            try {
                string str = string.Empty;
                string LocalSQLiteName = "Data\\Collection\\" + Model.TaskName + "\\SpiderResult.db";
                DataTable dtData = DbHelper.Query(LocalSQLiteName, "Select * From Content").Tables[0];
                if (!Directory.Exists(Model.SaveDirectory2))
                    Directory.CreateDirectory(Model.SaveDirectory2);
             var outPut= PluginUtility.ListIOutputFormatPlugin.Where(x => x.Format == Model.SaveFileFormat2);

                if (outPut != null)
                {
                    foreach (var item in outPut)
                    {
                       var r= item.RunSave(dtData,Model);
                        if (r.IsOk)
                        {
                            gatherEv.Message = "����ɹ�!" + r.Message;
                            PublishCompalteDelegate?.Invoke(this, gatherEv);
                        }
                        else
                        {
                            gatherEv.Message = "����!" + r.Message;
                            PublishCompalteDelegate?.Invoke(this, gatherEv);
                        }
                    }
                }               
            }
            catch (Exception ex) {
                Log4Helper.Write(LogLevel.Error, ex);
            }
        }

     
        #endregion

        #region ���浽���ݿ�
        private void StartDataBase() {
            string LocalSQLiteName = "Data\\Collection\\" + Model.TaskName + "\\SpiderResult.db";
            DataTable dtData = DbHelper.Query(LocalSQLiteName, "Select * From Content").Tables[0];

            int saveDateType = Model.SaveDataType3.Value;
            string connectionString = Model.SaveDataUrl3;
            string exeSQL = Model.SaveDataSQL3;
            string sql = string.Empty;

            DbHelperDapper.connectionString = Model.SaveDataUrl3;
            switch (saveDateType) {
                case 1://ACCESS
                    DbHelperDapper.dbType = DataBaseType.OleDb;
                    break;
                case 2://MSSQL
                    DbHelperDapper.dbType = DataBaseType.SqlServer;
                    break;
                case 3://SQLITE
                    DbHelperDapper.dbType = DataBaseType.SQLite;
                    break;
                case 4://MYSQL
                    DbHelperDapper.dbType = DataBaseType.MySql;
                    break;
                case 5://Oracle
                    DbHelperDapper.dbType = DataBaseType.Oracle;
                    break;
            }

            using (var conn = DbHelperDapper.GetDbConnection(DbHelperDapper.dbType, Model.SaveDataUrl3)) {
                if (conn == null || conn.State != ConnectionState.Open) {
                    MessageOut("���ݿ�����ʧ��!����Ҫ��������!");
                    return;
                }
            }

            foreach (DataRow dr in dtData.Rows) {
                try {
                    sql = exeSQL;
                    foreach (ModelTaskLabel mTaskLabel in Model.ListTaskLabel) {
                        if (string.IsNullOrEmpty(dr[mTaskLabel.LabelName].ToString())) {
                            break;
                        }
                        sql = sql.Replace("[" + mTaskLabel.LabelName + "]", dr[mTaskLabel.LabelName].ToString().Replace("'", "''").Replace("\\", "/"));
                    }
                    sql = sql.Replace("[Guid]", Guid.NewGuid().ToString());
                    sql = sql.Replace("[Url]", dr["HrefSource"].ToString());

                    DbHelperDapper.Execute(sql);
                    MessageOut(dr["HrefSource"].ToString() + "�����ɹ�!");
                }
                catch (Exception ex) {
                    Log4Helper.Write(LogLevel.Error, dr["HrefSource"].ToString() + ":�������ݿ�ʧ��!", ex);
                    MessageOut(dr["HrefSource"].ToString() + "����ʧ��!" + ex);
                    continue;
                }
            }
        }
        #endregion

        #region �����Զ�����վ
        private void StartDiyWeb() {
            string LocalSQLiteName = "Data\\Collection\\" + Model.TaskName + "\\SpiderResult.db";
            DataTable dtData = DbHelper.Query(LocalSQLiteName, "Select * From Content").Tables[0];

            var listDiyUrl = DALDiyWebUrlHelper.GetList(" And SelfId=" + Model.ID, "", 0);
            HttpHelper4 http = new HttpHelper4();
            int taskId = Model.ID;
            foreach (DataRow dr in dtData.Rows) {
                int resultId = int.Parse(dr["Id"].ToString());
                foreach (var m in listDiyUrl) {
                    try {

                        string getUrl = m.Url;
                        string postParams = m.UrlParams;
                        StringBuilder sbContent = new StringBuilder();
                        foreach (ModelTaskLabel mTaskLabel in Model.ListTaskLabel) {
                            string pageEncodeContent = dr[mTaskLabel.LabelName].ToString().Replace("'", "''");
                            //������Ҫ����ʵ�ʲ��Բ�֪��
                            getUrl = getUrl.Replace("[" + mTaskLabel.LabelName + "]", pageEncodeContent);
                            postParams = postParams.Replace("[" + mTaskLabel.LabelName + "]", pageEncodeContent);
                            sbContent.Append(pageEncodeContent);
                        }
                        string md5key = StringHelper.Instance.MD5(taskId.ToString() + resultId.ToString() + sbContent.ToString(), 32).ToLower();
                        //�жϸ�����¼���weburl�Ƿ񷢹�
                        if (!DALDataPublishLogHelper.ChkRecord(
                            Model.ID, resultId, md5key)) {
                            //��¼��־
                            DALDataPublishLogHelper.Insert(new ModelDataPublishLog() {
                                TaskId = taskId,
                                ResultId = resultId,
                                DesKey = md5key,
                                CreateTime = DateTime.Now.ToString()
                            });
                        }
                        else {
                            continue;
                        }

                        //��ʼ������վ
                        var result = http.GetHtml(new HttpItem() {
                            URL = getUrl,
                            Postdata = postParams,
                            ContentType = "application/x-www-form-urlencoded"
                        });
                        var html = result.Html;
                    }
                    catch (Exception ex) {
                        continue;
                    }
                }
            }
            if (PublishCompalteDelegate != null) {
                gatherEv.Message = "�������Զ���Web��վ���!";
                PublishCompalteDelegate(this, gatherEv);
            }
        }
        #endregion


    }

}
