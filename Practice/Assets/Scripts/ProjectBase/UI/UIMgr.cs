using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public enum E_UI_Layer
{
    bot,
    mid,
    top,
}


public class UIMgr : BaseManager<UIMgr>
{
    public Transform canvas;

    //容器来存储这些面板，方便获取
    public Dictionary<string , BasePanel> panelDic = new Dictionary<string , BasePanel>();

    protected Transform bot;
    protected Transform mid;
    protected Transform top;

    public UIMgr()
    {
        if (!GameObject.Find("Canvas"))
        {
            //动态加载Canvas预制体
            ResMgr.Instance.LoadResourcesAsync<GameObject>("UI/Canvas", (obj) =>
            {
                GameObject canvas = GameObject.Instantiate(obj);
                canvas.name = "Canvas";
                //异步加载完成后，将Canvas设置为加载不移除
                GameObject.DontDestroyOnLoad(canvas);
                bot = canvas.transform.Find("bot");
                mid = canvas.transform.Find("mid");
                top = canvas.transform.Find("top");
            });
        }
    }

    //获得父对象Transform
    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.mid:
                return mid;
            case E_UI_Layer.top:
                return top;
            case E_UI_Layer.bot:
                return bot;
        }
        return null;
    }

    /// <summary>
    /// 添加自定义监听
    /// </summary>
    /// <param name="control">想要添加自定义监听的控件</param>
    /// <param name="E_Type">监听类型</param>
    /// <param name="callBack">监听的函数</param>
    public static  void AddCustomEventListener(UIBehaviour control, EventTriggerType E_Type, UnityAction<BaseEventData> callBack)
    {
        //在对应控件里 添加 E_Type类型的监听，监听的函数为callback
        EventTrigger et = control.GetComponent<EventTrigger>();
        if (et == null)
        {
            et =control.AddComponent<EventTrigger>();   
        }
        EventTrigger.Entry en = new EventTrigger.Entry();
        en.eventID = E_Type;
        en.callback.AddListener(callBack);
        et.triggers.Add(en);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="layer">面板显示在哪一层</param>
    /// <param name="callback">面板异步加载完成后，回调监听</param>
    public void ShowPanel<T>(string panelName , E_UI_Layer layer, UnityAction<T> callback)where T :BasePanel
    {
        //在字典容器里搜索
        //面板还没显示
        string path = "UI/"+panelName;
        if (!panelDic.ContainsKey(panelName))
        {
            //动态加载面板预制体
            ResMgr.Instance.LoadResourcesAsync<GameObject>(path, (obj) =>
            {
                GameObject panelObj = GameObject.Instantiate(obj);
                panelObj.name = panelName;
                //设置父对象
                switch (layer)
                {
                    case E_UI_Layer.bot:
                        panelObj.transform.SetParent(bot, false);
                        break;
                    case E_UI_Layer.mid:
                        panelObj.transform.SetParent(mid, false);
                        break;
                    case E_UI_Layer.top:
                        panelObj.transform.SetParent(top, false);
                        break;
                }              
                //添加脚本
                T panel = panelObj.GetComponent<T>();

                //如果该异步加载被监听， 那么将 参数T赋值传回去，T => panel，供监听函数调用
                if (callback != null)
                    callback(panel);

                //把该面板添加到字典容器里
                panelDic.Add(panelName,panel);

                panel.ShowMe();
            });
        }
        //如果面板已经存在，则返回该面板
        else
        {
            //处理监听
            callback(panelDic[panelName] as T);
        }
    }

    public void HidePanel<T>(string panelName, bool isFade = true)where T :BasePanel
    {
        //容器里是否有该面板
        if (panelDic.ContainsKey(panelName))
        {
            //获得该面板
            T panel = panelDic[panelName] as T;
            //调用该面板上的隐藏方法
            if (isFade)
            {
                panel.HideMe(() =>
                {
                    //销毁面板
                    GameObject.Destroy(panelDic[panel.name].gameObject);
                    //从字典中移除
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                //销毁面板
                GameObject.Destroy(panelDic[panel.name].gameObject);
                //从字典中移除
                panelDic.Remove(panelName);
            }        
        }
    }

    public T GetPanel<T>() where T :BasePanel
    {
        string panelName =typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        return default(T);
    }
}
