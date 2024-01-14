using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoController : MonoBehaviour
{
    private event UnityAction updateEvent;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        updateEvent?.Invoke();
    }

    /// <summary>
    /// 提供给外部 ， 添加帧更新事件的函数
    /// 让外部没有继承 Mono的 对象也能使用帧更新
    /// </summary>
    /// <param name="action"></param>
    public void AddListenerUpdate(UnityAction action)
    {
        updateEvent += action;
    }

    /// <summary>
    /// 提供给外部 ， 移除帧更新事件的函数
    /// </summary>
    /// <param name="action"></param>
    public void RemoveListenerUpdate(UnityAction action)
    {
        updateEvent -= action;
    }
}
