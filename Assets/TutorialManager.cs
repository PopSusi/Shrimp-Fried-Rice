using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI TutorialText;
    [SerializeField] GameObject TutorialWrapper;
    public string movementText;
    public string liftText;
    public string flipText;
    public string congratsText;

    // Start is called before the first frame update
    void Start()
    {
        //SettingsManager.IsTutorial(true);
        Debug.Log(SettingsManager.IsTutorial() + " Is Tutorial");
        WokController.WokMovement += WokDone;
        LoadTutorial();
        TutorialText.text = movementText;
    }

    public void WokDone()
    {
        WokController.WokMovement -= WokDone;
        TutorialText.text = liftText;
        WokController.LiftDone += LiftDone;
        ResetToGameplay();
    }
    public void LiftDone()
    {
        WokController.LiftDone -= LiftDone;
        TutorialText.text = flipText;
        WokController.FlipPerformed += TutorialDone;
        ResetToGameplay();
    }
    public void TutorialDone()
    {
        WokController.FlipPerformed -= TutorialDone;
        TutorialText.text = congratsText;
        SettingsManager.IsTutorial(false);
    }

    IEnumerator TimerTillStartTutorial()
    {
        //Debug.Log("TUTORIAL POPUP LOAD");
        yield return new WaitForSeconds(10f);
        LoadTutorial();
    }
    IEnumerator End()
    {
        yield return new WaitForSeconds(1f);
        TutorialWrapper.SetActive(false);
    }
    private void ResetToGameplay()
    {
        Time.timeScale = 1f;
        TutorialWrapper.SetActive(false);
        StartCoroutine("TimerTillStartTutorial");
    }

    private void LoadTutorial()
    {
        //Debug.Log("TUTORIAL POPUP LOAD");
        Time.timeScale = 0f;
        TutorialWrapper.SetActive(true);
    }
}
