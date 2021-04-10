using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Microsoft.Azure.Kinect.BodyTracking;

public class FireFightSceneManager : MonoBehaviour
{
    public float tolerance;
    private List<string> poseKeys;

    private List<Body> firePoses;
    private float timer = 0;
    public float timerMax;
    public VisualEffect rHandFlame;
    public VisualEffect lHandFlame;
    public GameObject poseBody;
    // Start is called before the first frame update
    void Start()
    {
        poseChanger.setPoseBody(poseBody);
        poseChanger.LoadData();
        poseChanger.currentPose = 0;
        List<string> unfilteredPoseKeys = new List<string>(poseChanger.poses.Keys);
        poseKeys = unfilteredPoseKeys.FindAll(item => unfilteredPoseKeys.Contains("FireFight"));
    }

    void Update()
    {
        CreatePoses();
        CheckMoves();
    }

    public void CreatePoses()
    {
        System.Numerics.Vector3 playerPelvis = PlayerBody.playerPos.JointPositions3D[(int)JointId.Pelvis]; ;
        poseKeys = new List<string>(poseChanger.poses.Keys);
        foreach (string poseKey in poseKeys)
        {
            Body examplePose = new Body(poseChanger.poses[poseKey].Length);
            examplePose.JointPositions3D = poseChanger.poses[poseKey];
            examplePose.JointRotations = poseChanger.poseRotations[poseKey];
            poseChanger.DisplayPoseRelative(poseKey);
            System.Numerics.Vector3 pelvis = examplePose.JointPositions3D[(int)JointId.Pelvis];
            System.Numerics.Vector3 diff = playerPelvis - pelvis;
            for (int i = 0; i < examplePose.JointPositions3D.Length; i++)
            {

                examplePose.JointPositions3D[i] = new System.Numerics.Vector3(examplePose.JointPositions3D[i].X + diff.X, examplePose.JointPositions3D[i].Y, examplePose.JointPositions3D[i].Z + diff.Z);
            }
            
            firePoses.Add(examplePose);
        }
    }

    public void CheckMoves()
    {
        CheckWideStance();
    }

    public void CheckWideStance()
    {
        int[] focusPoints = { (int)JointId.KneeRight, (int)JointId.KneeLeft, (int)JointId.FootRight, (int)JointId.FootLeft };
        //float[] distances = new float[8];
        float count = 0;
        for (int i = 0; i < focusPoints.Length; i++)
        {
            if (System.Numerics.Vector3.Distance(PlayerBody.playerPos.JointPositions3D[focusPoints[i]], PlayerBody.guidePos.JointPositions3D[focusPoints[i]]) < tolerance)
            {
                count++;
            }
        }

        //Transform[] parts = playerBody.GetComponentsInChildren<Transform>();

        if (count == 8)
        {
            timer += Time.deltaTime;
            //equivalent to turning green
            rHandFlame.Play();
            lHandFlame.Play();
        }
        else
        {
            rHandFlame.Stop();
            lHandFlame.Stop();
            timer = 0;
            //equivalent to turning red
        }
        if (timer >= timerMax)
        {
            timer = 0;
            Debug.Log("Yay you did it!");
            //execute attack
        }
    }
}
