using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpSpeed = 6f;
    [SerializeField] private float downGravityMultiplier = 4f; 
    [SerializeField] private float minJumpMultiplier = 0.75f;
    [SerializeField] private CinemachineCamera freeLookCamera;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isHoldingJump;
    private bool isGrounded;
    private bool hasDoubleJumped;
    private bool jumpInput;
    private bool hasJumped;
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
        jump();
        increaseDownGravity();
        variableJumpHeight();
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

    private void jump(){
        if(isGrounded && jumpInput){
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            jumpInput = false; // Reset jump input
            isHoldingJump = true;
            hasJumped = true;
        }
        else if(jumpInput && !hasDoubleJumped){ //this is for double jump
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // We reset vertical velocity so its an actual double jump and doesnt stack when spammed, or cancel out when falling
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            jumpInput = false;
            hasDoubleJumped = true;
            isHoldingJump = true;
            hasJumped = true;
        }
    }
    private void variableJumpHeight(){
        if(!isHoldingJump && rb.linearVelocity.y > 0 && hasJumped){ 
            //If user stops holding jump we apply a strong downward force so we get variable jumping
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * minJumpMultiplier, rb.linearVelocity.z);
        }
    }

    private void increaseDownGravity(){
        //This is how we amp up down gravity so jumps feel less floaty
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (downGravityMultiplier - 1) * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision){
        //Checks if player is on the ground
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground")){
            isGrounded = true;
            hasDoubleJumped = false;
            hasJumped = false;
        }
    }

    private void OnCollisionExit(Collision collision){
        //Checks if leaves the ground
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground")){
            isGrounded = false;
        }
    }
    public void Move(InputAction.CallbackContext context){
        moveInput = context.ReadValue<Vector2>(); //Checks if player pressed wasd or arrow keys
    }
    public void Jump(InputAction.CallbackContext context){
        if(context.performed && (isGrounded || !hasDoubleJumped)){ //checks if user hit spacebar
            jumpInput = true;
        }else if(context.canceled){
            //This is for when user stops holding spacebar
            isHoldingJump = false;
        }
    }

}
