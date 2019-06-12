using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace V5_DataCollection.Forms.Task
{
    public class TaskEventHandler
    {
        /// <summary>
        /// �������
        /// </summary>
        public delegate void AddLinkUrl(object sender, TaskEvents.AddLinkUrlEvents e);
        /// <summary>
        /// ���View����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void AddViewLabel(object sender, TaskEvents.AddViewLabelEvents e);
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void TaskOpHandler(object sender, TaskEvents.TaskOpEvents e);
    }
  
}
