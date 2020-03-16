using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileWeapon : BaseWeapon{
    //Author: Patrik Ahlgren
    private float projectileSpeed;

    public ProjectileWeapon(string name, float damage, float range, float fireRate, float reloadTime, float impactForce, float spread, float projectileSpeed, int ammoInClip, int maxAmmoInClip, int totalAmmoLeft): base(name, damage, range, fireRate, reloadTime, impactForce, spread, ammoInClip, maxAmmoInClip, totalAmmoLeft) {
        this.projectileSpeed = projectileSpeed;
    }
    public float GetProjectileSpeed() {
        return projectileSpeed;
    }
    public void SetProjectileSpeed(float f) {
        projectileSpeed = f;
    }
}
