using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyGameEvent", menuName = "Toolbox/Events/Enemy Game Event", order = 0)]
public class EnemyGameEvent : GameEvent
{
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<EnemyGameEventListener> eventListeners = new List<EnemyGameEventListener>();

    public void Raise(EnemyBehaviour enemy)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventRaised(enemy);
        }
    }

    public void RegisterListener(EnemyGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    public void UnregisterListener(EnemyGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }
}
