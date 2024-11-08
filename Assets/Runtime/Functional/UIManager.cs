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

    

    public static UIManager instance;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI multText;
    [SerializeField] private TextMeshProUGUI debugHeat;
    [SerializeField] private RectMask2D heatBar;
    [SerializeField] private RectMask2D heatBarPrefab;
    [SerializeField] private GameObject lossMenu;
    [SerializeField] private TextMeshProUGUI lossScore;
    [SerializeField] private TextMeshProUGUI lossTime;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private TextMeshProUGUI winScore;
    [SerializeField] private TextMeshProUGUI winTime;
    [SerializeField] private GameObject TimeLeftGO;
    

    // Start is called before the first frame update
    void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        //heatBar = Instantiate(heatBarPrefab);
        ScoreReciever.scoreSend += ScoreUpdate;
        WokController.UIFlipUpdate += UpdateFlip;
        HeatManager.sendHeat += UpdateHeatBar;

        //HeatManager.miniGameStart += StartGame;
        HeatManager.endGame += GameOverUI;
        MiniGame.Gimme += GiveUI;
    }

    // Update is called once per frame
    void Update()
    {
        //HeatManager.instance.UpdateCheck();
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
        heatBar.padding = new Vector4(0f, 0f, (10000 - 3000f) / 10, 0f);
        Time.timeScale = 1f;
        HeatManager.instance.timePlayed = 0f;
        HeatManager.gameOver = false;
        lossMenu.SetActive(false);
        winMenu.SetActive(false);
        SceneManager.LoadScene("Primary");
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("UITest");
    }

    private void OnSceneUnloaded(Scene scene)
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        //heatBar = Instantiate(heatBarPrefab);
        ScoreReciever.scoreSend -= ScoreUpdate;
        WokController.UIFlipUpdate -= UpdateFlip;
        HeatManager.sendHeat -= UpdateHeatBar;

        //HeatManager.miniGameStart += StartGame;
        HeatManager.endGame -= GameOverUI;
        MiniGame.Gimme -= GiveUI;
    }

    private GameObject GiveUI(){
        TimeLeftGO.SetActive(true);
        return TimeLeftGO;
    }


}
