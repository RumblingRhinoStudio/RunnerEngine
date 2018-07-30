using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEvent1Parameter : UnityEvent<string> { }

[System.Serializable]
public class UnityEvent2Parameters : UnityEvent<string, string> { }

[CreateAssetMenu(fileName = "Game Event Listener", menuName = "Game Events/Game Event Listener", order = 2)]
public class GameEventListener : MonoBehaviour
{

    #region Properties

    [Tooltip("Event to register with.")]
    public GameEvent Event;

    [Tooltip("Response to invoke with 0 parameters when Event is raised.")]
    public UnityEvent Response;

    #endregion


    #region MonoBehaviour Messages

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    #endregion


    #region Public Methods

    public void OnEventRaised()
    {
        Response.Invoke();
    }

    #endregion
}
