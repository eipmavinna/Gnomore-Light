using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed;
    float horizontalDirection = 0;
    float verticalDirection = 0;
    string sceneName;
    float initialGScale;
    bool facingRight = true;
    bool isGliding = false;

    private PlayerInput playerInput;
    private InputAction verticalMove;
    private InputAction jump;
    public LayerMask groundLayers;

    float lastTimeGrounded = 0;
    float jumpCooldown;
    float lastTimeJumped = 0;
    float jumpsLeft = 1;
    float maxFallSpeedGliding = 4f; //Used for gliding
    float maxFallSpeed = 40f;
    public float fallAllowance;
    public float jumpForce;
    public bool sceneEnabledVerticalMove = false;
    public AudioClip step;

    float lastStepSoundPlayed = 0;
    float climbAnimSpeed = 0.5f;

    string buttonName;

    Rigidbody2D _rbody;
    BoxCollider2D _collider;
    SpriteRenderer _spriteRenderer;
    ParticleSystem _deathParticles;
    HudManagerScript _hudManager;
    AudioSource _audioSource;
    Animator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _deathParticles = GetComponent<ParticleSystem>();
        _collider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _hudManager = FindAnyObjectByType<HudManagerScript>();
        _audioSource = GetComponent<AudioSource>();

        lastStepSoundPlayed = -step.length;

        initialGScale = _rbody.gravityScale;  //Store the initial gravity scale so it can be properly restored once the player leaves a ladder

        //Configure movement
        playerInput = GetComponent<PlayerInput>();
        verticalMove = playerInput.actions["VerticalMove"];
        jump = playerInput.actions["Jump"];
        jumpCooldown = fallAllowance;
        InputAction glide = playerInput.actions["Glide"];

        //Only allow vertical movement in the mole hole scene
        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != "MoleHoleScene" && !sceneEnabledVerticalMove)
        {
            verticalMove.Disable();
        }
        else
        {
            //if in a top-down level, player shouldn't be able to jump or glide
            jump.Disable();
            glide.Disable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded())
        {
            lastTimeGrounded = Time.time;
            jumpsLeft = 0;
            DisableGliding();
            

        }

        if (IsGrounded() || !jump.enabled)
        {
            if (verticalDirection == 0 && horizontalDirection == 0)
            {
                _audioSource.Stop();
                lastStepSoundPlayed = -step.length;
            }
            else if (Time.time > lastStepSoundPlayed + step.length)
            {
                lastStepSoundPlayed = Time.time;
                _audioSource.PlayOneShot(step);
            }
        }
        else
        {
            _audioSource.Stop();
            lastStepSoundPlayed = -step.length;
        }

        //Animator code
        float moveDelta = 0.3f;
        _animator.SetBool("Moving", (Mathf.Abs(_rbody.linearVelocity.magnitude) >= 0.05f));
        //if(SceneName != "MoleHoleScene")
        if(!verticalMove.enabled)
        {
            if (!IsGrounded())
            {
                _animator.SetBool("Jumping", _rbody.linearVelocityY >= moveDelta);
                _animator.SetBool("Falling", _rbody.linearVelocityY <= -moveDelta);
            }
        }

        if (horizontalDirection < 0 && facingRight)
        {
            Flip();
        }
        else if (horizontalDirection > 0 && !facingRight)
        {
            Flip();
        }

    }

    private void FixedUpdate()
    {
        //basic player movement
        _rbody.linearVelocityX = horizontalDirection * moveSpeed;

        if (verticalMove.enabled)
        {
            _rbody.linearVelocityY = verticalDirection * moveSpeed;
        }

        //Max fall speed when gliding
        float maxFall = isGliding ? maxFallSpeedGliding : maxFallSpeed;
        if (Mathf.Abs(_rbody.linearVelocityY) > maxFall)
        {
            _rbody.linearVelocityY = Mathf.Sign(_rbody.linearVelocityY) * maxFall;
        }
    }

    //checks for enemy collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (
            collision.gameObject.CompareTag("Enemy") ||
            collision.gameObject.CompareTag("Mole") ||
            collision.gameObject.CompareTag("Squirrel") ||
            collision.gameObject.CompareTag("Ant") ||
            collision.gameObject.CompareTag("Bird")
            )
        {
            Die(collision.gameObject.tag);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)         
    {
        
        if (collision.gameObject.CompareTag("MoleWall"))
        {
            Die("Wandered into a mole hole");
        }
        else if (collision.gameObject.CompareTag("button"))
        {
            //store the name of the button's scene in case the player interacts with it
            buttonName = collision.gameObject.name;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //allow player to climb ladders
        if (collision.gameObject.CompareTag("Ladder"))
        {
            DisableGliding();
            _animator.SetBool("Climbing", true);
            _rbody.gravityScale = 4;
            verticalMove.Enable();
            //TODO: climbing animation
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //disabling vertical move on exiting a ladder
        if (collision.gameObject.CompareTag("Ladder"))
        {
            _rbody.gravityScale = initialGScale;
            verticalMove.Disable();
            _animator.SetBool("Climbing", false);
            //TODO: exit climb animation
        }
        else if (collision.gameObject.CompareTag("button"))
        {
            buttonName = null;
        }
    }

    /*
     * ====================
     * Utility Methods
     * ====================
     */

    //Flips the player's sprite
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void SavePosition()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetFloat(currentScene + "_PlayerX", transform.position.x);
        PlayerPrefs.SetFloat(currentScene + "_PlayerY", transform.position.y);
    }

    //Restore the player's last known position in the current scene
    public void RestorePosition()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        float loadX = PlayerPrefs.GetFloat(currentScene + "_PlayerX", transform.position.x);
        float loadY = PlayerPrefs.GetFloat(currentScene + "_PlayerY", transform.position.y);
        transform.position = new Vector3(loadX, loadY, transform.position.z);
    }

    //Used for jumping
    private bool WasGrounded()
    {
        //is the difference from now and last time grounded insignificant
        return (Time.time - lastTimeGrounded <= fallAllowance);
    }

    //checks if the player is touching the ground in order to determine if the player can jump
    private bool IsGrounded()
    {
        Vector3 pos = transform.position;
        RaycastHit2D hit1 = Physics2D.Raycast(new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), Vector2.down, 1f, groundLayers);  //add or subtract half a unit to check both sides of the player
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z), Vector2.down, 1f, groundLayers);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, groundLayers);
        //lastTimeJumped = 0;
        bool grounded = hit3.collider != null || hit2.collider != null || hit1.collider != null;
        if (grounded)
        {
            _animator.SetBool("Falling", false);
        }
        return (grounded);
    }

    //Player death logic
    public void Die(string reason)
    {
        LockMovement();
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        _rbody.gravityScale = 0;
       
        _deathParticles.Play();
        _hudManager.DisplayDeathOverlay(reason);
    }

    //Prevents player from moving
    public void LockMovement()
    {
        InputAction move = playerInput.actions["Move"];
        move.Disable();
        jump.Disable();
        verticalMove.Disable();
        horizontalDirection = 0;
        verticalDirection = 0;
    }


    //Gliding behavior
    public void ToggleGliding()
    {
        if (isGliding)
        {
            DisableGliding();
        }
        else
        {
            EnableGliding();
        }
    }

    public void EnableGliding()
    {
        if (isGliding) { return; }
        isGliding = true;
        initialGScale /= 5; //Update initial G scale for proper climbing interaction
        _rbody.gravityScale = initialGScale;
        _animator.SetBool("Gliding", true);
    }

    public void DisableGliding()
    {
        if (!isGliding) { return; }
        isGliding = false;
        initialGScale *= 5; //resetting gravity scale to original value
        _rbody.gravityScale = initialGScale;
        _animator.SetBool("Gliding", false);
    }


    /** 
     * ====================
     * Input Action Methods
     * ====================
     */
    private void OnPause(InputValue value)
    {
        _hudManager.Pause();
    }
    void OnMove(InputValue value)
    {
        float direction = value.Get<float>();
        horizontalDirection = direction;
    }

    //only works in scenes with vertical move enabled, or on ladders
    void OnVerticalMove(InputValue value)
    {
        float direction = value.Get<float>();
        verticalDirection = direction;
    }

    void OnGlide(InputValue button)
    {
        ToggleGliding();
    }

    void OnJump(InputValue button)
    {
        if (button.isPressed)
        {
            if (jumpsLeft > 0 || (WasGrounded() && (Time.time - lastTimeJumped > jumpCooldown)))
            {
                _animator.SetBool("Jumping", true);
                _rbody.AddForce(Vector2.up * jumpForce);
                lastTimeJumped = Time.time;
                jumpsLeft -= 0;

            }
        }
        else if (_rbody.linearVelocityY > 0) //not been pressed 
        {
            _rbody.linearVelocity = _rbody.linearVelocity * 0.5f;   //kills upward velocity
        }

    }

    //used for going through enterences 
    private void OnInteract(InputValue value)
    {
        Debug.Log("interacted");
        if (buttonName != null)
        {
            //Remember player position before changing scenes
            SavePosition();

            //Load the scene
            SceneManager.LoadScene(buttonName);
        }
    }

}
