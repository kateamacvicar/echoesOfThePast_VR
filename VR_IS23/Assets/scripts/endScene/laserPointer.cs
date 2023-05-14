using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Valve.VR;
using Valve.VR.Extras;
using UnityEngine.SceneManagement;

public class laserPointer : MonoBehaviour
{
    // currently attached to the player

    private SteamVR_LaserPointer laser; //delegate
    public GameObject laserObject;
    public GameObject optionsCanvas;
    public GameObject thanksCanvas;

    public GameObject snapTurnObject;
    public GameObject teleportObject;


    public string yesButtonTag;
    public string noButtonTag;

    // variables needed to fade to black
    public float fadeTime;
    public float restartTime;

    // awake subscription to laserPointer methods
    private void Awake()
    {
        laser = laserObject.GetComponent<SteamVR_LaserPointer>();

        laser.PointerIn += PointerInside;
        laser.PointerOut += PointerOutside;
        laser.PointerClick += PointerClicked;
    }

    // detect when laser hits a "hittable" object
    private void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.tag == yesButtonTag || e.target.tag == noButtonTag)
        {
            Debug.Log("hit");
            // should we like have an outline or something?
        }
    }

    private void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.tag == yesButtonTag || e.target.tag == noButtonTag)
        {
            Debug.Log("out");
            // reverses whatever is done in pointerInside
        }
    }

    // if yes, screen fades to black, ending message (?), restart scene
    // if no, nothing happens and ui disappears until they enter trigger area again
    private void PointerClicked(object sender, PointerEventArgs e)
    {
        if (e.target.tag == yesButtonTag)
        {
            Debug.Log("clicked yes");

            // fade to black by having a UI with a black panel covering everything
            // and then fade in using jordan's script
            // ACTUALLY just use steamVR's fade script lol
            //startFadeBlack = true;
            snapTurnObject.SetActive(false);
            teleportObject.SetActive(false);
            laserObject.SetActive(false);
            optionsCanvas.SetActive(false);
            thanksCanvas.SetActive(true);
            StartCoroutine(endingScene());

                // so text will probably be world space text? 
                    // actually, could be screen space bc idk how else to get the text to show in front of black panel
        }
        else if (e.target.tag == noButtonTag)
        {
            Debug.Log("clicked no");

            optionsCanvas.SetActive(false);
        }
    }

    IEnumerator endingScene()
    {
        yield return new WaitForSeconds(fadeTime);
        SteamVR_Fade.Start(Color.black, fadeTime);
        yield return new WaitForSeconds(restartTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
