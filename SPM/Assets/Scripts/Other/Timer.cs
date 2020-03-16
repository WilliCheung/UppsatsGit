using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour{
    //Author: Patrik Ahlgren
    private float minuteTimeCount = 0;
    private float secondsTimeCount = 0;
    private float totalSecondsTimeCount;

    public bool TimerIsActive;

    private Text timerText;

    private void Start() {
       timerText = GameObject.Find("TimerText").GetComponent<Text>();
        timerText.text = "00:00.00";
        //secondsTimeCount = 00;//TA BORT SEN
        //minuteTimeCount = 15; // TA BORT SEN
        TimerIsActive = true;
    }

    private void Update() {
        if (TimerIsActive) {
            if (!GameController.Instance.GameIsPaused) {
                secondsTimeCount += Time.unscaledDeltaTime;
            }
            if (secondsTimeCount >= 60f) {
                secondsTimeCount -= 60;
                ++minuteTimeCount;
            }
            timerText.text = minuteTimeCount.ToString("00") + ":" + secondsTimeCount.ToString("00.00");
            if (secondsTimeCount < 0) {//ifall vi ska ha system att tiden minskar om man dödar fiender eller något
                secondsTimeCount = 0;
            }
            totalSecondsTimeCount = (minuteTimeCount * 60) + secondsTimeCount;
        }       
    }

    public GameObject GetTimerObject() {
        return gameObject;
    }

    public float GetFinalTime() {
        return totalSecondsTimeCount;
    }

    public float GetMinutesTaken() {
        return minuteTimeCount;
    }

    public float GetSecondsTaken() {
        return secondsTimeCount;
    }

    public void SetTimer(float timer) {
        secondsTimeCount = timer;
    }

    public void AddToTimer(float timeAdded) {
        secondsTimeCount += timeAdded;
    }

    public void SubtractFromTimer(float timeSubtracted) {
        secondsTimeCount -= timeSubtracted;
    }
}
