using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateFountainHalo : MonoBehaviour
{
    //script on the particle system 
    public Transform playerCamera; // need direction camera is looking
    public Transform player;
    public Transform fountain;

    public Transform halo;
    
   

    private void Update()
    {

        transform.LookAt(player.position);



        /*Vector3 playerVector = player.position;
        Vector3 fountainVector = fountain.position;

        Vector3 direction = fountainVector - playerVector;
        Debug.Log("direction:" + direction);

        if (direction.x > 0 && direction.z > 0) //to the left (+,+)
        {
            float rotationy = halo.rotation.y;
            rotationy = 100;
        }
        else if (direction.x < 0 && direction.z < 0) //to the right(-,-)
        {
            float rotationy = halo.rotation.y;
            rotationy = 100;
        }
        else if (direction.x < 0 && direction.z > 0) //to the top (-,+)
        {
            float rotationy = halo.rotation.y;
            rotationy = 30;
        }
        else if (direction.x > 0 && direction.z < 0) //to the bottom (+,-)
        {
            float rotationy = halo.rotation.y;
            rotationy = 30;
        }*/
    }

    //gets the position of one object relative to another. 
    //get fountain positon from the transform
    //get player position from the transform
    //Subtract Fountain from Player, get the vector from Fountain to Player. 

    

}

