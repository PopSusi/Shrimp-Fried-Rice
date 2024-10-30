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

    public delegate void Pause(bool paused);
    public event Pause SendPause;

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
    [SerializeField] private GameObject SettingsMenuWrapper;
    [SerializeField] private GameObject PauseMenuWrapper;
    [SerializeField] private GameObject QuitMenuWrapper;
    [SerializeField] private GameObject ResetMenuWrapper;
    private string confirmChoice;
    public static bool paused;

    // Start is called before the first frame update
    void Awake()
    {

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
        lossMenu.SetActive(false);
        winMenu.SetActive(false);
        SceneManager.LoadScene("Primary");
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("UITest");
    }

    private void OnDestroy()
    {
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

    public void MenuInput(string menuName)
    {
        switch (menuName)
        {
            case "Pause":
                if (SettingsMenuWrapper.activeInHierarchy) SettingsMenuWrapper.SetActive(false);
                if (paused)
                {
                    PauseMenuWrapper.SetActive(false);
                    Time.timeScale = 1f;
                    Debug.Log("unpausing");
                    paused = false;
                } else
                {
                    PauseMenuWrapper.SetActive(true);
                    Time.timeScale = 0f;
                    paused = true;
                }
                if (ResetMenuWrapper.activeInHierarchy) ResetMenuWrapper.SetActive(false);
                if (QuitMenuWrapper.activeInHierarchy) QuitMenuWrapper.SetActive(false);
                break;
            case "Reset":
                if (SettingsMenuWrapper.activeInHierarchy) SettingsMenuWrapper.SetActive(false);
                if (PauseMenuWrapper.activeInHierarchy) PauseMenuWrapper.SetActive(false);
                if (QuitMenuWrapper.activeInHierarchy) QuitMenuWrapper.SetActive(false);
                if (!ResetMenuWrapper.activeInHierarchy)
                {
                    ResetMenuWrapper.SetActive(true);
                    confirmChoice = "ResetConfirm";
                }
                break;
            case "Settings":
                if (!SettingsMenuWrapper.activeInHierarchy) SettingsMenuWrapper.SetActive(true);
                if (PauseMenuWrapper.activeInHierarchy) PauseMenuWrapper.SetActive(false);
                if (ResetMenuWrapper.activeInHierarchy) ResetMenuWrapper.SetActive(false);
                if (QuitMenuWrapper.activeInHierarchy) QuitMenuWrapper.SetActive(false);
                break;
            case "Quit":
                if (SettingsMenuWrapper.activeInHierarchy) SettingsMenuWrapper.SetActive(false);
                if (PauseMenuWrapper.activeInHierarchy) PauseMenuWrapper.SetActive(false);
                if (ResetMenuWrapper.activeInHierarchy) ResetMenuWrapper.SetActive(false);
                if (!QuitMenuWrapper.activeInHierarchy) {
                    QuitMenuWrapper.SetActive(true);
                    Debug.Log("opening quit");
                    confirmChoice = "QuitConfirm";
                }
                break;
        }
        Debug.Log(paused);
    }

    public void ConfirmChoice()
    {
        switch (confirmChoice)
        {
            case "ResetConfirm":
                SceneManager.LoadScene("Primary");
                break;
            case "QuitConfirm":
                SceneManager.LoadScene("UITest");
                break;
        }
    }
}
