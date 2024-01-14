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

    //�������洢��Щ��壬�����ȡ
    public Dictionary<string , BasePanel> panelDic = new Dictionary<string , BasePanel>();

    protected Transform bot;
    protected Transform mid;
    protected Transform top;

    public UIMgr()
    {
        if (!GameObject.Find("Canvas"))
        {
            //��̬����CanvasԤ����
            ResMgr.Instance.LoadResourcesAsync<GameObject>("UI/Canvas", (obj) =>
            {
                GameObject canvas = GameObject.Instantiate(obj);
                canvas.name = "Canvas";
                //�첽������ɺ󣬽�Canvas����Ϊ���ز��Ƴ�
                GameObject.DontDestroyOnLoad(canvas);
                bot = canvas.transform.Find("bot");
                mid = canvas.transform.Find("mid");
                top = canvas.transform.Find("top");
            });
        }
    }

    //��ø�����Transform
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
    /// ����Զ������
    /// </summary>
    /// <param name="control">��Ҫ����Զ�������Ŀؼ�</param>
    /// <param name="E_Type">��������</param>
    /// <param name="callBack">�����ĺ���</param>
    public static  void AddCustomEventListener(UIBehaviour control, EventTriggerType E_Type, UnityAction<BaseEventData> callBack)
    {
        //�ڶ�Ӧ�ؼ��� ��� E_Type���͵ļ����������ĺ���Ϊcallback
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
    /// <typeparam name="T">�������</typeparam>
    /// <param name="panelName">�����</param>
    /// <param name="layer">�����ʾ����һ��</param>
    /// <param name="callback">����첽������ɺ󣬻ص�����</param>
    public void ShowPanel<T>(string panelName , E_UI_Layer layer, UnityAction<T> callback)where T :BasePanel
    {
        //���ֵ�����������
        //��廹û��ʾ
        string path = "UI/"+panelName;
        if (!panelDic.ContainsKey(panelName))
        {
            //��̬�������Ԥ����
            ResMgr.Instance.LoadResourcesAsync<GameObject>(path, (obj) =>
            {
                GameObject panelObj = GameObject.Instantiate(obj);
                panelObj.name = panelName;
                //���ø�����
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
                //��ӽű�
                T panel = panelObj.GetComponent<T>();

                //������첽���ر������� ��ô�� ����T��ֵ����ȥ��T => panel����������������
                if (callback != null)
                    callback(panel);

                //�Ѹ������ӵ��ֵ�������
                panelDic.Add(panelName,panel);

                panel.ShowMe();
            });
        }
        //�������Ѿ����ڣ��򷵻ظ����
        else
        {
            //�������
            callback(panelDic[panelName] as T);
        }
    }

    public void HidePanel<T>(string panelName, bool isFade = true)where T :BasePanel
    {
        //�������Ƿ��и����
        if (panelDic.ContainsKey(panelName))
        {
            //��ø����
            T panel = panelDic[panelName] as T;
            //���ø�����ϵ����ط���
            if (isFade)
            {
                panel.HideMe(() =>
                {
                    //�������
                    GameObject.Destroy(panelDic[panel.name].gameObject);
                    //���ֵ����Ƴ�
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                //�������
                GameObject.Destroy(panelDic[panel.name].gameObject);
                //���ֵ����Ƴ�
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
