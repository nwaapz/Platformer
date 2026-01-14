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
    private float jumpTimeCounter;
    public float jumpTime;
    public float jumpsQ;
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
        animator.SetBool("fall", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("Idle", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move_x = Input.GetAxisRaw("Horizontal"); ///taked standart run axis LOCK TO BUTTONS
        r2d.linearVelocity = new Vector2(move_x * speed, r2d.linearVelocity.y); ///run left right 
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGrounded); ///ground checker
        
        // Debug ground check
       // Debug.Log($"[Ground Debug] isGround={isGround}, groundCheck.pos={groundCheck.position}, checkRadius={checkRadius}, layerMask={whatIsGrounded.value}");
    }

    public void Update()
    {
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

        ///jump_controll
        if (Input.GetKeyDown(KeyCode.Z) && jumpsQ > 0)
        {
            r2d.linearVelocity = Vector2.up * jump_force;
            jumpTimeCounter = jumpTime;
            jumpsQ -= 1;
            jump = true;
            Instantiate(jump_fx, groundCheck.transform.position, groundCheck.transform.rotation);

        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            jump = false;
        }
        if (Input.GetKey(KeyCode.Z) && jump == true)
        {
            if (jumpTimeCounter > 0)
            {
                r2d.linearVelocity = Vector2.up * jump_force;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                jump = false;
            }
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
            jumpsQ = 1;
        }

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
        if (jumpi == true && jumpsQ > 0)
        {
            Instantiate(jump_fx, groundCheck.transform.position, groundCheck.transform.rotation);
            jumpTime = 0.25f;
            jump_force = 12;
            r2d.linearVelocity = Vector2.up * jump_force;
            jumpTimeCounter = jumpTime;
            jumpsQ -= 1;
            jump = true;
        }
        else if (jumpi == false)
        {
            jump_force = 0;
            jump = false;
        }
        if (jumpi == true && jump == true)
        {
            if (jumpTimeCounter > 0)
            {
                jump_force = 12;
                r2d.linearVelocity = Vector2.up * jump_force;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                jump = false;
            }
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
