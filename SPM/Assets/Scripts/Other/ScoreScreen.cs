using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour{
    //Author: Patrik Ahlgren

    [SerializeField] private Text killCountText;

    [SerializeField] private Text timeScoreText;
    [SerializeField] private Text killScoreText;

    [SerializeField] private Text totalScoreValueText;

    [SerializeField] private GameObject congratulationsText;
    [SerializeField] private GameObject scoreTexts;
    [SerializeField] private GameObject mainMenuButton;

    private int maxTimerScore = 1800;

    private int timeScore = 0;
    private int killScore = 0;
    private int totalScore = 0;

    private bool isCountingScore;

    private void Awake() {
        congratulationsText.SetActive(false);
        scoreTexts.SetActive(false);
        mainMenuButton.GetComponent<Button>().onClick.AddListener(delegate { MainMenu(); });
        mainMenuButton.SetActive(false);
    }

    private void Update() {
        if (isCountingScore) {
            totalScoreValueText.text = totalScore.ToString();
            timeScoreText.text = "TimeScore: " + timeScore;
            killScoreText.text = "KillScore: " + killScore;
        }
    }

    public void CountScore(float minutesTaken, float secondsTaken, float killCount) {
        isCountingScore = true;
        scoreTexts.gameObject.SetActive(true);
        killCountText.text = "Kills: " + killCount;

        gameObject.transform.SetAsLastSibling();

        StartCoroutine(Counter(minutesTaken, secondsTaken, killCount));
    }

    private IEnumerator Counter(float minutesTaken, float secondsTaken, float killCount) {
        int secondsScore = 60 - Mathf.RoundToInt(secondsTaken);
        int minuteScore = 29 - (int)minutesTaken;
        if(minuteScore <= 0) {
            minuteScore = 0;
        } else {
            for (int i = 0; i < minuteScore; i++) {
                timeScore += 60;
                totalScore += 60;
                maxTimerScore -= 60;
                yield return new WaitForFixedUpdate();
            }
        }
        //animation som förstorar timescore ett kort tag och minskar ner
        if(secondsScore <= 0) {
            secondsScore = 0;
        } else {
            for (int i = 0; i < secondsScore; i++) {
                timeScore++;
                totalScore++;
                yield return new WaitForFixedUpdate();
            }
        }

        //animation som förstorar killscore ett kort tag och minskar ner
        for(int i = 0; i < killCount; i++) {
            killScore += 10;
            totalScore += 10;
            yield return new WaitForFixedUpdate();
        }
        congratulationsText.SetActive(true);
        mainMenuButton.SetActive(true);
        isCountingScore = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameController.Instance.GamePaused();

        yield return null;
    }

    private void MainMenu() {
        SaveSystem.DeleteAllSaveFiles();
        GameObject.Find("MenuController").GetComponent<MenuController>().MainMenu();
    }

}
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