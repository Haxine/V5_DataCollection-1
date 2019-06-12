using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using V5_DataCollection._Class.Common;
using V5_DataCollection._Class.PythonExt;
using V5_Model;
using V5_WinLibs.Core;
using V5_WinLibs.DBUtility;
using System.Text.RegularExpressions;
namespace V5_DataCollection._Class.Gather {
    public class SpiderViewHelper {

        #region ��������
        /// <summary>
        /// ����ģ��
        /// </summary>
        public ModelTask Model { get; set; } = new ModelTask();

        public delegate void OutViewUrlContent(string content);
        public event OutViewUrlContent OutViewUrlContentHandler;
        #endregion

        #region ������ϸ��ַ
        /// <summary>
        /// �������б�ǩ
        /// </summary>
        /// <param name="Test_ViewUrl"></param>
        /// <param name="Test_LabelList"></param>
        public void TestAllLabel(string Test_ViewUrl, List<ModelTaskLabel> Test_LabelList) {
            try {
                string pageContent = CommonHelper.getPageContent(Test_ViewUrl, Model.PageEncode);
                if (pageContent.Equals("��������δ�����κ�����")) {
                    OutViewUrlContentHandler?.Invoke("�ɼ���ַ����ȷ!���߲ɼ�����ʧ��!");
                    return;
                }

                StringBuilder sbTest = new StringBuilder();
                string tempContent = pageContent;

                foreach (var itemLabel in Test_LabelList) {
                    var CutContent = string.Empty;
                    CutContent += "��" + itemLabel.LabelName + "���� ";
                    CutContent += GetLabelContent("����", 10, Model.TestViewUrl, itemLabel, pageContent, true);
                    sbTest.AppendLine(CutContent);
                }

                OutViewUrlContentHandler?.Invoke(sbTest.ToString());
            }
            catch (Exception ex) {
                OutViewUrlContentHandler?.Invoke("������ҳ�ɼ�ʧ��!" + ex.Message);
            }
        }
        /// <summary>
        /// ���Ե�����ǩ
        /// </summary>
        /// <param name="itemLabel"></param>
        /// <param name="pageContent"></param>
        /// <returns></returns>
        public string TestSingleLabel(ModelTaskLabel itemLabel, string pageContent) {
            var CutContent = string.Empty;
            CutContent += "��" + itemLabel.LabelName + "���� ";
            CutContent += GetLabelContent("����", 10, "", itemLabel, pageContent, true);
            return CutContent;
        }

        /// <summary>
        /// �ɼ�����
        /// </summary>
        /// <param name="viewUrl"></param>
        /// <param name="Test_LabelList"></param>
        public void SpiderContent(string viewUrl, List<ModelTaskLabel> Test_LabelList)
        {

            string SQL = string.Empty, cutContent = string.Empty;

            string pageContent = CommonHelper.getPageContent(viewUrl, Model.PageEncode);
            string title = CollectionHelper.Instance.CutStr(pageContent, "<title>([\\S\\s]*?)</title>")[0];

            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            StringBuilder strSql = new StringBuilder();
            StringBuilder sb3 = new StringBuilder();

            string tempContent = pageContent;
            bool isSkip = false;
            string labelName = "";
            foreach (ModelTaskLabel itemTaskLabel in Model.ListTaskLabel)
            {
                labelName = itemTaskLabel.LabelName;
                var CutContent = GetLabelContent(Model.TaskName, Model.CollectionContentStepTime.Value, Model.TestViewUrl, itemTaskLabel, pageContent, false);
                var nohtml = HtmlHelper.Instance.ParseTags(CutContent);
                if (string.IsNullOrEmpty(nohtml) && itemTaskLabel.IsSkipIfValueIsEmpty == 1)
                {
                    isSkip = true;
                    break;
                }
                if (!isSkip)
                {
                    #region �滻����
                    sb1.Append("" + itemTaskLabel.LabelName.Replace("'", "''") + ",");
                    sb2.Append("'" + CutContent.Replace("'", "''") + "',");
                    if (CutContent.Replace("'", "''").Length < 100)
                    {
                        sb3.Append(" " + itemTaskLabel.LabelName.Replace("'", "''") + "='" + CutContent.Replace("'", "''") + "' and");
                    }
                    #endregion
                }
            }

            if (!isSkip)
            {
                try
                {

                    //�Ƿ��вɼ��б� Ȼ���ڸ��»��߲���

                    #region �������ݿ�
                    string LocalSQLiteName = "Data\\Collection\\" + Model.TaskName + "\\SpiderResult.db";
                    string sql = " Select Count(1) From Content Where HrefSource='" + viewUrl + "' ";
                    object o = DbHelper.ExecuteScalar(LocalSQLiteName, sql);
                    if (Convert.ToInt32("0" + o) == 0)
                    {

                        strSql.Append("insert into Content(HrefSource,");
                        strSql.Append(sb1.ToString().Remove(sb1.Length - 1));
                        strSql.Append(")");
                        strSql.Append(" values ('" + viewUrl + "',");
                        strSql.Append(sb2.ToString().Remove(sb2.Length - 1));
                        strSql.Append(")");

                        DbHelper.Execute(LocalSQLiteName, strSql.ToString());
                    }
                    title = title.Replace('\\', ' ').Replace('/', ' ').Split(new char[] { '_' })[0].Split(new char[] { '-' })[0];
                    #endregion

                    #region ���²ɼ��б�Ϊ �Ѳɼ�

                    #endregion

                    OutViewUrlContentHandler?.Invoke(viewUrl + "=" + title);
                }
                catch (Exception ex)
                {
                    OutViewUrlContentHandler?.Invoke(viewUrl + "=" + title + "=" + ex.Message);
                }
            }
            else
            {
                OutViewUrlContentHandler?.Invoke(viewUrl + "=" + title+"��ǩ��"+labelName+"ֵΪ�գ�������");
            }
        }



