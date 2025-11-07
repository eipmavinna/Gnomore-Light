using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class SquirrelScript : MonoBehaviour
{
    Rigidbody2D _rbody;
    public float speed;
    public float startX;
    public float startY;
    public float endY;
    public float startPos;
    public bool goingRight;
    Animator _anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        if (goingRight)
        {
            _anim.Play("rightSquirrel");
        }
        else
        {
            _anim.Play("leftSquirrel");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        //squirrels just move from one end to another
        if(transform.position.y < endY)
        {
            Vector2 st = new Vector2(startX, startY);
            _rbody.position = st;
        }
        else
        {
            //if right:... if left:...
            if (goingRight)
            {
                transform.position += (Vector3)(Vector3.right * speed * Time.deltaTime);
            }
            else
            {
                transform.position += (Vector3)(Vector3.left * speed * Time.deltaTime);
            }
        }
    }
}
