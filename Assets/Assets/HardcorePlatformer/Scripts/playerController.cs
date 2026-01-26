using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerController : MonoBehaviour
{ 
    public float move_x;
    public float speed;
    public float jump_force;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGrounded;
    public bool jump;
    public bool run;
    public bool isGround;
    private bool wasGrounded; // Track previous ground state
    private float jumpTimeCounter;
    public float jumpTime;
    public float jumpsQ;
    public int maxJumps = 2; // Maximum jumps allowed (2 = double jump)
    private float jumpCooldown = 0f; // Cooldown timer to ignore ground detection after jump
    public Rigidbody2D r2d;
    public SpriteRenderer sr;
    public Animator animator;
    private GameMaster gm;
    public GameObject run_fx;
    public float TimeFX;
    public float StartTimeFX;
    public GameObject jump_fx;
    public SpriteRenderer run_fxsp;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        r2d = GetComponent<Rigidbody2D>(); ///take 2d phisics
        
        // Initialize ground check and animation state
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGrounded);
        wasGrounded = isGround; // Initialize wasGrounded to match starting ground state
        jumpsQ = maxJumps; // Initialize jump count
        animator.SetBool("fall", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("Idle", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move_x = Input.GetAxisRaw("Horizontal"); ///taked standart run axis LOCK TO BUTTONS
        r2d.linearVelocity = new Vector2(move_x * speed, r2d.linearVelocity.y); ///run left right 
    }

    public void Update()
    {
        // Countdown jump cooldown
        if (jumpCooldown > 0)
        {
            jumpCooldown -= Time.deltaTime;
        }
        
        // Check ground state - ignore if in jump cooldown
        bool overlapCheck = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGrounded);
        if (jumpCooldown > 0)
        {
            isGround = false; // Force not grounded during cooldown
        }
        else
        {
            isGround = overlapCheck && r2d.linearVelocity.y <= 0.1f;
        }
        
        // Debug only when grounded
        if (isGround)
        {
          //  Debug.Log($"[GROUNDED] jumpsQ={jumpsQ}, wasGrounded={wasGrounded}");
        }
        
        /// left controll (Arrow + WASD)
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {    
          run = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
          run = false;
        }

        ///right controll (Arrow + WASD)
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            run = true;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            run = false;
        }
        
        // Debug trace for animation state
      //  Debug.Log($"[Anim Debug] run={run}, isGround={isGround}, jump={jump}, move_x={move_x}");

        ///run animation and stay animation
        if (run == true && isGround == true)
        {
            animator.SetFloat("speed", Mathf.Abs(move_x));
            animator.SetBool("Idle", false);
            if (TimeFX <= 0)
            {
                Instantiate(run_fx, groundCheck.transform.position, groundCheck.transform.rotation);
                TimeFX = StartTimeFX;
            }
            else
            {
                TimeFX -= Time.deltaTime;
            }

        }
        else if (run == false && isGround == true)
        {
            animator.SetFloat("speed", Mathf.Abs(move_x));
            animator.SetBool("Idle", true);

        }

        ///jump_controll (Z key or Space key) - calls the same function as UI button
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space))
        {
            JumpBut(true);
        }
        else if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Space))
        {
            JumpBut(false);
        }
        if ((Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Space)) && jump == true)
        {
            JumpBut(true);
        }

        ///check jump player or not
        if (jump == true)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("fall", false);
            animator.SetBool("Idle", false);

        }
        else if (jump == false && isGround == false)
        {
            // Only show fall animation when actually in the air
            animator.SetBool("fall", true);
            animator.SetBool("isJumping", false);
        }

        /// flip player
        if (move_x > 0)
        {
            run_fxsp.flipX = false;
            sr.flipX = true;
        }
        else if (move_x < 0)
        {
            sr.flipX = false;
            run_fxsp.flipX = true;
        }

        // chek on ground player or not
        if (isGround == true)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("fall", false);
            // Only set Idle if not moving, let run animation handle it otherwise
            if (run == false)
            {
                animator.SetBool("Idle", true);
            }
            // Only reset jumps when player just landed (was not grounded, now is grounded)
            if (!wasGrounded)
            {
                jumpsQ = maxJumps;
            }
        }
        wasGrounded = isGround;

    }
    // button Left UI
    public void LeftBut(bool runUI) {
        if (runUI == true)
        {
            move_x = -1;
            r2d.linearVelocity = new Vector2(move_x * speed, r2d.linearVelocity.y);
            run = true;
        }
        else if (runUI == false)
        {
            move_x = 0;
            r2d.linearVelocity = new Vector2(move_x * speed, r2d.linearVelocity.y);
            run = false;
        }
    }
    // button Right UI
    public void RightBut(bool runUI)
    {
        if (runUI == true)
        {
            move_x = 1;
            r2d.linearVelocity = new Vector2(move_x * speed, r2d.linearVelocity.y);
            run = true;
        }
        else if (runUI == false)
        {
            move_x = 0;
            r2d.linearVelocity = new Vector2(move_x * speed, r2d.linearVelocity.y);
            run = false;
        }
    }
    /// button Jump UI
    public void JumpBut(bool jumpi)
    {
        if (jumpi == true && jumpsQ > 0 && jump == false)
        {
            // Start a new jump - only when not already jumping
            print("new jump");
            Instantiate(jump_fx, groundCheck.transform.position, groundCheck.transform.rotation);
            jumpTime = 0.25f;
            jump_force = 12;
            r2d.linearVelocity = Vector2.up * jump_force;
            jumpTimeCounter = jumpTime;
            jumpsQ -= 1;
            jump = true;
            wasGrounded = true;
            isGround = false;
            jumpCooldown = 0.2f; // Ignore ground detection for 0.2 seconds
        }
        else if (jumpi == true && jump == true)
        {
            // Continue jump (variable height) - only when already jumping
            if (jumpTimeCounter > 0)
            {
                /*print("double jump");
                jump_force = 12;
                r2d.linearVelocity = Vector2.up * jump_force;
                jumpTimeCounter -= Time.deltaTime;*/
            }
            else
            {
                jump = false;
            }
        }
        else if (jumpi == false)
        {
            jump_force = 0;
            jump = false;
        }
    }
    ///Check Check points true
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("checkpoints"))
        {
            gm.lastCheckPointPos = transform.position;
        }

    }

 

}
