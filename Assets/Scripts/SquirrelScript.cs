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
    public float startY;//change these once I have a rotation
    public float endY;
    public float startPos; //index of the starting position
    public bool goingRight;
    List<Vector2> StartPositions = new List<Vector2> { new Vector2(-20, 17), new Vector2(16, 15) };
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
        //if(startPos == 1)
        //{
        //    goingRight = false;
        //}
        //else
        //{
        //    goingRight = true;
        //}
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
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
