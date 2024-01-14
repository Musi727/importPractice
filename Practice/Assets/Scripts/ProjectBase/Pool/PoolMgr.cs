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

    //����������ѹ����
    public void PushObj(GameObject obj)
    {
        //�洢
        poolList.Add(obj);
        //���ø�����
        obj.transform.parent = fatherObj.transform;
        //ʧ���������
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
    //������������¹�
    Dictionary<string, PoolData> Dic  = new Dictionary<string, PoolData>();
    public GameObject obj;
    public GameObject pool;

    //���¹����ó�
    //֪������һ������
    public void GetGameObject(string name,UnityAction<GameObject> callback)
    {
        ///����иó���
        if (Dic.ContainsKey(name) && Dic[name].poolList.Count != 0) 
        {
            //���ó���API ȡ��һ������
            callback(Dic[name].GetObj()); 
        }
        //���û��������ͳ���
        else
        {
            //ʹ���첽���ػ�ȡ
            ResMgr.Instance.LoadResourcesAsync<GameObject>(name, (obj) =>
            {
                GameObject o = GameObject.Instantiate(obj);
                //�첽��������Ժ����Ϊ
                //���س�����GameObject�����ָ���
                o.name = name;
                callback(o);
            });
        }
    }

    public void PushGameObject(string name, GameObject obj)
    {
        if (pool == null)
            pool = new GameObject("Pool");
        //���û�и����ͳ���
        if(!Dic.ContainsKey(name))
        {
            //�½������ͳ���
            Dic.Add(name , new PoolData(obj, pool));
        }
        //����и����ͳ���
        else
        {
            //���ó���API �洢
            Dic[name].PushObj(obj);
        }
    }

    /// <summary>
    /// ��ջ����
    /// </summary>
    public void Clear()
    {
        Dic.Clear();
        obj = null;
    }
}
