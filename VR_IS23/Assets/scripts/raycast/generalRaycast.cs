using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generalRaycast : MonoBehaviour
{
    /*
 * PSEUDOCODE:
 * script on camera/empty object positioned near "head"
 * create invisible ray object to represent the guest's line of sight.
 * activate halo effect around "sighted" object. also activate haptic feedback. 
 * deactvaite halo effect when we are not looking at it. 
 * do math to calculate the distance between the guest and the object. This distance will determine the radius of our ray. 
 */

    private ParticleSystem halo;


    public float raycastRadius;
    public float raycastMaxDist;

    private Renderer rend;
    private bool hittingTarget;
    private LayerMask mask;
    private int raycastLayer;

    private Vector3 origin;

    private string objectTag;

    private void Start()
    {
        mask = LayerMask.GetMask("raycast", "barrier");
        raycastLayer = LayerMask.NameToLayer("raycast");
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        // origin placed slightly behind camera to fix raycastSphere starting within object collider
        origin = transform.TransformPoint(new Vector3(transform.localPosition.x, 0, transform.localPosition.z - raycastRadius * 2));

        if (Physics.SphereCast(origin, raycastRadius, transform.forward, out hit, raycastMaxDist, mask) && !simplifiedSnapshot.turnOffHalo)
        {
            //if the layer of the object you hit is raycast, play the particle system around the object you hit.
            //This particel system is the "halo"
            if (hit.transform.gameObject.layer == raycastLayer) 
            {
                //Debug.Log("raycast hit: " + hit.transform.gameObject.name);
                hittingTarget = true;
                rend = hit.transform.GetComponent<Renderer>();
   
                objectTag = hit.transform.gameObject.tag;
                halo = hit.transform.gameObject.GetComponentInChildren<ParticleSystem>();
                //Debug.Log("halo is " + halo.name);
                halo.Play();

            }
            else // hitting something, just not the target
            {
                //Debug.Log("not hitting target");
                hittingTarget = false;
                if (halo != null) // og was rend != null
                {
                    halo.Stop();
                }
            }
        }
        else if (hittingTarget && !simplifiedSnapshot.turnOffHalo) // currently not hitting anything but it was just hitting the target - we've just looked away
        {
            hittingTarget = false;
            if (halo != null)
            {
                halo.Stop();
            }
        }
    }

    //helper function to visualize the raycast
    private void OnDrawGizmos()
    {
/*        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(origin + transform.forward * raycastMaxDist, raycastRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(origin, raycastRadius);

        Vector3 rayDir = ((transform.position + transform.forward * raycastMaxDist) - origin).normalized; //not needed??
        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + transform.forward * raycastMaxDist);*/
    }
}
