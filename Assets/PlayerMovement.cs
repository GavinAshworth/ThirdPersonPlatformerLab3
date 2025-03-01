using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private CinemachineCamera freeLookCamera;
    private Rigidbody rb;
    private Vector2 moveInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        updateMovement();
        updateRotation();
    }

     private void updateMovement()
    {
        // This gets the cameras forward and side to side components so our players movements match the camera (We ignore the y so the player can look up and down without affecting falling or jumping)
        Vector3 cameraForward = freeLookCamera.transform.forward;
        Vector3 cameraRight = freeLookCamera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // This calcs move direction relative t othe camera
        Vector3 moveDirection = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

        rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);
    }

    private void updateRotation()
    {
        // Similarr to bowling, but we are using transform 
        Vector3 cameraForward = freeLookCamera.transform.forward;
        cameraForward.y = 0; // Ignore vertical rotation
        cameraForward.Normalize();
        transform.forward = cameraForward;
    }
    public void Move(InputAction.CallbackContext context){
        moveInput = context.ReadValue<Vector2>();
    }
}
