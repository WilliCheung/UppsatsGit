using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Marcus Söderberg
public class PlayerDeathController : MonoBehaviour
{

    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject respawnManager;
    [SerializeField] private GameObject player;

    [SerializeField] private bool isDead;
    [SerializeField] private float addToTimer;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        LoadGameObjectReferences();
        isDead = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.PlayerHP <= 0 && isDead == false)
        {
            isDead = true;
            LoadGameObjectReferences();
            OnPlayerDeath();
        }
    }

    public void OnPlayerDeath()
    {
        deathPanel.SetActive(true);
        anim.SetBool("isDead", true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(WaitASec());
        GameController.Instance.GetComponent<Timer>().TimerIsActive = false;
    }

    public void RespawnAtLastCheckpoint()
    {
        respawnManager.GetComponent<PlayerRespawner>().RespawnMethod();
        DisableCursor();
        PauseAndUnpauseGame();
        isDead = false;
        deathPanel.SetActive(false);
        GameController.Instance.GetComponent<Timer>().AddToTimer(addToTimer);
        GameController.Instance.GetComponent<Timer>().TimerIsActive = true;
    }

    private void PauseAndUnpauseGame()
    {
        GameController.Instance.GamePaused();
    }
    public void resetStatus()
    {
        GameController.Instance.PlayerHP = 100;
        GameController.Instance.PlayerArmor = 100;
        isDead = false;
    }

    private void DisableCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadGameObjectReferences()
    {
        player = GameController.Instance.Player;
        respawnManager = FindObjectOfType<PlayerRespawner>().gameObject;
    }

    private IEnumerator WaitASec()
    {
        yield return new WaitForSeconds(1.2f);
        PauseAndUnpauseGame();
    }
}
