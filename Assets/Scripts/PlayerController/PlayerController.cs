using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [Space]
    [SerializeField] float PlayerMaxSpeedDefault = 10;
    [SerializeField] float PlayerMaxSpeedCoke = 14;
    [SerializeField] float PlayerAcceleration = 3;
    private float playerMaxSpeed = 10;

    [Header("Time Scale")]
    [Space]
    [SerializeField] float DefaultTimeScale = 1;
    [SerializeField] float SlowMotionTimeScale = 0.4f;

    [Header("Player Jump")]
    [Space]
    [SerializeField] float PlayerJumpForce = 10;
    [SerializeField] int doubleJumpCounter = 0;

    [SerializeField] Transform playerRoot;
    [SerializeField] bool allowDoubleJump = false;
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
    private CapsuleCollider collider;

    private InputAction playerMovement;
    private InputAction playerLook;
    private InputAction playerJump;

    private float cameraRotationY;
    private float cameraRotationX;

    void Start()
    {
        collider = GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;

        playerMovement = playerInput.actions["Move"];
        playerLook = playerInput.actions["Look"];
        playerJump = playerInput.actions["Jump"];

        playerJump.started += Jump;

        LSDDrug.OnLSDEnabled += EnableDoubleJump;
        LSDDrug.OnLSDDisabled += DisableDoubleJump;

        CokeDrug.OnCokeEnabled += IncreaseMaxSpeed;
        CokeDrug.OnCokeDisabled += DecreaseMaxSpeed;

        HeroinDrug.OnHeroinEnabled += DisablePlayerCollision;
        HeroinDrug.OnHeroinDisabled += EnablePlayerCollision;

        HazeDrug.OnHazeEnabled += SlowMotion;
        HazeDrug.OnHazeDisabled += DisableSlowMotion;
    }


    private void OnDisable()
    {
        playerJump.started -= Jump;

        LSDDrug.OnLSDEnabled -= EnableDoubleJump;
        LSDDrug.OnLSDDisabled -= DisableDoubleJump;

        CokeDrug.OnCokeEnabled -= IncreaseMaxSpeed;
        CokeDrug.OnCokeDisabled -= DecreaseMaxSpeed;

        HeroinDrug.OnHeroinEnabled -= DisablePlayerCollision;
        HeroinDrug.OnHeroinDisabled -= EnablePlayerCollision;

        HazeDrug.OnHazeEnabled -= SlowMotion;
        HazeDrug.OnHazeDisabled -= DisableSlowMotion;
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
        Vector3 targetVelocity = new Vector3(temp.x, 0, 1);
        targetVelocity *= playerMaxSpeed;

        targetVelocity = transform.TransformDirection(targetVelocity);

        Vector3 velocityChange = targetVelocity - currentVelocity;
        velocityChange.y = 0f;
        velocityChange = Vector3.ClampMagnitude(velocityChange, PlayerAcceleration);

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

    private void Jump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            rigidbody.AddForce(new Vector3(0, PlayerJumpForce, 0), ForceMode.Impulse);
        }

        if (context.started && !isGrounded && doubleJumpCounter > 0)
        {
            rigidbody.AddForce(new Vector3(0, PlayerJumpForce, 0), ForceMode.Impulse);
            doubleJumpCounter--;
        }
    }

    private void GroundCheck()
    {
        if (Physics.Raycast(playerRoot.position, Vector3.down, 0.3f) == true)
        {
            isGrounded = true;
            if (allowDoubleJump)
                doubleJumpCounter = 1;
            else
                doubleJumpCounter = 0;      
        }

        else
            isGrounded = false;
    }

    #region Event Methods
    private void DisableDoubleJump(object sender, EventArgs e)
    {
        allowDoubleJump = false;
    }
    private void EnableDoubleJump(object sender, EventArgs e)
    {
        allowDoubleJump = true;
    }
    private void IncreaseMaxSpeed(object sender, EventArgs e)
    {
        playerMaxSpeed = PlayerMaxSpeedCoke;
    }
    private void DecreaseMaxSpeed(object sender, EventArgs e)
    {
        playerMaxSpeed = PlayerMaxSpeedDefault;
    }
    private void SlowMotion(object sender, EventArgs e)
    {
        Time.timeScale = SlowMotionTimeScale;
    }
    private void DisableSlowMotion(object sender, EventArgs e)
    {
        Time.timeScale = DefaultTimeScale;
    }

    private void EnablePlayerCollision(object sender, EventArgs e)
    {
        //collider.enabled = true;
        DrugsManager.playerCanDie = true;
    }

    private void DisablePlayerCollision(object sender, EventArgs e)
    {
        //collider.enabled = false;
        DrugsManager.playerCanDie = false;
    }
    #endregion
}
