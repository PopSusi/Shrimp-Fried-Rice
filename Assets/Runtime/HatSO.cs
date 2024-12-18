using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOGame", menuName = "Mini Games/NewHat")]
public class HatSO : ScriptableObject
{
    public GameObject hatModel;
    public Sprite hatIcon;
    public string indexString;
}
