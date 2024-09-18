using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeatManager : MonoBehaviour
{
    public delegate void CalculatedHeat(float heat, int spot);
    public static CalculatedHeat sendHeat;
    public static CalculatedHeat sectionHeat;

    public static float[] heatSpots = { 1000f, 1000f, 1000f, 1000f, 1000f };
    public static float heatTotal;
    public static float heatAvg = 1000;


    // Start is called before the first frame update
    public static void SubToRecieveHeat()
    {
        StoveFire.heatUpdate += UpdateHeat;
    }

    private static void UpdateHeat(float delta, int spot)
    {

        heatSpots[spot] += delta;
        sectionHeat(heatSpots[spot], spot);

        heatTotal += delta;
        heatAvg =  heatTotal / 5;
        sendHeat(heatAvg, spot);

        Debug.Log(heatSpots[spot] + " " + heatAvg);
    }
}
