using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI FlipAnnounce;
    private Coroutine flipCoroutine = null;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI multText;
    [SerializeField] private TextMeshProUGUI debugHeat;
    [SerializeField] private RectMask2D heatBar;

    // Start is called before the first frame update
    void Awake()
    {
        ScoreReciever.scoreSend += ScoreUpdate;
        WokController.UIFlipUpdate += UpdateFlip;
        HeatManager.sendHeat += UpdateHeatBar;

        HeatManager.miniGameStart += StartGame;
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
        heatBar.padding = new Vector4(0f, 0f, heat / 10, 0f);
        debugHeat.text = heat.ToString();
    }

    private void StartGame(ref MiniGame game)
    {
        
    }

}
