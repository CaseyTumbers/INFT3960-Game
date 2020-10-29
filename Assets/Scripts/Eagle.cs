using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : controllableEnemy
{
    private float initialX;
    private float initialY;

    public float speed = 40f;
    private Rigidbody2D rbody;
    private bool jumped = false;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float jumpForce = 20;

    private int jumpCount = 2;

    private Animator animator;

    //[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    //private Vector3 m_Velocity = Vector3.zero;
    //float xMovement = 0f;
    // Start is called before the first frame update
    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        initialX = this.transform.position.x;
        initialY = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isControlled)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, this.transform.position.z);
            GetComponent<BoxCollider2D>().enabled = false;
            Glide();
            Jump();

            if (Input.GetKeyDown(KeyCode.Z))
            {
                player.GetComponent<CollideWithEnemy>().Dismount();
                this.player = null;
                isControlled = false;
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        else
        {
            this.transform.position = new Vector2(initialX, initialY);
        }
    }

    /*private void FixedUpdate()
    {
        if (isControlled)
        {
            rbody = GetComponent<Rigidbody2D>();
            Move(xMovement * Time.fixedDeltaTime);
        }
    }*/

    void Glide()
    {
        if (player.GetComponent<Player>().isFalling && Input.GetKey(KeyCode.Space))
        {
            player.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        }
        else
        {
            player.GetComponent<Rigidbody2D>().gravityScale = 3f;
        }
    }

    /*void Move(float moveValue)
    {
        Vector3 targetVelocity = new Vector2(moveValue * 10f, rbody.velocity.y);
        // And then smoothing it out and applying it to the character
        rbody.velocity = Vector3.SmoothDamp(rbody.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        /*
        if (!stop || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            float x = Input.GetAxisRaw("Horizontal");
            float moveBy = x * speed;
            rbody.velocity = new Vector2(moveBy, rbody.velocity.y);
        }

        if (stop && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            stop = false;
        }
    }*/

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && player.GetComponent<Player>().ExternalJumpCheck())
        {
            jumpCount--;
            if (jumpCount == 1)
            {
                print("DOUBLE JUMP");
                player.GetComponent<Player>().externalJump();
            }
        }

        if (!player.GetComponent<Player>().ExternalJumpCheck())
        {
            jumpCount = 2;
        }
    }

    void CanJump()
    {
        if (player.GetComponent<Player>().ExternalJumpCheck())
        {
            jumped = false;
        }
        else
        {
            jumped = true;
        }
    }

    void BetterJump()
    {
        //print(launched);

        if (rbody.velocity.y < 0)
        {
            rbody.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rbody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rbody.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
