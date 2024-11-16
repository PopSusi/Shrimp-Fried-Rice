using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsInterface : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle invToggle;
    [SerializeField] private Toggle pluhToggle;

    private void Start()
    {
        if (musicToggle != null) musicToggle.isOn = SettingsManager.IsMusicOn();
        if (invToggle != null) invToggle.isOn = SettingsManager.IsInvincible();
        if (pluhToggle != null) pluhToggle.isOn = SettingsManager.IsPluh();
    }

    public void Music(bool val) => SettingsManager.IsMusicOn(val);
    public void Inv(bool val) => SettingsManager.IsInvincible(val);
    public void Pluh(bool val) => SettingsManager.IsPluh(val);

}
