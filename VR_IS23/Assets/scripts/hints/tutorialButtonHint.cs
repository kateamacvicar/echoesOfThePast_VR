using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class tutorialButtonHint : MonoBehaviour
{
    // attached to a trigger area that determines when hint turns on

    public string triggerTag;
    public SteamVR_Action_Boolean trigger;
    public string hintText;
    public SteamVR_Action_Vibration hintHaptic;
    //public simplifiedSnapshot snapshotInstance;

    private Player player;

    [SerializeField] private simplifiedSnapshot fadeEndObserve;

    void Start()
    {
        //trigger = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("seePast");
        player = Player.instance;
    }

    private void Awake()
    {
        if (fadeEndObserve != null)
        {
            fadeEndObserve.endFade += endHint;
        }
    }

    private void OnDestroy()
    {
        if (fadeEndObserve != null)
        {
            fadeEndObserve.endFade -= endHint;
        }
    }

    private void endHint()
    {
        if (hintText != null)
        {
            ControllerButtonHints.HideTextHint(player.rightHand, trigger);
            ControllerButtonHints.HideTextHint(player.leftHand, trigger);
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            ControllerButtonHints.HideButtonHint(player.rightHand, trigger);
            ControllerButtonHints.HideButtonHint(player.leftHand, trigger);
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == triggerTag)
        {
            if (hintText != null)
            {
                ControllerButtonHints.ShowTextHint(player.rightHand, trigger, hintText);
                ControllerButtonHints.ShowTextHint(player.leftHand, trigger, hintText);
                hintHaptic.Execute(0.0f, 0.5f, 80f, 0.2f, SteamVR_Input_Sources.RightHand);
            }
            else
            {
                ControllerButtonHints.ShowButtonHint(player.rightHand, trigger);
                ControllerButtonHints.ShowButtonHint(player.leftHand, trigger);
                hintHaptic.Execute(0.0f, 0.5f, 80f, 0.2f, SteamVR_Input_Sources.RightHand);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == triggerTag)
        {
            if (hintText != null)
            {
                ControllerButtonHints.HideTextHint(player.rightHand, trigger);
                ControllerButtonHints.HideTextHint(player.leftHand, trigger);
            }
            else
            {
                ControllerButtonHints.HideButtonHint(player.rightHand, trigger);
                ControllerButtonHints.HideButtonHint(player.leftHand, trigger);
            }
        }
    }

}
