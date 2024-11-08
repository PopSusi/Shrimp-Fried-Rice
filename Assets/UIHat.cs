using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UIHat : MonoBehaviour
{
    public Image[] icons;
    private List<string> hatsFromSave = new List<string>();
    private Dictionary<int, HatSO> hatByIcon = new Dictionary<int, HatSO>();
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
                    hatByIcon.Add(i, hat);
                    Debug.Log(hat.indexString);
                    break;
                }
            }
            i++;
        }
    }

    public void SetHat(Image self)
    {
        int index = System.Array.IndexOf(icons, self);
        if(hatByIcon.TryGetValue(index, out HatSO hat)){
            Instance instance = new Instance();
            instance.Load();
            Debug.Log(instance.currentHat);
            instance.currentHat = hat;
            instance.Save();
            Debug.Log(instance.currentHat);
        }
    }
}
