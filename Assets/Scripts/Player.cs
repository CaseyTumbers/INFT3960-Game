using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor.XR;
using System.Dynamic;

public class Player : MonoBehaviour
{
    public float speed = 40f;
    private Rigidbody2D rbody;
    private bool jumped = false;
    private bool controllingCreature = false;
    public bool stop = false;
    private bool facingRight = true;
    private bool launched = false;
    public bool isFalling = false;
    private bool glide = false;

    public Transform isGroundedChecker;
    public Transform isWallRightChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float jumpForce;

    public Transform respawnPoint;
    protected Animator animator;
    public GameObject sprite;

    private bool mountedAnimPlayer = false;

    public int health = 3;
    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private bool addHealth = true;
    private float healthCoolDown;

    public GameObject dialogueGroup;
    private bool dialogueOnScreen = false;

    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    private Vector3 m_Velocity = Vector3.zero;
    float xMovement = 0f;

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!dialogueOnScreen)
        {
            //runFlag = Input.GetKey("right");
            //print(controllingCreature);
            updateHealth();
            healthCoolDown -= Time.deltaTime;
            if (!controllingCreature)
            {
                xMovement = Input.GetAxisRaw("Horizontal") * speed;
                animate();
                CanJump();
                //WallCheck();
                Jump();
                BetterJump();
                mountedAnimPlayer = false;
            }
            else
            {
                animator.SetBool("isRunning", false);
                if (!mountedAnimPlayer)
                {
                    animator.SetBool("isMounted", true);
                    animator.SetTrigger("mounted");
                    mountedAnimPlayer = true;
                }
            }
        }
        else
        {
            xMovement = 0;
            animator.SetBool("isRunning", false);
            animator.SetInteger("inAir", 0);

            if (Input.GetKeyDown(KeyCode.C))
            {
                dialogueGroup.SetActive(false);
                dialogueOnScreen = false;
            }
        }
    }

    private void FixedUpdate()
    {
       
         Move(xMovement * Time.fixedDeltaTime);
       
    }
    void Move(float moveValue)
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
        }*/
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !jumped && !launched)
        {
            rbody.velocity = Vector2.up*jumpForce;
            animator.SetInteger("inAir", 1);
        }
    }

    public void externalJump()
    {
        rbody.velocity = Vector2.up * (jumpForce);
    }

    void CanJump()
    {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        if (collider != null)
        {
            jumped = false;
            launched = false;
            animator.SetInteger("inAir", 0);
        }
        else
        {
            jumped = true;
        }
    }

    public bool ExternalJumpCheck()
    {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        if (collider != null)
        {
            animator.SetInteger("inAir", 0);
            return false;
        }
        else
        {
            return true;
        }
    }

    void BetterJump()
    {
        //print(launched);
       
        if (rbody.velocity.y < 0 && !glide)
        {
            rbody.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rbody.velocity.y > 0 && (!Input.GetKey(KeyCode.Space) || launched))
        {
            rbody.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    public void loseHealth()
    {
        if (healthCoolDown < 0)
        {
            healthCoolDown = 1f;
            health -= 1;
            if (health <= 0)
            {
                transform.position = respawnPoint.position;
                health = 3;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.layer.Equals(9) && !Input.GetKey(KeyCode.Z))
        {
            TakeDamage();
        }
        else if (collision.gameObject.name.Contains("Spikes"))
        {
            print("OW");
            transform.position = respawnPoint.position;
            loseHealth();
        }
        else if (collision.gameObject.name.Contains("Key"))
        {
            UnlockDoor.numKeys--;
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Health Crystal"))
        {
            if (addHealth)
            {
                addHealth = false;
                health++;
                Destroy(collision.gameObject);
            }
            addHealth = true;
        }
        else if (collision.name.Contains("Gem"))
        {
            print("Score");
            Destroy(collision.gameObject);
            ScoreManager.instance.ChangeScore(1);
        }
        else if (collision.name.Contains("Dialogue"))
        {
            //Time.timeScale = 0;
            if (!dialogueOnScreen)
            {
                dialogueOnScreen = true;
                dialogueGroup.SetActive(true);
                dialogueGroup.GetComponentInChildren<Dialogue>().writeText();
                collision.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else if (collision.name.Contains("Checkpoint"))
        {
            respawnPoint = collision.transform;
        }
    }

    public void TakeDamage()
    {
        if (health > 1)
        {
            launched = true;
            rbody.velocity = new Vector2(-100f, 50f);
        }
        loseHealth();
    }

    public void setControllingCreature(bool value)
    {
        controllingCreature = value;
    }

    public void setGlide(bool value)
    {
        glide = value;
    }

    public bool getGlide()
    {
        return glide;
    }

    public bool getControllingCreature()
    {
        return controllingCreature;
    }

     public int getHealth()
    {
        return health;
    }

    public void setLaunched(bool l)
    {
        launched = l;
    }

    public bool getFacingRight()
    {
        return facingRight;
    }

    public void faceRight()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        facingRight = true;
    }

    public void faceLeft()
    {
        transform.eulerAngles = new Vector3(0, 180, 0);
        facingRight = false;
    }

    public Animator getAnimator()
    {
        return animator;
    }
    public void animate()
    {
        if (Input.GetKey("left") && !jumped)
        {
            faceLeft();
            animator.SetBool("isRunning", true);
        }
        else if (Input.GetKey("right") & !jumped)
        {
            faceRight();
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if(rbody.velocity.y < 0)
        {
            animator.SetInteger("inAir", -1);
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }
    }

    public void updateHealth()
    {
        for(int i = 1; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if(i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
