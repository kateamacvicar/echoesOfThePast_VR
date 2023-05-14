using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

/*
 * PSEUDOCODE:
 * script on a trigger area that determines when the hint should turn on
 */

public class buttonHint : MonoBehaviour
{
    

    public string triggerTag;
    public SteamVR_Action_Boolean trigger;
    public string hintText;
    public SteamVR_Action_Vibration hintHaptic;
    public bool onRightController = false;

    private Player player;
    private Collider boxCollider;

    [SerializeField] private simplifiedSnapshot fadeEndObserve;
    [SerializeField] private tutorialTextScrolling textEndObserve;
    [SerializeField] private teleportTutorial buttonPressObserve;

    void Start()
    {
        //trigger = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("seePast");
        player = Player.instance;
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Awake()
    {
        if (fadeEndObserve != null)
        {
            //if the object has finished fading in, disable hint
            fadeEndObserve.endFade += endHint;
        }

        if (textEndObserve != null)
        {
            textEndObserve.endScroll += startHint;
        }

        if (buttonPressObserve != null)
        {
            buttonPressObserve.endHint += endHint;
        }
    }

    private void OnDestroy()
    {
        if (fadeEndObserve != null)
        {
            fadeEndObserve.endFade -= endHint;
        }

        if (textEndObserve != null)
        {
            textEndObserve.endScroll -= startHint;
        }

        if (buttonPressObserve != null)
        {
            buttonPressObserve.endHint -= endHint;
        }
    }

    private void endHint()
    {
        if (hintText != null)
        {
            if (onRightController)
            {
                ControllerButtonHints.HideTextHint(player.rightHand, trigger);
            }
            else
            {
                ControllerButtonHints.HideTextHint(player.leftHand, trigger);
            }
        }
        else
        {
            if (onRightController)
            {
                ControllerButtonHints.HideButtonHint(player.rightHand, trigger);
            }
            else
            {
                ControllerButtonHints.HideButtonHint(player.leftHand, trigger);
            }
        }
        if (boxCollider != null)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void startHint()
    {
        if (hintText != null)
        {
            if (onRightController)
            {
                ControllerButtonHints.ShowTextHint(player.rightHand, trigger, hintText);
                hintHaptic.Execute(0.0f, 0.5f, 80f, 0.2f, SteamVR_Input_Sources.RightHand);
            }
            else
            {
                ControllerButtonHints.ShowTextHint(player.leftHand, trigger, hintText);
                hintHaptic.Execute(0.0f, 0.5f, 80f, 0.2f, SteamVR_Input_Sources.RightHand);
            }
        }
        else
        {
            if (onRightController)
            {
                ControllerButtonHints.ShowButtonHint(player.rightHand, trigger);
                hintHaptic.Execute(0.0f, 0.5f, 80f, 0.2f, SteamVR_Input_Sources.RightHand);
            }
            else
            {
                ControllerButtonHints.ShowButtonHint(player.leftHand, trigger);
                hintHaptic.Execute(0.0f, 0.5f, 80f, 0.2f, SteamVR_Input_Sources.RightHand);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == triggerTag)
        {
            startHint();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == triggerTag)
        {
            if (hintText != null)
            {
                if (onRightController)
                {
                    ControllerButtonHints.HideTextHint(player.rightHand, trigger);
                }
                else
                {
                    ControllerButtonHints.HideTextHint(player.leftHand, trigger);
                }
            }
            else
            {
                if (onRightController)
                {
                    ControllerButtonHints.HideButtonHint(player.rightHand, trigger);
                }
                else
                {
                    ControllerButtonHints.HideButtonHint(player.leftHand, trigger);
                }
            }
        }
    }


}
