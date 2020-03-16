using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : DestructibleObject {
    //Author: Patrik Ahlgren

    private void Start() {
        AudioController.Instance.Play_InWorldspace("LetsGetItOn", gameObject);
    }

    public override void Destroy() {
        if (!IsDestroyed) {
            GetComponent<RadioExplosion>().Explode();
            AudioController.Instance.Stop("LetsGetItOn");
            Destroy(gameObject);           
            IsDestroyed = true;
        }
    }
}
