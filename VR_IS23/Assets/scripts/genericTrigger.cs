using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * PSEUDOCODE:
 * generic trigger script for snapshot objects. 
 * Ensures that the objects are done fading in before allowing 
 * the unity event to be triggered. 
 *  
 */
public class genericTrigger : MonoBehaviour
{

    public string triggerTag;
    public UnityEvent onTriggerEvent;

   /*using [SerializeField] exposes private fields to the Unity Editor without making them public. 
    * Ensures that those variables can only be changed in the script.
    * other scripts cannot access it or modify it as well. 
    */
    [SerializeField] private simplifiedSnapshot fadeEndObserve;
    private bool fadeOver = false;

    private void Awake()
    {
        if (fadeEndObserve != null)
        {
            fadeEndObserve.endFade += fadeDone;
        }
    }

    private void OnDestroy()
    {
        if (fadeEndObserve != null)
        {
            fadeEndObserve.endFade -= fadeDone;
        }
    }

    private void fadeDone()
    {
        fadeOver = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == triggerTag && fadeOver)
        {
            onTriggerEvent.Invoke();
        }
    }
}
