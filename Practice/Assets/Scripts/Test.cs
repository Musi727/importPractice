using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        ABMgr.Instance.LoadResAsyncAB<GameObject>("model","Cube1");       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
