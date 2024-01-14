using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoMgr :BaseManager<MonoMgr>
{
    public MonoController Controller;
    public MonoMgr()
    {
        //通过管理器添加一个带MonoController脚本的物体
        GameObject obj = new GameObject("MonoController");
        Controller = obj.AddComponent<MonoController>();
    }

    public void AddUpdateListener(UnityAction action)
    {
        Controller.AddListenerUpdate(action);
    }

    public void RemoveUpdateListener(UnityAction action)
    {
        Controller.RemoveListenerUpdate(action);
    }

    public void StartCoroutine(IEnumerator routine)
    {
        Controller.StartCoroutine(routine);
    }
    public void StopCoroutine(IEnumerator routine)
    {
        Controller.StopCoroutine(routine);
    }

    public void StartCoroutine(string methodName)
    {
        Controller.StartCoroutine(methodName);
    }

    public void Invoke(string methodName, float time)
    {
        Controller.Invoke(methodName, time);
    }
}
