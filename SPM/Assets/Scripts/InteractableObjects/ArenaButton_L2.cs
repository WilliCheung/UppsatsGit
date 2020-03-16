using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaButton_L2 : MonoBehaviour
{

    public SpawnManager spawnerScript;
    public SceneManagerScript sceneManagerScript;

    public GameObject obj1;
    public GameObject obj2;

    private void Start()
    {
        sceneManagerScript = GameObject.Find("SceneManager").GetComponent<SceneManagerScript>();
    
    }

    public void PressButton() {
            sceneManagerScript.buttonIsPressed = true;
            Debug.Log("Knappen tryckt");
            spawnerScript.GetComponent<SpawnManager>().InitializeSpawner();
            AudioController.Instance.Play_ThenPlay("Song4Start", "Song4Loop");
            Destroy(gameObject);

            obj1.SetActive(false);
            obj2.SetActive(false);
    }

}
