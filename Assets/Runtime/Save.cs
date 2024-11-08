using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SaveObject
{
    public string[] savedHats;
    public HatSO currentHat;
    public int gamesCompleted;

}

public class Instance
{
    public static Instance instance;
    public string[] savedHats {
        get 
        {
            Load();
            return savedHats; 
        }
        set
        {
            savedHats = value;
            Save();
        }
    }
    public HatSO currentHat
    {
        get 
        { 
            Load();
            return currentHat; 
        }
        set
        {
            currentHat = value;
            Debug.Log("doin it");
            Save();
        }
    }
    public int gamesCompleted
    {
        get 
        {
            Load();
            return gamesCompleted; 
        }
        set
        {
            gamesCompleted = value;
            Save();
        }
    }

    public Instance()
    {
        Load();
    }
    public void Save()
    {
        SaveObject save = new SaveObject();
        save.currentHat = currentHat;
        save.savedHats = savedHats;
        save.gamesCompleted = gamesCompleted;
        string json = JsonUtility.ToJson(save);
        File.WriteAllText(Application.dataPath + "/save.json", json);
        Debug.Log(currentHat.indexString);
    }

    public void Load()
    {
        if (File.Exists(Application.dataPath + "/save.json"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/save.json");
            SaveObject loadedSave = JsonUtility.FromJson<SaveObject>(saveString);
            savedHats = loadedSave.savedHats;
            currentHat = loadedSave.currentHat;
            gamesCompleted = loadedSave.gamesCompleted;
        }
        else
        {
            NewSave();
        }
    }

    private void NewSave()
    {
        SaveObject save = new SaveObject();
        string json = JsonUtility.ToJson(save);
        File.WriteAllText(Application.dataPath + "/save.json", json);
    }

    public HatSO LoadHat()
    {
        Load(); 
        return currentHat;
    }
}

