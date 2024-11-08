using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrimpbley : MonoBehaviour
{
    public GameObject model;
    public GameObject modelLocation;
    // Start is called before the first frame update
    void Start()
    {
        string chosenHat = PlayerPrefs.GetString("chosenHat", null);
        HatSO[] HatsCollection = Resources.LoadAll<HatSO>("HatSO");
        foreach(HatSO h in HatsCollection)
        {
            if(h.indexString == chosenHat)
            {
                model = Instantiate(h.hatModel, gameObject.transform);
                model.transform.position = modelLocation.transform.position;
                model.transform.localScale = new Vector3(100f, 100f, 100f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
