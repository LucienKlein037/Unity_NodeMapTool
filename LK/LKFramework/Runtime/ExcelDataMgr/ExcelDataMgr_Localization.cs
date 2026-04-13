using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace LucienKlein
{

    public partial class ExcelDataMgr 
    {
        static ExcelDataMgr()
        {
            if (!Directory.Exists(GENERATE_BINARYDATA_PATH))
            {

                Debug.LogWarning("警告:存在不应出现的情况:"+GENERATE_BINARYDATA_PATH);
                //Directory.CreateDirectory(savePath);
            }
        }
        public sbool isLoadInfoData = 0;
        public bool isLoadLocalization = false;



        //路径被本地化路径引用,修改需要同步修改,搜索 LoadLocalizationWebGL
        public static string FILE_PATH = ExcelDataMgr.GENERATE_BINARYDATA_PATH+"/Localization/Dic";

        //二进制数据表.  键:容器类名,值:容器类对象
        private Dictionary<string, object> BinaryDataTable = new Dictionary<string, object>();

        public Dictionary<string, Dictionary<string, string>> dicLocalizationTable;

        //----------------------------------------------------------------------------------------------------------------------



        // 加载Excel导入的数据结构类其中特定变量数据,通过容器类名获取容器,根据key行数据获取是哪一个数据结构对象,根据最终变量名获取数据结构的特定变量值
        public T GetExcelData<T>(Type containerClass, object dicDataKey, string fieldName)
        {
            if (!BinaryDataTable.ContainsKey(containerClass.Name))
            {
                throw new Exception("未找到表,请确认InitExcelBinaryData()中已执行LoadExcelData<数据类,数据容器类>");
            }
            //object obj= Activator.CreateInstance(containerClass);
            var dic = BinaryDataTable[containerClass.Name].GetType().GetField("dataDic").GetValue(BinaryDataTable[containerClass.Name]) as IDictionary;

            //key类型识别
            switch (dicDataKey)
            {
                case int:
                    dicDataKey = (int)dicDataKey;
                    break;
                case float:
                    dicDataKey = (float)dicDataKey;
                    break;
                case string:
                    dicDataKey = (string)dicDataKey;
                    break;
                case short:
                    dicDataKey = (short)dicDataKey;
                    break;

                default:
                    break;
            }
            return (T)dic[dicDataKey].GetType().GetField(fieldName).GetValue(dic[dicDataKey]);
        }
        // 加载Excel导入的数据结构类对象
        public T GetExcelData<T>(Type containerClass, object dicDataKey)
        {
            if (!BinaryDataTable.ContainsKey(containerClass.Name))
            {
                throw new Exception($"未找到{containerClass.Name},请确认InitExcelBinaryData()中已执行LoadExcelData<数据类,数据容器类>");
            }
            var dic = BinaryDataTable[containerClass.Name].GetType().GetField("dataDic").GetValue(BinaryDataTable[containerClass.Name]) as IDictionary;
            return (T)dic[dicDataKey];

        }

        // 加载Excel导入的数据结构类的容器类对象
        public T GetExcelData<T>() where T : class
        {
            if (!BinaryDataTable.ContainsKey(typeof(T).Name))
            {
                throw new Exception($"未找到{typeof(T).Name},请确认InitExcelBinaryData()中已执行LoadExcelData<数据类,数据容器类>");
            }
            return BinaryDataTable[typeof(T).Name] as T;

        }




        //----------------------------------------------------------------------------------------------------------------------


    //        // 加载数据
    //        public void LoadExcelInfoData<T, K>() where T : class where K : class
    //        {
    //            isLoadInfoData--;

    //#if UNITY_WEBGL && !UNITY_EDITOR
    //        // WebGL 必须异步
    //        MonoMgr.Instance.StartCoroutine(LoadFromWebGL<T, K>());
    //#else
    //            // 非 WebGL 走 FileStream
    //            string path = Path.Combine(Application.streamingAssetsPath, $"BinaryData/Info/{typeof(T).Name}.data");
    //            if (!File.Exists(path))
    //            {
    //                Debug.LogError("文件不存在: " + path);
    //                return;
    //            }
    //            byte[] bytes = File.ReadAllBytes(path);
    //            ParseExcelData<T, K>(bytes);
    //#endif
    //        }

    //#if UNITY_WEBGL && !UNITY_EDITOR
    //    private IEnumerator LoadFromWebGL<T, K>() where T : class where K : class
    //    {
    //        //string url = Path.Combine(Application.streamingAssetsPath, $"BinaryData/Info/{typeof(T).Name}.data");
    //        string url = Path.Combine(Application.streamingAssetsPath, "BinaryData","Info",$"{typeof(T).Name}.data");

    //        UnityWebRequest www = UnityWebRequest.Get(url);
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.LogError("WebGL 加载失败: " + www.error);
    //        }
    //        else
    //        {
    //            byte[] bytes = www.downloadHandler.data;
    //            ParseExcelData<T, K>(bytes);
    //        }
    //    }
    //#endif

        //// 核心的二进制解析逻辑（不变）
        //private void ParseExcelData<T, K>(byte[] bytes) where T : class where K : class
        //{
        //    Type dataClassType = typeof(T);
        //    Type dataContainerType = typeof(K);

        //    Debug.Log(dataClassType.Name + "----dataClassType.Name");

        //    K dataContainerObj = Activator.CreateInstance(dataContainerType) as K;

        //    int index = 0;
        //    int rowLength = BitConverter.ToInt32(bytes, index);
        //    index += 4;
        //    int columnLength = BitConverter.ToInt32(bytes, index);
        //    index += 4;
        //    int keyNameLength = BitConverter.ToInt32(bytes, index);
        //    index += 4;

        //    string keyName = Encoding.UTF8.GetString(bytes, index, keyNameLength);
        //    index += keyNameLength;

        //    FieldInfo[] fields = dataClassType.GetFields();

        //    for (int i = 4; i < rowLength; i++)
        //    {
        //        T dataClassObj = Activator.CreateInstance(dataClassType) as T;
        //        for (int j = 0; j < columnLength; j++)
        //        {
        //            if (fields[j].FieldType == typeof(int))
        //            {
        //                fields[j].SetValue(dataClassObj, BitConverter.ToInt32(bytes, index));
        //                index += 4;
        //            }
        //            else if (fields[j].FieldType == typeof(float))
        //            {
        //                fields[j].SetValue(dataClassObj, BitConverter.ToSingle(bytes, index));
        //                index += 4;
        //            }
        //            else if (fields[j].FieldType == typeof(bool))
        //            {
        //                fields[j].SetValue(dataClassObj, BitConverter.ToBoolean(bytes, index));
        //                index += 1;
        //            }
        //            else if (fields[j].FieldType == typeof(string))
        //            {
        //                int strLen = BitConverter.ToInt32(bytes, index);
        //                index += 4;
        //                fields[j].SetValue(dataClassObj, Encoding.UTF8.GetString(bytes, index, strLen));
        //                index += strLen;
        //            }
        //            else if (fields[j].FieldType == typeof(short))
        //            {
        //                fields[j].SetValue(dataClassObj, BitConverter.ToInt16(bytes, index));
        //                index += 2;
        //            }
        //        }

        //        FieldInfo dataDicField = dataContainerType.GetField("dataDic");
        //        MethodInfo mInfo = dataDicField.FieldType.GetMethod("TryAdd");
        //        mInfo.Invoke(dataDicField.GetValue(dataContainerObj),
        //        new object[] { dataClassType.GetField(keyName).GetValue(dataClassObj), dataClassObj });
        //    }

        //    BinaryDataTable.Add(dataContainerType.Name, dataContainerObj);
        //    //isLoadInfoData++;
        //}

        #region localization

        private void LoadLocalization()
        {
            using (FileStream fs = new FileStream(FILE_PATH, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(fs, Encoding.UTF8))
            {
                dicLocalizationTable = ParseLocalization(reader);
            }
        }
        private IEnumerator LoadLocalizationWebGL()
        {
            string url = Path.Combine(Application.streamingAssetsPath, "BinaryData", "Localization", "Dic");

            using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
                {
                    Debug.LogError("WebGL加载本地化失败: " + www.error);
                    yield break;
                }

                byte[] bytes = www.downloadHandler.data;
                using (MemoryStream ms = new MemoryStream(bytes))
                using (BinaryReader reader = new BinaryReader(ms, Encoding.UTF8))
                {
                    dicLocalizationTable = ParseLocalization(reader);
                }
            }
        }
        private Dictionary<string, Dictionary<string, string>> ParseLocalization(BinaryReader reader)
        {
            Dictionary<string, Dictionary<string, string>> data = new Dictionary<string, Dictionary<string, string>>();
            int totalCount = reader.ReadInt32();
            for (int i = 0; i < totalCount; i++)
            {
                string a = ReadString(reader);
                string b = ReadString(reader);
                string c = ReadString(reader);

                if (!data.ContainsKey(a))
                    data[a] = new Dictionary<string, string>();

                data[a][b] = c;
            }

            isLoadLocalization =true;

            return data;

            string ReadString(BinaryReader r)
            {
                int length = r.ReadInt32();
                byte[] bytes = r.ReadBytes(length);
                return Encoding.UTF8.GetString(bytes);
            }

        }


        #endregion


        public void InitLocalization()
        {
            //isLoadInfoData+=1;
            dicLocalizationTable = new Dictionary<string, Dictionary<string, string>>();

    #if UNITY_WEBGL && !UNITY_EDITOR
    MonoMgr.Instance.StartCoroutine(LoadLocalizationWebGL());
    #else
            LoadLocalization();
    #endif
        }



    }
}
