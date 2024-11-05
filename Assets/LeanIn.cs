using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanIn : MonoBehaviour
{
    private Vector3 Home;
    private float cutoff = .5f;
    // Start is called before the first frame update
    void Awake()
    {
        Home = transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        if (!HeatManager.gameOver)
        {
            float tempPosMod = Mathf.Abs((HeatManager.heatAvg - 5000) / 1000) / 3.5f;
            //Debug.Log(tempPosMod);
            if (tempPosMod > cutoff)
            {
                transform.position = (tempPosMod - .6f) * transform.forward + Home;
            }
        }
    }
}
