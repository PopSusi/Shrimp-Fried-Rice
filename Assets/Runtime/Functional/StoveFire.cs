using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    // Update is called once per frame
    private void Awake()
    {
        //GetComponent<Collider>().enabled = true;
        HeatManager.endGame += DisableFire;
        //DontDestroyOnLoad(this.gameObject);
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        HeatManager.sectionHeat += UpdateDebugUI;
        WokController.UpdateScores += CoolBoost;
    }
    private void OnDestroy()
    {
        HeatManager.sectionHeat -= UpdateDebugUI;
        WokController.UpdateScores -= CoolBoost;
    }
    void FixedUpdate()
    {
        if (!SettingsMenu.paused) heatUpdate(heatRateUp, (int) mySpot);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!SettingsMenu.paused) heatUpdate(heatRateDown, (int) mySpot);
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