        /// <summary>
        /// ��ȡ��ǩ����
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="collectionContentStepTime"></param>
        /// <param name="spiderViewUrl"></param>
        /// <param name="itemTaskLebel"></param>
        /// <param name="pageContent"></param>
        /// <param name="isTest"></param>
        /// <returns></returns>
        private string GetLabelContent(string taskName, int collectionContentStepTime, string spiderViewUrl, ModelTaskLabel itemTaskLebel, string pageContent, bool isTest = false) {

            var remoteViewUrl = itemTaskLebel.TestViewUrl;
            if (string.IsNullOrEmpty(itemTaskLebel.TestViewUrl)) {
                remoteViewUrl = spiderViewUrl;
            }

            string regContent = HtmlHelper.Instance.ParseCollectionStrings(itemTaskLebel.LabelNameCutRegex);
            regContent = CommonHelper.ReplaceSystemRegexTag(regContent);
            string CutContent = CollectionHelper.Instance.CutStr(pageContent, regContent)[0];

            #region ������Դ
            var imgTag = ImageDownHelper.GetImgTag(CutContent);
            if (itemTaskLebel.IsDownResource!=null&& itemTaskLebel.IsDownResource.Value == 1) {
                string[] imgExtArr = itemTaskLebel.DownResourceExts.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                var downImgPath = Model.ResourceSavePath;// itemTaskLebel.ResourceSavePath; //AppDomain.CurrentDomain.BaseDirectory + "Data\\Collection\\" + taskName + "\\Images\\";
                if (string.IsNullOrEmpty(downImgPath))
                {
                    downImgPath= AppDomain.CurrentDomain.BaseDirectory + "Data\\Collection\\" + taskName + "\\Images\\";
                }
                int ii = 1;
                foreach (var img in imgTag) {
                    var remoteImg = CollectionHelper.Instance.FormatUrl(remoteViewUrl, img);
                    var imgExt = remoteImg.Substring(remoteImg.LastIndexOf("."));
                    var newImg = DateTime.Now.ToString("yyyyMMddHHmmssffffff") + "_" + ii + imgExt;
                    if (string.IsNullOrEmpty(itemTaskLebel.DownResourceExts)||imgExtArr.Any(x=>x.ToLower()==imgExt.ToLower())) {

                        // if (imgExtArr.SingleOrDefault(x => x.ToLower() == imgExt.ToLower()) != imgExt.ToLower()) {
                        //    continue;
                        //}
                        CutContent = CutContent.Replace(img, downImgPath + newImg);

                        if (!isTest)
                        {
                            QueueImgHelper.AddImg(Model.ID, downImgPath + newImg, remoteImg, collectionContentStepTime);
                        }
                        else
                        {
                            OutViewUrlContentHandler?.Invoke($"����������Դ��ꡣ�{itemTaskLebel.DownResourceExts}������Դ��ꡣ�{imgExt}\r\n");
                        }
                    }
                
                    ii++;
                }
            }
            else {
                foreach (var img in imgTag) {
                    var remoteImg = CollectionHelper.Instance.FormatUrl(remoteViewUrl, img);
                    CutContent = CutContent.Replace(img, remoteImg);
                }
            }
            #endregion

            #region ���Ϊѭ��
            if (itemTaskLebel.IsLoop == 1) {
                string[] LabelString = CollectionHelper.Instance.CutStr(pageContent, regContent);
                foreach (string s in LabelString) {
                    CutContent += s + "$$$$";
                }
                int n = CutContent.LastIndexOf("$$$$");
                CutContent = CutContent.Remove(n, 4);
            }
            #endregion

            #region ����Html
            if (!string.IsNullOrEmpty(itemTaskLebel.LabelHtmlRemove)) {
                string[] arr = itemTaskLebel.LabelHtmlRemove.Split(new string[] { "||||" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in arr) {
                    if (str == "all") {
                        CutContent = CollectionHelper.Instance.NoHtml(CutContent);
                        break;
                    }
                    else if (str == "table") {
                        CutContent = CollectionHelper.Instance.ScriptHtml(CutContent, "table", 2);
                    }
                    else if (str == "font<span>") {
                        CutContent = CollectionHelper.Instance.ScriptHtml(CutContent, "font", 3);
                        CutContent = CollectionHelper.Instance.ScriptHtml(CutContent, "span", 3);
                    }
                    else if (str == "a") {
                        CutContent = CollectionHelper.Instance.ScriptHtml(CutContent, "a", 3);
                    }
                    else 
                    {
                        CutContent = CollectionHelper.Instance.ScriptHtml(CutContent, str, 2);
                    }
                    
                }
            }
            #endregion

            #region �ų��ַ�
            if (!string.IsNullOrEmpty(itemTaskLebel.LabelRemove)) {
                foreach (string str in itemTaskLebel.LabelRemove.Split(new string[] { "$$$$" }, StringSplitOptions.RemoveEmptyEntries)) {
                    string[] ListStr = str.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    if (ListStr[1] == "2") {
                        // CutContent = CollectionHelper.RemoveHtml(CutContent, ListStr[0]);
                        CutContent = CollectionHelper.Instance.ScriptHtml(CutContent, ListStr[0], 2);
                    }
                    else {
                        // CutContent = CutContent.Replace(ListStr[0], "");
                        CutContent = CollectionHelper.Instance.ScriptHtml(CutContent, ListStr[0], 3);
                    }
                }
            }
            #endregion

            #region �滻�ַ�
            if (!string.IsNullOrEmpty(itemTaskLebel.LabelReplace)) {
                foreach (string str in itemTaskLebel.LabelReplace.Split(new string[] { "$$$$" }, StringSplitOptions.RemoveEmptyEntries)) {
                    string[] ListStr = str.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    Regex reg = new Regex(ListStr[0], RegexOptions.IgnoreCase);
                    if (ListStr.Length == 1)
                    {
                        CutContent = reg.Replace(CutContent, "");// CutContent.Replace(ListStr[0], "");
                    }
                    else
                    {
                        CutContent = reg.Replace(CutContent, ListStr[1]);
                      //  CutContent = CutContent.Replace(ListStr[0], ListStr[1]);
                    }
                }
            }
            #endregion

            #region ���ز��
            string SpiderLabelPlugin = itemTaskLebel.SpiderLabelPlugin;
            if (SpiderLabelPlugin != "��ʹ�ò��" && !string.IsNullOrEmpty(SpiderLabelPlugin)) {
                CutContent = PythonExtHelper.RunPython(PluginUtility.SpiderContentPluginPath + SpiderLabelPlugin, new object[] { remoteViewUrl, CutContent });
            }
            #endregion

            return CutContent;
        }

        #endregion
    }
}
