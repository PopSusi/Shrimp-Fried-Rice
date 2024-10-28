using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UIHat : MonoBehaviour
{
    public Image[] icons;
    private List<string> hatsFromSave = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        hatsFromSave.Add("TopHat");
        hatsFromSave.Add("ChefHat");


        HatSO[] HatsCollection = Resources.LoadAll<HatSO>("HatSO");
        for (int i = 0; i < 10; i++)
        {
            foreach(string savedHat in hatsFromSave)
            {
                Debug.Log("Saved: " + savedHat + " | Collection: " + HatsCollection[i].indexString);
                if (savedHat == HatsCollection[i].indexString)
                {
                    icons[i].sprite = HatsCollection[i].hatIcon;
                    Debug.Log("Removing from enumeration");
                    hatsFromSave.Remove(savedHat);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
