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
        hatsFromSave.Add("MagicHat");
        hatsFromSave.Add("ChefHat");

        string[] copy = hatsFromSave.ToArray();

        HatSO[] HatsCollection = Resources.LoadAll<HatSO>("HatSO");
        int i = 0;

        foreach (string hatAccessor in copy)
        {
            foreach(HatSO hat in HatsCollection)
            {
                if(hat.indexString == hatAccessor)
                {
                    icons[i].sprite = hat.hatIcon;
                    Debug.Log(hat.indexString);
                    break;
                }
            }
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
