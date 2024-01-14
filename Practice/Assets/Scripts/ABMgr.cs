using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    public void LoadAB(string abName)
    {
        //加载abName包
        AssetBundle abMain = AssetBundle.LoadFromFile(abName);
        //加载abName包的manifest包
        AssetBundleManifest abManifest = abMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        //从manifest包中读出依赖,abName包关联的依赖
        string[] dependencise = abManifest.GetAllDependencies(abName);
        //将所有依赖加载
        for(int i=0;i<dependencise.Length;i++)
        {
            //如果该依赖包没有被加载
            if(!abDic.ContainsKey(dependencise[i]))
            {
                //加载该依赖包
                abDic.Add(dependencise[i],AssetBundle.LoadFromFile(dependencise[i]));
            }
        }
    }
}
