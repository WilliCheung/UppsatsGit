using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//Author: Marcus Söderberg
public class EnemyData
{
    public float EnemyPositionX { get; set; }
    public float EnemyPositionY { get; set; }

    public float EnemyPositionZ { get; set; }

    public float EnemyRotationX { get; set; }
    public float EnemyRotationY { get; set; }
    public float EnemyRotationZ { get; set; }
    public float EnemyHealth { get; set; }

    public int EnemyNumber { get; set; }

    public string EnemyName { get; set; }

    public int ParentID { get; set; }

    public EnemyData(Enemy enemy)
    {
        EnemyName = enemy.gameObject.name;

        EnemyHealth = enemy.health;

        EnemyPositionX = enemy.transform.position.x;
        EnemyPositionY = enemy.transform.position.y;
        EnemyPositionZ = enemy.transform.position.z;

        EnemyRotationX = enemy.transform.eulerAngles.x;
        EnemyRotationY = enemy.transform.eulerAngles.y;
        EnemyRotationZ = enemy.transform.eulerAngles.z;

        ParentID = enemy.ParentID;
    }
}
