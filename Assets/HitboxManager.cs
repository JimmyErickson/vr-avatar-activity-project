using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
    public GameObject leftElbow;
    public GameObject rightElbow;
    public GameObject rightAnkle;
    public GameObject leftAnkle;
    public GameObject leftHand;
    public GameObject rightHand;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    static public bool HandForward(GameObject hand, GameObject elbow, double n)
    {
        Vector3 diff = hand.GetComponent<Transform>().localPosition - elbow.GetComponent<Transform>().localPosition;
        if(diff[2] > 0 && Mathf.Sqrt(Mathf.Pow(diff[0],2)+ Mathf.Pow(diff[1],2)) < n)
        {
            return true;
        }
        return false;
    }

    static public bool HandUp(GameObject hand, GameObject elbow, double n)
    {
        Vector3 diff = hand.GetComponent<Transform>().localPosition - elbow.GetComponent<Transform>().localPosition;
        if (diff[1] > 0 && Mathf.Sqrt(Mathf.Pow(diff[0], 2) + Mathf.Pow(diff[2], 2)) < n)
        {
            return true;
        }
        return false;
    }

    static public bool HandDown(GameObject hand, GameObject elbow, double n)
    {
        Vector3 diff = hand.GetComponent<Transform>().localPosition - elbow.GetComponent<Transform>().localPosition;
        if (diff[1] < 0 && Mathf.Sqrt(Mathf.Pow(diff[0], 2) + Mathf.Pow(diff[2], 2)) < n)
        {
            return true;
        }
        return false;
    }

    static public bool HandIn(GameObject hand, GameObject elbow, double n)
    {
        bool insideElbow = false;
        Vector3 diff = hand.GetComponent<Transform>().localPosition - elbow.GetComponent<Transform>().localPosition;
        if(Mathf.Abs(hand.GetComponent<Transform>().localPosition[0]) < Mathf.Abs(elbow.GetComponent<Transform>().localPosition[0]))
        {
            insideElbow = true;
        }
        if (insideElbow && Mathf.Sqrt(Mathf.Pow(diff[1], 2) + Mathf.Pow(diff[2], 2)) < n)
        {
            return true;
        }
        return false;
    }

    static public bool HandOut(GameObject hand, GameObject elbow, double n)
    {
        bool outsideElbow = false;
        Vector3 diff = hand.GetComponent<Transform>().localPosition - elbow.GetComponent<Transform>().localPosition;
        if (Mathf.Abs(hand.GetComponent<Transform>().localPosition[0]) > Mathf.Abs(elbow.GetComponent<Transform>().localPosition[0]))
        {
            outsideElbow = true;
        }
        if (outsideElbow && Mathf.Sqrt(Mathf.Pow(diff[1], 2) + Mathf.Pow(diff[2], 2)) < n)
        {
            return true;
        }
        return false;
    }

    static public bool AnkleNeutral(GameObject ankle, GameObject headset, double n)
    {
        Vector3 coord = ankle.GetComponent<Transform>().localPosition;
        if(Mathf.Sqrt(Mathf.Pow(coord[1], 2) + Mathf.Pow(coord[2], 2))/Mathf.Abs(coord[1]) < n)
        {
            return true;
        }
        return false;
    }

    static public bool AnkleStickingOut(GameObject ankle, double n, double m)
    {
        Vector3 coord = ankle.GetComponent<Transform>().localPosition;
        if (Mathf.Abs(coord[0])/coord[2] < n && coord[0] > m)
        {
            return true;
        }
        return false;
    }

    static public bool WideStance(GameObject ankle, double n, double m)
    {
        Vector3 coord = ankle.GetComponent<Transform>().localPosition;
        if (Mathf.Abs(coord[0]) / coord[2] < n && coord[0] > m)
        {
            return true;
        }
        return false;
    }

    static public bool AnkleUpForward(GameObject ankle, GameObject headset, double n, double m, double l)
    {
        Vector3 ankleCoord = ankle.GetComponent<Transform>().localPosition;
        Vector3 headCoord = headset.GetComponent<Transform>().localPosition;
        if (Mathf.Abs(ankleCoord[2]) / ankleCoord[0] < n && ankleCoord[2] > m && ankleCoord[1] > headCoord[1] / l)
        {
            return true;
        }
        return false;
    }
}
