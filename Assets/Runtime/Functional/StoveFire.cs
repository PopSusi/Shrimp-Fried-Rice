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

    // Update is called once per frame
    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
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
        if (heatUpdate != null) heatUpdate(heatRateUp, (int) mySpot);
    }

    private void OnTriggerStay(Collider other)
    {
        if (heatUpdate != null) heatUpdate(heatRateDown, (int) mySpot);
    }

    void UpdateDebugUI(float heat, int spot)
    {
        if(spot == (int) mySpot)
        {
            if (heat < 2000)
            {
                text.text = "Getting Chilly";
            } else if (heat < 4000)
            {
                text.text = "Warmer";
            } else if (heat < 6000)
            {
                text.text = "The Golden Ratio";
            } else if (heat < 8000)
            {
                text.text = "It's getting hot in here!";
            } else if(heat < 10000)
            {
                text.text = "So take off all your clothes";
            }
        }
    }
    private void CoolBoost(int score, float multiplier)
    {
        if (heatUpdate != null) heatUpdate(-(score * .4f), (int)mySpot);
    }
}
