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
    private float deltaForward = .5f;
    private float deltaBackward = -2f;

    private float registerFlipStart = .4f;
    private float registerFlipEnd = -.7f;
    private bool flipStarted;
    private float timeSinceFlipStart;

    private float goodLowFlip = .20f;
    private float perfectFlip = .16f;
    private float goodHighFlip = .13f;
    private float strongFlip = .1f;
    // Start is called before the first frame update
    protected void Awake()
    {
    }
    public void Update()
    {
        //tilt.y is the forward of the joystick, tilt.x is the side of the joystick


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

        //Debug.Log(tempTilt.x + " " + tempTilt.z);
        //Debug.Log(tempLoc.x + " " + tempLoc.z);

        if(tilt.y > registerFlipStart)
        {
            flipStarted = flipStarted ? false : true;
        }
        else if (tilt.y < registerFlipEnd && flipStarted)
        {
            flipStarted = false;
            EvaluateFlip(timeSinceFlipStart);
            timeSinceFlipStart = 0f;
        }

        if(flipStarted)
        {
            timeSinceFlipStart += Time.deltaTime;
        }

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


    public void EvaluateFlip(float time)
    {
        if(time < strongFlip) //.0 - .1
        {
            UIManager.instance.UpdateFlip("Too Strong");
        } 
        else if (time < goodHighFlip) //.1 - .13
        {
            UIManager.instance.UpdateFlip("Good - High");
        }
        else if (time < perfectFlip)
        {
            UIManager.instance.UpdateFlip("Perfect"); //.13 - .16
        }
        else if (time < goodLowFlip)
        {
            UIManager.instance.UpdateFlip("Good - Low");
        }
        else if (time > goodLowFlip)
        {
            UIManager.instance.UpdateFlip("Weak");
        } else
        {
            UIManager.instance.UpdateFlip("fell out");
        }
    }

}
