using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> { }

[CreateAssetMenu(menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
   public List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise(Component sender, params object[] obj)
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnEventRaised(sender, obj);
        }
    }



    public void RegisterListener(GameEventListener listener)
    {
        if( !listeners.Contains(listener) )
        {
            listeners.Add(listener);
        }
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if ( listeners.Contains(listener) )
        {
            listeners.Remove(listener);
        }
    }
}
