using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using V5_DataCollection._Class.DAL;
using V5_DataCollection._Class.Publish;
using V5_Model;
using V5_WinLibs;
using V5_WinLibs.Core;

namespace V5_DataCollection._Class.Gather
{

    /// <summary>
    /// ��������
    /// </summary>
    public enum EnumTaskType {
        /// <summary>
        /// �б�
        /// </summary>
        List,
        /// <summary>
        /// ��ϸ
        /// </summary>
        View,
        /// <summary>
        /// ����
        /// </summary>
        Publish,
        /// <summary>
        /// ���
        /// </summary>
        Over
    }

    /// <summary>
    /// �ɼ�����
    /// </summary>
    public class SpiderHelper {

        #region ����������
        /// <summary>
        /// �Ƿ�ֹͣ
        /// </summary>
        public bool Stopped { get; set; } = true;
        /// <summary>
        /// ��������
        /// </summary>
        public int TaskIndex { get; set; }
        private ModelTask _modelTask = new ModelTask();
        /// <summary>
        /// ����ģ��
        /// </summary>
        public ModelTask modelTask {
            get { return _modelTask; }
            set { _modelTask = value; }
        }
        #endregion

        #region ί�б���
        /// <summary>
        /// �ɼ�������
        /// </summary>
        public GatherEventHandler.GatherWorkHandler GatherWorkDelegate;
        /// <summary>
        /// �ɼ����
        /// </summary>
        public GatherEventHandler.GatherComplateHandler GatherComplateDelegate;
        /// <summary>
        /// �ɼ�������
        /// </summary>
        public MainEventHandler.OutPutTaskProgressBarHandler OutPutTaskProgressBarDelegate;
        #endregion

        #region ˽�б���
        private GatherEvents.GatherLinkEvents gatherEv = new GatherEvents.GatherLinkEvents();
        private Queue<ModelLinkUrl> _listLinkUrl = new Queue<ModelLinkUrl>();
        private cGatherFunction _gatherWork = new cGatherFunction();

        #endregion

        private void MessageOut(string strMsg) {
            if (GatherWorkDelegate != null) {
                gatherEv.Message = strMsg;
                GatherWorkDelegate(this, gatherEv);
            }
        }

        public delegate void OutTaskStatus(EnumTaskType type);
        public event OutTaskStatus OutTaskStatusHandler;

        #region ���

        public void Stop() {
            this.Stopped = true;
        }

        public void Start() {

            if (!this.Stopped || modelTask == null) {
                return;
            }

            this.Stopped = false;

            OutTaskStatusHandler = (EnumTaskType type) => {
                switch (type) {
                    case EnumTaskType.List:
                        StartList();
                        break;
                    case EnumTaskType.View:
                        StartView();
                        break;
                    case EnumTaskType.Publish:
                        StartPublish();
                        break;
                    case EnumTaskType.Over:
                        StartOver();
                        break;
                }
            };

            OutTaskStatusHandler?.Invoke(EnumTaskType.List);
        }
        #endregion

        #region �б�
        private void StartList() {

            _listLinkUrl.Clear();

            MessageOut($"[{modelTask.TaskName}]��ʼ�ɼ����ݣ����Ժ�...");
           
            var task = new TaskFactory().StartNew(() => {
                //����Ϊ�ɼ����б�
                if (modelTask.IsSpiderUrl == 1) {
                    var spiderList = new SpiderListHelper();
                    spiderList.Model = modelTask;
                    spiderList.OutTreeNodeHandler += (string url, string title, string cover, int nodeIndex) => {
                        var m = new ModelLinkUrl() {
                            Url = url,
                            Title = title,
                            Cover = cover
                        };
                        bool addFlag = true;
                        foreach (var item in _listLinkUrl.ToArray()) {
                            if (item.Url == url) {
                                addFlag = false;
                                break;
                            }
                        }
                        if (addFlag) {
                            string msg = url + "==" + HtmlHelper.Instance.ParseTags(title);
                            if (!DALContentHelper.ChkExistSpiderResult(modelTask.TaskName, url)) {
                                _listLinkUrl.Enqueue(m);
                            }
                            else {
                                msg += "�ɼ���ַ����!����Ҫ�ɼ�!";
                            }
                            MessageOut(msg);
                        }
                    };
                    spiderList.OutMessageHandler += (string msg) => {
                        MessageOut(msg);
                    };
                    spiderList.AnalyzeAllList();

                    MessageOut("������ȡ��ҳ����Ϊ" + _listLinkUrl.Count + "����");
                    MessageOut("�ɼ���վ�б����!");
                }
                else {
                    MessageOut("�ɼ��б�ر�,����Ҫ�ɼ�!");
                }
                OutTaskStatusHandler?.Invoke(EnumTaskType.View);
            });

        }
        #endregion

