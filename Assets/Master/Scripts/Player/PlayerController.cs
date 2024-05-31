using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private int JumpsLeft;
    private int facingDir = 1;
    private int lastWallJumpDirection;

    public int JumpAmounts = 1;

    private float moveInputDir;
    private float jumpTimer;
    private float turnTimer;
    private float wallJumpTimer;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100;
    private float knockbackStartTime;
    [SerializeField]
    private float knockbackDuration;

    private bool isFacingRight = true;
    public bool isGrounded;
    private bool isTouchingWall;
    private bool isRunning;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isWallSliding;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;
    private bool hasWallJumped;
    private bool isDashing;
    private bool knockback;

    [SerializeField]
    private Vector2 knockbackSpeed;

    public float moveSpeed = 10.0f;
    public float runSpeed = 15.0f; // Tambahan kecepatan saat sprint
    public float JumpPower = 10f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float ForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallHopForce;
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;
    public float turnTimerSet = 0.1f;
    public float wallJumpTimerSet = 0.5f;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCooldown;

    public Vector2 wallHopDir;
    public Vector2 wallJumpDir;

    public LayerMask whatIsGround;

    public Transform groundCheck;
    public Transform wallCheck;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        JumpsLeft = JumpAmounts;
        wallHopDir.Normalize();
        wallJumpDir.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMoveDir();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
        CheckDash();
        CheckKnockBack();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }
    private void CheckIfWallSliding()
    {
        if(isTouchingWall && moveInputDir == facingDir && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    public bool GetDashStatus()
    {
        return isDashing;
    }

    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    private void CheckKnockBack()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0.01f)
        {
            JumpsLeft = JumpAmounts;
        }

        if (isTouchingWall)
        {
            canWallJump = true;
        }
        if(JumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
    }

    private void CheckMoveDir()
    {
        if (isFacingRight && moveInputDir < 0)
        {
            Flip();
        }
        else if (!isFacingRight && moveInputDir > 0)
        {
            Flip();
        }

        if (Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(rb.velocity.x) > moveSpeed)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isRunning", isRunning);
    }

    private void CheckInput()
    {
        moveInputDir = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if(isGrounded || (JumpsLeft > 0 && isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        if(Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if (!isGrounded && moveInputDir != facingDir)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
        }

        if (!canMove)
        {
            turnTimer -= Time.deltaTime;

            if(turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }

        if (Input.GetButton("Dash"))
        {
            if(Time.time >= (lastDash + dashCooldown))
            {
                AttemptToDash();
            }
        }
    }

    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        AfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }

    public int GetFacingDir()
    {
        return facingDir;
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            if(dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;
                rb.velocity = new Vector2(dashSpeed * facingDir, 0);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    AfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }

            }
            if(dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }
        }
    }
    private void CheckJump()
    {
        if(jumpTimer > 0)
        {
            if(!isGrounded && isTouchingWall && moveInputDir != 0 && moveInputDir != facingDir)
            {
                WallJump();
            }
            else if (isGrounded)
            {
                    NormalJump();
            }
        }
        if(isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }
        if(wallJumpTimer > 0)
        {
            if(hasWallJumped && moveInputDir == -lastWallJumpDirection)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                hasWallJumped = false;
            }
            else if(wallJumpTimer <= 0)
            {
                hasWallJumped = false;
            }
            else
            {
                wallJumpTimer -= Time.deltaTime;
            }
        }

    }

    private void NormalJump()
    {
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
            JumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void WallJump()
    {
        if (canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            isWallSliding = false;
            JumpsLeft = JumpAmounts;
            JumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDir.x * -moveInputDir, wallJumpForce * wallJumpDir.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDir;

        }
    }
    private void ApplyMovement()
    {
        float speed = moveSpeed;

        // Check if the player is holding down the sprint key (left shift)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }
        if (!isGrounded && !isWallSliding && moveInputDir == 0 && !knockback)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if(canMove && !knockback)
        {
            rb.velocity = new Vector2(speed * moveInputDir, rb.velocity.y);
        }


        if (isWallSliding)
        {
            if(rb.velocity.y < -wallSlideSpeed)
            {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }

    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }
    private void Flip()
    {
        if (!isWallSliding && canFlip && !knockback)
        {
            facingDir *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

    }

    public bool IsFacingRight { get { return isFacingRight; } }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}

