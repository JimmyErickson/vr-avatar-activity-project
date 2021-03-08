using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*class Pose
{

    static Pose CreatePose(float rHandX, float rHandY, float rHandZ, float lHandX, float lHandY, float lHandZ, float rAnkleX, float rAnkleY, float rAnkleZ, float lAnkleX, float lAnkleY, float lAnkleZ)
    {
        var rHandCoord = Tuple.Create(rHandX, rHandY, rHandZ);
        var lHandCoord = Tuple.Create(lHandX, lHandY, lHandZ);
        var rAnkleCoord = Tuple.Create(rAnkleX, rAnkleY, rAnkleZ);
        var lAnkleCoord = Tuple.Create(lAnkleX, lAnkleY, lAnkleZ);
    }
}*/

public class GlobalScript : MonoBehaviour
{
    public GameObject rHand;
    public GameObject lHand;
    public GameObject rAnkle;
    public GameObject lAnkle;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello There");
        float[] pose = new float[] { rHand.GetComponent<Transform>().position.x, rHand.GetComponent<Transform>().position.y, rHand.GetComponent<Transform>().position.z, lHand.GetComponent<Transform>().position.x, lHand.GetComponent<Transform>().position.y, lHand.GetComponent<Transform>().position.z, rAnkle.GetComponent<Transform>().position.x, rAnkle.GetComponent<Transform>().position.y, rAnkle.GetComponent<Transform>().position.z, lAnkle.GetComponent<Transform>().position.x, lAnkle.GetComponent<Transform>().position.y, lAnkle.GetComponent<Transform>().position.z };
        //File.AppendAllText("C:/Users/vrcart01/Desktop/vr-avatar-activity-project/Assets/Poses/PosesFile", string.Join(",", pose));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
