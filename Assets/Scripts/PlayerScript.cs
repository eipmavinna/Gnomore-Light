using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    public float jumpStrength;
    public float moveSpeed;
    float horizontalDirection = 0;
    float verticalDirection = 0;
    string sceneName;
    float initialGScale;

    private PlayerInput playerInput;
    private InputAction verticalMove;



    Rigidbody2D _rbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        initialGScale = _rbody.gravityScale;  //taking the initial gravity scale so it can be edited freely in the unity editor instead of changing once the player leaves a ladder
        Debug.Log(initialGScale);

        playerInput = GetComponent<PlayerInput>();
        verticalMove = playerInput.actions["VerticalMove"];

        sceneName = SceneManager.GetActiveScene().name;
        if(sceneName != "MoleHoleScene") //or the rabbit hole scene
        {
            verticalMove.Disable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _rbody.linearVelocityX = horizontalDirection * moveSpeed;
        _rbody.linearVelocityY = verticalDirection * moveSpeed;
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

    //with the ladders, set gravity lower when on them? and disable the up action when off?
    //in fixed update could check for on ladder

    void OnJump(InputValue value)
    {
        _rbody.AddForce(Vector2.up * jumpStrength);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("MoleWall"))
        {
            //call destroy player 
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            _rbody.gravityScale = 4;
            verticalMove.Enable();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            _rbody.gravityScale = initialGScale;
            verticalMove.Disable();
        }
    }


}
