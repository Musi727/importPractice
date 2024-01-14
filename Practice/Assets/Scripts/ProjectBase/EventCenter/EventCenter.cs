using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public interface IEventInfo { }

public class EventCenter_Info<T>  :IEventInfo
{
    public UnityAction<T> actions;

    public EventCenter_Info(UnityAction<T> action)
    {
        actions += action;
    }
}

public class EventCenter_Info : IEventInfo
{
    public UnityAction acitons;

    public EventCenter_Info(UnityAction aciton)
    {
        acitons += aciton;
    }
}

public class EventCenter : BaseManager<EventCenter>
{
    //��������
   public Dictionary<string , IEventInfo> eventDic = new Dictionary<string , IEventInfo>();

    public void AddEventListener<T>(string name,UnityAction<T> action )
    {
        //����������Ѿ��и��¼�����
        if( eventDic.ContainsKey( name ) )
        {
            (eventDic[name] as EventCenter_Info<T>).actions += action;
        }
        else
        {
            //��Ӹ��¼�����
            eventDic.Add( name, new EventCenter_Info<T>( action ) );
        }
    }

    public void AddEventListener(string name, UnityAction action)
    {
        //����������Ѿ��и��¼�����
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventCenter_Info).acitons += action;
        }
        else
        {
            //��Ӹ��¼�����
            eventDic.Add(name, new EventCenter_Info(action));
        }
    }

    //�����¼�
    public void EventTrigger<T>(string name , T obj)
    {
        if(eventDic.ContainsKey( name ) )
        {
            (eventDic[name] as EventCenter_Info<T>).actions?.Invoke(obj);
        }
    }
    public void EventTrigger(string name)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventCenter_Info).acitons?.Invoke();
        }
    }

    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if(eventDic.ContainsKey( name ) )
        {
            (eventDic[name] as EventCenter_Info<T>).actions -= action; 
        }
    }
    public void RemoveEventListener(string name, UnityAction action)
    {
        if(eventDic.ContainsKey( name ) )
        {
            (eventDic[name] as EventCenter_Info).acitons -= action; 
        }
    }

    public void ClearEvent()
    {
        eventDic = null;
    }
}
