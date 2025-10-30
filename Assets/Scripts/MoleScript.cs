using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class MoleScript : MonoBehaviour
{
    //
    Rigidbody2D _rbody;
    public float speed;

    public float startingVal;
    public float endingVal;
    public bool vertical;



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
        if (!vertical)
        {
            if (transform.position.x > endingVal)
            {
                Vector2 initialPosition = new Vector2(startingVal, transform.position.y);
                _rbody.position = initialPosition;
            }
            else
            {
                transform.position += (Vector3)(Vector3.right * speed * Time.deltaTime);

            }
        }
        else
        {
            if (transform.position.y < endingVal)
            {
                Vector2 initialPosition = new Vector2(transform.position.x, startingVal);
                _rbody.position = initialPosition;
            }
            else
            {
                transform.position += (Vector3)(Vector3.down * speed * Time.deltaTime);

            }
        }

    }

    //TODO: Implement
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if it's a moleWall, delete the box collider/deactivate
        if (collision.gameObject.CompareTag("MoleWall"))
        {
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //if moleWall, put box collider back
        if (collision.gameObject.CompareTag("MoleWall"))
        {
            collision.gameObject.GetComponent<Collider2D>().enabled = true;
        }
    }
}
