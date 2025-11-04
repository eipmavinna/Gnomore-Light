using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent (typeof(Rigidbody2D))]
public class BoatScript : MonoBehaviour
{
    Rigidbody2D _rbody;
    //public Player player;
    public GameObject barrier;

    bool moving;
    public float finalX;
    public float speed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) //&& _rbody.linearVelocity.magnitude < 7)
        {
            //_rbody.AddForce(Vector2.right * speed);
            _rbody.linearVelocity = Vector2.right * speed;
        }
        else
        {
            _rbody.linearVelocity = _rbody.linearVelocity.normalized * 3f;
        }
        if (transform.position.x > finalX)
        {
            //SceneManager.LoadScene("MapScene");
            Debug.Log("loading next scene...");
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
            player.LockMovement();
            Destroy(barrier);
            moving = true;

            Invoke("ChangeScene", 5);
            //TODO: stop the timer so the day doesn't end while on the boat
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("MapScene");
    }

}
