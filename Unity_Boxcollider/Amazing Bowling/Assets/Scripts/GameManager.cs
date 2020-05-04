using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent onReset;

    public static GameManager instance;

    public GameObject readyPenal;
    public Text scoreText;
    public Text bestScoreText;
    public Text messageText;
    public bool isRoundActive = false;
    public ShooterRotator ShooterRotator;
    public CamFollow cam;

    private int score = 0;

    private void Awake()
    {
        instance = this;
        UpdateUI();
    }

    private void Start()
    {
        StartCoroutine(RoundRoutine());
    }

    public void AddSocre(int newScore)
    {
        score += newScore;
        UpdateBestScore();
        UpdateUI();
    }

    private void UpdateBestScore()
    {
        if(GetBestScore() < score)
        {
            PlayerPrefs.SetInt("BestScore", score);
        }
    }

    private int GetBestScore()
    {
        int bestSocre = PlayerPrefs.GetInt("BestScore");
        return bestSocre;
    }

    private void UpdateUI()
    {
        scoreText.text = "Score : " + score;
        bestScoreText.text = "Best Score : " + GetBestScore();
    }

    public void OnBallDestroy()
    {
        UpdateUI();
        isRoundActive = false;
    }

    public void Reset()
    {
        // 라운드를 다시 처음부터 시작 코드
        score = 0;
        UpdateUI();
        StartCoroutine(RoundRoutine());
    }

    IEnumerator RoundRoutine()
    {
        // Read
        onReset.Invoke();

        readyPenal.SetActive(true);
        cam.SetTarget(ShooterRotator.transform, CamFollow.State.Idle);
        ShooterRotator.enabled = false;

        isRoundActive = false;

        messageText.text = "Ready...";

        yield return new WaitForSeconds(3f);

        // Play
        isRoundActive = true;
        readyPenal.SetActive(false);
        ShooterRotator.enabled = true;

        cam.SetTarget(ShooterRotator.transform, CamFollow.State.Ready);
        
        while(isRoundActive == true)
        {
            yield return null;
        }

        // End

        readyPenal.SetActive(true);
        ShooterRotator.enabled = false;

        messageText.text = "Wait For Next Round...";

        yield return new WaitForSeconds(3f);

        Reset();
    }
}
