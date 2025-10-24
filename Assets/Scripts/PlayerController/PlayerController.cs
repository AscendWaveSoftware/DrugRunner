using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float PlayerMaxSpeed = 8;
    [SerializeField] float PlayerAcceleration = 3;

    [SerializeField] float PlayerLookSensitivity = 20;

    [SerializeField] float CameraClampUpAndDown = 50;
    [SerializeField] float CameraClampLeftAndRight = 50;

    [SerializeField] Transform cameraTransform;

    public PlayerInput playerInput;
    private Rigidbody rigidbody;

    private InputAction playerMovement;
    private InputAction playerLook;

    private float cameraRotationY;
    private float cameraRotationX;
    private Vector2 rotation;



    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        playerMovement = playerInput.actions["Move"];
        playerLook = playerInput.actions["Look"];
    }

    void FixedUpdate()
    {
        VelocityChange(PlayerDirection());
    }

    private void Update()
    {
        rotation = GetRotation();
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
        return new Vector3 ( temp.x, 0, temp.y );
    }

    private Vector2 GetRotation()
    {
        return playerLook.ReadValue<Vector2>();
    }

    private void UpdateCamera()
    {
        var rotation = GetRotation();

        cameraRotationY += -rotation.y * PlayerLookSensitivity * Time.deltaTime;
        cameraRotationY = Mathf.Clamp(cameraRotationY, -CameraClampUpAndDown, CameraClampUpAndDown);

        cameraRotationX += rotation.x * PlayerLookSensitivity * Time.deltaTime;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -CameraClampLeftAndRight, CameraClampLeftAndRight);

        cameraTransform.eulerAngles = new Vector3(cameraRotationY, cameraRotationX, cameraTransform.eulerAngles.z);
    }
}
