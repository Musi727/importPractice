using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BasePanel : MonoBehaviour
{
    public bool isShow = false;
    private CanvasGroup canvasGroup;
    private float fadeSpeed = 10f;
    //�ؼ�����
    private Dictionary<string, List<UIBehaviour>> controllerDic = new Dictionary<string, List<UIBehaviour>>();

    protected virtual void Awake()
    {
        //���CanvasGroup���
        canvasGroup = GetComponent<CanvasGroup>();
        if(canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        //�ҵ����еĿؼ�
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<Text>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<Slider>();
        FindChildrenControl<InputField>();
    }


    /// <summary>
    /// �õ���Ӧ���ֵĿؼ��Ľű�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    protected T GetController<T>(string controlName) where T: UIBehaviour
    {
        if (controllerDic.ContainsKey(controlName))
        {
            for(int i = 0; i < controllerDic[controlName].Count; i++)
            {
                if (controllerDic[controlName][i] is T)
                {
                    return controllerDic[controlName][i] as T;
                }
            }
        }
        return null;
    }

    protected virtual void OnClick(string name)
    {

    }

    protected virtual void OnValueChange(string name,bool value)
    {

    }

    protected virtual void OnValueChange(string name,float value)
    {

    }

    protected void FindChildrenControl<T>() where T: UIBehaviour
    {
        T[] controls = this.GetComponentsInChildren<T>();
        for(int i=0;i<controls.Length; i++)
        {
            //��ÿؼ��� �� ����ֵ�ļ�
            string objName = controls[i].gameObject.name;
            //����Ѿ����˸ÿؼ�
            if (controllerDic.ContainsKey(objName))
            {
                controllerDic[objName].Add(controls[i]);
            }
            //���û�о��������
            else
            {
                controllerDic.Add(objName, new List<UIBehaviour>() { controls[i] });
            }
            //Ϊ��ͬ�ؼ�������Ӷ�Ӧ����
            switch(controls[i])
            {
                case Button:
                    (controls[i] as Button).onClick.AddListener(()=> 
                    {
                        OnClick(objName);
                    });
                    break;
                case Toggle:
                    (controls[i] as Toggle).onValueChanged.AddListener((value) =>
                    {
                        OnValueChange(objName,value);
                    });
                    break;
                case Slider:
                    (controls[i] as Slider).onValueChanged.AddListener((value) =>
                    {
                        OnValueChange(objName,value);
                    });
                    break;
                case Text:
                    break;
                case Image:
                    break;
                case InputField:
                    break;
            }
        }
        

    }

    protected  virtual void Start()
    {
        //�����������µĿؼ�����
        Button[] btns =this.GetComponentsInChildren<Button>();

        Init();
    }

    public abstract void Init();

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isShow && canvasGroup.alpha <=1) 
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            if(canvasGroup.alpha >= 1)
            {
                canvasGroup.alpha = 1;
            }
        }
        else if(!isShow && canvasGroup.alpha >=0)
        {
            canvasGroup.alpha -=Time.deltaTime * fadeSpeed;
            if(canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
            }
        }
    }
    public virtual void ShowMe()
    {
        isShow = true;
        canvasGroup.alpha = 0;
    }

    public virtual void HideMe(UnityAction callback)
    {
        isShow = false;
        canvasGroup.alpha = 1;
        callback?.Invoke();
    }
}
