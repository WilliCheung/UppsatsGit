using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioExplosion : MonoBehaviour {
    //Author: Patrik Ahlgren
    [SerializeField] private GameObject explosionEffect;

    private GameObject explosion;

    public void Explode() {
        explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        AudioController.Instance.Play_InWorldspace("Explosion", explosion, 1.85f, 2f);
        Destroy(explosion, 1.2f);
    }

}
