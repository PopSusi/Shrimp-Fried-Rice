using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHat : MonoBehaviour
{
    public Image[] icons;
    private List<string> hatsFromSave = new List<string>();

    public int DebugHatsUnlocked;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        LoadHats(SettingsManager.GetHats());
    }

    private void LoadHats(int hatsTotal)
    {
        HatSO[] HatsCollection = Resources.LoadAll<HatSO>("HatSO");
        for (int i = 0; i < hatsTotal; i++)
        {
            icons[i].sprite = HatsCollection[i].hatIcon;
            string sendHat = HatsCollection[i].indexString;
            icons[i].gameObject.GetComponent<HatIcon>().hat = sendHat;
            icons[i].gameObject.GetComponent<Button>().interactable = true;
            Debug.Log(sendHat + " from ui");
        }
        for (int i = hatsTotal; i < 9 - hatsTotal; i++)
        {
            icons[i].gameObject.GetComponent<Button>().interactable = false;
        }
    }

    private void OnValidate()
    {
        LoadHats(DebugHatsUnlocked);
    }
}
