using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public float heatAvg = 3000;
    [SerializeField] float shownHeat;

    //private bool obstacle = false;
    public static bool gameOver;
    //public GameObject currentGameContainerPrefab;
    private GameObject currentGameContainer;
    private MiniGame currentGame;

    // - PUBLIC - //
    public float maxHeat = 10000;
    public float minHeat = 0;
    public float maxIndivHeat = 11000;
    public float minIndivHeat = -1000;
    public float timePlayed = 0f;

    public float intervalMinBeforeNextGame;
    public float intervalMaxBeforeNextGame;
    private float nextIntervalTime;

    public void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        instance = this;
        StoveFire.heatUpdate += UpdateHeat;
        MiniGame.OnOver += NewTime;
        //Debug.Log(heatTotal + " before set");
        heatTotal = 15000f;
        NewTime();
        //Debug.Log(heatTotal);
        //Debug.Log(heatTotal + " after set");
    }
    private void OnSceneUnloaded(Scene scene)
    {
        StoveFire.heatUpdate -= UpdateHeat;
        MiniGame.OnOver -= NewTime;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        Debug.Log("Destroyed");
    }
        
    public void Update()
    {
        shownHeat = heatAvg;
        timePlayed += Time.deltaTime;
        if (timePlayed > nextIntervalTime) //&& !SettingsManager.IsTutorial())
        {
            //Debug.Log(SettingsManager.IsTutorial() + " FUCK YOU");
            if (currentGame == null)
            {
                //currentGameContainer = Instantiate(currentGameContainerPrefab);
                currentGame = gameObject.AddComponent<MiniGame>();


                MiniGameSO[] potentialGames = Resources.LoadAll<MiniGameSO>("MiniGamesTypes");
                MiniGameSO selectedGame = potentialGames[Random.Range(0, potentialGames.Length - 1)];
                currentGame.settings = selectedGame;
                miniGameStart(ref currentGame);
                NewTime();
            }
        } else if (timePlayed >= 60f)
        {
            //Time.timeScale = 0f;
            endGame("Won!", timePlayed);
            int gamesWon = PlayerPrefs.GetInt("gamesWon", 0);
            gamesWon++;
            PlayerPrefs.SetInt("gamesWon", gamesWon);
            CheckHats(gamesWon);
            gameOver = true;
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
        //Debug.Log("Updating and game is over: " + gameOver.ToString());
        if (!gameOver)
        {
            //Update each spots individual counters with Delta
            heatSpots[spot] += delta;
            sectionHeat(heatSpots[spot], spot);
            heatTotal = Mathf.Clamp(heatTotal + delta, 100, 100000);

            //Debug.Log(heatTotal + "just before calculation");
            //Debug.Log(heatTotal / 5 + "math before calc");

            heatAvg = heatTotal / 5;

            //Debug.Log(heatTotal + "just after calculation");
            //Debug.Log(heatTotal / 5 + "math after calc");
            //Debug.Log("adding heat. without clamp is " + heatTotal + delta);
            //Debug.Log("adding heat. currently is " + Mathf.Clamp(heatTotal + delta, 100, 100000));
            //Debug.Log("adding heat. diff is " + delta);


            if (sendHeat != null) sendHeat(heatAvg, spot);

            if (!SettingsManager.IsInvincible()){

                //Lose Conditions
                if (heatAvg >= maxHeat)
                {
                    Time.timeScale = 0f;
                    //Debug.Log("game frozen because pan was too hot");
                    if (!gameOver) endGame("Too hot!", timePlayed);
                    //Debug.Log("too hot");
                    gameOver = true;
                }
                else if (heatAvg <= minHeat)
                {
                    Time.timeScale = 0f;
                    //Debug.Log("game frozen because pan too cold" + heatAvg);
                    //Debug.Log("HeatTotal is " + heatTotal);
                    if (!gameOver) endGame("Too cold!", timePlayed);
                    //Debug.Log("too cold");
                    gameOver = true;
                }
                foreach (var indivHeat in heatSpots)
                {
                    if (indivHeat <= minIndivHeat)
                    {
                        Time.timeScale = 0f;
                        //Debug.Log("game frozen because one side was too cold");
                        if (!gameOver) endGame("Too cold!", timePlayed);
                        gameOver = true;
                    }
                    else if (indivHeat >= maxIndivHeat)
                    {
                        Time.timeScale = 0f;
                        //Debug.Log("game frozen because one side was too hot");
                        if (!gameOver) endGame("Too hot!", timePlayed);
                        gameOver = true;
                    }
                }
            }
        }
    }

    private void NewTime(){
        nextIntervalTime = Random.Range(intervalMinBeforeNextGame, intervalMaxBeforeNextGame) + timePlayed;
    }

    private void CheckHats(int gamesWon)
    {
        switch (gamesWon)
        {
            case 1:
            case 3:
            case 5:
            case 8:
            case 12:
            case 15:
            case 18:
                int hatsUnlocked = PlayerPrefs.GetInt("hatsUnlocked", 2);
                hatsUnlocked++;
                PlayerPrefs.SetInt("hatsUnlocked", hatsUnlocked);
                break;
            default:
                break;
        }
    }
    public void Reset()
    {
        {
            endGame("ignore", 0f);
            timePlayed = 0f;
            gameOver = false;
            
        }
    }
}
