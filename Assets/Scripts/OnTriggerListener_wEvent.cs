using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerListener_wEvent : MonoBehaviour
{
    public UnityEvent OnTriggerEnter_Event = new UnityEvent();
    public UnityEvent OnTriggerExit_Event = new UnityEvent();

    // When an object interacts with the trigger, call the associated Event. 
    private void OnTriggerEnter2D(Collider2D collision) {
        OnTriggerEnter_Event.Invoke(); // Call the event
    }

    private void OnTriggerExit2D(Collider2D collision) {
        OnTriggerExit_Event.Invoke();
    }
}
