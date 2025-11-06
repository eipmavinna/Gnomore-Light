using NUnit.Framework.Internal.Filters;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent (typeof(Rigidbody2D))]
public class BoatScript : MonoBehaviour
{
    Rigidbody2D _rbody;
    HudManagerScript _hudManager;
    //public Player player;
    public GameObject barrier;

    bool moving;
    public float finalX;
    public float speed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _hudManager = FindAnyObjectByType<HudManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) //&& _rbody.linearVelocity.magnitude < 7)
        {
            //_rbody.AddForce(Vector2.right * speed);
            _rbody.linearVelocity = Vector2.right * speed;
        }
                
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        int bugsCollected = PlayerPrefs.GetInt("BugsCollected", 0);
        int totalBugs = PlayerPrefs.GetInt("TotalBugs", 0);
        if (bugsCollected < totalBugs)
        {
            //TODO: display message
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
            player.LockMovement();
            Destroy(barrier);
            moving = true;

            //stop the timer so the day doesn't end while on the boat
            PlayerPrefs.SetInt("InLevel", 0);
            _hudManager.DisplayLevelCompleteOverlay();

            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //TODO: Hide message
    }

}
