using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameButton : MonoBehaviour {
    //Author: Patrik Ahlgren

    [SerializeField] private GameObject timerGO;
    [SerializeField] private Animator anim;
    private Timer timer;

    private ScoreScreen scoreScreen;

    private void Start() {
        timerGO = GameObject.Find("TimerText");
        timer = GameController.Instance.GetComponent<Timer>();
        scoreScreen = GameObject.Find("ScoreScreen").GetComponent<ScoreScreen>();
        anim = GameObject.Find("Fade").GetComponent<Animator>();
    }

    public void PressButton() {
        GameController.Instance.Player.GetComponent<PlayerInput>().InputIsFrozen = true;
        timer.TimerIsActive = false;
        timerGO.transform.SetAsLastSibling();
        StartCoroutine(ExplosionEnding());
    }

    private IEnumerator ExplosionEnding() {
        AudioController.Instance.Play_InWorldspace("Explosion", GameController.Instance.Player, 0.95f, 1.5f);
        Camera.main.GetComponent<CameraShake>().ShakeIncrease(25f, 1.5f);
        yield return new WaitForSeconds(1f);
        AudioController.Instance.Play_InWorldspace("Explosion", GameController.Instance.Player, 0.95f, 1.5f);
        Camera.main.GetComponent<CameraShake>().ShakeIncrease(30f, 3f);
        yield return new WaitForSeconds(2f);
        anim.SetTrigger("FadeOut");
        AudioController.Instance.Play_InWorldspace("Explosion", GameController.Instance.Player, 0.95f, 1.5f);
        Camera.main.GetComponent<CameraShake>().ShakeIncrease(15f, 1f);
        Camera.main.GetComponent<CameraShake>().ShakeIncrease(25f, 2f);
        yield return new WaitForSeconds(2f);
        AudioController.Instance.StopAllSounds();
        scoreScreen.CountScore(timer.GetMinutesTaken(), timer.GetSecondsTaken(), GameController.Instance.KillCount);
        yield return null;
    }

//    ENDGAME

//Dörren öppnas till sista utrymmet
//AudioController drar igång AlarmLjud vid knappen
//Tip: Press the button to save humanity!

//Knappen går att trycka på/dyker upp(tydlig placering)

//Klicka på knappen
//Timern stannar(set as LastSibling)
//Skärmen fadear
//Skärmen skakar
//Explosionljud
//Allt ljud stoppas
//5sec

//Svart skärm med Score
//Score är tid och killcount

//Killcount +10 i Score per kill

//TimeBonus +1 per sec under 30min


//1800sec = 30min
//900sec

//30minfloat - totalTimefloat = Timescore

//Timer: 15:31:02
//Kills: 20

//SCORE: 581! (räknar upp)
}
