using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour{
    //Author: Patrik Ahlgren
    public GameObject RocketLaucherProjectileGO;
    public GameObject EnemyWeaponProjectileGO;
    public Sprite[] Crosshair;

    [Header ("Weapon Damage")]
    [SerializeField] private float rifleDmg = 7.5f;
    [SerializeField] private float shotgunDmg = 19f;
    [SerializeField] private float rocketLDmg = 35f;
    [Header("Weapon Ammo")]
    [SerializeField] private int rifleClipMaxAmmo = 50;
    [SerializeField] private int shotgunClipMaxAmmo = 8;
    [SerializeField] private int rocketLClipMaxAmmo = 3;
    [SerializeField] private int rifleTotalAmmoStart = 500;
    [SerializeField] private int shotgunTotalAmmoStart = 80;
    [SerializeField] private int rocketLTotalAmmoStart = 9;

    private static WeaponController _instance;

    public static WeaponController Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<WeaponController>();
#if UNITY_EDITOR
                if (FindObjectsOfType<WeaponController>().Length > 1) {
                    Debug.LogError("Found more than one weaponcontroller");
                }
#endif
            }
            return _instance;
        }
    }

    public BaseWeapon GetRifle() {
        BaseWeapon rifle = new BaseWeapon("Rifle", rifleDmg, 150, 9f, 1.6f, 0.1f, 15, rifleClipMaxAmmo, rifleClipMaxAmmo, rifleTotalAmmoStart);
        return rifle;
    }

    public BaseWeapon GetShotgun() {
        BaseWeapon shotgun = new BaseWeapon("Shotgun", shotgunDmg, 20, 2f, 2f, 0.1f, 30, shotgunClipMaxAmmo, shotgunClipMaxAmmo, shotgunTotalAmmoStart);
        return shotgun;
    }

    public ProjectileWeapon GetRocketLauncher() {
        ProjectileWeapon rocketLauncher = new ProjectileWeapon("Rocket Launcher", rocketLDmg, 100, 1.15f, 3.3f, 0.01f, 20, 15, rocketLClipMaxAmmo, rocketLClipMaxAmmo, rocketLTotalAmmoStart);
        return rocketLauncher;
    }

    public ProjectileWeapon GetEnemyProjectileWeapon() {
        ProjectileWeapon enemyWeapon = new ProjectileWeapon("Enemy Projectile", 10, 50, 0.9f, 0.3f, 0.1f, 20, 20, 99999, 99999, 99999);
        return enemyWeapon;
    }

}
