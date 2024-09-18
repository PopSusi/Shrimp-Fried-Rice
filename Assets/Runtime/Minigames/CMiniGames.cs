using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CGame", menuName = "Mini Games/New Mini Game Class")]
public class MiniGame : MonoBehaviour
{
    public MiniGameSO settings;
    public delegate void MiniGameOver();
    public static event MiniGameOver OnOver;

    protected float timeInMiniGame;
    protected float reps;
    protected bool inputEnabled;

    protected void Awake()
    {
        OnOver += Deletion;
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
            OnOver();
            Debug.Log("out of time");
        } else if(reps >= settings.maxReps)
        {
            OnOver();
            Debug.Log("Beat Threshold");
        }
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
