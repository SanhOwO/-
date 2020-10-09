using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine.Instance;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;

public class ObjectPool : InstanceNull<ObjectPool>  //衣柜
{
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();
    private GameObject pool;
    public void GetObj(string name, UnityAction<GameObject> callback)
    {
        if (poolDic.ContainsKey(name) && poolDic[name].GetLength()>0 )
        {
           callback(poolDic[name].GetObj());
        }
        else 
        {
           ResourcesMgr.Instance.LoadAsyn<GameObject>(name, (obj) => { obj.name = name;callback(obj);});
        }
    }
    public GameObject GetObj2(string name)
    {       
        if (poolDic.ContainsKey(name) && poolDic[name].GetLength() > 0)
        {
            var obj = poolDic[name].GetObjInActive();
            if (obj == null)
                return ResourcesMgr.Instance.LoadSyn<GameObject>(name);
            return obj;
        }
        else
        {
            return ResourcesMgr.Instance.LoadSyn<GameObject>(name);
        }
    }
    public void RevertObj(string name, GameObject obj)
    {
        if (pool == null)
            pool = new GameObject("Pool");
        if (!poolDic.ContainsKey(name))
        {
            PoolData pd = new PoolData(obj, pool);
            poolDic.Add(name, pd);
        }
        poolDic[name].RevertObj(obj); 
    }

    public void ClearPool()
    {
        poolDic.Clear();
        pool = null;
    }
}
public class PoolData   //抽屉
{
    private GameObject fatherObj;        //父节点
    private List<GameObject> poolList;
    public PoolData(GameObject obj, GameObject poolObj)
    {
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolObj.transform;
        poolList = new List<GameObject>() { obj };
    }
    
    public void RevertObj(GameObject obj)
    {
        poolList.Add(obj);
        obj.transform.parent = fatherObj.transform;
        obj.SetActive(false);
    }

    public GameObject GetObj()
    {
        GameObject obj = null;
        obj = poolList[0];
        poolList.RemoveAt(0); 
        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }
    public GameObject GetObjInActive()
    {
        GameObject obj = null;
        for (int i = 0; i < poolList.Count; i++)
        {
            if (poolList[i].activeSelf == false)
            {
                obj = poolList[i];
                poolList.RemoveAt(i);
                obj.SetActive(true);
                obj.transform.parent = null;
                break;
            }
        }
        return obj;
    }
    public int GetLength()
    {
        return poolList.Count;
    }
}
