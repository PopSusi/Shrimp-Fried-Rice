using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI FlipAnnounce;
    private Coroutine flipCoroutine = null;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI multText;
    [SerializeField] private TextMeshProUGUI debugHeat;
    [SerializeField] private RectMask2D heatBar;
    [SerializeField] private GameObject lossMenu;
    [SerializeField] private TextMeshProUGUI lossScore;
    [SerializeField] private TextMeshProUGUI lossTime;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private TextMeshProUGUI winScore;
    [SerializeField] private TextMeshProUGUI winTime;

    // Start is called before the first frame update
    void Awake()
    {
        ScoreReciever.scoreSend += ScoreUpdate;
        WokController.UIFlipUpdate += UpdateFlip;
        HeatManager.sendHeat += UpdateHeatBar;

        //HeatManager.miniGameStart += StartGame;
        HeatManager.endGame += GameOverUI;
    }

    // Update is called once per frame
    void Update()
    {
        HeatManager.UpdateCheck();
    }

    public void UpdateFlip(string incomingMessage)
    {
        if (flipCoroutine != null) StopCoroutine(flipCoroutine);
        FlipAnnounce.gameObject.SetActive(true);
        FlipAnnounce.text = incomingMessage;
        flipCoroutine = StartCoroutine(WaitandDissolve(2f, FlipAnnounce));
    }

    public IEnumerator WaitandDissolve(float waitTime, TextMeshProUGUI dissolvedText)
    {
        yield return new WaitForSeconds(waitTime);
        dissolvedText.gameObject.SetActive(false);
        flipCoroutine = null;
    }

    public void ScoreUpdate(int score, float scoreMult)
    {
        scoreText.text = score.ToString();
        multText.text = scoreMult.ToString("0.0") + "x";
    }

    public void UpdateHeatBar(float heat, int spot)
    {
        heatBar.padding = new Vector4(0f, 0f, (10000 - heat) / 10, 0f);
        debugHeat.text = heat.ToString();
    }

    private void GameOverUI(string reasoning, float time)
    {
        Debug.Log(reasoning + " " + time);
        if(reasoning == "Too hot!" || reasoning == "Too cold!")
        {
            lossMenu.SetActive(true);
            lossScore.text = scoreText.text;
            int tempTimeToInt = (int) time;
            lossTime.text = tempTimeToInt.ToString();
        } else 
        {
            winMenu.SetActive(true);
            winScore.text = scoreText.text;
            int tempTimeToInt = (int)time;
            winTime.text = tempTimeToInt.ToString();
        }
    }
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Primary");
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("UITest");
    }

}
