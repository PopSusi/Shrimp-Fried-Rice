using System.Collections;
using System.Collections.Generic;
using TMPro;
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
<<<<<<< Updated upstream
    private float angleForward = 30f;
    private float angleBackward = -30f;
    private float angleLeft = -30f;
    private float angleRight = 30f;
    private float deltaForward = .5f;
    private float deltaBackward = -2f;
=======
    [SerializeField] private float angleForward = 30f;
    [SerializeField] private float angleBackward = -30f;
    [SerializeField] private float deltaForward = .5f;
    [SerializeField] private float deltaBackward = -2f;

    [SerializeField] private float registerFlipStart = .4f;
    private float registerFlipEnd = -.7f;
    private bool flipStarted;
    private float timeSinceFlipStart;

    private float cancelOutTime = .23f;
    private float goodLowFlip = .20f;
    private float perfectFlip = .16f;
    private float goodHighFlip = .13f;
    private float strongFlip = .1f;
    private Coroutine flipCoroutine = null;
>>>>>>> Stashed changes
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
        tempTilt.x = (tilt.y + 1) / (1 + 1) * (angleForward - angleBackward) + angleBackward;
        tempTilt.z = (tilt.x + 1) / (1 + 1) * (angleForward - angleBackward) + angleBackward;
        transform.rotation = Quaternion.Euler(tempTilt);
       
        Vector3 tempLoc = Vector3.zero;
        tempLoc.z = (tilt.y + 1) / (1 +1 ) * (deltaForward - deltaBackward) + deltaBackward;
        transform.position = tempLoc;

<<<<<<< Updated upstream
        Debug.Log(tempTilt.x + " " + tempTilt.z);
=======
        //Debug.Log(tempTilt.x + " " + tempTilt.z);
        //Debug.Log(tempLoc.x + " " + tempLoc.z);

        if(tilt.y > registerFlipStart)
        {
            flipStarted = flipStarted ? false : true;
            if (flipCoroutine != null) StopCoroutine(flipCoroutine);
            flipCoroutine = StartCoroutine(WaitandReset(cancelOutTime));
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
            Debug.Log(timeSinceFlipStart);
        }

>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
=======

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
            UIManager.instance.UpdateFlip("Good - Low"); //.16 - .2
        }
        else
        {
            UIManager.instance.UpdateFlip("Weak"); //.2 - .23
        }
    }

    public IEnumerator WaitandReset(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        flipStarted = false;
        flipCoroutine = null;
        timeSinceFlipStart = 0;
    }

>>>>>>> Stashed changes
}
