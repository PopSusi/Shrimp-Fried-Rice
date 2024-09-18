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

    [SerializeField] private float heatRateUpInspect;
    [SerializeField] private float heatRateDownInspect;

    private static float heatRateDown;
    private static float heatRateUp;

    // Update is called once per frame

    private void Start()
    {
        HeatManager.sectionHeat += UpdateDebugUI;
        if (heatRateDown != heatRateDownInspect) heatRateDown = heatRateDownInspect;
        if (heatRateUp != heatRateUpInspect) heatRateUp = heatRateUpInspect;
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
            text.text = heat.ToString();
        }
    }

    private void OnValidate()
    {
        if (heatRateDown != heatRateDownInspect) heatRateDown = heatRateDownInspect;
        if (heatRateUp != heatRateUpInspect) heatRateUp = heatRateUpInspect;
    }
}
