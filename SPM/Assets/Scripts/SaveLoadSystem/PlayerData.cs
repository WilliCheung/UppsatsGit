using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//Author: Marcus Söderberg
public class PlayerData
{
    public float PlayerHP { get; set; }
    public float PlayerArmor { get; set; }
    public float SlowmotionValue { get; set; }
    public float[] PlayerPosition { get; set; }
    public float[] PlayerRotation { get; set; }

    public List<BaseWeapon> PlayerWeapons { get; set; }
    public BaseWeapon SelectedWeapon { get; set; }


    public PlayerData(GameController gameController)
    {
        PlayerHP = gameController.PlayerHP;
        PlayerArmor = gameController.PlayerArmor;
        SlowmotionValue = gameController.SlowmotionSlider.value;

        PlayerPosition = new float[3];
        PlayerPosition[0] = gameController.Player.transform.position.x;
        PlayerPosition[1] = gameController.Player.transform.position.y;
        PlayerPosition[2] = gameController.Player.transform.position.z;

        PlayerRotation = new float[3];
        PlayerRotation[0] = gameController.Player.transform.eulerAngles.x;
        PlayerRotation[1] = gameController.Player.transform.eulerAngles.y;
        PlayerRotation[2] = gameController.Player.transform.eulerAngles.z;

        PlayerWeapons = gameController.PlayerWeapons;
        SelectedWeapon = gameController.SelectedWeapon;

    }
}
