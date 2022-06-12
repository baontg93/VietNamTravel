using System;
using System.Collections.Generic;
using UnityEngine;

class PublishData
{
    public BaseEvent BaseEvent;
    public object Data;
    public PublishData(BaseEvent baseEvent, object data)
    {
        BaseEvent = baseEvent;
        Data = data;
    }
}

public class EventManager : SingletonBehaviour<EventManager>
{
    private readonly Dictionary<BaseEvent, List<Action<object>>> dictListeningAction = new();
    private readonly Queue<PublishData> publishDatas = new();

    public void Register(BaseEvent baseEvent, Action<object> action)
    {
        if (!dictListeningAction.ContainsKey(baseEvent))
        {
            dictListeningAction[baseEvent] = new List<Action<object>>();
        }
        dictListeningAction[baseEvent].Add(action);
    }

    public void Unregister(BaseEvent baseEvent, Action<object> action)
    {
        if (dictListeningAction.ContainsKey(baseEvent) && dictListeningAction[baseEvent].Contains(action))
        {
            dictListeningAction[baseEvent].Remove(action);
        }
    }

    public void Publish(BaseEvent baseEvent, object data = null)
    {
        PublishData enqueueData = new(baseEvent, data);
        publishDatas.Enqueue(enqueueData);
    }

    private void Update()
    {
        if (publishDatas.Count > 0)
        {
            PublishData publishData = publishDatas.Dequeue();
            Debug.Log($"Publish event {publishData.BaseEvent}");
            if (dictListeningAction.ContainsKey(publishData.BaseEvent))
            {
                List<Action<object>> actions = dictListeningAction[publishData.BaseEvent];
                for (int index = 0; index < actions.Count; index++)
                {
                    actions[index](publishData.Data);
                }
            }
        }
    }
}
