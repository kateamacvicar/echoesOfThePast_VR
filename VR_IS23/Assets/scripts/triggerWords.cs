using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class triggerWords : MonoBehaviour
{
    //script on lockedDoorTrigger in LockedDoorCont
    public GameObject words;
    //public GameObject trigger;
    public string triggerTag;
    public string triggerTag2;

    public static bool startText = false;

    public delegate void StartScroll();
    //instance of StartScroll
    public StartScroll startScroll;

    public SteamVR_Action_Boolean leftTurnTrigger;
    public SteamVR_Action_Boolean rightTurnTrigger;


    private void Update()
    {
        if (leftTurnTrigger != null && rightTurnTrigger != null)
        {
            if (leftTurnTrigger.GetState(SteamVR_Input_Sources.Any) || rightTurnTrigger.GetState(SteamVR_Input_Sources.Any))
            {
                startScroll?.Invoke();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("something in trigger");
        if(other.gameObject.tag == triggerTag || other.gameObject.tag == triggerTag2)
        {
            Debug.Log("player in trigger");
            words.SetActive(true); // is that needed?
            startScroll?.Invoke();
            startText = true;
            Debug.Log("start text is true");
        }
    }
}
