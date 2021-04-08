using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class distanceTracker : MonoBehaviour
{
    public GameObject playerBody;
    public GameObject poseBody;
    public float tolerance;
    public Material green;
    public Material red;
    private float timer = 0;
    public float timerMax;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        float count = 0;
        float rHand = Vector3.Distance(playerBody.transform.GetChild(15).transform.position, poseBody.transform.GetChild(15).transform.position);
        float lHand = Vector3.Distance(playerBody.transform.GetChild(8).transform.position, poseBody.transform.GetChild(8).transform.position);
        float rElbow = Vector3.Distance(playerBody.transform.GetChild(13).transform.position, poseBody.transform.GetChild(13).transform.position);
        float lElbow = Vector3.Distance(playerBody.transform.GetChild(6).transform.position, poseBody.transform.GetChild(6).transform.position);
        float rKnee = Vector3.Distance(playerBody.transform.GetChild(23).transform.position, poseBody.transform.GetChild(23).transform.position);
        float lKnee = Vector3.Distance(playerBody.transform.GetChild(20).transform.position, poseBody.transform.GetChild(20).transform.position);
        float rFoot = Vector3.Distance(playerBody.transform.GetChild(25).transform.position, poseBody.transform.GetChild(25).transform.position);
        float lFoot = Vector3.Distance(playerBody.transform.GetChild(21).transform.position, poseBody.transform.GetChild(21).transform.position);
        float avgDist = (rHand + lHand + rElbow + lElbow + rKnee + lKnee + rFoot + lFoot) / 8;

        float[] distances = { rHand, lHand, rElbow, lElbow, rKnee, lKnee, rFoot, lFoot };

        foreach (float distance in distances)
        {
            if (distance < tolerance)
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
            poseChanger.currentPose ++;
        }
    }
}


