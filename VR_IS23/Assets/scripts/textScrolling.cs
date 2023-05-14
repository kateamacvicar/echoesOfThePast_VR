using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR;
using UnityEngine.UI;

/*
 * PSEUDOCODE:
 * script on PastWords Game Object
 * waits for the past model to fade in then 
 * creates a text scroll with audio narration
 * the canvas translates depending on where the guest is looking. 
 * After 10 seconds, the text disappears. 
 *  
 */

public class textScrolling : MonoBehaviour
{
    public Transform playerCamera; // need direction camera is looking
    public Transform player;
    public Transform pastModel;
  
    public TextMeshProUGUI textComponent;
    public string line;
    public float spawnDistance;
    [Tooltip("range from transform of model to form boundary with")]
    public float range = 1.0f;
    [Tooltip("time between each char, the smaller the faster")]
    public float textSpeed;
    public float waitTime;
    public float fadeTime;

    public static bool isTextDone = false;
    public static bool startText = false;

    public SteamVR_Action_Boolean teleportAction;
    public Transform[] textPoints;

    public AudioClip audioClip;
    public AudioSource source;

    //coin + water audio source
    public GameObject coinClinkSource;
    public GameObject waterDropSource;
    public GameObject crowdSource;
 
    private bool rotateOn = false;
    private bool teleportRotate = false;


    // Update is called once per frame
    void Update()
    {
        //Debug.Log("bool start text is: " + snapshot.startText);
        // if variable in snapshot script is true
        // start coroutine
        if (startText)
        {
            StartDialogue();
        }

        if (rotateOn)
        {
            transform.LookAt(transform.position + playerCamera.forward);
        }

        if (teleportRotate && teleportAction.GetStateUp(SteamVR_Input_Sources.Any))
        {
            StartCoroutine(rotateWait());
        }
    }


    void StartDialogue()
    {
        //Debug.Log("starting dialogue");
        textComponent.text = ""; //make sures it starts off w/o text
        startText = false;
        teleportRotate = false;
        //Debug.Log("forward of canvas is: " + transform.forward);
        //Debug.Log("camera look direction is: " + playerCamera.forward);
        // rotate and place canvas relative to the player
        BillboardText();

        StartCoroutine(TypeLine());
    }

    private void BillboardText()
    {

        transform.position = playerCamera.position + playerCamera.forward * spawnDistance;

        float minValue = 1000f;
        int lowestIndex = -1;

        // find the closest point to new position of text
        for (int i = 0; i < textPoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, textPoints[i].position);

            if (distance < minValue)
            {
                minValue = distance;
                lowestIndex = i;
            }
        }

        transform.position = textPoints[lowestIndex].position;
        rotateOn = true;
    }

    IEnumerator rotateWait()
    {
        yield return new WaitForSeconds(0.5f);
        transform.LookAt(transform.position + playerCamera.forward);
    }

    IEnumerator TypeLine()
    {
        isTextDone = false;
        yield return new WaitForSeconds(fadeTime); //must be same amount of time as fade transition

        rotateOn = false;
        //playing audio
        source.Play();
        crowdSource.SetActive(true);

        foreach (char c in line.ToCharArray())
        {
          textComponent.text += c;
          yield return new WaitForSeconds(textSpeed);
        }

        teleportRotate = true;

        coinClinkSource.SetActive(true);
        yield return new WaitForSeconds(1);
        waterDropSource.SetActive(true);

        yield return new WaitForSeconds(waitTime);
        //sounds play onAwake, so we want to set them inactive so they play again when the text scrolls again. 
        coinClinkSource.SetActive(false);
        waterDropSource.SetActive(false);
        textComponent.text = "";
        isTextDone = true;
    }
}
