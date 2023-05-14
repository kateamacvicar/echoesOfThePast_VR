using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PSEUDOCODE:
 * script on "locked" objects. 
 *  Controls what happens when the player gets the key and unlocks a door. 
 */

public class unlockDoor : MonoBehaviour
{
    public bool hasKey;

    public Animator doorAnim;

    public AudioSource doorOpen;

    public GameObject keyPastDoor;
    public GameObject keyHand;

    public GameObject lockedDoorWords;


    public void playAnimation()
    {
        if (hasKey)
        {
            
            doorAnim.enabled = true; //start animation
            doorOpen.enabled = true; //start door opening sound

            //show key in lock
            keyPastDoor.SetActive(true);
            //deactivate key object in hand
            keyHand.SetActive(false);
            //in the case of locked door only, deactivate canvas
            lockedDoorWords.SetActive(false);
        }
    }

    public void holdingKey()
    {
        hasKey = true;
        Debug.Log("holding key");
    }
}
