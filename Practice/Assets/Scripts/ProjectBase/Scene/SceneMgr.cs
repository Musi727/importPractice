using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneMgr : BaseManager<SceneMgr>
{
    /// <summary>
    /// 同步切换场景
    /// </summary>
    public void LoadScene(string name,UnityAction action = null)
    {
        SceneManager.LoadScene(name);
        //加载完后执行
        action?.Invoke();
    }

    public void LoadSceneAsync(string name, UnityAction action = null)
    {
        MonoMgr.Instance.StartCoroutine(ReallyLoadSceneAsync(name, action));
    }

    /// <summary>
    /// 异步切换场景
    /// </summary>
    public IEnumerator ReallyLoadSceneAsync(string name, UnityAction action = null)
     {
        AsyncOperation ao =  SceneManager.LoadSceneAsync(name);
        while (!ao.isDone)
        {
            //更新进度条
            EventCenter.Instance.EventTrigger("更新进度条", ao.progress);
            Debug.Log(ao.progress);

            yield return 1;
        }
        action?.Invoke();
    }
}
