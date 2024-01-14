using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public enum E_JsonType
{
    JsonUtility,
    LitJson
}

public class JsonMgr 
{
    private static JsonMgr instance = new JsonMgr();
    public static JsonMgr Instance => instance;

    private JsonMgr() { }

    public void SaveData(object data,string fileName,E_JsonType type = E_JsonType.LitJson) 
    {
        //确定存储路径
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        string JsonStr = JsonMapper.ToJson(data);
        File.WriteAllText(path, JsonStr);
    }

    public T LoadData<T>(string fileName, E_JsonType type = E_JsonType.LitJson) where T : new()
    {      
        //确定从哪个路径读取
        //默认文件中寻找
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        if (!File.Exists(path))
        {
            //读写文件夹中寻找
            path = Application.persistentDataPath+ "/" + fileName + ".json";
        }
        //都没有返回一个默认对象
        if(!File.Exists(path))
        {
            return new T();
        }

        //进行反序列化
        string jsonStr = File.ReadAllText(path);

        //返回对象
        return JsonMapper.ToObject<T>(jsonStr);
    }
}
