using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    public float jumpStrength;
    public float moveSpeed;
    float horizontalDirection = 0;

    Rigidbody2D _rbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
