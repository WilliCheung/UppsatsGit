using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp3 : MonoBehaviour
{
    //Author: Marcus Söderberg
    private Animator anim;

    private void Start()
    {
        anim = GameObject.Find("SliderArmor").GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "InteractionPlayer")
        {
            if(GameController.Instance.PlayerArmor < 100)
            {
                AudioController.Instance.Play("ArmorPickup");
                for (int i = 0; i <= 50; i++) {
                    if (GameController.Instance.PlayerArmor != 100) {
                        GameController.Instance.PlayerArmor++;
                    }
                }
                GetComponentInParent<PowerUpSpawner>().Respawner();
                Destroy(gameObject);
                anim.SetTrigger("FullArmor");
            }
        }
    }

}
