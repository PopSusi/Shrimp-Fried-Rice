using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreReciever : MonoBehaviour
{
    public static ScoreReciever instance;
    [Range(0f, 5f)]
    public float scoreMult = 1;
    public float scoreMultVelocity = .02f;

    public int score;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
    }

    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        scoreMult -= scoreMultVelocity * Time.deltaTime;
        UIManager.instance.ScoreUpdate(score, scoreMult);
    }

    public void UpdateScore(int scoreIncoming)
    {
        score += (int)scoreMult * scoreIncoming;
        UIManager.instance.ScoreUpdate(score, scoreMult);
    }
    public void UpdateScore(int scoreIncoming, float multiplier)
    {
        scoreMult += multiplier;
        score += (int)scoreMult * scoreIncoming;
        UIManager.instance.ScoreUpdate(score, scoreMult);
    }
}
