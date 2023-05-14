using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycasting : MonoBehaviour

{

    /*
 * PSEUDOCODE:
 * script on camera/empty object positioned near "head"
 * create invisible ray object to represent the guest's line of sight.
 * activate halo effect around "sighted" object. also activate haptic feedback. 
 * deactvaite halo effect when we are not looking at it. 
 * do math to calculate the distance between the guest and the object. This distance will determine the radius of our ray. 
 */
    //public GameObject setactive; //??

    public string triggerTag;
    public Material haloMat;
    public Material originalMat;
    public Material switchMat; //remove

    public float raycastRadius;
    public float raycastMaxDist;

    private Renderer rend;
    private bool hittingTarget;
    private LayerMask mask;
    private int raycastLayer;

    private Vector3 origin;

    private void Start()
    {
        mask = LayerMask.GetMask("raycast");
        raycastLayer = LayerMask.NameToLayer("raycast");
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        // origin placed slightly behind camera to fix raycastSphere starting within object collider
        origin = transform.TransformPoint(new Vector3(transform.localPosition.x, 0, transform.localPosition.z - raycastRadius * 2));

        //origin = new Vector3(transform.position.x - raycastRadius * 2, transform.position.y, transform.position.z);

        if (snapshot.turnOffHalo)
        {
            rend.material = originalMat;
        }

        // cast a sphere forward from the camera and see what it hits

        // the turnOffHalo is a problem... 
        if (Physics.SphereCast(origin, raycastRadius, transform.forward, out hit, raycastMaxDist, mask) && !simplifiedSnapshot.turnOffHalo)
        {
            //Debug.Log("raycast hit: " + hit.transform.gameObject.name);
            if (hit.transform.gameObject.layer == raycastLayer)
            {
                //Debug.Log("raycast hit: " + hit.transform.gameObject.name);
                hittingTarget = true;
                rend = hit.transform.GetComponent<Renderer>();
                // store original material
                //originalMat = rend.material;
                rend.material = haloMat;
            }
            else // hitting something, just not the target
            {
                //Debug.Log("not hitting target");
                hittingTarget = false;
                if (rend != null)
                {
                    //Debug.Log("rend already grabbed: " + rend.gameObject.name);
                    rend.material = originalMat;
                }
            }

            if (hit.transform.gameObject.tag == triggerTag) // untagged grass as "fountain" for this
            {
                //Debug.Log("raycast hit: " + hit.transform.gameObject.name);
                hittingTarget = true;
                rend = hit.transform.GetComponent<Renderer>();
                // store original material
                //originalMat = rend.material;
                rend.material = haloMat;
            }
            // if we remain using the layerMask, this entire else statement can be deleted
            else // hitting something, just not the target
            {
                //Debug.Log("not hitting target");
                hittingTarget = false;
                if (rend != null)
                {
                    //Debug.Log("rend already grabbed: " + rend.gameObject.name);
                    rend.material = originalMat;
                }
            }
        }
        else if (hittingTarget) // but currently not hitting anything
        {
            hittingTarget = false;
            if (rend != null)
            {
                rend.material = originalMat;
            }
        }

        if (simplifiedSnapshot.turnOffHalo)
        {
            rend.material = switchMat; // problem occurs when you have two of the same script... says there's no object reference
        }

    }

    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(origin + transform.forward * raycastMaxDist, raycastRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(origin, raycastRadius);

        Vector3 rayDir = ((transform.position + transform.forward * raycastMaxDist) - origin).normalized; //not needed??
        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + transform.forward * raycastMaxDist);*/
    }
}
