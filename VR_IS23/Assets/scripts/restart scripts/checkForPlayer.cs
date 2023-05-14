using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkForPlayer : MonoBehaviour
{
    private static GameObject playerInstance;

    private void Awake()
    {
        if (playerInstance == null)
        {
            playerInstance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
