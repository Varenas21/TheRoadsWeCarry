using Assets.Scripts.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 2f;

    [Header("Camera Settings")]
    [SerializeField] private float mouseSensitivity = 0.2f;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform camTransform;

    [Header("Interactions")]
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float interactRange = 3f;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private Transform _transform;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
        //Debug.Log("Looking Around");
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //Debug.Log($"Move Input: {moveInput}");

    }
    public void OnJump(InputAction.CallbackContext context)
    {
        //Debug.Log($"Jumping {context.performed} - Is Grounded: {controller.isGrounded}");
        if (context.performed && controller.isGrounded)
        {
            //Debug.Log("We are jumping");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void Interaction(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (!Physics.Raycast(camTransform.position, camTransform.forward, out var hit, interactRange, interactableLayer)) 
        { 
            Debug.Log("No Interactable hit"); 
            return; 
        }
       

        if (!hit.transform.TryGetComponent(out InteractableObject interactable)) 
        {
            Debug.Log($"Hit {hit.transform.name}, it has no interactableObject");
            return; 
        }

        Debug.Log($"Interacting with object {hit.collider.gameObject.name}");    
        interactable.Interact();
    }



     private void CameraMovement()
    {
        float mouseX  = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        camTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
        
    }

    private void PlayerMovement()
    {
        Vector3 move = (playerBody.right * moveInput.x) + (playerBody.forward * moveInput.y);
        move.y = 0f;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0) { velocity.y = -2f; }

    }




    private void Update()
    {
        PlayerMovement();
        CameraMovement();

    }

}
