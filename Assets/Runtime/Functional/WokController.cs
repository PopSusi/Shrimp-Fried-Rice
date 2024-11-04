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
    private float tempElapsedTime = 0f;
    private Vector3 origin;
    private Vector3 locOffset;
    private Vector3 apex;
    private bool WokUp;
    private bool WokDown;

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

    private bool blockFlip;

    public delegate void ScoreUpdate(int score, float multiplier);
    public static event ScoreUpdate UpdateScores;

    public delegate void FlipPerformance(string text);
    public static event FlipPerformance UIFlipUpdate;


    // Start is called before the first frame update
    protected void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        //HeatManager.instance.SubToRecieveHeat();
        HeatManager.miniGameStart += DisableWok;
        MiniGame.OnOver += EnableWok;
        EnhancedTouchSupport.Enable();

        actions.Enable();
        EnableWok();
        origin = Vector3.zero;
    }
    protected void OnDestroy()
    {
        HeatManager.miniGameStart -= DisableWok;
        MiniGame.OnOver -= EnableWok;
    }
    public void Update()
    {
        tilt = actions["TiltKeys"].ReadValue<Vector2>();

        //Conditioning Input to Relative Space Translation

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

        //Apply Relative Translations
        UpdateTransform(tempTilt, tempLoc);

        //If Wok has started
        if(tilt.y > registerFlipStart && !blockFlip)
        {
            flipStarted = flipStarted ? false : true;
            if (flipCoroutine != null) StopCoroutine(flipCoroutine);
            flipCoroutine = StartCoroutine(WaitandReset(cancelOutTime));
        }
        else if (tilt.y < registerFlipEnd && flipStarted && !blockFlip)
        {
            CancelFlip();
            EvaluateFlip(timeSinceFlipStart);
        }

        if(flipStarted)
        {
            timeSinceFlipStart += Time.deltaTime;
            //Debug.Log(timeSinceFlipStart);
        }
    }

    private void FixedUpdate()
    {
        if (WokUp)
        {
            if (elapsedTime < liftWokTravelTime)
            {
                locOffset = Vector3.Lerp(Vector3.zero, new Vector3(0, .4f, 0), (elapsedTime / liftWokTravelTime));
                elapsedTime += Time.fixedDeltaTime;
            }
        }
        else if (WokDown)
        {
            if (tempElapsedTime > 0)
            {
                locOffset = Vector3.Lerp(origin, apex, (tempElapsedTime / elapsedTime));
                tempElapsedTime -= Time.fixedDeltaTime;
            } else
            {
                WokLiftDone();
            }
        }
    }

    private void CancelFlip()
    {
        flipStarted = false;
        timeSinceFlipStart = 0f;
    }

    public void OnTiltKeys(InputAction.CallbackContext context)
    {
        tilt = context.ReadValue<Vector2>();
    }
    public void ILiftWok(InputAction.CallbackContext context)
    {
        WokUpSet();
    }
    public void IDownWok(InputAction.CallbackContext context)
    {
        WokDownSet();
    }
    public void WokLiftDone()
    {
        locOffset = Vector3.zero;
        blockFlip = false;
        WokDown = false;
        tempElapsedTime = 0f;
        elapsedTime = 0f;
    }

    private void WokDownSet(){
        tempElapsedTime = elapsedTime;
        apex = transform.position;
        WokUp = false;
        WokDown = true;
    }

    private void WokUpSet(){
        WokUp = true;
        elapsedTime = 0;
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
            UIFlipUpdate("Good - Low!"); //.16 - .2
        }
        else if (time < cancelOutTime)
        {
            UpdateScores(weakFlipScore, weakFlipScoreMult);
            UIFlipUpdate("Weak!!!"); //.2 - .23
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

    private void DisableWok(ref MiniGame game)
    {
        actions["TiltKeys"].Disable();
        actions["LiftTouch"].started -= ILiftWok;
        actions["LiftTouch"].canceled -= IDownWok;
        actions["LiftKey"].started -= ILiftWok;
        actions["LiftKey"].canceled -= IDownWok;
        WokDownSet();
    }
    private void EnableWok(ref MiniGame game)
    {
        actions["TiltKeys"].Enable();
        actions["LiftTouch"].started += ILiftWok;
        actions["LiftTouch"].canceled += IDownWok;
        actions["LiftKey"].started += ILiftWok;
        actions["LiftKey"].canceled += IDownWok;

        game.EnableControls();
    }
    private void DisableWok()
    {
        actions["TiltKeys"].Disable();
        actions["LiftTouch"].started -= ILiftWok;
        actions["LiftTouch"].canceled -= IDownWok;
        actions["LiftKey"].started -= ILiftWok;
        actions["LiftKey"].canceled -= IDownWok;
        WokDownSet();
    }
    private void EnableWok()
    {
        actions["TiltKeys"].Enable();
        actions["LiftTouch"].started += ILiftWok;
        actions["LiftTouch"].canceled += IDownWok;
        actions["LiftKey"].started += ILiftWok;
        actions["LiftKey"].canceled += IDownWok;
    }
}
