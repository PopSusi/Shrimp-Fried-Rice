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
    private static float[] heatSpots = { 1000f, 1000f, 1000f, 1000f, 1000f };
    private static float heatTotal;
    private static float heatAvg = 1000;

    private static bool obstacle = false;
    private static bool gameOver;
    private static MiniGame currentGame;

    // - PUBLIC - //
    public static float maxHeat;
    public static float minHeat;
    public static float timePlayed;

    public static void UpdateCheck()
    {
        //timePlayed += Time.deltaTime;
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

        if (heatAvg >= maxHeat)
        {
            if (!gameOver && endGame != null) endGame("Too hot!", timePlayed);
            //Debug.Log("too hot");
        }
        else if (heatAvg <= minHeat)
        {
            if (!gameOver && endGame != null) endGame("Too cold!", timePlayed);
            //Debug.Log("too cold");
        }

        heatSpots[spot] += delta;
        sectionHeat(heatSpots[spot], spot);

        heatTotal += delta;
        heatAvg =  heatTotal / 5;
        sendHeat(heatAvg, spot);

        //Debug.Log(heatSpots[spot] + " " + heatAvg);
    }
}
