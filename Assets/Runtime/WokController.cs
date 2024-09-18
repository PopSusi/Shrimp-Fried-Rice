using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Unity.Mathematics;
using System;

public class WokController : MonoBehaviour
{
    public InputActionAsset actions;
    Vector2 tilt = Vector2.zero;

    [Tooltip("Wok Offset values determined by controls")]
    [Header("Movement Parameters")]
    [SerializeField] private float angleForward = 30f;
    [SerializeField] private float angleBackward = -30f;
    [SerializeField] private float deltaForward = .5f;
    [SerializeField] private float deltaBackward = -2f;
    [SerializeField] private float deltaRight = .2f;
    [SerializeField] private float deltaLeft = -.2f;

    [SerializeField]
    private float liftWokTravelTime = .7f;
    private float elapsedTime = 0f;
    private Vector3 origin;
    private Vector3 locOffset;
    private Coroutine liftCoroutine = null;

    [SerializeField] private float registerFlipStart = .4f;
    private float registerFlipEnd = -.7f;
    private bool flipStarted;
    private float timeSinceFlipStart;

    [Tooltip("Time values from start of flip to end, determining score" +
        "by being under the value.")]
    [Header("Time Brackets")]
    [SerializeField] private float cancelOutTime = .23f;
    [SerializeField] private float goodLowFlipTime = .20f;
    [SerializeField] private float perfectFlipTime = .16f;
    [SerializeField] private float goodHighFlipTime = .13f;
    [SerializeField] private float strongFlipTime = .1f;
    private Coroutine flipCoroutine = null;

    [Tooltip("Score values based on prior stated timings")]
    [Header("Flip Scores")]
    [SerializeField] private int weakFlipScore = 1000;
    [SerializeField] private int goodLowFlipScore = 1500;
    [SerializeField] private int perfectFlipScore = 3000;
    [SerializeField] private int goodHighFlipScore = 2000;
    [SerializeField] private int strongFlipScore = 1000;

    [Tooltip("Multiplier adjustments based on prior stated timings")]
    [Header("Flip Scores")]
    [SerializeField] private float weakFlipScoreMult = .02f;
    [SerializeField] private float goodLowFlipScoreMult = .05f;
    [SerializeField] private float perfectFlipScoreMult = .08f;
    [SerializeField] private float goodHighFlipScoreMult = .05f;
    [SerializeField] private float strongFlipScoreMult = .03f;

    public delegate void ScoreUpdate(int score, float multiplier);
    public static event ScoreUpdate UpdateScores;

    public delegate void FlipPerformance(string text);
    public static event FlipPerformance UIFlipUpdate;


    // Start is called before the first frame update
    protected void Awake()
    {
        HeatManager.SubToRecieveHeat();
        EnhancedTouchSupport.Enable();

        actions.Enable();
        actions["LiftTouch"].started += ILiftWok;
        actions["LiftTouch"].canceled += IDownWok;

        actions["LiftKey"].started += ILiftWok;
        actions["LiftKey"].canceled += IDownWok;
        origin = Vector3.zero;
    }
    public void Update()
    {
        tilt = actions["TiltKeys"].ReadValue<Vector2>();

        Vector3 tempTilt = Vector3.zero;
        tempTilt.x = math.remap(-1f, 1f, angleBackward, angleForward, tilt.y) + 5f;
        tempTilt.z = -1f * (math.remap(-1f, 1f, angleBackward, angleForward, tilt.x)) - 5f;
        //transform.rotation = Quaternion.Euler(tempTilt);

        Vector3 tempLoc = Vector3.zero;
        tempLoc.x = math.remap(-1f, 1f, deltaLeft, deltaRight, tilt.x);
        tempLoc.x += Mathf.Abs((deltaRight + deltaLeft) / 2);
        
        tempLoc.z = math.remap(-1f, 1f, deltaBackward, deltaForward, tilt.y);
        tempLoc.z += Mathf.Abs((deltaForward + deltaBackward) / 2);
        tempLoc += locOffset;
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
    public void ILiftWok(InputAction.CallbackContext context)
    {
        if (liftCoroutine != null)
        {
            StopCoroutine(liftCoroutine);
            liftCoroutine = StartCoroutine("ELiftWok");
        }
        liftCoroutine = StartCoroutine("ELiftWok");
    }
    public void IDownWok(InputAction.CallbackContext context)
    {
        if(liftCoroutine != null)
        {
            StopCoroutine(liftCoroutine);
            liftCoroutine = StartCoroutine("EDownWok");
        }
        liftCoroutine = StartCoroutine("EDownWok");
    }

    public void EvaluateFlip(float time)
    {
        if(time < strongFlipTime) //.0 - .1
        {
            UpdateScores(strongFlipScore, strongFlipScoreMult);
            UIFlipUpdate("Too Strong!!!");
        } 
        else if (time < goodHighFlipTime) //.1 - .13
        {
            UpdateScores(goodHighFlipScore, goodHighFlipScoreMult);
            UIFlipUpdate("Good - High!");
        }
        else if (time < perfectFlipTime)
        {
            UpdateScores(perfectFlipScore, perfectFlipScoreMult);
            UIFlipUpdate("Perfect!!!!!"); //.13 - .16
        }
        else if (time < goodLowFlipTime)
        {
            UpdateScores(goodLowFlipScore, goodLowFlipScoreMult);
            UIFlipUpdate("Good - Low"); //.16 - .2
        }
        else if (time < cancelOutTime)
        {
            UpdateScores(weakFlipScore, weakFlipScoreMult);
            UIFlipUpdate("Weak"); //.2 - .23
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

    IEnumerator ELiftWok()
    {
        //Debug.Log(elapsedTime + " " + origin + " " + origin + new Vector3(0, 0.4f, 0) + " " + liftWokTravelTime);
        while (elapsedTime < liftWokTravelTime)
        {
            locOffset = Vector3.Lerp(Vector3.zero, new Vector3(0, .4f, 0), (elapsedTime / liftWokTravelTime));
            elapsedTime += Time.fixedDeltaTime;

            // Yield here
            yield return null;
        }
        liftCoroutine = null;
        yield return null;
    }

    IEnumerator EDownWok()
    {
        Vector3 currentPos = transform.position;
        float tempElapsedTime = elapsedTime;
        while (tempElapsedTime > 0)
        {
            locOffset = Vector3.Lerp(origin, currentPos, (tempElapsedTime / elapsedTime));
            tempElapsedTime -= Time.fixedDeltaTime;

            // Yield here
            yield return null;
        }
        locOffset = Vector3.zero;
        liftCoroutine = null;
        elapsedTime = 0;
        yield return null;
    }
}
