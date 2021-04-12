using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.Kinect.BodyTracking;

public class OpeningSceneManager : MonoBehaviour
{
     
    //private List<string> poseKeys;
    private List<string> MenuKey;
    public GameObject guideAvatar;
    //private GameObject poseChangerGO;
    //PoseChanger poseChanger;
    public GameObject poseChangerObject;
    public GameObject playerBody;
    //public GameObject poseBody;
    public float tolerance;
    public Material green;
    public Material red;
    private float timer = 0;
    public float timerMax;
    public GameObject MenuUI;
    private bool displayPose;
    public float menuTimerMax;
    



    // Start is called before the first frame update
    void Start()
    {
        //poseChanger = poseChangerGO.AddComponent<PoseChanger>();
        //Debug.Log("Hello There");
        displayPose = true;
        poseChanger.setPoseBody(poseChangerObject);
        poseChanger.LoadData();
        poseChanger.currentPose = 0;
        List<string> unfilteredPoseKeys = new List<string>(poseChanger.poses.Keys);
        MenuKey = unfilteredPoseKeys.FindAll(FindKey);
        SceneManagerStuff.MenuUI = MenuUI;
        SceneManagerStuff.menuTimerMax = menuTimerMax;
        SceneManagerStuff.green = green;
        SceneManagerStuff.red = red;
        SceneManagerStuff.myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/scenes");
        SceneManagerStuff.scenePaths = SceneManagerStuff.myLoadedAssetBundle.GetAllScenePaths();
    }

    private static bool FindKey(string item)
    {

        if (item.Contains("Menu"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (displayPose) {
            poseChanger.DisplayPoseRelative(MenuKey[0]);
        }
        
        Debug.Log(MenuKey[0]);
        guideAvatar.GetComponent<Animator>().SetInteger("animationState", poseChanger.currentPose);
        
        int[] focusPoints = { (int)JointId.HandRight, (int)JointId.HandLeft, (int)JointId.ElbowRight, (int)JointId.ElbowLeft, (int)JointId.KneeRight, (int)JointId.KneeLeft, (int)JointId.FootRight, (int)JointId.FootLeft };
        //float[] distances = new float[8];
        float count = 0;
        for (int i = 0; i < focusPoints.Length; i++)
        {
            if (System.Numerics.Vector3.Distance(PlayerBody.playerPos.JointPositions3D[focusPoints[i]], PlayerBody.guidePos.JointPositions3D[focusPoints[i]]) < tolerance)
            {
                count++;
            }
        }

        Transform[] parts = playerBody.GetComponentsInChildren<Transform>();

        if (count == 8)
        {
            timer += Time.deltaTime;
            for (int i = 1; i < parts.Length; i++)
            {

                parts[i].GetComponent<MeshRenderer>().material = green;

            }
        }
        else
        {

            timer = 0;
            for (int i = 1; i < parts.Length; i++)
            {
                parts[i].GetComponent<MeshRenderer>().material = red;
            }
        }
        if (timer >= timerMax)
        {
            timer = 0;
            Debug.Log("Yay you did it!");
            displayPose = false;
            SceneManagerStuff.MenuUI.SetActive(true);
        }
    }

    
}

public static class SceneManagerStuff
{
    public static AssetBundle myLoadedAssetBundle;
    public static string[] scenePaths;
    public static GameObject MenuUI;
    public static float menuTimerMax;
    public static Material green;
    public static Material red;
}
