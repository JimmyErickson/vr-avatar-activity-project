using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.Kinect.BodyTracking;
using System.Collections;
using System;
using System.IO;
using System.Text;

public class TrackerHandler : MonoBehaviour
{
    
    public Quaternion[] absoluteJointRotations = new Quaternion[(int)JointId.Count];
    public bool drawSkeletons = true;
    Quaternion Y_180_FLIP = new Quaternion(0.0f, 1.0f, 0, 90.0f);
    public GameObject headset;
    //public GameObject poseSaver;

    // Start is called before the first frame update
    void Awake()
    {
        BodyReference.parentJointMap = new Dictionary<JointId, JointId>();

        // pelvis has no parent so set to count
        BodyReference.parentJointMap[JointId.Pelvis] = JointId.Count;
        BodyReference.parentJointMap[JointId.SpineNavel] = JointId.Pelvis;
        BodyReference.parentJointMap[JointId.SpineChest] = JointId.SpineNavel;
        BodyReference.parentJointMap[JointId.Neck] = JointId.SpineChest;
        BodyReference.parentJointMap[JointId.ClavicleLeft] = JointId.SpineChest;
        BodyReference.parentJointMap[JointId.ShoulderLeft] = JointId.ClavicleLeft;
        BodyReference.parentJointMap[JointId.ElbowLeft] = JointId.ShoulderLeft;
        BodyReference.parentJointMap[JointId.WristLeft] = JointId.ElbowLeft;
        BodyReference.parentJointMap[JointId.HandLeft] = JointId.WristLeft;
        BodyReference.parentJointMap[JointId.HandTipLeft] = JointId.HandLeft;
        BodyReference.parentJointMap[JointId.ThumbLeft] = JointId.HandLeft;
        BodyReference.parentJointMap[JointId.ClavicleRight] = JointId.SpineChest;
        BodyReference.parentJointMap[JointId.ShoulderRight] = JointId.ClavicleRight;
        BodyReference.parentJointMap[JointId.ElbowRight] = JointId.ShoulderRight;
        BodyReference.parentJointMap[JointId.WristRight] = JointId.ElbowRight;
        BodyReference.parentJointMap[JointId.HandRight] = JointId.WristRight;
        BodyReference.parentJointMap[JointId.HandTipRight] = JointId.HandRight;
        BodyReference.parentJointMap[JointId.ThumbRight] = JointId.HandRight;
        BodyReference.parentJointMap[JointId.HipLeft] = JointId.SpineNavel;
        BodyReference.parentJointMap[JointId.KneeLeft] = JointId.HipLeft;
        BodyReference.parentJointMap[JointId.AnkleLeft] = JointId.KneeLeft;
        BodyReference.parentJointMap[JointId.FootLeft] = JointId.AnkleLeft;
        BodyReference.parentJointMap[JointId.HipRight] = JointId.SpineNavel;
        BodyReference.parentJointMap[JointId.KneeRight] = JointId.HipRight;
        BodyReference.parentJointMap[JointId.AnkleRight] = JointId.KneeRight;
        BodyReference.parentJointMap[JointId.FootRight] = JointId.AnkleRight;
        BodyReference.parentJointMap[JointId.Head] = JointId.Pelvis;
        BodyReference.parentJointMap[JointId.Nose] = JointId.Head;
        BodyReference.parentJointMap[JointId.EyeLeft] = JointId.Head;
        BodyReference.parentJointMap[JointId.EarLeft] = JointId.Head;
        BodyReference.parentJointMap[JointId.EyeRight] = JointId.Head;
        BodyReference.parentJointMap[JointId.EarRight] = JointId.Head;

        Vector3 zpositive = Vector3.forward;
        Vector3 xpositive = Vector3.right;
        Vector3 ypositive = Vector3.up;
        // spine and left hip are the same
        Quaternion leftHipBasis = Quaternion.LookRotation(xpositive, -zpositive);
        Quaternion spineHipBasis = Quaternion.LookRotation(xpositive, -zpositive);
        Quaternion rightHipBasis = Quaternion.LookRotation(xpositive, zpositive);
        // arms and thumbs share the same basis
        Quaternion leftArmBasis = Quaternion.LookRotation(ypositive, -zpositive);
        Quaternion rightArmBasis = Quaternion.LookRotation(-ypositive, zpositive);
        Quaternion leftHandBasis = Quaternion.LookRotation(-zpositive, -ypositive);
        Quaternion rightHandBasis = Quaternion.identity;
        Quaternion leftFootBasis = Quaternion.LookRotation(xpositive, ypositive);
        Quaternion rightFootBasis = Quaternion.LookRotation(xpositive, -ypositive);

        BodyReference.basisJointMap = new Dictionary<JointId, Quaternion>();

        // pelvis has no parent so set to count
        BodyReference.basisJointMap[JointId.Pelvis] = spineHipBasis;
        BodyReference.basisJointMap[JointId.SpineNavel] = spineHipBasis;
        BodyReference.basisJointMap[JointId.SpineChest] = spineHipBasis;
        BodyReference.basisJointMap[JointId.Neck] = spineHipBasis;
        BodyReference.basisJointMap[JointId.ClavicleLeft] = leftArmBasis;
        BodyReference.basisJointMap[JointId.ShoulderLeft] = leftArmBasis;
        BodyReference.basisJointMap[JointId.ElbowLeft] = leftArmBasis;
        BodyReference.basisJointMap[JointId.WristLeft] = leftHandBasis;
        BodyReference.basisJointMap[JointId.HandLeft] = leftHandBasis;
        BodyReference.basisJointMap[JointId.HandTipLeft] = leftHandBasis;
        BodyReference.basisJointMap[JointId.ThumbLeft] = leftArmBasis;
        BodyReference.basisJointMap[JointId.ClavicleRight] = rightArmBasis;
        BodyReference.basisJointMap[JointId.ShoulderRight] = rightArmBasis;
        BodyReference.basisJointMap[JointId.ElbowRight] = rightArmBasis;
        BodyReference.basisJointMap[JointId.WristRight] = rightHandBasis;
        BodyReference.basisJointMap[JointId.HandRight] = rightHandBasis;
        BodyReference.basisJointMap[JointId.HandTipRight] = rightHandBasis;
        BodyReference.basisJointMap[JointId.ThumbRight] = rightArmBasis;
        BodyReference.basisJointMap[JointId.HipLeft] = leftHipBasis;
        BodyReference.basisJointMap[JointId.KneeLeft] = leftHipBasis;
        BodyReference.basisJointMap[JointId.AnkleLeft] = leftHipBasis;
        BodyReference.basisJointMap[JointId.FootLeft] = leftFootBasis;
        BodyReference.basisJointMap[JointId.HipRight] = rightHipBasis;
        BodyReference.basisJointMap[JointId.KneeRight] = rightHipBasis;
        BodyReference.basisJointMap[JointId.AnkleRight] = rightHipBasis;
        BodyReference.basisJointMap[JointId.FootRight] = rightFootBasis;
        BodyReference.basisJointMap[JointId.Head] = spineHipBasis;
        BodyReference.basisJointMap[JointId.Nose] = spineHipBasis;
        BodyReference.basisJointMap[JointId.EyeLeft] = spineHipBasis;
        BodyReference.basisJointMap[JointId.EarLeft] = spineHipBasis;
        BodyReference.basisJointMap[JointId.EyeRight] = spineHipBasis;
        BodyReference.basisJointMap[JointId.EarRight] = spineHipBasis;
    }

