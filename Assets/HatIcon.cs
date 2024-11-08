using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatIcon : MonoBehaviour
{
    public delegate void HatIconAlert(HatSO hat);
    public event HatIconAlert alert;

    public HatSO myHat;

    public void OnClickAlert()
    {
        
    }
}
