using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour{
    //Author: Patrik Ahlgren

    [SerializeField] private LayerMask layerMask;
    
    [SerializeField] private GameObject bulletImpactMetalGO;
    [SerializeField] private GameObject bulletImpactMetalSGGO;
    [SerializeField] private GameObject bulletImpactAlienGO;

    [SerializeField] private ParticleSystem muzzleFlash;//------------

    [SerializeField] private float shotgunRecoil = 4;// TA BORT SEN
    [SerializeField] private float shotgunRecoilDuration = 0.3f; // TA BORT SEN

    private CameraShake camShake;
    [SerializeField] private Transform weaponCamera;
    private GameObject bulletImpact;
    private WeaponAnimation weaponAnimation;
    private float alienWoundTimer = 0.2f;

    private void Start(){
        camShake = Camera.main.GetComponent<CameraShake>();       
        muzzleFlash = transform.GetChild(0).GetComponent<ParticleSystem>();
        weaponAnimation = WeaponController.Instance.GetComponent<WeaponAnimation>();
    }

    //public void Melee() {
    //    bool hitTarget = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 5f, layerMask);
    //    if (hitTarget) {
    //        if (hit.rigidbody != null) {
    //            hit.rigidbody.AddForce(-hit.normal * 10f);
    //        }
    //        if (hit.collider.gameObject.layer == 9) {
    //            hit.transform.GetComponent<Enemy>().TakeDamage(100f);
    //        }
    //        InstantiateSingleBulletHit(bulletImpactMetalGO, hit, 2.0f);
    //    }
    //}

    public void StartShooting(BaseWeapon weapon) {
        if (weapon is ProjectileWeapon) {
            ShootProjectile((ProjectileWeapon) weapon);
        }
        else if (weapon.GetName().Equals("Shotgun")){
            ShootShotgunHitScan(weapon);         
        }            
        else {
            ShootHitScan(weapon);           
        }            
    }

    private void ShootHitScan(BaseWeapon weapon) {

        if (weapon.GetAmmoInClip() != 0) {           
            AudioController.Instance.PlaySFX("Rifle", 0.92f, 1f);
            muzzleFlash.Play();//----------------Animation
            weaponAnimation.ShootWeaponAnimation(weapon.GetName());
            weapon.DecreaseAmmoInClip();
            bool hitTarget = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, weapon.GetRange(), layerMask);

            if (hitTarget) {
                if (hit.rigidbody != null && hit.collider.gameObject.layer != 2) {
                    hit.rigidbody.AddForce(-hit.normal * weapon.GetImpactForce());
                }
                if (hit.collider.gameObject.layer == 9){
                    GameController.Instance.ShowHitmark(0.2f);
                    hit.transform.GetComponent<Enemy>().TakeDamage(weapon.GetDamage());
                    InstantiateSingleBulletHit(bulletImpactAlienGO, hit, alienWoundTimer);
                } else if(hit.collider.gameObject.layer == 13) {
                    hit.transform.GetComponent<DestructibleObject>().TakeDamage(weapon.GetDamage());
                    InstantiateSingleBulletHit(bulletImpactMetalGO, hit, 0.2f);
                } else {
                    InstantiateSingleBulletHit(bulletImpactMetalGO, hit, 2.0f);
                }
            }
        } else if (weapon.GetAmmoInClip() <= 0) {
            Debug.Log("Out of Ammo");
        }
        camShake.Shake(1f, 0.4f);
    }

    private void ShootShotgunHitScan(BaseWeapon weapon){
        if (weapon.GetAmmoInClip() != 0){           
            AudioController.Instance.PlaySFX("Shotgun", 0.95f, 1f);
            muzzleFlash.Play();//------------Animation
            weaponAnimation.ShootWeaponAnimation(weapon.GetName());
            weapon.DecreaseAmmoInClip();
            bool[] hitTarget = new bool[15];
            RaycastHit[] hits = new RaycastHit[15];
            for(int i = 0; i < hitTarget.Length; i++){
                float rndX = Random.Range(-0.1f, 0.1f), rndY = Random.Range(-0.03f, 0.03f);
                hitTarget[i] = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward + new Vector3(rndX, rndY, 0), out hits[i], weapon.GetRange(), layerMask);
            }
            for(int x = 0; x < hitTarget.Length; x++){
                if (hitTarget[x]){
                    if (hits[x].rigidbody != null && hits[x].collider.gameObject.layer != 2) {
                        hits[x].rigidbody.AddForce(-hits[x].normal * weapon.GetImpactForce());
                    }
                    if (hits[x].collider.gameObject.layer == 9){
                        GameController.Instance.ShowHitmark(0.5f);
                        hits[x].transform.GetComponent<Enemy>().TakeDamage(ShotGunHits(hits, x, weapon.GetDamage()));
                        //Debug.Log(ShotGunHits(hits, x, weapon.GetDamage()));
                        InstantiateMultipleBulletHits(bulletImpactAlienGO, hits, x, alienWoundTimer);                       
                    } else if (hits[x].collider.gameObject.layer == 13) {
                        hits[x].transform.GetComponent<DestructibleObject>().TakeDamage(ShotGunHits(hits, x, weapon.GetDamage()));
                        InstantiateMultipleBulletHits(bulletImpactMetalSGGO, hits, x, 0.2f);
                    } else {
                        InstantiateMultipleBulletHits(bulletImpactMetalSGGO, hits, x, 2.0f);
                    }               
                }
            }
            camShake.RecoilShake(shotgunRecoil, shotgunRecoilDuration);
            camShake.Shake(shotgunRecoil, 0.5f);
        }
        else if (weapon.GetAmmoInClip() <= 0){
            Debug.Log("Out of Ammo");
        }
    }

    private float ShotGunHits(RaycastHit[] hits, int hit, float damage) {
        float damageFallOff = Vector3.Distance(GameController.Instance.Player.transform.position, hits[hit].point);
        if (damageFallOff > (damage-5)) {
            damageFallOff = (damage - 5.1f);
        }
        float finalDamage = damage - damageFallOff;
        return finalDamage;
    }

    private void ShootProjectile(ProjectileWeapon weapon) {
        if (weapon.GetAmmoInClip() != 0) {
            weaponAnimation.ShootWeaponAnimation(weapon.GetName());
            AudioController.Instance.PlaySFX("RocketLauncher_Launch", 0.95f, 1f);
            weapon.DecreaseAmmoInClip();

            GameObject rocketProj = Instantiate(WeaponController.Instance.RocketLaucherProjectileGO, transform.position + (Camera.main.transform.forward*0.1f), Camera.main.transform.rotation);
            rocketProj.GetComponent<RocketProjectile>().SetProjectileSpeed(weapon.GetProjectileSpeed());
            rocketProj.GetComponent<RocketProjectile>().SetProjectileForce(weapon.GetImpactForce());
            rocketProj.GetComponent<RocketProjectile>().SetProjectileDamage(weapon.GetDamage());

        } else if (weapon.GetAmmoInClip() <= 0) {
            Debug.Log("Out of Ammo");
        }
        camShake.RecoilShake(4, 0.3f);
        camShake.Shake(2, 0.5f);
    }
    
    private void InstantiateMultipleBulletHits(GameObject impactGO, RaycastHit[] hits, int numberOfHits, float timeUntilDestroy) {
        bulletImpact = Instantiate(impactGO, hits[numberOfHits].point, Quaternion.LookRotation(hits[numberOfHits].normal));
        Destroy(bulletImpact, timeUntilDestroy);
    }
    private void InstantiateSingleBulletHit(GameObject impactGO, RaycastHit hit, float timeUntilDestroy) {
        bulletImpact = Instantiate(impactGO, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(bulletImpact, timeUntilDestroy);
    }

}