    public void updateTracker(BackgroundData trackerFrameData)
    {
        //this is an array in case you want to get the n closest bodies
        int closestBody = findClosestTrackedBody(trackerFrameData);
        

        // render the closest body
        Body skeleton = trackerFrameData.Bodies[closestBody];
        //Vector3 rightEye = new Vector3(skeleton.JointPositions3D[(int)JointId.EyeRight].X, skeleton.JointPositions3D[(int)JointId.EyeRight].Y, skeleton.JointPositions3D[(int)JointId.EyeRight].Z);
        //Vector3 leftEye = new Vector3(skeleton.JointPositions3D[(int)JointId.EyeLeft].X, skeleton.JointPositions3D[(int)JointId.EyeLeft].Y, skeleton.JointPositions3D[(int)JointId.EyeLeft].Z);
        //Vector3 midPoint = rightEye + (leftEye - rightEye) / 2;
        //Vector3 diff = headset.GetComponent<Transform>().position - midPoint;
        Vector3 headPos = new Vector3(skeleton.JointPositions3D[(int)JointId.Head].X, skeleton.JointPositions3D[(int)JointId.Head].Y, skeleton.JointPositions3D[(int)JointId.Head].Z);
        Vector3 diff = headset.GetComponent<Transform>().position - headPos;
        System.Numerics.Vector3 kinectDiff = new System.Numerics.Vector3(diff.x, diff.y, diff.z);
        for (int i = 0; i < skeleton.JointPositions3D.Length; i++)
        {
            skeleton.JointPositions3D[i] = new System.Numerics.Vector3(skeleton.JointPositions3D[i].X + kinectDiff.X, skeleton.JointPositions3D[i].Y, skeleton.JointPositions3D[i].Z + kinectDiff.Z);
        }
        
        //Keep this, it's for saving poses.
        /*if (poseSaver.activeSelf)
        {
            Debug.Log("Lookin' Good!");
            saveCurrentPose(skeleton);
            poseSaver.SetActive(false);
        }*/
        PlayerBody.playerPos = skeleton;
        renderSkeleton(skeleton, 0, diff);
    }

