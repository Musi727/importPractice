using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResMgr : BaseManager<ResMgr>
{

    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <param name="name"></param>
    public T LoadResource<T>(string name) where T : Object
    {
        T res = Resources.Load<T>(name);
        //如果该资源是GameObject类型
        if (res is GameObject)
        {
            //在场景实例化资源，并返回该资源
            return GameObject.Instantiate(res);
        }
        else
            return res;
    }

    /// <summary>
    /// 提供给外部调用的方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// 返回值是void 不是T 的原因 ： 异步加载的资源不会立即 加载完，最快也得下一帧才能加载完，如果有返回值，则在没有加载完成时无法返回
    public void LoadResourcesAsync<T>(string name,UnityAction<T> callback = null) where T: Object
    {
        //开启协程
        MonoMgr.Instance.StartCoroutine(RellyLoadResourcesAsync<T>(name,callback));
    }

    /// <summary>
    /// 有异步加载  就会 有协程，对应出现的 
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
