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

    //audio controlling variables
    public float maxVolume = 1f;
    public float minDistance = 3f;
    public float maxDistance = 9f;
    public float minY;
    AudioSource audioSource;

    public GameObject player;

    public LayerMask birdWall;
    Vector2 home;
    Vector2 _moveDirection;
    bool facingRight = true;
    BirdState currentState = BirdState.Patrolling;
    Rigidbody2D _rbody;
    AudioSource _audioSource;


    void Start()
    {
        //set the bird default position
        home = transform.position;
        _rbody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _moveDirection = Vector2.right; // Initial move direction
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            ReturnHome();
            return;
        }

        //change the current state based on how close the player is
        switch (currentState)
        {
            case BirdState.Patrolling:
                Patrol();
                if (Vector2.Distance(transform.position, player.transform.position) < aggroDistance)
                {
                    currentState = BirdState.Aggressive;
                    _audioSource.PlayOneShot(screech);
                }
                break;

            case BirdState.Aggressive:
                ChasePlayer();
                if (Vector2.Distance(transform.position, player.transform.position) > followDistance ||
                    transform.position.y < minY
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



        //audio distance controlling

        float dist = Vector3.Distance(player.transform.position, transform.position);

        if (dist <= minDistance)
        {
            audioSource.volume = maxVolume;
        }
        else if (dist >= maxDistance)
        {
            audioSource.volume = 0f;
        }
        else
        {
            float t = (dist - minDistance) / (maxDistance - minDistance);
            audioSource.volume = Mathf.Lerp(maxVolume, 0f, t);
        }


    }

    private void FixedUpdate()
    {
        //change the bird's behaviour based on the current state
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
