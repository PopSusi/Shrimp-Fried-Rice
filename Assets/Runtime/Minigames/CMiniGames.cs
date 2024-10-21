using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "CGame", menuName = "Mini Games/New Mini Game Class")]
public class MiniGame : MonoBehaviour
{
    public MiniGameSO settings;
    public delegate void MiniGameAnnounce();
    public static event MiniGameAnnounce OnOver;
    public static event MiniGameAnnounce ChopSound;

    public delegate void MiniGameScores(int score, float mult);
    public static event MiniGameScores Scores;

    protected float timeInMiniGame;
    protected float reps;
    protected bool inputEnabled;

    private GameObject model;

    protected void Start()
    {
        EnableControls();
        SpawnModel();
    }

    protected void SpawnModel()
    {
        //Spawn random model list of models in setting
        model = Instantiate(settings.models[UnityEngine.Random.Range(0, settings.models.Length - 1)], new Vector3(0, 1.4f, 0), Quaternion.identity);
    }

    protected void Update()
    {
        Debug.Log("Mini game is updating");
        timeInMiniGame += Time.deltaTime;
        MiniGameCheck();
        switch (settings.controls)
        {
            case (TouchMethods.Tap):
                TapCheck();
                break;
            case (TouchMethods.Swipe):
                SwipeCheck();
                break;
            case (TouchMethods.Gyro):
                GyroCheck();
                break;
            default:
                TapCheck();
                break;
        }

    }
    protected void MiniGameCheck()
    {
        if(timeInMiniGame >= settings.timeLimit)
        {
            GameDead();
            OnOver();
            Debug.Log("out of time");
        } else if(reps >= settings.maxReps)
        {
            OnOver();
            Debug.Log("Beat Threshold");
        }
    }
    private void GameDead()
    {
        float repMod;
        // --- SEND OUT SCORE --- //
        //XREP = REMAP REPS DONE BETWEEN REPS MIN AND MAX TO A VALUE BETWEEN 0 AND 1
        repMod = (reps - settings.minReps) / (settings.maxReps - settings.minReps) * (1 - 0) + 0;
        //(value - from1) / (to1 - from1) * (to2 - from2) + from2;
        if (reps < settings.minReps)
        {
            repMod = 0;
        } else if (reps > settings.maxReps)
        {
            repMod = 1;
        }
        //https://discussions.unity.com/t/mapping-or-scaling-values-to-a-new-range/503453/10?clickref=1101lzLyPwuu&utm_source=partnerize&utm_medium=affiliate&utm_campaign=unity_affiliate
        //SCORE = REMAP XREP FROM 0 TO 1 INTO MIN SCORE TO MAX SCORE
        float score = (repMod - 0) / (1 - 0) * (settings.maxScore - settings.minScore) + settings.minScore;
        //(value - from1) / (to1 - from1) * (to2 - from2) + from2;
        //SEND SCORE TO SCORERECIEVER
        Scores( (int) score, repMod);
        Debug.Log("score: " + score + "score int: " + (int) score + "repMod: " + repMod + " from " + reps + " reps.");
        Destroy(this);
    }
    public void EnableControls()
    {
        inputEnabled = true;
    } 
    protected void DisableControls()
    {
        OnOver();
    }
    public enum TouchMethods
    {
        Swipe,
        Tap,
        Gyro
    }
    protected void TapCheck()
    {
        if (inputEnabled)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                reps += 1;
                ChopSound();
                Debug.Log("Tap");
            }
        }
    }
    protected void GyroCheck()
    {
        throw new NotImplementedException();
    }
    protected void SwipeCheck()
    {
        throw new NotImplementedException();
    }
    protected void ControlOff()
    {
        inputEnabled = false;
    }
    protected void Deletion()
    {
        Destroy(gameObject.GetComponent<MiniGame>());
    }

    ~MiniGame()
    {
        Debug.Log("I go bye bye");
    }
}
