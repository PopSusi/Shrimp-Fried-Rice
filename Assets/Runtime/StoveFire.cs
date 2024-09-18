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

    // Update is called once per frame

    private void Start()
    {
        HeatManager.sectionHeat += UpdateDebugUI;
    }
    void FixedUpdate()
    {
        if (heatUpdate != null) heatUpdate(-4f, (int) mySpot);
    }

    private void OnTriggerStay(Collider other)
    {
        if (heatUpdate != null) heatUpdate(2f, (int) mySpot);
    }

    void UpdateDebugUI(float heat, int spot)
    {
        if(spot == (int) mySpot)
        {
            text.text = heat.ToString();
        }
    }
}
