using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [Space]
    [SerializeField] float PlayerMaxSpeed = 8;
    [SerializeField] float PlayerAcceleration = 3;

    [Header("Player Jump")]
    [Space]
    [SerializeField] float PlayerJumpForce = 10;
    [SerializeField] int minJumpCount = 1;
    [SerializeField] int maxJumpCount = 2;
    private int jumpsAllowed;

    [SerializeField] Transform playerRoot;
    private bool allowDoubleJump;
    private bool isGrounded;

    [Header("Player Camera Values")]
    [Space]
    [SerializeField] float PlayerLookSensitivity = 20;
    [SerializeField] float CameraClampDown = 50;
    [SerializeField] float CameraClampUp = 50;
    [SerializeField] float CameraClampLeft = 50;
    [SerializeField] float CameraClampRight = 50;
    [SerializeField] Transform cameraTransform;
    [Space]
    [SerializeField] PlayerInput playerInput;

    private Rigidbody rigidbody;

    private InputAction playerMovement;
    private InputAction playerLook;
    private InputAction playerJump;

    private float cameraRotationY;
    private float cameraRotationX;


    void Start()
    {
        jumpsAllowed = ResetJumps();
        rigidbody = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        playerMovement = playerInput.actions["Move"];
        playerLook = playerInput.actions["Look"];

        playerJump = playerInput.actions["Jump"];
        playerJump.started += Jump;
    }

    void FixedUpdate()
    {
        VelocityChange(PlayerDirection());
    }

    private void Update()
    {
        GroundCheck();
        Debug.Log(isGrounded);
    }
    private void LateUpdate()
    {
        UpdateCamera();
    }


    public void VelocityChange(Vector3 _playerDirection)
    {
        var temp = _playerDirection;

        Vector3 currentVelocity = rigidbody.linearVelocity;
        Vector3 targetVelocity = new Vector3(temp.x, 0, temp.z);
        targetVelocity *= PlayerAcceleration;

        targetVelocity = transform.TransformDirection(targetVelocity);

        Vector3 velocityChange = targetVelocity - currentVelocity;
        velocityChange.y = 0f;
        velocityChange = Vector3.ClampMagnitude(velocityChange, PlayerMaxSpeed);

        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private Vector3 PlayerDirection()
    {
        var temp = playerMovement.ReadValue<Vector2>();
        return new Vector3(temp.x, 0, temp.y);
    }

    private Vector2 GetRotation()
    {
        return playerLook.ReadValue<Vector2>();
    }

    private void UpdateCamera()
    {
        var rotation = GetRotation();

        cameraRotationY += -rotation.y * PlayerLookSensitivity * Time.deltaTime;
        cameraRotationY = Mathf.Clamp(cameraRotationY, -CameraClampUp, CameraClampDown);

        cameraRotationX += rotation.x * PlayerLookSensitivity * Time.deltaTime;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -CameraClampRight, CameraClampLeft);

        cameraTransform.eulerAngles = new Vector3(cameraRotationY, cameraRotationX, cameraTransform.eulerAngles.z);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && jumpsAllowed >= 1)
        {
            rigidbody.AddForce(new Vector3(0, PlayerJumpForce, 0), ForceMode.Impulse);
            jumpsAllowed--;
        }
    }

    private void GroundCheck()
    {
        if (Physics.Raycast(playerRoot.position, Vector3.down, 0.3f) == true)
        {
            jumpsAllowed = ResetJumps();
        }
    }

    private int ResetJumps()
    {
        if (allowDoubleJump)
        {
            return maxJumpCount;
        }
        return minJumpCount;
    }
}
