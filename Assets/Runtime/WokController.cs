using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class WokController : MonoBehaviour
{
    public PlayerInput input;
    Vector2 tilt = Vector2.zero;
    // Start is called before the first frame update
    protected void Awake()
    {
    }
    public void Update()
    {
        input.actions.Enable();
        tilt = input.actions["TiltKeys"].ReadValue<Vector2>();
        Vector3 tempTilt = Vector3.zero;
        tempTilt.x = (tilt.x - (-1)) / (1 - (-1)) * (30 - (-30)) + (-30);
        tempTilt.z = (tilt.y - (-1)) / (1 - (-1)) * (30 - (-30)) + (-30);
        Debug.Log(tilt.ToString());
    }

    public void OnTiltKeys(InputAction.CallbackContext context)
    {
        tilt = context.ReadValue<Vector2>();
    }
    public void OnLiftKey(InputAction.CallbackContext context)
    {
        Debug.Log("Space touched");
        if (context.started)
        {
            Debug.Log("I go up");
        } else if (context.canceled)
        {
            Debug.Log("I go down");
        }
    }

}
