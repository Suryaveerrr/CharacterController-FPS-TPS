using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class AdvancedPlayerMovement : MonoBehaviour
{
    [Header("Player References")]
    public Transform cameraTransform;

    [Header("Movement Speeds")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 8f;

    [Header("Jumping & Gravity")]
    public float jumpHeight = 1.5f;
    public float gravity = -15f;

    
    [Header("Ground Check")]
    public float groundCheckDistance = 0.3f;
    public LayerMask groundMask;

    [Header("Rotation Smoothing")]
    [Range(0.0f, 0.3f)]
    public float turnSmoothTime = 0.1f;

    private CharacterController controller;
    private Animator animator;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private float turnSmoothVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        
        
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        // --- Handle Movement & Rotation ---
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = transform.forward;
            controller.Move(moveDir * currentSpeed * Time.deltaTime);
        }

       
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");
        }

        // --- Apply Gravity ---
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // --- ANIMATOR LOGIC ---
        if (direction.magnitude >= 0.1f)
        {
            float animationSpeed = currentSpeed * direction.magnitude;
            animator.SetFloat("Speed", animationSpeed / sprintSpeed, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
        }

        animator.SetBool("isGrounded", isGrounded);
    }

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, groundCheckDistance);
    }
}