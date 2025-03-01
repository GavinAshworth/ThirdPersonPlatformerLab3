using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
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

    private void updateMovement(){
        //Move input is vector 2 so we change to vector 3 and sub in our current y velocity for y
        rb.linearVelocity = new Vector3(moveInput.x * speed, rb.linearVelocity.y, moveInput.y * speed);
    }

    private void updateRotation(){
        transform.forward = freeLookCamera.transform.forward;
        transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y,0);
    }
    public void Move(InputAction.CallbackContext context){
        moveInput = context.ReadValue<Vector2>();
    }
}
