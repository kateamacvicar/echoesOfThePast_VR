using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * PSEUDOCODE:
 * generic trigger script. 
 * If a certain object with a certain trigger tag 
 * (which is set by the developer, usually hand) 
 * collides with the object this script is on, then a trigger event is invoked. 
 * That event can set objects inactive or active, trigger sounds, trigger animations, etc. 
 *  
 */
public class trueGenericTrigger : MonoBehaviour
{

    public string triggerTag;
    public UnityEvent onTriggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == triggerTag)
        {
            onTriggerEvent.Invoke();
        }
    }
}

