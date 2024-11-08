using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{

    //public delegate void Pause(bool paused);
    //public event Pause SendPause;

    [SerializeField] private GameObject SettingsMenuWrapper;
    [SerializeField] private GameObject PauseMenuWrapper;
    [SerializeField] private GameObject QuitMenuWrapper;
    [SerializeField] private GameObject ResetMenuWrapper;
    private string confirmChoice;
    public static bool paused;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MenuInput(string menuName)
    {
        switch (menuName)
        {
            case "Pause":
                if (SettingsMenuWrapper.activeInHierarchy) SettingsMenuWrapper.SetActive(false);
                if (paused)
                {
                    PauseMenuWrapper.SetActive(false);
                    Time.timeScale = 1f;
                    Debug.Log("unpausing");
                    paused = false;
                }
                else
                {
                    PauseMenuWrapper.SetActive(true);
                    Time.timeScale = 0f;
                    paused = true;
                }
                if (ResetMenuWrapper.activeInHierarchy) ResetMenuWrapper.SetActive(false);
                if (QuitMenuWrapper.activeInHierarchy) QuitMenuWrapper.SetActive(false);
                break;
            case "Reset":
                if (SettingsMenuWrapper.activeInHierarchy) SettingsMenuWrapper.SetActive(false);
                if (PauseMenuWrapper.activeInHierarchy) PauseMenuWrapper.SetActive(false);
                if (QuitMenuWrapper.activeInHierarchy) QuitMenuWrapper.SetActive(false);
                if (!ResetMenuWrapper.activeInHierarchy)
                {
                    ResetMenuWrapper.SetActive(true);
                    confirmChoice = "ResetConfirm";
                }
                break;
            case "Settings":
                if (!SettingsMenuWrapper.activeInHierarchy) SettingsMenuWrapper.SetActive(true);
                if (PauseMenuWrapper.activeInHierarchy) PauseMenuWrapper.SetActive(false);
                if (ResetMenuWrapper.activeInHierarchy) ResetMenuWrapper.SetActive(false);
                if (QuitMenuWrapper.activeInHierarchy) QuitMenuWrapper.SetActive(false);
                break;
            case "Quit":
                if (SettingsMenuWrapper.activeInHierarchy) SettingsMenuWrapper.SetActive(false);
                if (PauseMenuWrapper.activeInHierarchy) PauseMenuWrapper.SetActive(false);
                if (ResetMenuWrapper.activeInHierarchy) ResetMenuWrapper.SetActive(false);
                if (!QuitMenuWrapper.activeInHierarchy)
                {
                    QuitMenuWrapper.SetActive(true);
                    Debug.Log("opening quit");
                    confirmChoice = "QuitConfirm";
                }
                break;
        }
        Debug.Log(paused);
    }

    public void ConfirmChoice()
    {
        switch (confirmChoice)
        {
            case "ResetConfirm":
                HeatManager.gameOver = false;
                SceneManager.LoadScene("Primary");
                break;
            case "QuitConfirm":
                SceneManager.LoadScene("UITest");
                break;
        }
    }
}
