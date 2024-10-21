using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
   [System.Serializable] 
    struct Sound 
    {
        [SerializeField] public string soundNames;
        [SerializeField] public AudioClip soundClip;
    }

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

        PlaySound("Music", 0);
        PlaySound("Metronome", 1);

        // --- GET EVENTS --- //
        WokController.UIFlipUpdate += FlipNoise;
        MiniGame.ChopSound += ChopNoise;

    }

    public void Update()
    {
        AudioSources[1].volume = 0f;
        if (HeatManager.heatAvg > 6000)
        {
            AudioSources[1].volume = .5f;
            AudioSources[1].pitch = HeatManager.heatAvg / 6000;
        } else if (HeatManager.heatAvg < 4000)
        {
            AudioSources[1].volume = .5f;
            AudioSources[1].pitch = HeatManager.heatAvg / 4000;
        }
        else
        {
            AudioSources[1].volume = 0f;
            AudioSources[1].pitch = 1f;
        }
    }

    public void PlaySound(string soundKey, int access)
    {
        if (!soundStorage.ContainsKey(soundKey)) 
        {
            throw new Exception("Sound not found!");
        }

        AudioSources[access].PlayOneShot(soundStorage[soundKey].soundClip);
    }

    void FlipNoise(string dontusethis) => PlaySound("Flip Noise", 2);
    void ChopNoise() => PlaySound("Chops", 3);
}
