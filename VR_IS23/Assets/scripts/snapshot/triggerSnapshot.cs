using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/*
 * PSEUDOCODE:
 * script on all present version of game objects that will activate the snapshots. 
 * When you enter and stay inside the trigger area of the present objects...
 * checks if the hand is what collided with the object, that you held down the correct button, and that the past model is not active. 
 * If those are true, set the past model active, and invoke fade start
 *  
 */
public class triggerSnapshot : MonoBehaviour
{
    // script attached to actual model of present obj, not the empty parent

    public SteamVR_Action_Boolean pastToggle;
    public GameObject pastModel; // the empty parent of the past models

    // delegate for simplifiedSnapshot script
    public delegate void FadeStart();
    public FadeStart fadeStart;

    private void OnTriggerStay(Collider other) 
    {
        // can't really check if past model is active... unless...
        if (other.CompareTag("hand") && pastToggle.GetState(SteamVR_Input_Sources.Any) && !pastModel.activeSelf) 
        {
            //Debug.Log("fountain triggered in stay");
            pastModel.SetActive(true);

            // let all the listeners know?
            fadeStart?.Invoke();
        }
        else if (gameObject.CompareTag("fountain") && other.CompareTag("hand") && pastToggle.GetState(SteamVR_Input_Sources.Any) && textScrolling.isTextDone)
        {
            //Debug.Log("text retriggered");
            textScrolling.startText = true;
        }
    }
}
