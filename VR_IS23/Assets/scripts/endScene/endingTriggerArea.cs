using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PSEUDOCODE:
 * script on trigger area for ending scene. 
 * If the player enters the trigger, canvas appears asking them if they want to end the game. 
 *  
 */

public class endingTriggerArea : MonoBehaviour
{
    public GameObject optionsCanvas;
    public GameObject laserObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // pull up ui with options on if they want to end the game
            optionsCanvas.SetActive(true);
            // laser turns on, as does the controller hint
            laserObject.SetActive(true);

            // if yes, screen fades to black, ending message (?), restart scene
            // if no, nothing happens and ui disappears until they enter trigger area again
        }
    }

    // need a laser pointer script that is only on for this portion of the game
    // should come with a controller hint on button to press to confirm choice

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            optionsCanvas.SetActive(false);
            laserObject.SetActive(false);
        }
    }
}
