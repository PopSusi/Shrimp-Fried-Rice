using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsBGSprite;
    public GameObject creditsBGSprite;
    public GameObject baseBGSprite;
    public GameObject Wrapper;

    public float yOffset;
    public float timeToLand;
    private bool isLifted = false;


    public void Start()
    {
        SettingsManager.CheckInitialization();
        if (SettingsManager.IsTutorial())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LiftMenu(string menu)
    {

        if (menu == "Settings")
        {
            settingsBGSprite.SetActive(true);
            baseBGSprite.SetActive(false);
            creditsBGSprite.SetActive(false);
        }
        else
        {
            creditsBGSprite.SetActive(true);
            baseBGSprite.SetActive(false);
            settingsBGSprite.SetActive(false);
        }
        if (!isLifted)
        {
            StartCoroutine("DriftInMenu");
            isLifted = true;
        }
    }

    private IEnumerator DriftInMenu()
    {
        Vector2 home = Wrapper.GetComponent<RectTransform>().transform.position;
        float t = 0.0f;
        Vector2 position = home;
        while (t <= 1.0)
        {
            t += Time.deltaTime / .75f; // length of slide in
            position.y = Mathf.Lerp(home.y, home.y + yOffset, Mathf.SmoothStep(0.0f, 1.0f, t)); //ease in ease out
            Wrapper.GetComponent<RectTransform>().transform.position = position;
            yield return null;
        }
        yield return null;
    }
}
