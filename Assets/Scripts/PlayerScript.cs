using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerScript : MonoBehaviour
{
    //public float jumpStrength;
    public float moveSpeed;
    float horizontalDirection = 0;
    float verticalDirection = 0;
    string sceneName;
    float initialGScale;

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

    Rigidbody2D _rbody;
    SpriteRenderer _spriteRenderer;
    ParticleSystem _deathParticles;
    HudManagerScript _hudManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _deathParticles = GetComponent<ParticleSystem>();
        initialGScale = _rbody.gravityScale;  //taking the initial gravity scale so it can be edited freely in the unity editor instead of changing once the player leaves a ladder
        //Debug.Log(initialGScale);

        playerInput = GetComponent<PlayerInput>();
        verticalMove = playerInput.actions["VerticalMove"];
        jump = playerInput.actions["Jump"];
        jumpCooldown = fallAllowance;


        sceneName = SceneManager.GetActiveScene().name;
        if(sceneName != "MoleHoleScene") //or the rabbit hole scene
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
        
    }

    private void FixedUpdate()
    {
        _rbody.linearVelocityX = horizontalDirection * moveSpeed;
        if(verticalMove.enabled)
        {
            _rbody.linearVelocityY = verticalDirection * moveSpeed;
        }
    }

    void OnMove(InputValue value)
    {
        float direction = value.Get<float>();
        horizontalDirection = direction;
    }

    //this won't work if the inputaction is disabled
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
        return (hit3.collider != null || hit2.collider != null || hit1.collider != null);
    }


    
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

    private void OnTriggerEnter2D(Collider2D collision)         
    {
        
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("MoleWall"))
        {
            Die();
        }
        else if (collision.gameObject.CompareTag("button"))
        {
            buttonName = collision.gameObject.name;
            Debug.Log(buttonName);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            _rbody.gravityScale = 4;
            verticalMove.Enable();
            //jump.Disable();
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            _rbody.gravityScale = initialGScale;
            verticalMove.Disable();
            //jump.Enable();
        }
        else if (collision.gameObject.CompareTag("button"))
        {
            buttonName = null;
            Debug.Log("null");
        }
    }

    public void Die()
    {
        LockMovement();
        _spriteRenderer.enabled = false;
        _deathParticles.Play();
        Invoke("ReturnToMap", 3f);
    }

    public void ReturnToMap()
    {
        SceneManager.LoadScene("MapScene");
    }

    public void LockMovement()
    {
        //_rbody.gravityScale = 0;
        InputAction move = playerInput.actions["Move"];
        move.Disable();
        jump.Disable();
        verticalMove.Disable();
        horizontalDirection = 0;
        verticalDirection = 0;
    }

}