    public void saveCurrentPose(Body skeleton)
    {
        saveCurrentPoseRotation(skeleton);
        string poseFilePath = "C:/Users/vrcart01/Desktop/vr-avatar-activity-project/Assets/Poses/PosesFile.csv";
        //C:\Users\vrcart03\Desktop\vr - avatar - activity - project\Assets\Poses
        string seperator = ",";
        StringBuilder outputString = new StringBuilder();

        string[] positions = new string[skeleton.JointPositions3D.Length];

        for (int i = 0; i < skeleton.JointPositions3D.Length; i++)
        {
            positions[i] = string.Join(seperator, skeleton.JointPositions3D[i]).Trim('<', '>');
        }

        outputString.AppendLine(string.Join(seperator, positions));

        string trimmedOutput = outputString.ToString().Trim('<', '>');

        File.AppendAllText(poseFilePath, trimmedOutput);
    }

    public void saveCurrentPoseRotation(Body skeleton)
    {
        string poseFilePath = "C:/Users/vrcart01/Desktop/vr-avatar-activity-project/Assets/Poses/PosesRotationsFile.csv";
        //C:\Users\vrcart03\Desktop\vr - avatar - activity - project\Assets\Poses
        string seperator = ",";
        StringBuilder outputString = new StringBuilder();

        string[] positions = new string[skeleton.JointRotations.Length];

        for (int i = 0; i < skeleton.JointRotations.Length; i++)
        {
            positions[i] = string.Join(seperator, skeleton.JointRotations[i]).Trim('<', '>');
        }

        outputString.AppendLine(string.Join(seperator, positions));

        string trimmedOutput = outputString.ToString().Trim('<', '>');

        File.AppendAllText(poseFilePath, trimmedOutput);
    }

    int findIndexFromId(BackgroundData frameData, int id)
    {
        int retIndex = -1;
        for (int i = 0; i < (int)frameData.NumOfBodies; i++)
        {
            if ((int)frameData.Bodies[i].Id == id)
            {
                retIndex = i;
                break;
            }
        }
        return retIndex;
    }

