using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level instance { get; private set; }

    public MiniGameSO[] minigames;

    public MiniGameSO RandomGame()
    {
        return minigames[Random.Range(0, (minigames.Length - 1))];
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }

    private void Start()
    {
        SoundManager.Instance.PlaySound("Theme Music");
    }

}
