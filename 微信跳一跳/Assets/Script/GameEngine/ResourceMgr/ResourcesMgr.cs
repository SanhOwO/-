using GameEngine.Instance;
using GameEngine.MonoTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourcesMgr : InstanceNull<ResourcesMgr>
{
    //同步加载资源
    public T LoadSyn<T>(string name) where T : Object
    {
        T res = Resources.Load<T>(name);
        res.name = name;
        if (res is GameObject)
        {
            var obj = GameObject.Instantiate(res);
            obj.name = name;
            return obj;
        }
        else //TextAssest AudioClip
            return res;
    }
    //异步加载资源
    public void LoadAsyn<T>(string name, UnityAction<T> callback) where T : Object
    {
        MonoGlobal.Instance.StartCoroutineTool(RealLoadAsyn<T>(name,callback));
    }

    private IEnumerator RealLoadAsyn<T>(string name, UnityAction<T> callback) where T:Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;
        if (r.asset is GameObject)  
            //因为协程 没有办法return，所以使用委托
            callback(GameObject.Instantiate(r.asset) as T); //r 默认是GameObject类型的，所以要转换为T
        else
            callback(r.asset as T);
    }
}
