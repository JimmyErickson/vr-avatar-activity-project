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
        other.GetComponent<MeshRenderer>().material = SceneManagerStuff.green;
        //Debug.Log("Collision");
        timer += Time.deltaTime;
        if (timer >= SceneManagerStuff.menuTimerMax)
        {
            timer = 0;
            if (other.gameObject.name == "Tutorial")
            {
                Debug.Log("Scene2 loading: " + SceneManagerStuff.scenePaths[0]);
                SceneManager.LoadScene("Tutorial");
            }
            else if (other.gameObject.name == "Airbending")
            {
                Debug.Log("Scene2 loading: " + SceneManagerStuff.scenePaths[0]);
                SceneManager.LoadScene("Airbending");
            }
            else if (other.gameObject.name == "Firebending")
            {
                Debug.Log("Scene2 loading: " + SceneManagerStuff.scenePaths[0]);
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
        other.GetComponent<MeshRenderer>().material = SceneManagerStuff.red;
        timer = 0;
    }
}

