using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CancelTeleportHint : MonoBehaviour
{
    private void Update()
    {
        Teleport.instance.CancelTeleportHint();
    }
}
