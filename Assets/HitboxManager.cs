using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
    /*public GameObject leftElbow;
    public GameObject rightElbow;
    public GameObject rightAnkle;
    public GameObject leftAnkle;
    public GameObject leftHand;*/
    public GameObject hand;
    public GameObject headset;

    public Object fireball;
    private bool isPunch = false;

    public Object fireWall;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*float dist = Vector3.Distance(rightHand.GetComponent<Transform>().position, rightElbow.GetComponent<Transform>().position);
        //Debug.Log(dist);
        if (HandUp(rightHand, rightElbow, 1))
        {
            //Debug.Log("Hand Up");
        }
        if (HandDown(rightHand, rightElbow, 1))
        {
            //Debug.Log("Hand Down");
        }
        if (HandOut(rightHand, rightElbow, 1))
        {
            //Debug.Log("Hand Out");
            
        }
        if (HandIn(rightHand, rightElbow, 0.5))
        {
            //Debug.Log("Hand In");
            
        }
        if(dist < 0.45)
        {
            Debug.Log("Reload");
            isPunch = false;
        }
        if (HandPunch(rightHand, rightElbow, 1) && dist > 0.49)
        {
            Debug.Log("Punch");
            if (isPunch == false)
            {
                ThrowFireball(fireball, rightHand, rightElbow);
                Debug.Log("Fireball!!");
                isPunch = true;
            }
            
        } else
        {
            
        }*/
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision");
        
        if (other.gameObject.name == "Punch")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("detected Punch name");
        }

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (other.gameObject.tag == "punchBlock" && isPunch == false)
        {
            //If the GameObject has the same tag as specified, output this message in the console
            Debug.Log("Fireball!!");
            isPunch = true;
            ThrowFireball(fireball, hand, headset);
        }
        if (other.gameObject.tag == "wallOfFire" && ComboTracker.Instance.wallReload == true)
        {
            Debug.Log("Wall Hit!!");
            ComboTracker.Instance.wallCheck++;
            if(ComboTracker.Instance.wallCheck == 2){
                Debug.Log("FireWall!!!!!!!!!!!!");
                CreateWall(fireWall, hand, headset);
                ComboTracker.Instance.wallReload = false;
            }
        }
        if (other.gameObject.tag == "FootOut")
        {
            
            Debug.Log("Kick!!");
            
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //Debug.Log("Collision");

        if (other.gameObject.name == "Punch")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("detected Punch name");
        }

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (other.gameObject.tag == "punchBlock" && isPunch == true)
        {
            Debug.Log("Reload Punch");
            isPunch = false;
        }
        if (other.gameObject.tag == "wallOfFire")
        {
            Debug.Log("Reload Wall");
           ComboTracker.Instance.wallCheck--;
           ComboTracker.Instance.wallReload = true;
        }
    }

    static public void CreateWall(Object wallObject, GameObject hand, GameObject headset){
        float diff = hand.GetComponent<Transform>().position.z - headset.GetComponent<Transform>().position.z;
        float direction = headset.GetComponent<Transform>().eulerAngles.y;
        Debug.Log(direction);
        //new fireball.transform.addVelocity(diff);
        
        //wall.GetComponent<Transform>().eulerAngles = new Vector3(0, direction, 0);
        //wall.GetComponent<Transform>().rotation.x = 0;
        //wall.GetComponent<Transform>().rotation.y = direction;
        //wall.GetComponent<Transform>().rotation.z = 0;
        Vector3 displacement = new Vector3(0, 0, diff);
        //wall.GetComponent<Transform>().parent = headset.GetComponent<Transform>();
        Vector3 thePosition = headset.GetComponent<Transform>().TransformPoint(Vector3.forward * 5 / 2);
        
        //wall.GetComponent<Transform>().localPosition = thePosition;
        //headset.GetComponent<Transform>().localPosition + (displacement * 3);
        GameObject wall = (GameObject)Instantiate(wallObject, thePosition, Quaternion.Euler(-90, direction, 0));
        wall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        wall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    static public void ThrowFireball(Object fireball, GameObject hand, GameObject headset)
    {
        Vector3 diff = hand.GetComponent<Transform>().position - headset.GetComponent<Transform>().position;
        //new fireball.transform.addVelocity(diff);
        GameObject fire = (GameObject)Instantiate(fireball, hand.GetComponent<Transform>().position, hand.GetComponent<Transform>().rotation);
        fire.GetComponent<Rigidbody>().velocity = diff*20;
   
    }

    static public bool HandOut(GameObject hand, GameObject elbow, double n)
    {
        Vector3 diff = hand.GetComponent<Transform>().position - elbow.GetComponent<Transform>().position;
        if(diff[2] > 0 && Mathf.Sqrt(Mathf.Pow(diff[0],2)+ Mathf.Pow(diff[1],2)) < (Mathf.Abs(diff[2])/n))
        {
            return true;
        }
        return false;
    }

    static public bool HandUp(GameObject hand, GameObject elbow, double n)
    {
        Vector3 diff = hand.GetComponent<Transform>().position - elbow.GetComponent<Transform>().position;
        if (diff[1] > 0 && Mathf.Sqrt(Mathf.Pow(diff[0], 2) + Mathf.Pow(diff[2], 2)) < (Mathf.Abs(diff[1]) / n))
        {
            return true;
        }
        return false;
    }

    static public bool HandDown(GameObject hand, GameObject elbow, double n)
    {
        Vector3 diff = hand.GetComponent<Transform>().position - elbow.GetComponent<Transform>().position;
        if (diff[1] < 0 && Mathf.Sqrt(Mathf.Pow(diff[0], 2) + Mathf.Pow(diff[2], 2)) < (Mathf.Abs(diff[1]) / n))
        {
            return true;
        }
        return false;
    }

    static public bool HandIn(GameObject hand, GameObject elbow, double n)
    {
        bool insideElbow = false;
        Vector3 diff = hand.GetComponent<Transform>().position - elbow.GetComponent<Transform>().position;
        if(Mathf.Abs(hand.GetComponent<Transform>().position[0]) < Mathf.Abs(elbow.GetComponent<Transform>().position[0]))
        {
            insideElbow = true;
        }
        if (insideElbow && Mathf.Sqrt(Mathf.Pow(diff[1], 2) + Mathf.Pow(diff[2], 2)) < (Mathf.Abs(diff[0]) / n))
        {
            return true;
        }
        return false;
    }

    static public bool HandPunch(GameObject hand, GameObject elbow, double n)
    {
        bool outsideElbow = false;
        Vector3 diff = hand.GetComponent<Transform>().position - elbow.GetComponent<Transform>().position;
        if (Mathf.Abs(hand.GetComponent<Transform>().position[0]) > Mathf.Abs(elbow.GetComponent<Transform>().position[0]))
        {
            outsideElbow = true;
        }
        if (outsideElbow && Mathf.Sqrt(Mathf.Pow(diff[1], 2) + Mathf.Pow(diff[2], 2)) < (Mathf.Abs(diff[0]) / n))
        {
            return true;
        }
        return false;
    }

    static public bool AnkleNeutral(GameObject ankle, GameObject headset, double n)
    {
        Vector3 coord = ankle.GetComponent<Transform>().position;
        if(Mathf.Sqrt(Mathf.Pow(coord[1], 2) + Mathf.Pow(coord[2], 2))/Mathf.Abs(coord[1]) < n)
        {
            return true;
        }
        return false;
    }

    static public bool AnkleStickingOut(GameObject ankle, double n, double m)
    {
        Vector3 coord = ankle.GetComponent<Transform>().position;
        if (Mathf.Abs(coord[0])/coord[2] < n && coord[0] > m)
        {
            return true;
        }
        return false;
    }

    static public bool WideStance(GameObject ankle, double n, double m)
    {
        Vector3 coord = ankle.GetComponent<Transform>().position;
        if (Mathf.Abs(coord[0]) / coord[2] < n && coord[0] > m)
        {
            return true;
        }
        return false;
    }

    static public bool AnkleUpForward(GameObject ankle, GameObject headset, double n, double m, double l)
    {
        Vector3 ankleCoord = ankle.GetComponent<Transform>().position;
        Vector3 headCoord = headset.GetComponent<Transform>().position;
        if (Mathf.Abs(ankleCoord[2]) / ankleCoord[0] < n && ankleCoord[2] > m && ankleCoord[1] > headCoord[1] / l)
        {
            return true;
        }
        return false;
    }

    
}
