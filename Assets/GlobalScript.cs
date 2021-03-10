using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;


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
        File.AppendAllText("C:/Users/vrcart01/Desktop/vr-avatar-activity-project/Assets/Poses/PosesFile.csv", string.Join(",", pose));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
