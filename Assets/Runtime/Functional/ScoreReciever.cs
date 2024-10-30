using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WokController;

public class ScoreReciever : MonoBehaviour
{
    [Range(0f, 5f)]
    public float scoreMult = 1;
    public float scoreMultVelocity = .01f;

    public int score;

    public delegate void ScoreSend(int scoreOut, float multOut);
    public static event ScoreSend scoreSend;
    
    // Start is called before the first frame update
    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        score = 10000;
        scoreMult = 1f;
        MiniGame.Scores += UpdateScore;
        WokController.UpdateScores += UpdateScore;
    }
    private void Start()
    {
        if (scoreSend != null)  scoreSend(score, scoreMult);
    }
    private void FixedUpdate()
    {
        scoreMult -= scoreMultVelocity * Time.deltaTime;
        if(scoreSend != null) scoreSend(score, scoreMult);
    }
    public void UpdateScore(int scoreIncoming, float multiplier)
    {
        if (!HeatManager.gameOver) {
            scoreMult += multiplier;
            score += (int)scoreMult * scoreIncoming;
            Debug.Log(score + " with " + scoreIncoming + " incoming");
            if(scoreMult < 1f)
            {
                scoreMult = 1f;
            }
            scoreSend(score, scoreMult);
        }
    }

    ~ScoreReciever()
    {
        MiniGame.Scores -= UpdateScore;
        WokController.UpdateScores -= UpdateScore;
    }
}
