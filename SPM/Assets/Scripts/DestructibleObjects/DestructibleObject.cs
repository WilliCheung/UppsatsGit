using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour {
//Author: Patrik Ahlgren

    protected float Durability = 10;
    public bool IsDestroyed { get; set; }

    public virtual void TakeDamage(float damage) {
        Durability -= damage;
        if(Durability <= 0) {
            Destroy();
        }
    }

    public virtual void Destroy() {
        if (!IsDestroyed) {         
            Destroy(gameObject);
            IsDestroyed = true;
        }      
    }

}
