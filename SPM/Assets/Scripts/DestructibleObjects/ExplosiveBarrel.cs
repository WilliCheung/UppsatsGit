using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : DestructibleObject{
    //Author: Patrik Ahlgren

    public override void Destroy() {
        if (!IsDestroyed) {
            GetComponent<Explosion>().Explode(5, 35);
            Destroy(gameObject);
            IsDestroyed = true;
        }       
    }
}
