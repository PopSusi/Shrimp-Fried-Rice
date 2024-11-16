using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
   [System.Serializable] 
    struct Sound 
    {
        [SerializeField] public string soundNames;
        [SerializeField] public AudioClip soundClip;
    }

    public AudioClip pluhClip;

    [SerializeField] List <Sound> soundList = new List <Sound>();

    Dictionary<string, Sound> soundStorage = new Dictionary<string, Sound>();

    private AudioSource audioSource;

    public static SoundManager Instance;

    public AudioSource[] AudioSources;
    //0 is music - metronome underlay
    //1 is drums
    //2 is flips
    //3 is SFX 1
    //4 is SFX 2

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        
        foreach (Sound sound in soundList)
        {
            soundStorage.Add(sound.soundNames, sound);
        }

        Debug.Log(soundStorage);

        if (Instance != null)
        {
            Destroy(gameObject);   
        }
        Instance = this;

        if(SettingsManager.IsMusicOn()) PlaySound("Music", 0, true);
        if (SettingsManager.IsMusicOn()) PlaySound("Metronome", 1, true);

        // --- GET EVENTS --- //
        WokController.UIFlipUpdate += FlipNoise;
        MiniGame.ChopSound += ChopNoise;
        HeatManager.endGame += GameEnd;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public void Update()
    {
        //Metronome Modulation
        if (!HeatManager.gameOver)
        {
            float heatAverage = HeatManager.instance.heatAvg;
            AudioSources[1].volume = 0f;
            if (heatAverage > 6000)
            {
                AudioSources[1].volume = .3f;
                AudioSources[1].pitch = heatAverage / 6000;
            }
            else if (heatAverage < 4000)
            {
                AudioSources[1].volume = .3f;
                AudioSources[1].pitch = heatAverage / 4000;
            }
            else
            {
                AudioSources[1].volume = 0f;
                AudioSources[1].pitch = 1f;
            }
        } else
        {
            AudioSources[1].volume = 0f;
        }
    }

    public void PlaySound(string soundKey, int access)
    {
        if (!soundStorage.ContainsKey(soundKey))
        {
            throw new Exception("Sound not found!");
        }
        if (!SettingsManager.IsPluh())
        {
            AudioSources[access].PlayOneShot(soundStorage[soundKey].soundClip);
        }
        else
        {
            AudioSources[access].PlayOneShot(pluhClip);
            Debug.Log("pluh");
        }
    }
    public void PlaySound(string soundKey, int access, bool overridePlug)
    {
        if (!soundStorage.ContainsKey(soundKey))
        {
            throw new Exception("Sound not found!");
        }
        if (!SettingsManager.IsPluh() || overridePlug)
        {
            AudioSources[access].PlayOneShot(soundStorage[soundKey].soundClip);
        }
        else
        {
            AudioSources[access].PlayOneShot(pluhClip);
            Debug.Log("pluh");
        }
    }

    void FlipNoise(string performance)
    {
        switch (performance) {
            case ("Weak!!!"):
            case ("Too Strong!!!"):
                performance = "Bad Flip";
                break;
            case ("Perfect!!!!!"):
                performance = "Perfect Flip";
                break;
            case ("Good - High!"):
            case ("Good - Low!"):
            default:
                performance = "Good Flip";
                break;
        }
        PlaySound(performance, 2);
    }
    void ChopNoise() => PlaySound("Chops", 3);
    void GameEnd(string reasoning, float time)
    {
        if(reasoning == "Too hot!" || reasoning == "Too cold!")
        {
            PlaySound("Bad Flip", 2);
        } else if (reasoning == "Won!")
        {
            PlaySound("Perfect Flip", 2);
        }
    }
    private void OnSceneUnloaded(Scene scene)
    {
        WokController.UIFlipUpdate -= FlipNoise;
        MiniGame.ChopSound -= ChopNoise;
        HeatManager.endGame -= GameEnd;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