    private int findClosestTrackedBody(BackgroundData trackerFrameData)
    {
        int closestBody = -1;
        const float MAX_DISTANCE = 5000.0f;
        float minDistanceFromKinect = MAX_DISTANCE;
        for (int i = 0; i < (int)trackerFrameData.NumOfBodies; i++)
        {
            var pelvisPosition = trackerFrameData.Bodies[i].JointPositions3D[(int)JointId.Pelvis];
            Vector3 pelvisPos = new Vector3((float)pelvisPosition.X, (float)pelvisPosition.Y, (float)pelvisPosition.Z);
            if (pelvisPos.magnitude < minDistanceFromKinect)
            {
                closestBody = i;
                minDistanceFromKinect = pelvisPos.magnitude;
            }
        }
        return closestBody;
    }

    public void turnOnOffSkeletons()
    {
        drawSkeletons = !drawSkeletons;
        const int bodyRenderedNum = 0;
        for (int jointNum = 0; jointNum < (int)JointId.Count; jointNum++)
        {
            transform.GetChild(bodyRenderedNum).GetChild(jointNum).gameObject.GetComponent<MeshRenderer>().enabled = drawSkeletons;
            transform.GetChild(bodyRenderedNum).GetChild(jointNum).GetChild(0).GetComponent<MeshRenderer>().enabled = drawSkeletons;
        }
    }

    public void renderSkeleton(Body skeleton, int skeletonNumber, Vector3 diff)
    {
        System.Numerics.Vector3 kinectDiff = new System.Numerics.Vector3(diff.x, diff.y, diff.z);
        for (int jointNum = 0; jointNum < (int)JointId.Count; jointNum++)
        {
            Vector3 jointPos = new Vector3(skeleton.JointPositions3D[jointNum].X, -skeleton.JointPositions3D[jointNum].Y, skeleton.JointPositions3D[jointNum].Z);
            Vector3 offsetPosition = transform.rotation * jointPos;
            Vector3 positionInTrackerRootSpace = transform.position + offsetPosition;
            Quaternion jointRot = Y_180_FLIP * new Quaternion(skeleton.JointRotations[jointNum].X, -skeleton.JointRotations[jointNum].Y,
                skeleton.JointRotations[jointNum].Z, skeleton.JointRotations[jointNum].W) * Quaternion.Inverse(BodyReference.basisJointMap[(JointId)jointNum]);
            absoluteJointRotations[jointNum] = jointRot;
            // these are absolute body space because each joint has the body root for a parent in the scene graph
            transform.GetChild(skeletonNumber).GetChild(jointNum).localPosition = jointPos;
            transform.GetChild(skeletonNumber).GetChild(jointNum).localRotation = jointRot;

            const int boneChildNum = 0;
            if (BodyReference.parentJointMap[(JointId)jointNum] != JointId.Head && BodyReference.parentJointMap[(JointId)jointNum] != JointId.Count)
            {
                Vector3 parentTrackerSpacePosition = new Vector3(skeleton.JointPositions3D[(int)BodyReference.parentJointMap[(JointId)jointNum]].X,
                    -skeleton.JointPositions3D[(int)BodyReference.parentJointMap[(JointId)jointNum]].Y, skeleton.JointPositions3D[(int)BodyReference.parentJointMap[(JointId)jointNum]].Z);
                Vector3 boneDirectionTrackerSpace = jointPos - parentTrackerSpacePosition;
                Vector3 boneDirectionWorldSpace = transform.rotation * boneDirectionTrackerSpace;
                Vector3 boneDirectionLocalSpace = Quaternion.Inverse(transform.GetChild(skeletonNumber).GetChild(jointNum).rotation) * Vector3.Normalize(boneDirectionWorldSpace);
                transform.GetChild(skeletonNumber).GetChild(jointNum).GetChild(boneChildNum).localScale = new Vector3(1, 20.0f * 0.5f * boneDirectionWorldSpace.magnitude, 1);
                transform.GetChild(skeletonNumber).GetChild(jointNum).GetChild(boneChildNum).localRotation = Quaternion.FromToRotation(Vector3.up, boneDirectionLocalSpace);
                transform.GetChild(skeletonNumber).GetChild(jointNum).GetChild(boneChildNum).position = transform.GetChild(skeletonNumber).GetChild(jointNum).position - 0.5f * boneDirectionWorldSpace;
            }
            else
            {
                transform.GetChild(skeletonNumber).GetChild(jointNum).GetChild(boneChildNum).gameObject.SetActive(false);
            }
        }
    }

