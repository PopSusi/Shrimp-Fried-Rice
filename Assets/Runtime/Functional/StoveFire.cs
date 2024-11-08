using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Spots
{
    Top = 0,
    Bottom = 1,
    Left = 2,
    Right = 3,
    Middle = 4
}

[RequireComponent(typeof(Rigidbody))]
public class StoveFire : MonoBehaviour
{

    public delegate void HeatUpdate(float delta, int spot);
    public static event HeatUpdate heatUpdate;

    [SerializeField] private Spots mySpot;
    [SerializeField] private TextMeshProUGUI text;

    public float heatRateDown = 2f;
    public float heatRateUp = -4.5f;

    [SerializeField] private NotifyIcons iconScript;
    private bool hot = false;
    private bool cold = false;
    private bool touched = true;

    // Update is called once per frame
    private void Awake()
    {
        //GetComponent<Collider>().enabled = true;
        HeatManager.endGame += DisableFire;
        //DontDestroyOnLoad(this.gameObject);
        Application.targetFrameRate = 60;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        HeatManager.sectionHeat += UpdateDebugUI;
        WokController.UpdateScores += CoolBoost;
    }
    private void OnSceneUnloaded(Scene scene)
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        HeatManager.sectionHeat -= UpdateDebugUI;
        WokController.UpdateScores -= CoolBoost;
    }
    void FixedUpdate()
    {
        if (touched)
        {
            if (heatUpdate != null && !SettingsMenu.paused) heatUpdate(heatRateDown, (int)mySpot);
            Debug.Log("Sending Heat up ");
        } else
        {
            if (heatUpdate != null && !SettingsMenu.paused) heatUpdate(heatRateUp, (int)mySpot);
            Debug.Log("Sending Heat down ");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wok") touched = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Wok" ) touched = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Wok") touched = false;
    }

    void UpdateDebugUI(float heat, int spot)
    {
        if(spot == (int) mySpot)
        {
            if (heat < 2000)
            {
                text.text = "Getting Chilly";
                if (!cold) iconScript.SetIconCold();
            } else if (heat < 4000)
            {
                iconScript.SetOff();
                text.text = "Warmer";
            } else if (heat < 6000)
            {
                text.text = "The Golden Ratio";
            } else if (heat < 8000)
            {
                iconScript.SetOff();
                text.text = "It's getting hot in here!";
            } else if(heat < 10000)
            {
                if (!hot) iconScript.SetIconHot();
                
                text.text = "So take off all your clothes";
            }
        }
    }
    private void CoolBoost(int score, float multiplier)
    {
        heatUpdate(-(score * .4f), (int)mySpot);
        Debug.Log("cool boost");
    }
    private void DisableFire(string reasoning, float time)
    {
        GetComponent<Collider>().enabled = false;
    }

    ~StoveFire()
    {
        HeatManager.endGame -= DisableFire;
    }


}
