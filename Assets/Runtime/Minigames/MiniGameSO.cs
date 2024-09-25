using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOGame", menuName = "Mini Games/New Mini Game Data")]
public class MiniGameSO : ScriptableObject
{
    public GameObject[] models;
    public MiniGame.TouchMethods controls;

    public int maxScore;
    public int minScore;
    public int maxReps;
    public int minReps;
    public float timeLimit;
}
