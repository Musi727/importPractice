using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneMgr : BaseManager<SceneMgr>
{
    /// <summary>
    /// ͬ���л�����
    /// </summary>
    public void LoadScene(string name,UnityAction action = null)
    {
        SceneManager.LoadScene(name);
        //�������ִ��
        action?.Invoke();
    }

    public void LoadSceneAsync(string name, UnityAction action = null)
    {
        MonoMgr.Instance.StartCoroutine(ReallyLoadSceneAsync(name, action));
    }

    /// <summary>
    /// �첽�л�����
    /// </summary>
    public IEnumerator ReallyLoadSceneAsync(string name, UnityAction action = null)
     {
        AsyncOperation ao =  SceneManager.LoadSceneAsync(name);
        while (!ao.isDone)
        {
            //���½�����
            EventCenter.Instance.EventTrigger("���½�����", ao.progress);
            Debug.Log(ao.progress);

            yield return 1;
        }
        action?.Invoke();
    }
}
