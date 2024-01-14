using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

public class PoolData
{
    public GameObject fatherObj;
    public List<GameObject> poolList;

    public PoolData(GameObject obj, GameObject poolObj)
    {
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.SetParent(poolObj.transform);

        poolList = new List<GameObject>();
        obj.transform.parent = fatherObj.transform;
        PushObj(obj);
    }

    //往抽屉里面压东西
    public void PushObj(GameObject obj)
    {
        //存储
        poolList.Add(obj);
        //设置父对象
        obj.transform.parent = fatherObj.transform;
        //失活，让其隐藏
        obj.SetActive(false);
    }

    public GameObject GetObj()
    {
        GameObject obj = null;
        obj = poolList[poolList.Count-1];
        poolList.RemoveAt(poolList.Count-1);
        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }
}

public class PoolMgr : BaseManager<PoolMgr>
{
    //缓存池容器（衣柜）
    Dictionary<string, PoolData> Dic  = new Dictionary<string, PoolData>();
    public GameObject obj;
    public GameObject pool;

    //从衣柜中拿出
    //知道是哪一个抽屉
    public void GetGameObject(string name,UnityAction<GameObject> callback)
    {
        ///如果有该抽屉
        if (Dic.ContainsKey(name) && Dic[name].poolList.Count != 0) 
        {
            //调用抽屉API 取出一个物体
            callback(Dic[name].GetObj()); 
        }
        //如果没有这个类型抽屉
        else
        {
            //使用异步加载获取
            ResMgr.Instance.LoadResourcesAsync<GameObject>(name, (obj) =>
            {
                GameObject o = GameObject.Instantiate(obj);
                //异步加载完成以后的行为
                //加载出来的GameObject的名字更改
                o.name = name;
                callback(o);
            });
        }
    }

    public void PushGameObject(string name, GameObject obj)
    {
        if (pool == null)
            pool = new GameObject("Pool");
        //如果没有该类型抽屉
        if(!Dic.ContainsKey(name))
        {
            //新建该类型抽屉
            Dic.Add(name , new PoolData(obj, pool));
        }
        //如果有该类型抽屉
        else
        {
            //调用抽屉API 存储
            Dic[name].PushObj(obj);
        }
    }

    /// <summary>
    /// 清空缓存池
    /// </summary>
    public void Clear()
    {
        Dic.Clear();
        obj = null;
    }
}