        #region ��ϸ
        private void StartView() {
            var taskList = new TaskFactory().StartNew(() => {
                if (modelTask.IsSpiderContent == 1 && _listLinkUrl.Count > 0) {

                    MessageOut($"��ʼ�ɼ��б��ַ��ϸ����!�ɼ����{modelTask.CollectionContentStepTime}����");

                    var spiderViewHelper = new SpiderViewHelper();
                    spiderViewHelper.Model = modelTask;
                    spiderViewHelper.OutViewUrlContentHandler += (string content) => {
                        MessageOut(content);
                    };

                    var ProressNum = 0;
                    var TaskCount = _listLinkUrl.Count;

                    while (true) {
                        if (_listLinkUrl.Count == 0) {
                            break;
                        }
                        var mlink = _listLinkUrl.Dequeue();
                        try {
                            #region ������

                            ProressNum++;
                            MainEvents.OutPutTaskProgressBarEventArgs ea = new MainEvents.OutPutTaskProgressBarEventArgs();
                            ea.ProgressNum = ProressNum;
                            ea.RecordNum = TaskCount;
                            ea.TaskIndex = TaskIndex;
                            OutPutTaskProgressBarDelegate?.Invoke(this, ea);

                            #endregion
                            spiderViewHelper.SpiderContent(mlink.Url, modelTask.ListTaskLabel);
                            Thread.Sleep(modelTask.CollectionContentStepTime.Value);
                        }
                        catch (Exception ex) {
                            LoggerHelper.Write(V5_WinLibs.LogLevel.Error, ex);
                        }
                    }

                    MessageOut("�ɼ���վUrl������ɣ�");
                }
                else {
                    MessageOut("�ɼ���վ����ѡ��ر�!���߲ɼ��б�ĵ�ַΪ0!����Ҫ�ɼ�!");
                }

                OutTaskStatusHandler?.Invoke(EnumTaskType.Publish);
            });
        }
        #endregion

        #region ����
        private void StartPublish() {
            var taskView = new TaskFactory().StartNew(() => {

                if (modelTask.IsPublishContent!=null&&modelTask.IsPublishContent.Value == 1) {

                    var publich = new PublishContentHelper();
                    publich.Model = modelTask;
                    publich.PublishCompalteDelegate = GatherWorkDelegate;

                    MessageOut("���ڿ�ʼ��������!");
                    publich.PublishCompalteDelegate = (object sender, GatherEvents.GatherLinkEvents e) => {
                        MessageOut(e.Message);
                        GatherComplateDelegate?.Invoke(modelTask);
                        this.Stop();
                    };
                    publich.Start();
                }
                else {
                    GatherComplateDelegate(modelTask);
                    MessageOut("��������û�п���!����Ҫ��������!");
                }

                OutTaskStatusHandler?.Invoke(EnumTaskType.Over);
            });
        }
        #endregion

        #region ���
        private void StartOver() {
            MessageOut($"[{modelTask.TaskName}] �ɼ��������!");
            this.Stop();
        }
        #endregion
    }
}
