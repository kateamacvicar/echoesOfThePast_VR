using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialSwap : MonoBehaviour
{
    // script will be attached to each past / present model
    public bool startOpaque = false;
    public Material transitionMaterial;

    //private Material startingMaterial; // may not be needed...
    private Renderer modelRend;

    [SerializeField] private triggerSnapshot fadeStartObserve;
    [SerializeField] private simplifiedSnapshot fadeEndObserve;

    private void swapMaterial()
    {
        modelRend.material = transitionMaterial;
    }

    private void Awake()
    {
        //startingMaterial = GetComponent<Renderer>().material;
        modelRend = GetComponent<Renderer>();

        if (startOpaque && fadeStartObserve != null)
        {
            //Debug.Log("start opaque " + name);
            fadeStartObserve.fadeStart += swapMaterial;
        }
        else if (!startOpaque && fadeEndObserve != null)
        {
            //Debug.Log("start fade" + name);
            fadeEndObserve.endFade += swapMaterial;
        }
    }

    private void OnDestroy()
    {
        if (fadeStartObserve != null)
        {
            fadeStartObserve.fadeStart -= swapMaterial;
        }
        else if (fadeEndObserve != null)
        {
            fadeEndObserve.endFade -= swapMaterial;
        }
    }
}
