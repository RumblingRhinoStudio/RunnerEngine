using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FloatGameEvent", menuName = "Toolbox/Events/Float Game Event", order = 0)]
public class FloatGameEvent : GameEvent
{
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<FloatGameEventListener> eventListeners = new List<FloatGameEventListener>();

    public void Raise(float argument)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventRaised(argument);
        }
    }

    public void RegisterListener(FloatGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    public void UnregisterListener(FloatGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }
}
