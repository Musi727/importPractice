using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : BaseManager<InputMgr>
{
    public bool isCheck = false;
    //ÔÚUpdateÖÐÌí¼Ó¼àÌý
    public InputMgr()
    {
        MonoMgr.Instance.AddUpdateListener(UpdateKey);
    }

    public void CheckKeys(KeyCode key)
    {
        if (Input.GetKeyDown(key))
            EventCenter.Instance.EventTrigger("SomeKeyDown", key);

        if (Input.GetKeyUp(key))
            EventCenter.Instance.EventTrigger("SomeKeyUp", key);
    }

    public void StartOrEndCheck(bool isOpen)
    {
        isCheck = isOpen;
    }

    public void UpdateKey()
    {

        EventCenter.Instance.EventTrigger("Horizontal", Input.GetAxisRaw("Horizontal"));
        EventCenter.Instance.EventTrigger("Vertical", Input.GetAxisRaw("Vertical"));

        CheckKeys(KeyCode.J);
        CheckKeys(KeyCode.K);
        CheckKeys(KeyCode.Space);
        CheckKeys(KeyCode.L);
        CheckKeys(KeyCode.N);
        CheckKeys(KeyCode.Alpha1);
        CheckKeys(KeyCode.Alpha2);
        CheckKeys(KeyCode.Alpha3);
    }
}
