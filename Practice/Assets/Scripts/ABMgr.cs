using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ABMgr
{
    private static ABMgr _instance;
    public static ABMgr Instance
    {
        get
        {
            if(_instance == null)
                _instance = new ABMgr();
            return _instance;
        }
    }

    private ABMgr(){}
    private Dictionary<string,AssetBundle> abDic = new Dictionary<string, AssetBundle>();
    private AssetBundle _abMain;
    private AssetBundleManifest _abManifest;
    private string[] _dependencise;
    public void LoadAB(string abName)
    {
        //加载abName包
        _abMain = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/StandaloneWindows");
        //加载abName包的manifest包
        _abManifest = _abMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        //从manifest包中读出依赖,abName包关联的依赖
        string[] _dependencise = _abManifest.GetAllDependencies(abName);
        //将所有依赖加载
        for(int i=0;i<_dependencise.Length;i++)
        {
            //如果该依赖包没有被加载
            if(!abDic.ContainsKey(_dependencise[i]))
            {
                //加载该依赖包
                abDic.Add(_dependencise[i],AssetBundle.LoadFromFile(Application.streamingAssetsPath +"/"+ _dependencise[i]));
            }
        }
        if(!abDic.ContainsKey(abName))
        {
            abDic.Add(abName,AssetBundle.LoadFromFile(Application.streamingAssetsPath +"/"+ abName));
        }
    }
    public void LoadABAsync(string abName,UnityAction callback)
    {
        MonoMgr.Instance.StartCoroutine(ReallyLoadABAsync(abName,callback));
    }
    /// <summary>
    /// 异步加载AB包资源
    /// 异步加载AB包，同步加载资源
    /// </summary>
    public IEnumerator ReallyLoadABAsync(string abName,UnityAction callback)
    {
        if (_abMain == null)
        {
            //异步加载AB包中的主包
            AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/StandaloneWindows");
            yield return abcr;
            _abMain = abcr.assetBundle;
            //异步加载AB包中主包的Manifest
            AssetBundleRequest abr = _abMain.LoadAssetAsync<AssetBundleManifest>("AssetBundleManifest");
            yield return abr;
            _abManifest = abr.asset as AssetBundleManifest;
        }
        //从manifest包中读出依赖,abName包关联的依赖
        _dependencise = _abManifest.GetAllDependencies(abName);
        //将所有依赖加载
        for(int i=0;i<_dependencise.Length;i++)
        {
            //如果该依赖包没有被加载
            if(!abDic.ContainsKey(_dependencise[i]))
            {
                //加载该依赖包
                AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath +"/"+ _dependencise[i]);
                yield return abcr;
                abDic.Add(_dependencise[i], abcr.assetBundle);
            }
        }
        if (!abDic.ContainsKey(abName))
        {
            AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/" + abName);
            abDic.Add(abName, abcr.assetBundle);
        }
        callback?.Invoke();
    }

    /// <summary>
    /// 同步加载AB包，同步加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="abName">资源所在的AB包名称</param>
    /// <param name="resName">资源名称</param>
    /// <returns>资源类型</returns>
    public T LoadABRes<T>(string abName,string resName)where T:Object
    {
        LoadAB(abName);
        //加载资源
        T obj = abDic[abName].LoadAsset<T>(resName);
        if (obj is GameObject)
        {
            //实例化
            obj = GameObject.Instantiate(obj);
        }
        return obj;
    }
    public T LoadResAsyncAB<T>(string abName,string resName)where T:Object
    {
        T gameObject = null;
        LoadABAsync(abName, () =>
        {
            //加载资源
            T obj = abDic[abName].LoadAsset<T>(resName);
            if (obj is GameObject)
            {
                //实例化
                obj = GameObject.Instantiate(obj);
            }
            gameObject = obj;
        });
        return gameObject;
    }
}
