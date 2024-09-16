using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using System;

public class WokController : MonoBehaviour
{
    public InputActionAsset actions;
    Vector2 tilt = Vector2.zero;

    [Header("Movement Parameters")]
    [SerializeField] private float angleForward = 30f;
    [SerializeField] private float angleBackward = -30f;
    [SerializeField] private float deltaForward = .5f;
    [SerializeField] private float deltaBackward = -2f;
    [SerializeField] private float deltaRight = .2f;
    [SerializeField] private float deltaLeft = -.2f;

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
    // Start is called before the first frame update
    protected void Awake()
    {
    }
    public void Update()
    {
        actions.Enable();
        tilt = actions["TiltKeys"].ReadValue<Vector2>();
        Vector3 tempTilt = Vector3.zero;
        tempTilt.x = math.remap(-1f, 1f, angleBackward, angleForward, tilt.y) + 5f;
        tempTilt.z = -1f * (math.remap(-1f, 1f, angleBackward, angleForward, tilt.x)) - 5f;
        //transform.rotation = Quaternion.Euler(tempTilt);

        Vector3 tempLoc = Vector3.zero;
        tempLoc.x = math.remap(-1f, 1f, deltaLeft, deltaRight, tilt.x);
        tempLoc.z = math.remap(-1f, 1f, deltaBackward, deltaForward, tilt.y);
        //transform.position = tempLoc;
        //Debug.Log(tempTilt.x + " " + tempTilt.z);
        //Debug.Log(tempLoc.x + " " + tempLoc.z);
        UpdateTransform(tempTilt, tempLoc);

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
            //Debug.Log(timeSinceFlipStart);
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
            UIManager.instance.UpdateFlip("Too Strong!!!");
        } 
        else if (time < goodHighFlip) //.1 - .13
        {
            UIManager.instance.UpdateFlip("Good - High!");
        }
        else if (time < perfectFlip)
        {
            UIManager.instance.UpdateFlip("Perfect!!!!!"); //.13 - .16
        }
        else if (time < goodLowFlip)
        {
            UIManager.instance.UpdateFlip("Good - Low"); //.16 - .2
        }
        else
        {
            UIManager.instance.UpdateFlip("Weak!!"); //.2 - .23
        }
    }

    public IEnumerator WaitandReset(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        flipStarted = false;
        flipCoroutine = null;
        timeSinceFlipStart = 0;
    }

    void UpdateTransform(Vector3 rot, Vector3 loc)
    {
        transform.rotation = Quaternion.Euler(rot);
        transform.position = loc;
    }
}
