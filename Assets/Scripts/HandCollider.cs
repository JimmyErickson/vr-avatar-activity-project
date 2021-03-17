using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour
{
    public Material green;
    public Material red;
    public GameObject hand;

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision");

        if (other.gameObject.tag == "Collider")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            other.gameObject.GetComponent<MeshRenderer>().material = green;
            Debug.Log("boom");
        }

        /*//Check for a match with the specific tag on any GameObject that collides with your GameObject
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
            if (ComboTracker.Instance.wallCheck == 2)
            {
                Debug.Log("FireWall!!!!!!!!!!!!");
                CreateWall(fireWall, hand, headset);
                ComboTracker.Instance.wallReload = false;
            }
        }
        if (other.gameObject.tag == "FootOut")
        {

            Debug.Log("Kick!!");

        }*/
    }

    public void OnTriggerExit(Collider other)
    {
        //Debug.Log("Collision");

        if (other.gameObject.tag == "Collider")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            other.gameObject.GetComponent<MeshRenderer>().material = red;
        }

        /*//Check for a match with the specific tag on any GameObject that collides with your GameObject
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
        }*/
    }
}
