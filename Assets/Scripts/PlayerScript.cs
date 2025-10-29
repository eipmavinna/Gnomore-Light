using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    public float jumpStrength;
    public float moveSpeed;
    float horizontalDirection = 0;

    Rigidbody2D _rbody;
    HudManagerScript _hudManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _hudManager = FindAnyObjectByType<HudManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bug"))
        {
            _hudManager.OnBugFound(collision.gameObject);
        }
    }

    private void FixedUpdate()
    {
        _rbody.linearVelocityX = horizontalDirection * moveSpeed;
    }

    void OnMove(InputValue value)
    {
        float direction = value.Get<float>();
        horizontalDirection = direction;
    }

    void OnJump(InputValue value)
    {
        _rbody.AddForce(Vector2.up * jumpStrength);
    }
}
