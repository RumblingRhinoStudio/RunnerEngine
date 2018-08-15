using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class FloatGameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public FloatGameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public FloatEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(float argument)
    {
        Response.Invoke(argument);
    }
}

// This is needed, because the UnityEditor can't serialize generics
[System.Serializable]
public class FloatEvent : UnityEvent<float>
{

}