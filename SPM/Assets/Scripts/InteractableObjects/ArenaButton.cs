using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaButton : MonoBehaviour
{
    //Author: Teo
    public SpawnManager spawnerScript;
    public SceneManagerScript sceneManagerScript;

    public Level1ArenaDoors level1ArenaDoors;

    public GameObject obj1;
    public GameObject obj2;

    private void Start() {
        sceneManagerScript = GameObject.Find("SceneManager").GetComponent<SceneManagerScript>();
        level1ArenaDoors = GameObject.Find("InnerArenaDoors").GetComponent<Level1ArenaDoors>();
    }

    public void PressButton() {
            sceneManagerScript.buttonIsPressed = true;
            Debug.Log("Knappen tryckt");
            spawnerScript.GetComponent<SpawnManager>().InitializeSpawner();
            
            obj1.SetActive(false);
            obj2.SetActive(false);
            level1ArenaDoors.ArenaChange();
        AudioController.Instance.Play_ThenPlay("Song3Start", "Song3Loop");
            Destroy(gameObject);
    }
}
