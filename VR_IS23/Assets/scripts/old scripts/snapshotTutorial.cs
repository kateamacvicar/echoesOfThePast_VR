using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class snapshotTutorial : MonoBehaviour
{
    public SteamVR_Action_Boolean pastToggle;
    public GameObject teleportBlocker;
    public static bool hasFaded = false;

    [Header("transition animation gameobjects")]
    public GameObject presentModel;
    public GameObject presentModelRock;
    public GameObject pastModelWood;
    public GameObject pastModelStretchedWood;
    public GameObject pastModelHandle;

    public float waitTime;

    [Header("Materials to switch to at the end of fade transition")]
    public Material fadePresentMat;
    public Material fadePresentRock;
    public Material opaquePastWood;
    public Material opaquePastStretched;
    public Material opaquePastGold;

    // variables for door fading in/out

    // renderers for all door components
    private Renderer presentRend;
    private Renderer pastWoodRend;
    private Renderer pastStretchedRend;
    private Renderer pastGoldRend;

    // initial color values
    private Color presentColor;
    private Color pastWoodColor;
    private Color pastStretchedColor;
    private Color pastGoldColor;

    // target colors to lerp to
    private Color targetPresent;
    private Color targetPastWood;
    private Color targetPastStretched;
    private Color targetPastGold;

    // variables to combine rocks and fade out
    private MeshFilter[] meshFilters2;
    private CombineInstance[] combine2;
    private Renderer presentRockRend;
    private Color presentRockColor;
    private Color targetPresentRock;

    private IEnumerator coroutine;
    private float elapsedTime = 0f;

    private void Start()
    {
        meshFilters2 = presentModelRock.GetComponentsInChildren<MeshFilter>();
        combine2 = new CombineInstance[meshFilters2.Length];

        int i = 0;
        while (i < meshFilters2.Length)
        {
            combine2[i].mesh = meshFilters2[i].sharedMesh;
            combine2[i].transform = meshFilters2[i].transform.localToWorldMatrix;
            meshFilters2[i].gameObject.SetActive(false);
            i++;
        }

        presentModelRock.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        presentModelRock.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine2);
        presentModelRock.transform.position = Vector3.zero;
        presentModelRock.gameObject.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "door" && pastToggle.GetState(SteamVR_Input_Sources.Any) && !pastModelWood.activeSelf)
        {
            Debug.Log("door triggered in stay");
            hasFaded = true;

            teleportBlocker.SetActive(false);
            pastModelWood.SetActive(true);
            pastModelStretchedWood.SetActive(true);
            pastModelHandle.SetActive(true);

            callCoroutine();
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
        Debug.Log("in coroutine");
        presentRend = presentModel.transform.GetComponent<Renderer>();
        presentColor = presentRend.material.color;
        targetPresent = new Color(presentColor.r, presentColor.g, presentColor.b, 0f);

        presentRockRend = presentModelRock.transform.GetComponent<Renderer>();
        presentRockColor = presentRockRend.material.color;
        targetPresentRock = new Color(presentRockColor.r, presentRockColor.g, presentRockColor.b, 0f);

        pastWoodRend = pastModelWood.transform.GetComponent<Renderer>();
        pastWoodColor = pastWoodRend.material.color;
        targetPastWood = new Color(pastWoodColor.r, pastWoodColor.g, pastWoodColor.b, 1f);

        pastStretchedRend = pastModelStretchedWood.transform.GetComponent<Renderer>();
        pastStretchedColor = pastStretchedRend.material.color;
        targetPastStretched = new Color(pastStretchedColor.r, pastStretchedColor.g, pastStretchedColor.b, 1f);

        pastGoldRend = pastModelHandle.transform.GetComponent<Renderer>();
        pastGoldColor = pastGoldRend.material.color;
        targetPastGold = new Color(pastGoldColor.r, pastGoldColor.g, pastGoldColor.b, 1f);

        // change present material to fade material
        presentRend.material = fadePresentMat;
        presentRockRend.material = fadePresentRock;

        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            //Debug.Log("elapsed time is " + elapsedTime);

            //Debug.Log("decreasing alpha at time " + Time.deltaTime);
            presentRend.material.color = Color.Lerp(presentColor, targetPresent, elapsedTime / waitTime);
            presentRockRend.material.color = Color.Lerp(presentRockColor, targetPresentRock, elapsedTime / waitTime);
            pastWoodRend.material.color = Color.Lerp(pastWoodColor, targetPastWood, elapsedTime / waitTime);
            pastStretchedRend.material.color = Color.Lerp(pastStretchedColor, targetPastStretched, elapsedTime / waitTime);
            pastGoldRend.material.color = Color.Lerp(pastGoldColor, targetPastGold, elapsedTime / waitTime);

            yield return null;
            //Debug.Log("finished while at time " + Time.time);
        }

        //replacing fade materials with opaque ones
        pastWoodRend.material = opaquePastWood;
        pastStretchedRend.material = opaquePastStretched;
        pastGoldRend.material = opaquePastGold;

        presentModel.SetActive(false);
        presentModelRock.SetActive(false);
    }
}
