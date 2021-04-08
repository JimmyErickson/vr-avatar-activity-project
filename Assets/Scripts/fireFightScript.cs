using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Microsoft.Azure.Kinect.BodyTracking;


public class fireFightScript : MonoBehaviour
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
    public float tolerance;
    private List<string> poseKeys;
    
    private List<Body> firePoses;
    private float timer = 0;
    public float timerMax;
    public VisualEffect rHandFlame;
    public VisualEffect lHandFlame;


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Hello There");
        LoadData("C:/Users/vrcart01/Desktop/vr-avatar-activity-project/Assets/Poses/FirePosesFile.csv");
        firePoses = new List<Body>();
        rHandFlame.Stop();
        lHandFlame.Stop();
        
    }

    void Update()
    {
        CreatePoses();
        CheckMoves();
    }

    public void CreatePoses()
    {
        System.Numerics.Vector3 playerPelvis = PlayerBody.playerPos.JointPositions3D[(int)JointId.Pelvis];;
        poseKeys = new List<string>(poses.Keys);
        foreach(string poseKey in poseKeys){
            Body examplePose = new Body(poses[poseKey].Length);
            examplePose.JointPositions3D = poses[poseKey];
            examplePose.JointRotations = poseRotations[poseKey];
            System.Numerics.Vector3 pelvis = examplePose.JointPositions3D[(int)JointId.Pelvis];
            System.Numerics.Vector3 diff = playerPelvis - pelvis;
            for (int i = 0; i < examplePose.JointPositions3D.Length; i++){

                examplePose.JointPositions3D[i] = new System.Numerics.Vector3(examplePose.JointPositions3D[i].X + diff.X, examplePose.JointPositions3D[i].Y, examplePose.JointPositions3D[i].Z + diff.Z);
            }
            firePoses.Add(examplePose);
        }
    }

    public void CheckMoves(){

    }

    public void CheckWideStance(){
        int[] focusPoints = {(int)JointId.KneeRight, (int)JointId.KneeLeft, (int)JointId.FootRight, (int)JointId.FootLeft};
        //float[] distances = new float[8];
        float count = 0;
        for(int i = 0; i<focusPoints.Length; i++){
            if(System.Numerics.Vector3.Distance(PlayerBody.playerPos.JointPositions3D[focusPoints[i]], PlayerBody.guidePos.JointPositions3D[focusPoints[i]]) < tolerance){
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

    private static readonly string logPrefix = "DataParser: ";

    public void LoadData(string filePath)
    {
        LoadRotationData("C:/Users/vrcart01/Desktop/vr-avatar-activity-project/Assets/Poses/FirePosesRotationsFile.csv");
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
                    for (int i = 1; i < parsedLine.Length - 3; i += 3)
                    {
                        int index = (i - 1) / 3;
                        //jointPositions[index, 0] = float.Parse(parsedLine[i + 1]);
                        //jointPositions[index, 1] = float.Parse(parsedLine[i + 2]);
                        //jointPositions[index, 2] = float.Parse(parsedLine[i + 3]);
                        jointPositions[index] = new System.Numerics.Vector3(float.Parse(parsedLine[i]), float.Parse(parsedLine[i + 1]), float.Parse(parsedLine[i + 2]));
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
                    Debug.Log(logPrefix + "Rotations" + line);
                    var parsedLine = ParseCSVLine(line);
                    if (IfEmptyValues(parsedLine))
                        continue;

                    //float[,] jointRotations = new float[parsedLine.Length, 4];
                    System.Numerics.Quaternion[] jointRotations = new System.Numerics.Quaternion[parsedLine.Length-1];
                    for (int i = 0; i < parsedLine.Length-1; i ++)
                    {
                        string[] splitOnSpace = parsedLine[i+1].Split(' ');
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
            Debug.LogError(logPrefix + "The rotations file could not be read:");
            Debug.LogError(e.Message);
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