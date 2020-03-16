using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorsAnimated : MonoBehaviour
{
    //Main Author: Teo
    //Secondary Author: Patrik Ahlgren
    bool isOpen;

    [SerializeField] private GameObject redPanel;
    [SerializeField] private GameObject greenPanel;
    [SerializeField] private SpawnManager spawnManager;

    [SerializeField] private Animator anim;
    [SerializeField] private bool spawnEnemies;

    public int DoorID { get; set; }

    void Start()
    {
        greenPanel = gameObject.transform.GetChild(0).gameObject;
        redPanel = gameObject.transform.GetChild(1).gameObject;
        greenPanel.SetActive(false);
        redPanel.SetActive(true);
        DoorID = gameObject.GetInstanceID();
    }

    public void OpenAndClose() {

        
        isOpen = !isOpen;
        AudioController.Instance.Play_InWorldspace("Button", gameObject);
        if (isOpen) {
            anim.SetBool("isOpen", true);
            greenPanel.SetActive(!greenPanel.activeSelf);
            redPanel.SetActive(!redPanel.activeSelf);

           

            if (spawnEnemies) {
                spawnManager.InitializeSpawner();
                spawnEnemies = false;
            }
        

        } else {
            anim.SetBool("isOpen", false);
            greenPanel.SetActive(!greenPanel.activeSelf);
            redPanel.SetActive(!redPanel.activeSelf);
        }
    }
}

