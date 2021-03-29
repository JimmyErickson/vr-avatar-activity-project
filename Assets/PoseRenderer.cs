using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.Kinect.BodyTracking;


public class PoseRenderer : MonoBehaviour
{
    public Dictionary<JointId, JointId> parentJointMap;
    Dictionary<JointId, Quaternion> basisJointMap;
    public Quaternion[] absoluteJointRotations = new Quaternion[(int)JointId.Count];
    public bool drawSkeletons = true;
    Quaternion Y_180_FLIP = new Quaternion(0.0f, 1.0f, 0, 90.0f);
    public string poseDescription;
    public List<float> positions;
    public Dictionary<string, System.Numerics.Vector3[]> poses { get; set; } = new Dictionary<string, System.Numerics.Vector3[]>();
    public Dictionary<string, System.Numerics.Quaternion[]> poseRotations { get; set; } = new Dictionary<string, System.Numerics.Quaternion[]>();
    public GameObject renderPose;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello There");
        LoadData("C:/Users/vrcart01/Desktop/vr-avatar-activity-project/Assets/Poses/PosesFile.csv");
        
    }

    void Update()
    {
        if (renderPose.activeSelf)
        {
            DisplayPose("Dragon");
        }
    }

    public void DisplayPose(string poseKey)
    {
        Body examplePose = new Body(poses[poseKey].Length);
        examplePose.JointPositions3D = poses[poseKey];
        examplePose.JointRotations = poseRotations[poseKey];
        renderSkeleton(examplePose, 1);
    }

    private static readonly string logPrefix = "DataParser: ";

    public void LoadData(string filePath)
    {
        LoadRotationData("C:/Users/vrcart01/Desktop/vr-avatar-activity-project/Assets/Poses/PosesRotationsFile.csv");
        Debug.Log(logPrefix + $"Trying to load {filePath}...");
        try
        {
            // Create an instance of StreamReader to read from a file.
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                // Read and display lines from the file until the end of
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    Debug.Log(logPrefix + line);
                    var parsedLine = ParseCSVLine(line);
                    if (IfEmptyValues(parsedLine))
                        continue;

                    //float[,] jointPositions = new float[parsedLine.Length, 3];
                    System.Numerics.Vector3[] jointPositions = new System.Numerics.Vector3[(parsedLine.Length-1)/3];
                    for (int i = 0; i < parsedLine.Length - 2; i += 3)
                    {
                        int index = (i - 1) / 3;
                        //jointPositions[index, 0] = float.Parse(parsedLine[i + 1]);
                        //jointPositions[index, 1] = float.Parse(parsedLine[i + 2]);
                        //jointPositions[index, 2] = float.Parse(parsedLine[i + 3]);
                        jointPositions[index] = new System.Numerics.Vector3(float.Parse(parsedLine[i + 1]), float.Parse(parsedLine[i + 2]), float.Parse(parsedLine[i + 3]));
                    }

                    poses.Add(parsedLine[0], jointPositions);

                }
            }
        }
        catch (System.Exception e)
        {
            // Let the user know what went wrong.
            Debug.LogError(logPrefix + "The file could not be read:");
            Debug.LogError(e.Message);
        }
    }
    public void LoadRotationData(string filePath)
    {
        Debug.Log(logPrefix + $"Trying to load {filePath}...");
        try
        {
            // Create an instance of StreamReader to read from a file.
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                // Read and display lines from the file until the end of
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    Debug.Log(logPrefix + line);
                    var parsedLine = ParseCSVLine(line);
                    if (IfEmptyValues(parsedLine))
                        continue;

                    //float[,] jointRotations = new float[parsedLine.Length, 4];
                    System.Numerics.Quaternion[] jointRotations = new System.Numerics.Quaternion[parsedLine.Length];
                    for (int i = 0; i < parsedLine.Length; i ++)
                    {
                        string[] splitOnSpace = parsedLine[i].Split(' ');
                        float[] splitOnColon = new float[4];
                        for(int j=0; j<4; j++) {
                            splitOnColon[j] = float.Parse(splitOnSpace[j].Split(':')[1].Trim('}'));
                        }
                        
                        jointRotations[i] = new System.Numerics.Quaternion(splitOnColon[0], splitOnColon[1], splitOnColon[2], splitOnColon[3]);
                    }

                    poseRotations.Add(parsedLine[0], jointRotations);

                }
            }
        }
        catch (System.Exception e)
        {
            // Let the user know what went wrong.
            Debug.LogError(logPrefix + "The file could not be read:");
            Debug.LogError(e.Message);
        }
    }

    public void renderSkeleton(Body skeleton, int skeletonNumber)
    {
        for (int jointNum = 0; jointNum < (int)JointId.Count; jointNum++)
        {
            Vector3 jointPos = new Vector3(skeleton.JointPositions3D[jointNum].X, -skeleton.JointPositions3D[jointNum].Y, skeleton.JointPositions3D[jointNum].Z);
            Vector3 offsetPosition = transform.rotation * jointPos;
            Vector3 positionInTrackerRootSpace = transform.position + offsetPosition;
            Quaternion jointRot = Y_180_FLIP * new Quaternion(skeleton.JointRotations[jointNum].X, -skeleton.JointRotations[jointNum].Y,
                skeleton.JointRotations[jointNum].Z, skeleton.JointRotations[jointNum].W) * Quaternion.Inverse(basisJointMap[(JointId)jointNum]);
            absoluteJointRotations[jointNum] = jointRot;
            // these are absolute body space because each joint has the body root for a parent in the scene graph
            transform.GetChild(skeletonNumber).GetChild(jointNum).localPosition = jointPos;
            transform.GetChild(skeletonNumber).GetChild(jointNum).localRotation = jointRot;

            const int boneChildNum = 0;
            if (parentJointMap[(JointId)jointNum] != JointId.Head && parentJointMap[(JointId)jointNum] != JointId.Count)
            {
                Vector3 parentTrackerSpacePosition = new Vector3(skeleton.JointPositions3D[(int)parentJointMap[(JointId)jointNum]].X,
                    -skeleton.JointPositions3D[(int)parentJointMap[(JointId)jointNum]].Y, skeleton.JointPositions3D[(int)parentJointMap[(JointId)jointNum]].Z);
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

    private static string[] ParseCSVLine(string line)
    {
        var splitLine = line.Split(',');
        for (int i = 0; i < splitLine.Length; i++)
        {
            splitLine[i] = splitLine[i].Trim();
        }
        return splitLine;
    }

    private static bool IfEmptyValues(string[] vs)
    {
        foreach (var v in vs)
        {
            if (v == null || v.Length == 0)
                return true;
        }
        return false;
    }
}
