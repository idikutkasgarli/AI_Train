using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Keybindings")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    [Header("Movement")]
    private float MoveSpeed = 5f;
    public float WalkSpeed = 5f;
    public float SprintSpeed = 10f;
    [Header("Ground Check")]
    public float groundDrag;
    public float playerHeight;
    public LayerMask WhatIsGround;
    [Header ("Jump")]
    public float jumpForce; 
    public float jumpCoolDown;
    public float airMultiplier;
    bool readyToJump = true;
    [Header("Crouch Speed")]
    public float crouchSpeed;
    public float crouchScaleY;
    private float startScaleY;
    private bool IsCrouching = false;
    [Header ("Slope Handling")]
    public float maxSlopeAngle;
    private  RaycastHit slopeHit;
    private bool exitingSlope;
    public float slopGravityvalue;
    [Header("Other")]
    public Transform orientation;

    float horizontalInput, verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    public MovementState state;
    public enum MovementState {Walking, Sprinting, Crouching, air}
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startScaleY = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        SpeedControl();
        StateHandler();
        Drag();
    }
    private void FixedUpdate()
    {
        //Debug.Log(rb.velocity.magnitude);
        //IsGrounded();
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        //Jump
        if(Input.GetKey(jumpKey) && IsGrounded() && readyToJump && state != MovementState.Crouching)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown);       
        }
        //Start Crouch
        if(Input.GetKeyDown(crouchKey) && IsGrounded())
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchScaleY, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            IsCrouching = true;
        }
        //Stop Crouch
        if(Input.GetKeyUp(crouchKey))
        {
            //nothing there on top
            if(!TopCast())
            {
                transform.localScale = new Vector3(transform.localScale.x, startScaleY, transform.localScale.z);
                IsCrouching = false;
            }
            //somethin there on top
            else
            {
                StandUpAgain();
            }
        }
        /*
        if(Input.GetKeyUp(crouchKey) && !TopCast())
        {
            transform.localScale = new Vector3(transform.localScale.x, startScaleY, transform.localScale.z);
            IsCrouching = false;
        }
*/
    }

    void StateHandler()
    {
        // Mode Crouch
        if((Input.GetKey(crouchKey) && IsGrounded()) || IsCrouching)
        {
            state = MovementState.Crouching;
            MoveSpeed = crouchSpeed;
        }
        // Mode Spring
        else if (IsGrounded()&&Input.GetKey(sprintKey))
        {
            state = MovementState.Sprinting;
            MoveSpeed = SprintSpeed;          
        }
         // Mode Walk        
        else if (IsGrounded())
        {
            state = MovementState.Walking;
            MoveSpeed = WalkSpeed;
        }
        // Mode air
        else
        {
            state = MovementState.air;
        }

    }
    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        // on slope
        if(OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection()* MoveSpeed * 20f, ForceMode.Force);
            //slope y axis lock
            if(rb.velocity.y >0)
            {
            rb.AddForce(Vector3.down * slopGravityvalue, ForceMode.Force);
            }
                
        }
        //on ground
        else if (IsGrounded())
        {
            rb.AddForce(moveDirection.normalized * MoveSpeed * 10f, ForceMode.Force);
        }
        //in air
        else
        {
            rb.AddForce(moveDirection.normalized * MoveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    rb.useGravity = !OnSlope();
    }
    bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight * 0.5f + 0.1f, WhatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void Drag()
    {
        if (IsGrounded())
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }
    
    void SpeedControl()
    {
        // limit speed on slope
        if(OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > MoveSpeed)
            {
                rb.velocity = rb.velocity.normalized * MoveSpeed;
            }
        }
        else
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //limit velocity if needed
            if(flatVelocity.magnitude > MoveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * MoveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
    }

    void Jump()
    {
        exitingSlope = true;
        //add jump force
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);

    }
    void ResetJump()
    {
        exitingSlope = false;
        readyToJump = true;
    }
    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle > maxSlopeAngle && angle !=0;
        }
    return false;
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    private bool TopCast()
    {
        return Physics.CapsuleCast(new Vector3(transform.position.x,transform.position.y - 1f,transform.position.z),new Vector3(transform.position.x,transform.position.y,transform.position.z),0.5f,Vector3.up,1f);
    }
    void StandUpAgain()
    {
        if(!TopCast())
        {
            transform.localScale = new Vector3(transform.localScale.x, startScaleY, transform.localScale.z);
            IsCrouching = false;
            return;
        }
        else
        {
            Invoke(nameof(StandUpAgain), 0.2f);
        }
    }
}

    

