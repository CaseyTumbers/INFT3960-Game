using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Dynamic;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //Base Movement Variables
    public float speed = 40f;
    private Rigidbody2D rbody;
    private bool jumped = false;
    private bool controllingCreature = false;
    public bool stop = false;
    private bool facingRight = true;
    private bool launched = false;
    public bool isFalling = false;
    private bool glide = false;
    private bool addToScore = true;

    //Ground Checker Variables
    public Transform isGroundedChecker;
    public Transform isWallRightChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;

    //Jump and Fall Variables
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float jumpForce;

    //Animation Variables
    public Transform respawnPoint;
    protected Animator animator;
    public GameObject sprite;
    private bool mountedAnimPlayer = false;

    //Health Variables
    public int health = 3;
    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private bool addHealth = true;
    private float healthCoolDown = 0;

    //Dialogue Variables
    public GameObject dialogueGroup;
    private bool dialogueOnScreen = false;

    //Smoother Movement Variables
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    private Vector3 m_Velocity = Vector3.zero;
    float xMovement = 0f;

    //Audio Variables
    AudioSource audioData;
    public AudioClip collectGem;
    public AudioClip collectCrystal;
    public AudioClip jumpSound;

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioData = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!dialogueOnScreen)
        {
            //runFlag = Input.GetKey("right");
            //print(controllingCreature);
            updateHealth();
            if (healthCoolDown > 0)
            {
                healthCoolDown -= Time.deltaTime;
            }
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
                //print(dialogueGroup.GetComponentInChildren<Dialogue>().checkProgress());
                if (dialogueGroup.GetComponentInChildren<Dialogue>().checkProgress())
                {
                    dialogueGroup.GetComponentInChildren<Dialogue>().NextSentence();
                }

                if (dialogueGroup.GetComponentInChildren<Dialogue>().checkFinished())
                {
                    dialogueGroup.SetActive(false);
                    dialogueOnScreen = false;
                }
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
            audioData.clip = jumpSound;
            audioData.Play(0);
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
        if (healthCoolDown <= 0)
        {
            healthCoolDown = 1f;
            health -= 1;
            if (health <= 0)
            {
                transform.position = respawnPoint.position;
                health = 3;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
            audioData.clip = collectCrystal;
            audioData.Play(0);
            if (addHealth)
            {
                addHealth = false;
                if (health < 3)
                {
                    health++;
                }
                Destroy(collision.gameObject);
            }
            addHealth = true;
        }
        else if (collision.name.Contains("Gem"))
        {
            Destroy(collision.gameObject);
            if (addToScore)
            {
                addToScore = false;
                //print("Score");
                audioData.clip = collectGem;
                audioData.Play(0);
                ScoreManager.instance.ChangeScore(1);
            }
            addToScore = true;
        }
        else if (collision.name.Contains("Dialogue"))
        {
            //Time.timeScale = 0;
            if (!dialogueOnScreen)
            {
                if (collision.name.Contains("1"))
                {
                    dialogueGroup.GetComponentInChildren<Dialogue>().setNum(1);
                    //print("number 1");
                }
                else if (collision.name.Contains("2"))
                {
                    dialogueGroup.GetComponentInChildren<Dialogue>().setNum(2);
                }
                else if (collision.name.Contains("3"))
                {
                    dialogueGroup.GetComponentInChildren<Dialogue>().setNum(3);
                }

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

    public void chargeAnim()
    {
        animator.SetTrigger("charge");
    }
    public void attackAnim(bool value)
    {
        animator.SetBool("attack", value);
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
