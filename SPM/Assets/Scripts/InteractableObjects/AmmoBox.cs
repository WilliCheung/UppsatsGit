using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    //Author: Patrik Ahlgren
    [SerializeField] private int clipIncrease;

    private Animator anim;


    private void Start()
    {
        anim = GameObject.Find("AmmunitionText").GetComponent<Animator>();   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "InteractionPlayer")
        {   anim.SetTrigger("AmmunitionPickUp");
            foreach (BaseWeapon weapon in GameController.Instance.PlayerWeapons)
            {
                weapon.IncreaseTotalAmmoLeft(weapon.GetMaxAmmoInClip() * clipIncrease);
            }
            GameController.Instance.UpdateSelectedWeapon_AmmoText();
            GetComponentInParent<PowerUpSpawner>().Respawner();
            AudioController.Instance.Play("AmmoPickup");


        }
        Destroy(gameObject);
    }
    
    public void setClipIncrease(int clipIncreaseAmount)
    {
        clipIncrease = clipIncreaseAmount;
    }
}