using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace V5_DataCollection._Class.Common {
    public class SerializeHelper {
        /// <summary>
        /// ��ȡδ������Ķ���
        /// </summary>
        private T DeserializeObject<T>() {
            T t = default(T);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            try {
                string apppath = AppDomain.CurrentDomain.BaseDirectory;
                string fileName = apppath + "Config\\MtBuf.config";
                FileStream fs = new FileStream(fileName, FileMode.Open);
                t = (T)serializer.Deserialize(fs);
                fs.Close();
            }
            catch {
            }
            return t;
        }
        /// <summary>
        /// ����δ������Ķ���
        /// </summary>
        private bool SerializeObject<T>(T ListBuf) {
            try {
                string apppath = AppDomain.CurrentDomain.BaseDirectory;
                string fileName = apppath + "Config\\MtBuf.config";
                if (!File.Exists(fileName)) {
                    File.Create(fileName).Close();
                }
                lock (this) {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    FileStream fs = new FileStream(fileName, FileMode.Create);
                    serializer.Serialize(fs, ListBuf);
                    fs.Close();
                }
                return true;
            }
            catch {
            }
            return false;
        }
        public static T DeserializeObject<T>(string fileName)
        {
            T t = default(T);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            try
            {                
                FileStream fs = new FileStream(fileName, FileMode.Open);
                t = (T)serializer.Deserialize(fs);
                fs.Close();
            }
            catch
            {
            }
            return t;
        }
        public static bool SerializeObject<T>(T t,string fileName)
        {
            try
            {
                
                if (!File.Exists(fileName))
                {
                    File.Create(fileName).Close();
                }
                
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    FileStream fs = new FileStream(fileName, FileMode.Create);
                    serializer.Serialize(fs, t);
                    fs.Close();
                
                return true;
            }
            catch
            {
            }
            return false;
        }
    }
}
