using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HeatManager : MonoBehaviour
{
    // --- EVENTS --- //
    public delegate void CalculatedHeat(float heat, int spot);
    public static CalculatedHeat sendHeat;
    public static CalculatedHeat sectionHeat;

    public delegate void GameState(string reasoning, float time);
    public delegate void MiniGameState(ref MiniGame game);
    public static GameState endGame;
    public static MiniGameState miniGameStart;

    // --- VARIABLES --- //

    // - PRIVATE - //
    private static float[] heatSpots = { 3000f, 3000f, 3000f, 3000f, 3000f };
    private static float heatTotal;
    private static float heatAvg = 3000;

    private static bool obstacle = false;
    private static bool gameOver;
    private static MiniGame currentGame;

    // - PUBLIC - //
    public static float maxHeat = 10000;
    public static float minHeat = 0;
    public static float maxIndivHeat = 11000;
    public static float minIndivHeat = -1000;
    public static float timePlayed = 0f;

    public static void UpdateCheck()
    {
        timePlayed += Time.deltaTime;
    }

    // Start is called before the first frame update
    public static void SubToRecieveHeat()
    {
        StoveFire.heatUpdate += UpdateHeat;
        foreach(int heat in heatSpots)
        {
            heatTotal += heat;
        }
    }

    private static void UpdateHeat(float delta, int spot)
    {
        if(timePlayed >= 60f)
        {
            Time.timeScale = 0f;
            endGame("Won!", timePlayed);
        }
        if (heatAvg >= maxHeat)
        {
            Time.timeScale = 0f;
            if (!gameOver && endGame != null) endGame("Too hot!", timePlayed);
            //Debug.Log("too hot");
        }
        else if (heatAvg <= minHeat)
        {
            Time.timeScale = 0f;
            if (!gameOver && endGame != null) endGame("Too cold!", timePlayed);
            //Debug.Log("too cold");
        }
        foreach (var indivHeat in heatSpots)
        {
            if (indivHeat <= minIndivHeat)
            {
                Time.timeScale = 0f;
                if (!gameOver && endGame != null) endGame("Too cold!", timePlayed);
            } else if (indivHeat >= maxIndivHeat)
            {
                Time.timeScale = 0f;
                if (!gameOver && endGame != null) endGame("Too hot!", timePlayed);
            }
        }

        heatSpots[spot] += delta;
        sectionHeat(heatSpots[spot], spot);

        heatTotal += delta;
        heatAvg =  heatTotal / 5;
        sendHeat(heatAvg, spot);

        //Debug.Log(heatSpots[spot] + " " + heatAvg);
    }
}
