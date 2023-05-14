using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class restartScene : MonoBehaviour
{
    public SteamVR_Action_Boolean restartButton;

    private void Update()
    {
        if (restartButton.GetState(SteamVR_Input_Sources.RightHand) && restartButton.GetState(SteamVR_Input_Sources.LeftHand))
        {
            Debug.Log("scene restarted");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
