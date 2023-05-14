using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/*
 * PSEUDOCODE:
 * script on right hand hover point
 * if statement detect if trigger is held down and if we have collided with the old object. 
 * If true:
 * fade in new objects, character model, etc. 
 * fade out old objects
 */

public class snapshot : MonoBehaviour
{

    //public GameObject pastFountain;
    //public GameObject presentFountain;

    /*
    public AudioSource backgroundAudio;
    public GameObject characterModel;
    */

    public SteamVR_Action_Boolean pastToggle;

    [Header("transition animation gameobjects")]
    public GameObject presentModel;
    public GameObject pastModel;
    public GameObject waterObj;
    public GameObject grass;
    public GameObject coinsObj;
    public GameObject charObj;

    public float waitTime;

    [Header("materials to switch to at the end of fade transition")]
    public Material fadePresentMat;
    public Material opaquePastMat;
    public Material coinsOpaque;
    public Material charOpaque;
    public Material grassFade;

    public static bool startText = false;
    public static bool turnOffHalo = false;

    //variables for fountain fading in/out
    private Renderer presentRend;
    private Renderer pastRend;
    private Color presentColor;
    private Color pastColor;
    private Color targetPresent;
    private Color targetPast;

    //variables for water fading in. 
    private Renderer waterRend;
    private Color waterColor;
    private Color targetWaterColor;

    //variables for gold coins to fade in 
    private Renderer coinsRend;
    private Color coinsColor;
    private Color targetCoinsColor;

    //variables for person to fade in
    private Renderer charRend;
    private Color charColor;
    private Color targetCharColor;

    //variables for grass to fade out
    private MeshFilter[] meshFilters;
    private CombineInstance[] combine;
    private Renderer grassRend;
    private Color grassColor;
    private Color targetGrassColor;

    private IEnumerator coroutine;
    private float elapsedTime = 0f;

    private void Start()
    {
        meshFilters = grass.GetComponentsInChildren<MeshFilter>();
        combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }

        grass.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        grass.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        grass.transform.position = Vector3.zero;
        grass.transform.gameObject.SetActive(true);

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "fountain" && pastToggle.GetState(SteamVR_Input_Sources.Any) && !pastModel.activeSelf)
        {
            Debug.Log("fountain triggered in stay");
            turnOffHalo = true;
            pastModel.SetActive(true);
            coinsObj.SetActive(true);
            waterObj.SetActive(true);
            //charObj.SetActive(true);
            callCoroutine();
        }
        else if (other.gameObject.tag == "pastFountain" && pastToggle.GetState(SteamVR_Input_Sources.Any) && textScrolling.isTextDone)
        {

            Debug.Log("text retriggered");
            startText = true;
        }
    }

    private void callCoroutine()
    {
        //Debug.Log("coroutine called");
        coroutine = fade();
        StartCoroutine(coroutine);
    }

    private IEnumerator fade()
    {
        //var presentColl = presentModel.GetComponent<Collider>();
        //presentColl.enabled = false;

        Debug.Log("in coroutine");
        presentRend = presentModel.transform.GetComponent<Renderer>();
        presentColor = presentRend.material.color;
        targetPresent = new Color(presentColor.r, presentColor.g, presentColor.b, 0f);
        //targetPresent = new Color(1f, 1f, 1f, 0f);

        pastRend = pastModel.transform.GetComponent<Renderer>();
        pastColor = pastRend.material.color;
        targetPast = new Color(pastColor.r, pastColor.g, pastColor.b, 1f);
        //targetPast = new Color(0f, 0f, 0f, 1f);

        //fading water in
        waterRend = waterObj.transform.GetComponent<Renderer>();
        waterColor = waterRend.material.color; //should be almost transparent
        targetWaterColor = new Color(waterColor.r, waterColor.g, waterColor.b, 0.3f); //not all the way faded in

        //fading coins in
        coinsRend = coinsObj.transform.GetComponent<Renderer>();
        coinsColor = coinsRend.material.color; //should be transparent
        targetCoinsColor = new Color(coinsColor.r, coinsColor.g, coinsColor.b, 1f);

        //fading character in
        /*charRend = charObj.transform.GetComponent<Renderer>();
        charColor = charRend.material.color; //should start as transparent
        targetCharColor = new Color(charColor.r, charColor.g, charColor.b, 1f);*/

        // fading grass out
        grassRend = grass.transform.GetComponent<Renderer>();
        grassColor = grassRend.material.color;
        targetGrassColor = new Color(grassColor.r, grassColor.g, grassColor.b, 0f);

        Debug.Log("present material before is: " + presentRend.material.name);
        presentRend.material = fadePresentMat;
        Debug.Log("present material is: " + presentRend.material.name);
        grassRend.material = grassFade;

        startText = true;
        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            //Debug.Log("elapsed time is " + elapsedTime);

            //Debug.Log("decreasing alpha at time " + Time.deltaTime);

            // objects to fade out
            presentRend.material.color = Color.Lerp(presentColor, targetPresent, elapsedTime / waitTime);
            //Debug.Log("present fountain color: " + presentRend.material.color);

            grassRend.material.color = Color.Lerp(grassColor, targetGrassColor, elapsedTime / waitTime);
            //Debug.Log("grass color: " + grassRend.material.color);

            /*pastRend.material.color = Color.Lerp(pastColor, targetPast, elapsedTime / waitTime);

            waterRend.material.color = Color.Lerp(waterColor, targetWaterColor, elapsedTime / waitTime);

            coinsRend.material.color = Color.Lerp(coinsColor, targetCoinsColor, elapsedTime / waitTime);*/

            //charRend.material.color = Color.Lerp(charColor, targetCharColor, elapsedTime / waitTime);

            yield return null;
            //Debug.Log("finished while at time " + Time.time);
        }
        presentModel.SetActive(false);
        grass.SetActive(false);

        //yield return new WaitForSeconds(waitTime / 2);

        float halfTime = 0f;
        while (elapsedTime >= waitTime && elapsedTime < waitTime * 2)
        {
            //Debug.Log("within second while loop");

            elapsedTime += Time.deltaTime;
            halfTime += Time.deltaTime;
            //Debug.Log("elapsed time: " + elapsedTime);

            // objects to fade in
            pastRend.material.color = Color.Lerp(pastColor, targetPast, halfTime / waitTime);

            //pastColor.a = Mathf.Lerp(pastColor.a, 1f, halfTime / waitTime);
            //Debug.Log("past color is: " + pastRend.material.color);

            //Debug.Log("past fountain color: " + pastRend.material.color);
            //Debug.Log("elapsed time: " + elapsedTime);

            coinsRend.material.color = Color.Lerp(coinsColor, targetCoinsColor, halfTime / waitTime);
            //Debug.Log("coin color: " + coinsRend.material.color);

            waterRend.material.color = Color.Lerp(waterColor, targetWaterColor, halfTime / waitTime);
            //Debug.Log("water color: " + waterRend.material.color);

            //charRend.material.color = Color.Lerp(charColor, targetCharColor, elapsedTime / waitTime);

            yield return null;
        }

        //replacing fade materials with opaque ones
        pastRend.material = opaquePastMat;
        coinsRend.material = coinsOpaque;
        //charRend.material = charOpaque;

        /*presentModel.SetActive(false);
        grass.SetActive(false);*/
        // startText = true;

        Rigidbody rb = pastModel.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
