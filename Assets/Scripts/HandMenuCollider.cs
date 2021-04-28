using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HandMenuCollider : MonoBehaviour
{
    private static float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit a menu");
        if (other.gameObject.tag == "MenuButton" )
        {
            other.GetComponent<Image>().color = Color.green;
        }

        //Debug.Log("Collision");
        timer += Time.deltaTime;
        Debug.Log(timer);
        if (timer >= SceneManagerStuff.menuTimerMax)
        {
            timer = 0;

            Debug.Log("HERE IS THE NAME");
            Debug.Log(other.gameObject.name);
            if (other.gameObject.name == "Airbending")
            {
                Debug.Log("Air");
                PlayerBody.playerObject.SetActive(false);
                SceneManagerStuff.MenuUI.SetActive(false);

                SceneManager.LoadScene("Airbending", LoadSceneMode.Single);
            }
            else if (other.gameObject.name == "Firebending")
            {
                Debug.Log("Fire");
                PlayerBody.playerObject.SetActive(false);
                SceneManagerStuff.MenuUI.SetActive(false);

                SceneManager.LoadScene("Firebending", LoadSceneMode.Single);
            }
            else if (other.gameObject.tag == "exitmenu")
            {
                Debug.Log("Exited Menu");
                SceneManagerStuff.MenuUI.SetActive(false);
            }
            else if (other.gameObject.name == "SkipPose")
            {
                if (poseChanger.currentPose == poseChanger.numberOfKeys - 1)
                {
                    poseChanger.currentPose = 0;
                }
                else
                {
                    poseChanger.currentPose++;
                }
                SceneManagerStuff.MenuUI.SetActive(false);
                Debug.Log("Skip Pose");
                //poseChanger.currentPose++;
            }
        }



    }

    public void OnTriggerExit(Collider other)
    {
        // other.GetComponent<MeshRenderer>().material = SceneManagerStuff.red;
        timer -= Time.deltaTime / 100;
        Debug.Log("LEAVING NOW");
        if (other.gameObject.tag == "MenuButton")
        {
            other.GetComponent<Image>().color = Color.white;
        }
    }
}