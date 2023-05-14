using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/*
 * PSEUDOCODE:
 * script on the parent object of the present game objects that will trigger snapshots. 
 *  
 */
public class simplifiedSnapshot : MonoBehaviour
{
    public SteamVR_Action_Boolean pastToggle;
    public bool hasWater = false;

    [Header("Empty parents of present and past models")]
    public GameObject presentModel;
    public GameObject pastModel;

    [Header("Speed of fade and alpha thresholds")]
    public float speed;
    //for water/transparent objects, we want to set the max of the alpha value to be less than 1 so that they appear slightly transparent. 
    public float waterMax;
    public float max;
    public float min;

    public static bool turnOffHalo = false;
    public static bool turnOffHint = false;
    public static bool fadeDone = false;

    [Header("Listening to")]
    [SerializeField] private triggerSnapshot subjectToObserve;

    // delegate for the materialSwap script - fade switched to opaque material
    public delegate void EndFade();
    public EndFade endFade;

    private bool updateStart = false;
    private bool startFadeIn = false;

    private Renderer[] presentColorsRend;
    private Renderer[] pastColorsRend;

    void Start()
    {
        //array of all of the renderers of the children
        presentColorsRend = presentModel.GetComponentsInChildren<Renderer>();
        pastColorsRend = pastModel.GetComponentsInChildren<Renderer>();
    }

    private void OnTriggerSnapshot()
    {
        //when spanshot is triggered, we turn off the halo, and update start, which , becomes true. 
        turnOffHalo = true;
        updateStart = true;
    }

    private void Awake()
    {
        if (subjectToObserve != null)
        {
            subjectToObserve.fadeStart += OnTriggerSnapshot;
        }
    }

    private void OnDestroy() 
    {
        if (subjectToObserve != null)
        {
            subjectToObserve.fadeStart -= OnTriggerSnapshot;
        }
    }

    void Update()
    {
        if (updateStart)
        {

            // start fading out
            if (presentColorsRend[0].material.color.a > min)
            {
                for (int i = 0; i < presentColorsRend.Length; i++)
                {
                    Color newColor = presentColorsRend[i].material.color;
                    newColor.a -= speed;
                    presentColorsRend[i].material.color = newColor;
                }

            }
            else // we've finished fading out
            {
                startFadeIn = true; 
            }

            // start fading in
            if (pastColorsRend[0].material.color.a < max && startFadeIn)  
            {
                for (int i = 0; i < pastColorsRend.Length; i++)
                {
                    if (pastColorsRend[i].gameObject.tag == "water")
                    {
                        if (pastColorsRend[i].material.color.a < waterMax)
                        {
                            Color waterColor = pastColorsRend[i].material.color;
                            waterColor.a += speed;
                            pastColorsRend[i].material.color = waterColor;
                        }
                    }
                    else
                    {
                        Color newColor = pastColorsRend[i].material.color;
                        newColor.a += speed;
                        pastColorsRend[i].material.color = newColor;
                    }
                }


            }
            else if (startFadeIn) // have finished fading in
            {
                fadeDone = true;
                endFade?.Invoke();
                presentModel.SetActive(false);
                if (pastModel.tag == "pastFountain")
                {
                    textScrolling.startText = true;
                }
                turnOffHalo = false;
                turnOffHint = true;
            }
        }
    }

}
