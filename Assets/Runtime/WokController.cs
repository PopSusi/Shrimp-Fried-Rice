using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using static UnityEngine.Rendering.DebugUI;

public class WokController : MonoBehaviour
{
    public InputActionAsset actions;
    Vector2 tilt = Vector2.zero;

    [Header("Movement Parameters")]
    private float angleForward = 30f;
    private float angleBackward = -30f;
    private float angleLeft = -30f;
    private float angleRight = 30f;
    private float deltaForward = .5f;
    private float deltaBackward = -2f;
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
        tempTilt.x = (tilt.y + 1) / (1 + 1) * (angleForward - angleBackward) + (angleBackward);
        tempTilt.z = (tilt.x + 1) / (1 + 1) * (angleForward - angleBackward) + (angleBackward);
        transform.rotation = Quaternion.Euler(tempTilt);
       
        Vector3 tempLoc = Vector3.zero;
        tempLoc.z = (tilt.y + 1) / (1 +1 ) * (deltaForward - deltaBackward) + (deltaBackward);
        transform.position = tempLoc;

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
        }
        else if (context.canceled)
        {
            Debug.Log("I go down");
        }
    }

}
