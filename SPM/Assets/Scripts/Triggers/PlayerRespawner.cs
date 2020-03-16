using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    //Author: Teo

    public GameObject Player;
    public GameObject[] respawn;
    //public GameController cntrl;
    [SerializeField] private Animator anim;

    void Start()
    {
        anim = GameObject.Find("Fade").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (GameController.Instance.PlayerHP < 1) {
        //    RespawnMethod();
        //}
    }


    public void RespawnMethod()
    {
        if (GameController.Instance.GameEventID == 1)
        {
          
            Player.transform.position = respawn[0].transform.position;
            resetStatus();
        }
        if (GameController.Instance.GameEventID == 2)
        {
            Player.transform.position = respawn[1].transform.position;
            resetStatus();
        }
        if (GameController.Instance.GameEventID == 3)
        {
            Player.transform.position = respawn[2].transform.position;
            resetStatus();
        }
        if (GameController.Instance.GameEventID == 4)
        {
            Player.transform.position = respawn[3].transform.position;
            resetStatus();
        }
        if (GameController.Instance.GameEventID == 5)
        {
            Player.transform.position = respawn[4].transform.position;
            resetStatus();
        }
        if (GameController.Instance.GameEventID == 6)
        {
            Player.transform.position = respawn[5].transform.position;
            resetStatus();
        }
    }
    public void resetStatus()
    {
        anim.SetTrigger("FadeIn");
        GameController.Instance.PlayerHP = 100;
        GameController.Instance.PlayerArmor = 100;
    }
}
