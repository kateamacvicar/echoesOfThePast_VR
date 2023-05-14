using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;

/*
 * PSEUDOCODE:
 * script on teleport tutorial words, which contains the canvases 
 * with the directions players first see when entering the VR experience. 
 */


public class teleportTutorial : MonoBehaviour
{
    public float waitTime;

    public GameObject teleportObject;
    public GameObject snapTurnObject;

    public GameObject snapTurnCanvas; //first canvas. "You look lost..."
    public GameObject teleportCanvas; //second canvas. "I wonder whats down those stairs...."

    public SteamVR_Action_Boolean leftTurnTrigger;
    public SteamVR_Action_Boolean rightTurnTrigger;
    public SteamVR_Action_Boolean teleportTrigger;

    public delegate void EndHint();
    public EndHint endHint;

    [SerializeField] private tutorialTextScrolling snapturnOnObserve;

    private void Awake()
    {
        if (snapturnOnObserve != null)
        {
            snapturnOnObserve.endScroll += enableTurn;
        }
    }

    private void OnDestroy()
    {
        if (snapturnOnObserve != null)
        {
            snapturnOnObserve.endScroll -= enableTurn;
        }
    }

    private void Start()
    {
        //disable teleporting and snapturn at start
        teleportObject.SetActive(false);
        snapTurnObject.SetActive(false);

        //ensure the second canvas is inactive. 
        teleportCanvas.SetActive(false);
    }

    private void Update()
    {
        //if you use the thumbstick to turn left or right. 
        if (leftTurnTrigger.GetState(SteamVR_Input_Sources.Any) || rightTurnTrigger.GetState(SteamVR_Input_Sources.Any))
        {
            if (snapTurnObject.activeSelf)
            {
                //Debug.Log("inside active self");
                endHint?.Invoke(); //turn off hint
                teleportObject.SetActive(true); //allow player to teleport
            }
        }
        //if you teleport
        if (teleportTrigger.GetState(SteamVR_Input_Sources.Any))
        {
            //Debug.Log("teleport trigger");
            endHint?.Invoke();
            StartCoroutine(turnOffText()); //canvases disappear after a certain amount of time. 
        }
    }

    private void enableTurn()
    {
        snapTurnObject.SetActive(true);
        teleportCanvas.SetActive(true);
    }

    IEnumerator turnOffText()
    {
        yield return new WaitForSeconds(waitTime);
        snapTurnCanvas.SetActive(false);
        teleportCanvas.SetActive(false);
    }

}
