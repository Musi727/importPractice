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
    /// �ṩ���ⲿ �� ���֡�����¼��ĺ���
    /// ���ⲿû�м̳� Mono�� ����Ҳ��ʹ��֡����
    /// </summary>
    /// <param name="action"></param>
    public void AddListenerUpdate(UnityAction action)
    {
        updateEvent += action;
    }

    /// <summary>
    /// �ṩ���ⲿ �� �Ƴ�֡�����¼��ĺ���
    /// </summary>
    /// <param name="action"></param>
    public void RemoveListenerUpdate(UnityAction action)
    {
        updateEvent -= action;
    }
}
