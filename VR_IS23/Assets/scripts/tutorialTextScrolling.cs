using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * PSEUDOCODE:
 * script on each of the canvases in the tutorial area. Key, locked Door, both upper room canvases. 
 *  
 */

public class tutorialTextScrolling : MonoBehaviour
{

    public TextMeshProUGUI textComponent;
    public string line;
    public float textSpeed;
    public float waitTime;

    public GameObject audioCont;
    
    //instance of 
    [SerializeField] private triggerWords textStartObserve;

    private bool textHasScrolled = false;

    //can be used to reference methiods that match it. So other methods that don't take parameters or return a value.
    public delegate void EndScroll();
    //delegate instance of EndScroll
    public EndScroll endScroll;

    private void Awake()
    {
        if (textStartObserve != null)
        {
            textStartObserve.startScroll += StartDialogue; //assigning StartDialogue to the delegate 
        }
        else
        {
            StartCoroutine(StartWaitTime());
        }
    }

    private void OnDestroy()
    {
        if (textStartObserve != null)
        {
            textStartObserve.startScroll -= StartDialogue;
        }
    }

    void StartDialogue()
    {
        if (!textHasScrolled)
        {
           // Debug.Log("start dialogue");

            textComponent.text = ""; //make sures it starts off w/o text
            textHasScrolled = true;
            StartCoroutine(TypeLine());
        }
    }

    IEnumerator StartWaitTime()
    {
        yield return new WaitForSeconds(waitTime);
        StartDialogue();
    }

    IEnumerator TypeLine()
    {
        Debug.Log("type line");
        //playing audio
        audioCont.SetActive(true);

        //type each character 1 by 1
        foreach (char c in line.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed); 
        }
        endScroll?.Invoke();
    }
}
