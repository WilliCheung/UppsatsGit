using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    //Author: Marcus Söderberg
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject EndGamePanel;
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject controller;
    [SerializeField] GameObject settings;


    private GameObject scenemanager;

    public bool InGameMenuActive;


    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(false);
        InGameMenuActive = false;
        //DontDestroyOnLoad(gameObject);
        scenemanager = GameObject.Find("SceneManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateMenu()
    {
        menuPanel.SetActive(true);
        InGameMenuActive = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameController.Instance.PauseAudio = true;
        GameController.Instance.GamePaused();

    }

    public void DeactivateMenu()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        try
        {
            menuPanel.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.Log("MenuPanel already inactive");
        }
        try
        {
            optionPanel.SetActive(false);

        }
        catch (Exception e)
        {
            Debug.Log("OptionPanel already inactive");
        }
        try
        {
            controller.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.Log("ControllerPanel already inactive");
        }
        try
        {
            settings.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.Log("SettingsPanel already inactive");
        }
        InGameMenuActive = false;
        GameController.Instance.GamePaused();
    }

    public void MainMenu()
    {
        Debug.Log("Clicked button: Main Menu");
        scenemanager = GameObject.Find("SceneManager");
        scenemanager.GetComponent<SceneManagerScript>().MainMenu();
        DeactivateMenu();
    }

    public void Restart()
    {
        scenemanager.GetComponent<SceneManagerScript>().StartLevelOne();
    }

    public void EndGameActivate()
    {

        //End button activates

        scenemanager = GameObject.Find("SceneManager");
        EndGamePanel.SetActive(true);
        InGameMenuActive = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameController.Instance.PauseAudio = true;
        GameController.Instance.GamePaused();
    }

    public void EndGameDeactivate()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        EndGamePanel.SetActive(false);
        InGameMenuActive = false;
        GameController.Instance.GamePaused();
    }

    public void SaveGame()
    {
        GameObject.FindObjectOfType<DataStorage>().SaveGame();
    }

    public void LoadGame()
    {
        GameObject.FindObjectOfType<DataStorage>().LoadGameData();
        //DeactivateMenu();
    }

}
