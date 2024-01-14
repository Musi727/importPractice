using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonAutoBaseMgr<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject(typeof(T).ToString());

                GameObject.DontDestroyOnLoad(obj);

                instance = obj.AddComponent<T>();
            }
            return instance;
        }
    }
}
