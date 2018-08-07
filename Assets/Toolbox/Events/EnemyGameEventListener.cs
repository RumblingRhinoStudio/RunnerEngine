using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EnemyGameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public EnemyGameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public EnemyBehaviourEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(EnemyBehaviour enemy)
    {
        Response.Invoke(enemy);
    }
}

// This is needed, because the UnityEditor can't serialize generics
[System.Serializable]
public class EnemyBehaviourEvent : UnityEvent<EnemyBehaviour>
{

}