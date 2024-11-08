using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotifyIcons : MonoBehaviour
{
    public Sprite[] sprites;
    private Image image;
    // Start is called before the first frame update

    //0 cooling
    //1 warming
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void SetIconHot()
    {
        if (!gameObject.activeInHierarchy) gameObject.SetActive(true);
        image.sprite = sprites[1];
    }
    public void SetIconCold()
    {
        if (!gameObject.activeInHierarchy) gameObject.SetActive(true);
        image.sprite = sprites[0];
    }
    public void SetOff()
    {
        gameObject.SetActive(false);
    }
}
