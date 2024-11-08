using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatIcon : MonoBehaviour
{
    [HideInInspector]
    public string hat = "balls";
    public void SaveHat()
    {
        PlayerPrefs.SetString("chosenHat", hat);
        string outHat = PlayerPrefs.GetString("chosenHat", "Couldn't get");
        Debug.Log(outHat);
    }
}
