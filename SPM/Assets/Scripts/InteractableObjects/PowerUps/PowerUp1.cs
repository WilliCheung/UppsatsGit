using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp1 : MonoBehaviour
{
    //Author: Marcus Söderberg
    [SerializeField] private float speedIncrease = 1f;
    [SerializeField] private float speedDuration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "InteractionPlayer")
        {
            other.transform.parent.GetComponent<PlayerMovementController>().SpeedMultiplier(speedDuration, speedIncrease);       
            GetComponentInParent<PowerUpSpawner>().Respawner();
            AudioController.Instance.Play("SpeedPickUp");
            Destroy(gameObject);
        }
    }
}
