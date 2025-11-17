using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]

public class PlayerScript : MonoBehaviour
{
    //public float jumpStrength;
    public float moveSpeed;
    float horizontalDirection = 0;
    float verticalDirection = 0;
    string sceneName;
    float initialGScale;
    Animator _animator;
    bool facingRight = true;

    private PlayerInput playerInput;
    private InputAction verticalMove;
    private InputAction jump;
    public LayerMask groundLayers;

    float lastTimeGrounded = 0;
    float jumpCooldown;
    float lastTimeJumped = 0;
    float jumpsLeft = 1;
    public float fallAllowance;
    public float jumpForce;

    string buttonName;
    string SceneName;

    Rigidbody2D _rbody;
    BoxCollider2D _collider;
    SpriteRenderer _spriteRenderer;
    ParticleSystem _deathParticles;
    HudManagerScript _hudManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _deathParticles = GetComponent<ParticleSystem>();
        _collider = GetComponent<BoxCollider2D>();
        initialGScale = _rbody.gravityScale;  //taking the initial gravity scale so it can be edited freely in the unity instead of changing once the player leaves a ladder
        //Debug.Log(initialGScale);

        _hudManager = FindAnyObjectByType<HudManagerScript>();
        playerInput = GetComponent<PlayerInput>();
        verticalMove = playerInput.actions["VerticalMove"];
        jump = playerInput.actions["Jump"];
        jumpCooldown = fallAllowance;

        _animator = GetComponent<Animator>();
        sceneName = SceneManager.GetActiveScene().name;




        //only allow vertical movement if in the mole hole scene
        if (sceneName != "MoleHoleScene")
        {
            verticalMove.Disable();
        }
        else
        {
            //if in a top-down level, player shouldn't be able to jump
            jump.Disable();
        }
    }

    //Restore the player's last known position in the current scene
    public void RestorePosition()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        float loadX = PlayerPrefs.GetFloat(currentScene + "_PlayerX", transform.position.x);
        float loadY = PlayerPrefs.GetFloat(currentScene + "_PlayerY", transform.position.y);
        transform.position = new Vector3(loadX, loadY, transform.position.z);
    }

    public void SavePosition()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetFloat(currentScene + "_PlayerX", transform.position.x);
        PlayerPrefs.SetFloat(currentScene + "_PlayerY", transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {

        if (IsGrounded())
        {
            lastTimeGrounded = Time.time;
            jumpsLeft = 0;
            //Debug.Log("is grounded");
        }



        //animator code
        float moveDelta = 0.3f;
        _animator.SetBool("Moving", (Mathf.Abs(_rbody.linearVelocityX) >= 0.005f));
        if(SceneName != "MoleHoleScene")
        {
            
            _animator.SetBool("Jumping", _rbody.linearVelocityY >= moveDelta);
            _animator.SetBool("Falling", _rbody.linearVelocityY <= -moveDelta);
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

    //flips the player's sprite
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }



    private void FixedUpdate()
    {
        //basic player movement
        _rbody.linearVelocityX = horizontalDirection * moveSpeed;
        if(verticalMove.enabled)
        {
            _rbody.linearVelocityY = verticalDirection * moveSpeed;
        }
    }




    //Input action methods:
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
    
    //used for jumping
    private bool WasGrounded()
    {
        //is the difference from now and last time grounded insignificant
        return (Time.time - lastTimeGrounded <= fallAllowance);
    }

    //checks if the player is touching the ground in order to determine if the player can jump
    private bool IsGrounded()
    {
        Debug.Log("Grounded");
        Vector3 pos = transform.position;
        RaycastHit2D hit1 = Physics2D.Raycast(new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), Vector2.down, 1f, groundLayers);  //add or subtract half a unit to check both sides of the player
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z), Vector2.down, 1f, groundLayers);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, groundLayers);
        //lastTimeJumped = 0;
        bool grounded = hit3.collider != null || hit2.collider != null || hit1.collider != null;
        if (grounded){
            _animator.SetBool("Falling", false);
        }
        return (grounded);
    }
    
    //used for going through enterences 
    private void OnInteract(InputValue value)
    {
        Debug.Log("interacted");
        if(buttonName != null)
        {
            //Remember player position before changing scenes
            SavePosition();

            //Load the scene
            SceneManager.LoadScene(buttonName);
        }
    }






    //checks for enemy collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die(collision.gameObject.name);
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
        //allowing player to move up the ladder
        if (collision.gameObject.CompareTag("Ladder"))
        {
            _rbody.gravityScale = 4;
            verticalMove.Enable();
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //disabling vertical move on exiting a ladder
        if (collision.gameObject.CompareTag("Ladder"))
        {
            _rbody.gravityScale = initialGScale;
            verticalMove.Disable();
        }
        else if (collision.gameObject.CompareTag("button"))
        {
            buttonName = null;
        }
    }


    //logic upon player death
    public void Die(string reason)
    {
        LockMovement();
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        _deathParticles.Play();
        _hudManager.DisplayDeathOverlay(reason);
    }

    public void LockMovement()
    {
        InputAction move = playerInput.actions["Move"];
        move.Disable();
        jump.Disable();
        verticalMove.Disable();
        horizontalDirection = 0;
        verticalDirection = 0;
    }

}
