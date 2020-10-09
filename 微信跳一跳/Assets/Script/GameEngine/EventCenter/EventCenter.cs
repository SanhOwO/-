using GameEngine.Instance;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public interface IEventInfo
{

}
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;
    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}

public class EventInfo : IEventInfo
{
    public UnityAction actions;
    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}

public class EventCenter : InstanceNull<EventCenter>
{
    public Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    public void AddEventListener<T>(string EventType, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(EventType))
            //(IEventInfo as EventInfo).actions 里式转换，父类继承子类
            (eventDic[EventType] as EventInfo<T>).actions  += action;
        else
            eventDic.Add(EventType, new EventInfo<T>(action));

    }
    public void AddEventListener(string EventType, UnityAction action)
    {
        if (eventDic.ContainsKey(EventType))
            //(IEventInfo as EventInfo).actions 里式转换，父类继承子类
            (eventDic[EventType] as EventInfo).actions += action;
        else
            eventDic.Add(EventType, new EventInfo(action));

    }
    public void RemoveEventListener<T>(string EventType, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(EventType))
            (eventDic[EventType] as EventInfo<T>).actions -= action;
    }
    public void RemoveEventListener(string EventType, UnityAction action)
    {
        if (eventDic.ContainsKey(EventType))
            (eventDic[EventType] as EventInfo).actions -= action;
    }
    public void EventTrigger<T>(string EventType, T info)
    {
        if (eventDic.ContainsKey(EventType))
            if ((eventDic[EventType] as EventInfo<T>).actions != null)
                (eventDic[EventType] as EventInfo<T>).actions.Invoke(info);
    }
    public void EventTrigger(string EventType)
    {
        if (eventDic.ContainsKey(EventType))
            if ((eventDic[EventType] as EventInfo).actions != null)
                (eventDic[EventType] as EventInfo).actions.Invoke();
    }
    //给物体添加自动事件监听EventTrigger
    //所有的自定义函数都需要 Drag(*BaseEventData data*)
    public void AddCustomEventListener(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> callBack)
    {
        EventTrigger trg = obj.GetComponent<EventTrigger>();
        if (trg == null)
            trg = obj.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);
        trg.triggers.Add(entry);
    }
    public void Clear()
    {
        eventDic.Clear();
    }
}
