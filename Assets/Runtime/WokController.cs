using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class WokController : MonoBehaviour
{
    public InputActionAsset actions;
    Vector2 tilt = Vector2.zero;
    // Start is called before the first frame update
    protected void Awake()
    {
    }
    public void Update()
    {
        actions.Enable();
        tilt = actions["TiltKeys"].ReadValue<Vector2>();
        tilt.x *= -1;
        Vector3 tempTilt = Vector3.zero;
        tempTilt.x = (tilt.y - (-1)) / (1 - (-1)) * (30 - (-30)) + (-30);
        tempTilt.z = (tilt.x - (-1)) / (1 - (-1)) * (30 - (-30)) + (-30);
        transform.rotation = Quaternion.Euler(tempTilt);
        Debug.Log(tempTilt.x + " " + tempTilt.z);
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
