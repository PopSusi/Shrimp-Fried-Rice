using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timePlayed;
    [SerializeField]  private TextMeshProUGUI gamesPlayed;
    // Update is called once per frame
    public void UpdateText()
    {
        float time = HeatManager.instance.timePlayed;
        timePlayed.text = time.ToString("00.00") + "s";
        gamesPlayed.text = PlayerPrefs.GetInt("gamesWon", 0).ToString();
    }
}
