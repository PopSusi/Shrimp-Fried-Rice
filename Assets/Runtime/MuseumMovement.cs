using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MuseumMovement : MonoBehaviour
{
    public InputActionAsset actions;

    //MOVEMENT
    Rigidbody rb;
    Vector2 moveInput;
    public float pSpeed;

    // CAMERA CONTROLLERS
    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = value; }
    }
    [Range(0.1f, 9f)] [SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)] [SerializeField] float yRotationLimit = 88f;

    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X"; //Strings in direct code generate garbage, storing and re-using them creates no garbage
    const string yAxis = "Mouse Y";



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        actions.Enable();

    }


    // Update is called once per frame
    void Update()
    {
        // MOVEMENT
        moveInput = actions.FindAction("Move").ReadValue<Vector2>();

        // CAMERA
        Vector3 input = new Vector3(moveInput.x, 0, moveInput.y);
        float cameraRot = Camera.main.transform.rotation.eulerAngles.y;
        rb.position += Quaternion.Euler(0, cameraRot, 0) * input * pSpeed * Time.deltaTime;

        // CAMERA CONTROLLERS
        rotation = actions.FindAction("Look").ReadValue<Vector2>();
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.localRotation *= xQuat * yQuat;
    }
}
