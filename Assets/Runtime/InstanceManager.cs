using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceManager : MonoBehaviour
{
    public void Awake()
    {
        if (Instance.instance != null)
        {
            Instance.instance = null;
        }
    //Instance.instance = new Instance();
  }
}
