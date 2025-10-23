using Unity.VisualScripting;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        //transform.position = new Vector3(player.transform.position.x,0,-10);
        //^^ works for side-to-side levels, but when going up the tree it keeps the camera too low

        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, - 10);
    }
}
