using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using V5_Model;
using V5_DataCollection._Class.Common;
using V5_WinLibs.DBUtility;

namespace V5_DataCollection._Class.DAL {
    public class DALTaskLabel {

        #region  Method
        public int GetMaxID() {
            object obj = DbHelper.ExecuteScalar(CommonHelper.SQLiteConnectionString, "select max(id)+1 from S_TaskLabel");
            if (obj == null) {
                return 1;
            }
            else {
                return int.Parse(obj.ToString());
            }
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(ModelTaskLabel model) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [S_TaskLabel] (");
            strSql.Append("LabelName,IsLoop,TestViewUrl,LabelNameCutRegex,LabelHtmlRemove,LabelRemove,LabelReplace,TaskID,GuidNum,OrderID,CreateTime,SpiderLabelPlugin,IsDownResource,DownResourceExts,IsSkipIfValueIsEmpty)");
            strSql.Append(" values (");
            strSql.Append("@LabelName,@IsLoop,@TestViewUrl,@LabelNameCutRegex,@LabelHtmlRemove,@LabelRemove,@LabelReplace,@TaskID,@GuidNum,@OrderID,@CreateTime,@SpiderLabelPlugin,@IsDownResource,@DownResourceExts,@IsSkipIfValueIsEmpty)");
            object obj = DbHelper.Execute(CommonHelper.SQLiteConnectionString, strSql.ToString(), model);
            if (obj == null) {
                return 0;
            }
            else {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public bool Update(ModelTaskLabel model) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [S_TaskLabel] set ");
            strSql.Append("LabelName=@LabelName,");
            strSql.Append("IsLoop=@IsLoop,");
            strSql.Append("TestViewUrl=@TestViewUrl,");
            strSql.Append("LabelNameCutRegex=@LabelNameCutRegex,");
            strSql.Append("LabelHtmlRemove=@LabelHtmlRemove,");
            strSql.Append("LabelRemove=@LabelRemove,");
            strSql.Append("LabelReplace=@LabelReplace,");
            strSql.Append("TaskID=@TaskID,");
            strSql.Append("GuidNum=@GuidNum,");
            strSql.Append("OrderID=@OrderID,");
            strSql.Append("CreateTime=@CreateTime,");
            strSql.Append("SpiderLabelPlugin=@SpiderLabelPlugin,");
            strSql.Append("IsDownResource=@IsDownResource,");
            strSql.Append("DownResourceExts=@DownResourceExts,");
            strSql.Append("IsSkipIfValueIsEmpty=@IsSkipIfValueIsEmpty");
            strSql.Append(" where ID=@ID ");
            int rowsAffected = DbHelper.Execute(CommonHelper.SQLiteConnectionString, strSql.ToString(), model);
            if (rowsAffected > 0) {
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public bool Delete(int ID) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from S_TaskLabel ");
            strSql.Append(" where ID=" + ID + "");
            int rowsAffected = DbHelper.Execute(CommonHelper.SQLiteConnectionString, strSql.ToString());
            if (rowsAffected > 0) {
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public bool DeleteList(string IDlist) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from S_TaskLabel ");
            strSql.Append(" where ID in (" + IDlist + ")  ");
            int rows = DbHelper.Execute(CommonHelper.SQLiteConnectionString, strSql.ToString());
            if (rows > 0) {
                return true;
            }
            else {
                return false;
            }
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public ModelTaskLabel GetModel(int ID) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  ");
            strSql.Append(" * ");
            strSql.Append(" from S_TaskLabel ");
            strSql.Append(" where ID=" + ID + "");
            return DbHelper.Query<ModelTaskLabel>(CommonHelper.SQLiteConnectionString, strSql.ToString()).SingleOrDefault();
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(string strWhere) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM S_TaskLabel ");
            if (strWhere.Trim() != "") {
                strSql.Append(" where " + strWhere);
            }
            return DbHelper.Query(CommonHelper.SQLiteConnectionString, strSql.ToString());
        }

        #endregion  Method

        public void UpdateTaskLabelByTaskID(int TaskID) {
            DbHelper.Execute(CommonHelper.SQLiteConnectionString, "Update S_TaskLabel Set TaskID=" + TaskID + " Where TaskID=0 ");
        }

        public ModelTaskLabel GetModel(string LabelName, int TaskID) {
            DataTable dt = this.GetList(" TaskID=" + TaskID + " And LabelName='" + LabelName + "' ").Tables[0];
            int EditID = int.Parse("0" + dt.Rows[0]["ID"].ToString());
            return this.GetModel(EditID);
        }

        public List<ModelTaskLabel> GetModelByTaskID(int TaskID) {
            List<ModelTaskLabel> list = new List<ModelTaskLabel>();
            DataTable dt = this.GetList(" TaskID=" + TaskID + " Order by OrderID asc").Tables[0];
            foreach (DataRow dr in dt.Rows) {
                ModelTaskLabel model = this.GetModel(int.Parse("0" + dr["ID"]));
                list.Add(model);
            }
            return list;
        }

        public bool Delete(string LabelName, int TaskID) {
            DataTable dt = this.GetList(" TaskID=" + TaskID + " And LabelName='" + LabelName + "' ").Tables[0];
            int EditID = int.Parse("0" + dt.Rows[0]["ID"].ToString());
            return this.Delete(EditID);
        }
        /// <summary>
        /// ��ȡ��������ID
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public int GetMaxOrderID(int taskID) {
            int orderID = int.Parse("0" + DbHelper.ExecuteScalar(CommonHelper.SQLiteConnectionString, "Select max(OrderID) From S_TaskLabel Where TaskID=" + taskID));
            return orderID;
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="orderType">1Ϊ���� -1����</param>
        /// <returns></returns>
        public bool UpdateOrder(int TaskID, int ID, int orderType) {
            int OrderID = 0, tempID = 0;
            string sql = string.Empty;
            if (orderType == -1) {
                sql = string.Format(@"Select Max(OrderID),ID From S_TaskLabel Where orderid <(select orderid from S_TaskLabel where id={0}) And TaskID={1} Group By ID ", ID, TaskID);
                DataTable dt = DbHelper.Query(CommonHelper.SQLiteConnectionString, sql).Tables[0];
                if (dt != null && dt.Rows.Count > 0) {
                    OrderID = int.Parse("0" + dt.Rows[0][0]);
                    tempID = int.Parse("0" + dt.Rows[0][1]);
                    if (tempID != 0) {
                        sql = "Update S_TaskLabel Set OrderID=" + OrderID + " Where ID=" + ID;
                        DbHelper.Execute(CommonHelper.SQLiteConnectionString, sql);
                        sql = "Update S_TaskLabel Set OrderID=OrderID+1 Where ID=" + tempID;
                        DbHelper.Execute(CommonHelper.SQLiteConnectionString, sql);
                    }
                }
            }
            else {
                sql = string.Format("Select Min(OrderID),ID From S_TaskLabel Where orderid >(select orderid from S_TaskLabel where id={0}) And TaskID={1} Group By ID ", ID, TaskID);
                DataTable dt = DbHelper.Query(CommonHelper.SQLiteConnectionString, sql).Tables[0];
                if (dt != null && dt.Rows.Count > 0) {
                    OrderID = int.Parse("0" + dt.Rows[0][0]);
                    tempID = int.Parse("0" + dt.Rows[0][1]);
                    if (tempID != 0) {
                        sql = "Update S_TaskLabel Set OrderID=" + OrderID + " Where ID=" + ID;
                        DbHelper.Execute(CommonHelper.SQLiteConnectionString, sql);
                        sql = "Update S_TaskLabel Set OrderID=OrderID-1 Where ID=" + tempID;
                        DbHelper.Execute(CommonHelper.SQLiteConnectionString, sql);
                    }
                }
            }
            return true;
        }


        public void TaskLabelCopy(int ID) {
            string sql = string.Empty;
            int maxID = this.GetMaxID();
            sql = string.Format(@"
                    INSERT INTO s_tasklabel 
                    SELECT 
                      {0} , 
                      labelname,  
                      labelnamecutregex,  
                      labelhtmlremove,  
                      labelremove,  
                      labelreplace,  
                      taskid,  
                      guidnum,  
                      orderid,  
                      createtime,  
                      isloop,  
                      isnonull,  
                      islinkurl,  
                      ispager,  
                      labelvaluelinkurlregex,  
                      labelvaluepagerregex,                      
IsSkipIfValueIsEmpty
                    FROM 
                      s_tasklabel where id={1};
            ", maxID, ID);
            DbHelper.Execute(CommonHelper.SQLiteConnectionString, sql);
        }
    }
}
