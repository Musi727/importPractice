using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResMgr : BaseManager<ResMgr>
{

    /// <summary>
    /// ͬ��������Դ
    /// </summary>
    /// <param name="name"></param>
    public T LoadResource<T>(string name) where T : Object
    {
        T res = Resources.Load<T>(name);
        //�������Դ��GameObject����
        if (res is GameObject)
        {
            //�ڳ���ʵ������Դ�������ظ���Դ
            return GameObject.Instantiate(res);
        }
        else
            return res;
    }

    /// <summary>
    /// �ṩ���ⲿ���õķ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// ����ֵ��void ����T ��ԭ�� �� �첽���ص���Դ�������� �����꣬���Ҳ����һ֡���ܼ����꣬����з���ֵ������û�м������ʱ�޷�����
    public void LoadResourcesAsync<T>(string name,UnityAction<T> callback = null) where T: Object
    {
        //����Э��
        MonoMgr.Instance.StartCoroutine(RellyLoadResourcesAsync<T>(name,callback));
    }

    /// <summary>
    /// ���첽����  �ͻ� ��Э�̣���Ӧ���ֵ� 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    IEnumerator RellyLoadResourcesAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest rq = Resources.LoadAsync<T>(name);
        yield return rq;
        callback(rq.asset as T);
    }
}