    public Quaternion GetRelativeJointRotation(JointId jointId)
    {
        JointId parent = BodyReference.parentJointMap[jointId];
        Quaternion parentJointRotationBodySpace = Quaternion.identity;
        if (parent == JointId.Count)
        {
            parentJointRotationBodySpace = Y_180_FLIP;
        }
        else
        {
            parentJointRotationBodySpace = absoluteJointRotations[(int)parent];
        }
        Quaternion jointRotationBodySpace = absoluteJointRotations[(int)jointId];
        Quaternion relativeRotation =  Quaternion.Inverse(parentJointRotationBodySpace) * jointRotationBodySpace;

        return relativeRotation;
    }

}

public static class PlayerBody{
    public static Body playerPos;
    public static Body guidePos;
    public static Body menuPos;
}

public static class BodyReference{
    public static Dictionary<JointId, JointId> parentJointMap;
    public static Dictionary<JointId, Quaternion> basisJointMap;

    
    
}

/*
parentJointMap = new Dictionary<JointId, JointId>();

    // pelvis has no parent so set to count
    parentJointMap[JointId.Pelvis] = JointId.Count;
    parentJointMap[JointId.SpineNavel] = JointId.Pelvis;
    parentJointMap[JointId.SpineChest] = JointId.SpineNavel;
    parentJointMap[JointId.Neck] = JointId.SpineChest;
    parentJointMap[JointId.ClavicleLeft] = JointId.SpineChest;
    parentJointMap[JointId.ShoulderLeft] = JointId.ClavicleLeft;
    parentJointMap[JointId.ElbowLeft] = JointId.ShoulderLeft;
    parentJointMap[JointId.WristLeft] = JointId.ElbowLeft;
    parentJointMap[JointId.HandLeft] = JointId.WristLeft;
    parentJointMap[JointId.HandTipLeft] = JointId.HandLeft;
    parentJointMap[JointId.ThumbLeft] = JointId.HandLeft;
    parentJointMap[JointId.ClavicleRight] = JointId.SpineChest;
    parentJointMap[JointId.ShoulderRight] = JointId.ClavicleRight;
    parentJointMap[JointId.ElbowRight] = JointId.ShoulderRight;
    parentJointMap[JointId.WristRight] = JointId.ElbowRight;
    parentJointMap[JointId.HandRight] = JointId.WristRight;
    parentJointMap[JointId.HandTipRight] = JointId.HandRight;
    parentJointMap[JointId.ThumbRight] = JointId.HandRight;
    parentJointMap[JointId.HipLeft] = JointId.SpineNavel;
    parentJointMap[JointId.KneeLeft] = JointId.HipLeft;
    parentJointMap[JointId.AnkleLeft] = JointId.KneeLeft;
    parentJointMap[JointId.FootLeft] = JointId.AnkleLeft;
    parentJointMap[JointId.HipRight] = JointId.SpineNavel;
    parentJointMap[JointId.KneeRight] = JointId.HipRight;
    parentJointMap[JointId.AnkleRight] = JointId.KneeRight;
    parentJointMap[JointId.FootRight] = JointId.AnkleRight;
    parentJointMap[JointId.Head] = JointId.Pelvis;
    parentJointMap[JointId.Nose] = JointId.Head;
    parentJointMap[JointId.EyeLeft] = JointId.Head;
    parentJointMap[JointId.EarLeft] = JointId.Head;
    parentJointMap[JointId.EyeRight] = JointId.Head;
    parentJointMap[JointId.EarRight] = JointId.Head;

    Vector3 zpositive = Vector3.forward;
    Vector3 xpositive = Vector3.right;
    Vector3 ypositive = Vector3.up;
    // spine and left hip are the same
    Quaternion leftHipBasis = Quaternion.LookRotation(xpositive, -zpositive);
    Quaternion spineHipBasis = Quaternion.LookRotation(xpositive, -zpositive);
    Quaternion rightHipBasis = Quaternion.LookRotation(xpositive, zpositive);
    // arms and thumbs share the same basis
    Quaternion leftArmBasis = Quaternion.LookRotation(ypositive, -zpositive);
    Quaternion rightArmBasis = Quaternion.LookRotation(-ypositive, zpositive);
    Quaternion leftHandBasis = Quaternion.LookRotation(-zpositive, -ypositive);
    Quaternion rightHandBasis = Quaternion.identity;
    Quaternion leftFootBasis = Quaternion.LookRotation(xpositive, ypositive);
    Quaternion rightFootBasis = Quaternion.LookRotation(xpositive, -ypositive);

    basisJointMap = new Dictionary<JointId, Quaternion>();

    // pelvis has no parent so set to count
    basisJointMap[JointId.Pelvis] = spineHipBasis;
    basisJointMap[JointId.SpineNavel] = spineHipBasis;
    basisJointMap[JointId.SpineChest] = spineHipBasis;
    basisJointMap[JointId.Neck] = spineHipBasis;
    basisJointMap[JointId.ClavicleLeft] = leftArmBasis;
    basisJointMap[JointId.ShoulderLeft] = leftArmBasis;
    basisJointMap[JointId.ElbowLeft] = leftArmBasis;
    basisJointMap[JointId.WristLeft] = leftHandBasis;
    basisJointMap[JointId.HandLeft] = leftHandBasis;
    basisJointMap[JointId.HandTipLeft] = leftHandBasis;
    basisJointMap[JointId.ThumbLeft] = leftArmBasis;
    basisJointMap[JointId.ClavicleRight] = rightArmBasis;
    basisJointMap[JointId.ShoulderRight] = rightArmBasis;
    basisJointMap[JointId.ElbowRight] = rightArmBasis;
    basisJointMap[JointId.WristRight] = rightHandBasis;
    basisJointMap[JointId.HandRight] = rightHandBasis;
    basisJointMap[JointId.HandTipRight] = rightHandBasis;
    basisJointMap[JointId.ThumbRight] = rightArmBasis;
    basisJointMap[JointId.HipLeft] = leftHipBasis;
    basisJointMap[JointId.KneeLeft] = leftHipBasis;
    basisJointMap[JointId.AnkleLeft] = leftHipBasis;
    basisJointMap[JointId.FootLeft] = leftFootBasis;
    basisJointMap[JointId.HipRight] = rightHipBasis;
    basisJointMap[JointId.KneeRight] = rightHipBasis;
    basisJointMap[JointId.AnkleRight] = rightHipBasis;
    basisJointMap[JointId.FootRight] = rightFootBasis;
    basisJointMap[JointId.Head] = spineHipBasis;
    basisJointMap[JointId.Nose] = spineHipBasis;
    basisJointMap[JointId.EyeLeft] = spineHipBasis;
    basisJointMap[JointId.EarLeft] = spineHipBasis;
    basisJointMap[JointId.EyeRight] = spineHipBasis;
    basisJointMap[JointId.EarRight] = spineHipBasis;
    */
    