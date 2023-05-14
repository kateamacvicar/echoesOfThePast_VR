using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportAreas : MonoBehaviour
{
    //attached to pastDoorCont object 
    public GameObject[] teleAreas;

    public void turnOnAreas()
    {
        for (int i = 0; i < teleAreas.Length; i++)
        {
            teleAreas[i].SetActive(true);
        }
    }
}
