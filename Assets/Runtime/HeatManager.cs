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
    public static HeatManager instance;

    // - PRIVATE - //
    public float[] heatSpots = { 3000f, 3000f, 3000f, 3000f, 3000f };
    public float heatTotal = 15000f;
    public static float heatAvg = 3000;

    private bool obstacle = false;
    private bool gameOver;
    //public GameObject currentGameContainerPrefab;
    private GameObject currentGameContainer;
    private MiniGame currentGame;

    // - PUBLIC - //
    public float maxHeat = 10000;
    public float minHeat = 0;
    public float maxIndivHeat = 11000;
    public float minIndivHeat = -1000;
    public float timePlayed = 0f;

    public void Awake()
    {
        instance = this;
        StoveFire.heatUpdate += UpdateHeat;
    }
    private void OnDestroy()
    {
        StoveFire.heatUpdate -= UpdateHeat;
    }
    public void Start()
    {
        heatTotal = 15000f;
    }
    public void Update()
    {
        timePlayed += Time.deltaTime;
        if (timePlayed > 5f)
        {
            if (currentGame == null)
            {
                //currentGameContainer = Instantiate(currentGameContainerPrefab);
                currentGame = gameObject.AddComponent<MiniGame>();


                MiniGameSO[] potentialGames = Resources.LoadAll<MiniGameSO>("MiniGamesTypes");
                MiniGameSO selectedGame = potentialGames[Random.Range(0, potentialGames.Length - 1)];
                currentGame.settings = selectedGame;
                miniGameStart(ref currentGame);
            }
        }
    }

    // Start is called before the first frame update
    /*public void SubToRecieveHeat()
    {
        
        foreach(int heat in heatSpots)
        {
            heatTotal += heat;
        }
    }*/

    private void UpdateHeat(float delta, int spot)
    {
        heatSpots[spot] += delta;
        sectionHeat(heatSpots[spot], spot);
        heatTotal += delta;
        heatAvg = heatTotal / 5;
        if (sendHeat != null) sendHeat(heatAvg, spot);
        if (timePlayed >= 60f)
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

        

        //Debug.Log(heatSpots[spot] + " " + heatAvg);
    }
}
