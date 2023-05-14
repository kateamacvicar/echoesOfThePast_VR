using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class openGate : MonoBehaviour
{
    // attached to trigger area around gate lock
    public string keyTag;
    //public Animator gateAnimation;

    public UnityEvent onTriggerEvents;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == keyTag)
        {
            //gateAnimation.enabled = true;
            onTriggerEvents.Invoke();
        }
    }
}
