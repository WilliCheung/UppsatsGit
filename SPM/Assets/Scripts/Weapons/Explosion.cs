using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    //Author: Patrik Ahlgren
    [SerializeField] private float explosionRadius;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject fireEffect;

    private GameObject explosion;
    private GameObject fire;

    public void Explode(float explosionForce, float damage) {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders){

            if (nearbyObject.gameObject.layer == 9){
                GameController.Instance.ShowHitmark(0.5f);
                nearbyObject.transform.GetComponent<Enemy>().TakeDamage(CalculateDamage(nearbyObject, damage));
            }
            if (nearbyObject.gameObject.layer == 12) {
                GameController.Instance.TakeDamage(CalculateDamage(nearbyObject, damage));
            }
            if (nearbyObject.gameObject.layer == 13 && nearbyObject.gameObject != gameObject) {
                nearbyObject.transform.GetComponent<DestructibleObject>().TakeDamage(CalculateDamage(nearbyObject, damage));
            }

            Rigidbody rigidBody = nearbyObject.GetComponent<Rigidbody>();
            if (rigidBody != null) {
                rigidBody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

        }
        explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Camera.main.GetComponent<CameraShake>().ShakeIncreaseDistance(25f, 1.5f, GameController.Instance.Player, gameObject);
        AudioController.Instance.Play_InWorldspace("Explosion", explosion, 0.95f, 1f);
        fire = Instantiate(fireEffect, transform.position, Quaternion.identity);
        Destroy(explosion, 3.75f);
        Destroy(fire, 10f);
    }

    private float CalculateDamage(Collider objects, float damage) {
        float damageDropoff = Vector3.Distance(transform.position, objects.transform.position) * 2.5f;
        if (damageDropoff > damage) {
            damageDropoff = (damage - 1);
        }
        float finalDamage = damage - damageDropoff;
        return finalDamage;
    }

}
