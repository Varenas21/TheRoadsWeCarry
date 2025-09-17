using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player Objects")]
    private Rigidbody rb;
    public Transform camTransform;
    public float mouseSensitivity;

    [Header("Player Movement")]
    float x_Rotation = 0f;
    private float horizontalAxis;
    private float verticalAxis;
    public float playerSpeed;

    [Header("Player Interactavity")]
    public GameObject pickedObj;
    public LayerMask pickableLayer;
    public float detectRange;
    public float throwForce;

    [Header("UI")]
    public TMP_Text detectedObjName;
    public GameObject pickablePopup;
    public Image cursorDot;
    public Color[] cursorColors;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        CameraMovement();
    }

    void FixedUpdate()
    {
        ControlsUpdate();
    }

    private void ControlsUpdate()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * verticalAxis + right * horizontalAxis).normalized;

        if (moveDirection.magnitude > 0)
        {
            rb.AddForce(moveDirection * playerSpeed * 10f, ForceMode.Force);
        }

        //RaycastToCenter();
        HoldObject();
    }


    void CameraMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        x_Rotation -= mouseY;
        x_Rotation = Mathf.Clamp(x_Rotation, -90f, 90f);

        camTransform.localRotation = Quaternion.Euler(x_Rotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void RaycastToCenter()
    {
        RaycastHit hit;

        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, detectRange))
        {
            //cursorDot.color = cursorColors[1];

            if (!detectedObjName.gameObject.activeSelf) { detectedObjName.gameObject.SetActive(true); }

            if (detectedObjName.text != hit.collider.name) { detectedObjName.text = hit.collider.name; }

            RaycastHit hitObjPickable;

            if (Physics.Raycast(camTransform.position, camTransform.forward, out hitObjPickable, detectRange, pickableLayer))
            {
                if (!pickablePopup.activeSelf) { pickablePopup.SetActive(true); }
            }
            else
            {
                if (pickablePopup.activeSelf) { pickablePopup.SetActive(false); }
            }

        }

    }

    private void HoldObject()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, detectRange, pickableLayer))
            {
                pickedObj = hit.collider.gameObject;
                pickedObj.transform.parent = camTransform;
                Rigidbody objRb = pickedObj.GetComponent<Rigidbody>();
                objRb.isKinematic = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            if (pickedObj != null)
            {
                Rigidbody objRb = pickedObj.GetComponent<Rigidbody>();
                objRb.isKinematic = false;
                pickedObj.transform.parent = null;
                pickedObj = null;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (pickedObj != null)
            {
                Rigidbody objRb = pickedObj.GetComponent<Rigidbody>();

                pickedObj.transform.parent = null;
                objRb.isKinematic = false;
                objRb.AddForce(throwForce * camTransform.forward, ForceMode.Impulse);
                pickedObj = null;
            }
        }
    }
}
