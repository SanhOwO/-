using GameEngine.Instance;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum E_UI_Layer
{
    Top, Mid, Bottom, Canvass, System
}
public class UIMgr : InstanceNull<UIMgr>
{
    private Dictionary<string, UIBase> panelDic = new Dictionary<string, UIBase>();
    private Transform top;
    private Transform mid;
    private Transform bottom;
    private Transform system;
    //记录UI的Canvas父对象，方便以后外部可能的使用
    public RectTransform canvas;
    public UIMgr()
    {
        //动态创建canvas防止切换场景时被删除
        canvas = ResourcesMgr.Instance.LoadSyn<GameObject>("UI/Canvas").transform as RectTransform;
        GameObject.DontDestroyOnLoad(canvas.gameObject);
        GameObject.DontDestroyOnLoad(ResourcesMgr.Instance.LoadSyn<GameObject>("UI/EventSystem"));
        top = canvas.Find("Top");
        mid = canvas.Find("Mid");
        bottom = canvas.Find("Bottom");
        system = canvas.Find("System");
    }
    public void ShowPanel<T>(string name, E_UI_Layer layer = E_UI_Layer.Canvass, UnityAction<T> callback = null) where T : UIBase
    {
        if (panelDic.ContainsKey(name))
        {
            if (callback != null)
                callback(panelDic[name] as T);
            return;
        }
        ResourcesMgr.Instance.LoadAsyn<GameObject>(name, (obj) =>
        {
            Transform father = canvas;
            switch (layer)
            {
                case E_UI_Layer.Top:
                    father = top;
                    break;
                case E_UI_Layer.Mid:
                    father = mid;
                    break;
                case E_UI_Layer.Bottom:
                    father = bottom;
                    break;
                case E_UI_Layer.System:
                    father = system;
                    break;
                case E_UI_Layer.Canvass:
                    father = canvas;
                    break;
                default:
                    break;
            }
            obj.transform.SetParent(father);
            obj.transform.localPosition = obj.transform.position;
            //obj.transform.localScale = Vector3.one;
            //(obj.transform as RectTransform).offsetMax = Vector2.zero;
            //得到预设体上的面板 这里obj和panel都代表实例化的物体(clone)
            T panel = obj.GetComponent<T>();
            if (callback != null)
                callback(panel);
            panelDic.Add(name, panel);
            if (panel != null)
                panel.Display();
        });
    }
    public void HidePanel(string name)
    {
        if (panelDic.ContainsKey(name))
        {
            panelDic[name].Undisplay();
            GameObject.Destroy(panelDic[name].gameObject);
            panelDic.Remove(name);
        }
    }
    //获得面板 主要用于使用面板逻辑 比如背包面板 使用移除物品逻辑
    public T GetPanel<T>(string name) where T : UIBase
    {
        if (panelDic.ContainsKey(name))
            return panelDic[name] as T;
        return null;
    }
    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Top:
                return this.top;
            case E_UI_Layer.Mid:
                return this.mid;
            case E_UI_Layer.Bottom:
                return this.top;
            case E_UI_Layer.System:
                return this.top;
            case E_UI_Layer.Canvass:
                return this.canvas;
            default:
                return null;
        }
    }
    //给控件添加自动事件监听EventTrigger
    //所有的自定义函数都需要 Drag(*BaseEventData data*)
    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBack)
    {
        EventTrigger trg = control.GetComponent<EventTrigger>();
        if (trg == null)
            trg = control.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);

        trg.triggers.Add(entry);
    }
}
