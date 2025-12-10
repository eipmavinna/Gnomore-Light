using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class BirdScript : MonoBehaviour
{
    enum BirdState
    {
        Patrolling,
        Aggressive,
        ReturningHome
    }

    public float aggroDistance = 5.0f; // Distance at which the bird becomes aggressive
    public float followDistance = 7.0f; // Distance at which the bird stops following the player
    public float patrolSpeed = 2.0f; // Speed while patrolling
    public float chaseSpeed = 4.0f; // Speed while chasing the player
    public AudioClip screech;
    public GameObject player;

    public LayerMask birdWall;
    Vector2 home;
    Vector2 _moveDirection;
    bool facingRight = true;
    BirdState currentState = BirdState.Patrolling;
    Rigidbody2D _rbody;
    AudioSource _audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        home = transform.position;
        _rbody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _moveDirection = Vector2.right; // Initial move direction
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case BirdState.Patrolling:
                Patrol();
                if (Vector2.Distance(home, player.transform.position) < aggroDistance)
                {
                    currentState = BirdState.Aggressive;
                    _audioSource.PlayOneShot(screech);
                }
                break;
            case BirdState.Aggressive:
                ChasePlayer();
                if (Vector2.Distance(transform.position, home) > followDistance ||
                    Vector2.Distance(transform.position, player.transform.position) > followDistance

                    )
                {
                    currentState = BirdState.ReturningHome;
                }
                break;
            case BirdState.ReturningHome:
                ReturnHome();
                if (Vector2.Distance(transform.position, home) < 0.1f)
                {
                    currentState = BirdState.Patrolling;
                    _moveDirection = Vector2.right; // Reset move direction
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case BirdState.Patrolling:
                _rbody.linearVelocity = _moveDirection.normalized * patrolSpeed;
                break;
            case BirdState.Aggressive:
                _rbody.linearVelocity = _moveDirection.normalized * chaseSpeed;
                break;
            case BirdState.ReturningHome:
                _rbody.linearVelocity = _moveDirection.normalized * chaseSpeed;
                break;
        }
        if (_rbody.linearVelocityX > 0 && !facingRight)
        {
            Flip();
        }
        else if (_rbody.linearVelocity.x < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Patrol()
    {
        //Change direction if hitting a wall
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + _moveDirection, _moveDirection, 0.1f, birdWall);
        if (hit.collider != null)
        {
            _moveDirection = -_moveDirection;
            Flip();
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        _moveDirection = direction;
    }

    void ReturnHome()
    {
        Vector2 direction = (home - (Vector2)transform.position).normalized;
        _moveDirection = direction;
    }
}
