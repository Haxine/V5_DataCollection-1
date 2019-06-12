using System;
using System.Text.RegularExpressions;
using System.Threading;
using V5_DataCollection._Class.Common;
using V5_Model;
using V5_Utility.Utility;
using V5_WinLibs.Core;

namespace V5_DataCollection._Class.Gather
{

    public class SpiderListHelper {

        /// <summary>
        /// ����ģ��
        /// </summary>
        public ModelTask Model { get; set; } = new ModelTask();

        public delegate void OutMessage(string msg);
        /// <summary>
        /// �������
        /// </summary>
        public event OutMessage OutMessageHandler;

        public delegate void OutTreeNode(string url, string title, string cover, int nodeIndex);
        /// <summary>
        /// �ɼ������
        /// </summary>
        public event OutTreeNode OutTreeNodeHandler;

        #region ��ַ
        /// <summary>
        /// �����б�����
        /// </summary>
        /// <param name="Url"></param>
        public void AnalyzeSingleList(string Url) {
            if (Model.IsSource == 0) {
                var listUrl = cGatherFunction.Instance.SplitWebUrl(Url);
                for (int i = 0; i < listUrl.Count; i++) {
                    ResolveList(listUrl[i], i);
                }
            }
            else if (Model.IsSource == 1) {
                ResolveList(Model.DemoListUrl, 0);
            }
        }
        /// <summary>
        /// �����б�����
        /// </summary>
        /// <param name="testUrl"></param>
        /// <param name="num"></param>
        public void ResolveList(string testUrl, int num) {
            string pageContent = string.Empty;
            if (Model.IsSource == 1) {
                pageContent = Model.SourceText;
            }
            else {
                pageContent = CommonHelper.getPageContent(testUrl, Model.PageEncode);
                if (string.IsNullOrEmpty(pageContent)) {
                    OutMessageHandler?.Invoke("�ɼ��б�ʧ��!");
                    return;
                }
            }

            if (Model.LinkUrlCutAreaStart?.Trim() != "" && Model.LinkUrlCutAreaEnd?.Trim() != "") {
                pageContent = HtmlHelper.Instance.ParseCollectionStrings(pageContent);
                pageContent = CollectionHelper.Instance.GetBody(pageContent,
                    HtmlHelper.Instance.ParseCollectionStrings(Model.LinkUrlCutAreaStart),
                    HtmlHelper.Instance.ParseCollectionStrings(Model.LinkUrlCutAreaEnd),
                    false,
                    false);
                pageContent = HtmlHelper.Instance.UnParseCollectionStrings(pageContent);
            }

            string regexHref = cRegexHelper.RegexATag;
           // int i = 0;
            if (Model.IsHandGetUrl == 1) {
                regexHref = Model.HandCollectionUrlRegex;
                regexHref = HtmlHelper.Instance.ParseCollectionStrings(regexHref);
                regexHref = HtmlHelper.Instance.UnParseCollectionStrings(regexHref);
                regexHref = regexHref.Replace("[", "(?<");
                regexHref = regexHref.Replace("]", ">.*?)");
                regexHref = regexHref.Replace("(*)", ".*?");
                
                // regexHref = regexHref.Replace(" ", "\\s+");
                //��ʽ��
                //20190523������������⣬��ע�͵�
                // regexHref = HtmlHelper.Instance.ParseCollectionStrings(regexHref);

            }

            Match mch = null;
            Regex reg = new Regex(regexHref, RegexOptions.IgnoreCase);
            string url = string.Empty;
            string title = string.Empty;
            string strUrl = string.Empty;
            string cover = string.Empty;

            MatchCollection matches = reg.Matches(pageContent);
            for (mch = reg.Match(pageContent); mch.Success; mch = mch.NextMatch()) {
                if (mch.Groups["����"] != null)
                {
                    url = CollectionHelper.Instance.FormatUrl(testUrl, mch.Groups["����"].Value);
                }
                if (mch.Groups["����"] != null)
                {
                    title = mch.Groups["����"].Value;
                }
                if (mch.Groups["����"] != null)
                {
                    cover = CollectionHelper.Instance.FormatUrl(testUrl, mch.Groups["����"].Value);
                }
                if (Model.LinkUrlMustIncludeStr.Trim() != "") {
                    if (url.IndexOf(Model.LinkUrlMustIncludeStr) == -1) {
                        continue;
                    }
                }

                if (Model.LinkUrlNoMustIncludeStr.Trim() != "") {
                    bool isFlag = true;
                    foreach (string str in Model.LinkUrlNoMustIncludeStr.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)) {
                        if (url.IndexOf(str) > -1) {
                            isFlag = false;
                            break;
                        }
                    }
                    if (!isFlag) {
                        continue;
                    }
                }

                #region ���ز��
                //string SpiderLabelPlugin = Model.PluginSpiderUrl;
                //if (SpiderLabelPlugin != "��ʹ�ò��" && !string.IsNullOrEmpty(SpiderLabelPlugin)) {
                //    CutContent = PythonExtHelper.RunPython(SpiderLabelPlugin, url, title);
                //}
                #endregion

                OutTreeNodeHandler?.Invoke(url, title, cover, num);

                if (Model.IsSource == 0) {
                    int minTime = 10;
                    int maxTime = 100;
                    Thread.Sleep(new Random().Next(minTime, maxTime));
                }
            }
        }

        /// <summary>
        /// �����б�
        /// </summary>
        public void AnalyzeAllList() {
            OutMessageHandler?.Invoke("���ڷ����ɼ��б����!");

            if (Model.IsSource == 1) {
                AnalyzeSingleList(Model.DemoListUrl);
            }
            else {
                foreach (string linkUrl in Model.CollectionContent.Split(new string[] { "$$$$" }, StringSplitOptions.RemoveEmptyEntries)) {
                    try {
                        AnalyzeSingleList(linkUrl);
                    }
                    catch (Exception ex1) {
                        Log4Helper.Write(LogLevel.Error, ex1.StackTrace, ex1);
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// ����û�вɼ����б� �������
        /// </summary>
        public void LoadNoSpiderList() {

        }
        #endregion



    }


}
