using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Splines;
using UnityEngine.U2D;

public class AntScript : MonoBehaviour
{
    //public SplineContainer spline;
    Rigidbody2D _rbody;
    //public float speed = 3;

    float t = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();

        
    }

    Quaternion targetRotation;
    void Update()
    {
        targetRotation = Quaternion.Euler(0, 0, transform.rotation.z);
    }
    private void LateUpdate()
    {
        //Quaternion rot = spline.Evaluate(t);
        //transform.rotation = targetRotation;
        Vector3 rot = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0, 0, rot.z);
    }

    private void FixedUpdate()
    {
        

        //Vector3 pos = spline.EvaluatePosition(t);
        //Quaternion rot = spline.Eval
        //_rbody.MovePosition(pos);
        //_rbody.MoveRotation(rot);
        //Spline splineCurve = spline.Spline;
        //Vector3 targetPos = splineCurve.EvaluatePosition(t);

        //Vector2 dir = (targetPos - transform.position).normalized;
        //_rbody.MovePosition(_rbody.position + dir * speed * Time.fixedDeltaTime);
        //t += Time.fixedDeltaTime * speed * 0.1f;
        //t = Mathf.Clamp01(t);
    }

  
}
