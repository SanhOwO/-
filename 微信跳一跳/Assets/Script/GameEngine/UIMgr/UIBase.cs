using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    //里式转换原则UIBehaviour 来储存所有的控件
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();
    protected virtual void Awake()
    {
        AddChildComponents<Button>();
        AddChildComponents<Text>();
        AddChildComponents<Image>();
        AddChildComponents<Toggle>();
        AddChildComponents<Slider>();
        AddChildComponents<ScrollRect>();
        AddChildComponents<InputField>();

    }
    //存储旗下所有子集的组件
    private void AddChildComponents<T>() where T : UIBehaviour
    {
        T[] comps = this.GetComponentsInChildren<T>();
        foreach (var c in comps)
        {
            string objName = c.gameObject.name;
            if (controlDic.ContainsKey(objName))
                controlDic[objName].Add(c);
            else
                controlDic.Add(objName, new List<UIBehaviour> { c });
            if (c is Button)
                //因为AddListener不能有参数，所以这里使用Lambda制造个无参数的函数调用有参数的函数，同时还能使用objName
                (c as Button).onClick.AddListener(() => { onClick(objName); });
            else if (c is Toggle)
                //Toggle需要一个参数，所以加一个
                (c as Toggle).onValueChanged.AddListener((value) => { onValueChanged(objName, value); });
        }
    }
    protected T GetChildComponents<T>(string name) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(name))
        {
            foreach (var c in controlDic[name])
            {
                if (c is T)
                    return c as T;
            }
        }
        return null;
    }
    public virtual void Display()
    {

    }
    public virtual void Undisplay()
    {

    }
    protected virtual void onClick(string btName)
    {

    }

    protected virtual void onValueChanged(string name, bool value)
    {

    }
}
