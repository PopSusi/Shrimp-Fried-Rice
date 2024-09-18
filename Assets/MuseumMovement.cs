using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumMovement : MonoBehaviour
{
    //MOVEMENT
    Rigidbody rb;
    float xInput;
    float yInput;
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


    }


    // Update is called once per frame
    void Update()
    {
        // MOVEMENT
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        // CAMERA
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        float cameraRot = Camera.main.transform.rotation.eulerAngles.y;
        rb.position += Quaternion.Euler(0, cameraRot, 0) * input * pSpeed * Time.deltaTime;

        // CAMERA CONTROLLERS
        rotation.x += Input.GetAxis(xAxis) * sensitivity;
        rotation.y += Input.GetAxis(yAxis) * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.localRotation = xQuat * yQuat;
    }
}
