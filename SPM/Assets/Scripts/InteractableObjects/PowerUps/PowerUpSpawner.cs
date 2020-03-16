using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    //Author: Marcus Söderberg
    public GameObject Armor, Health, Speed, Ammo;

    public bool ArmorPowerUp, HealthPowerUp, MoveSpeedPowerUp, AmmoBoxPowerUp;

    public float RespawnTime = 5f;

    public int clipIncrease;

    private GameObject PowerUpToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        if (ArmorPowerUp) { PowerUpToSpawn = Armor; }
        if (HealthPowerUp) { PowerUpToSpawn = Health; }
        if (MoveSpeedPowerUp) { PowerUpToSpawn = Speed; }
        if (AmmoBoxPowerUp) { PowerUpToSpawn = Ammo; }

        var pu = Instantiate(PowerUpToSpawn, transform.position, Quaternion.identity);
        pu.transform.parent = gameObject.transform;

        if (AmmoBoxPowerUp)
        {
            GetComponentInChildren<AmmoBox>().setClipIncrease(clipIncrease);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Respawner()
    {

        StartCoroutine(Spawntimer(PowerUpToSpawn, RespawnTime));
    }

    IEnumerator Spawntimer(GameObject PowerUpToSpawn, float RespawnTime)
    {
        yield return new WaitForSeconds(RespawnTime);

        SpawnPowerUp();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    void SpawnPowerUp()
    {
        var pu = Instantiate(PowerUpToSpawn, transform.position, Quaternion.identity);
        pu.transform.parent = gameObject.transform;
        
        if (AmmoBoxPowerUp)
        {
            GetComponentInChildren<AmmoBox>().setClipIncrease(clipIncrease);
        }
    }
}
