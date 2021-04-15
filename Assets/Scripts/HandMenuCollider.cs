using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        // other.GetComponent<MeshRenderer>().material = SceneManagerStuff.green;
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
                SceneManager.LoadScene("Airbending");
            }
            else if (other.gameObject.name == "Firebending")
            {
                Debug.Log("Fire");
                SceneManager.LoadScene("Firebending");
            }
            else if (other.gameObject.name == "Exit Menu")
            {
                SceneManagerStuff.MenuUI.SetActive(false);
            }
        }
        

        
    }

    public void OnTriggerExit(Collider other)
    {
        // other.GetComponent<MeshRenderer>().material = SceneManagerStuff.red;
        timer -= Time.deltaTime / 100;
        Debug.Log("LEAVING NOW");
    }
}

