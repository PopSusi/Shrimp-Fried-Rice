using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
   [System.Serializable] 
    class Sound 
    {
        [SerializeField] public string soundNames;
        [SerializeField] public AudioClip soundClip;
    }

    [SerializeField] List <Sound> soundList = new List <Sound>();

    Dictionary<string, Sound> soundStorage = new Dictionary<string, Sound>();

    private AudioSource audioSource;

    public static SoundManager Instance;


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
    }

    public void PlaySound(string soundKey)
    {
        if (!soundStorage.ContainsKey(soundKey)) 
        {
            throw new Exception("Sound not found!");
        }

        audioSource.PlayOneShot(soundStorage[soundKey].soundClip);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            PlaySound("Theme Music");
        }
    }
}
