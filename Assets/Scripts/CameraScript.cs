using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CameraScript : MonoBehaviour
{
    bool isTreeScene = false;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (SceneManager.GetActiveScene().name == "TreeScene")
        {
            isTreeScene = true;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        //transform.position = new Vector3(player.transform.position.x,0,-10);
        //^^ works for side-to-side levels, but when going up the tree it keeps the camera too low
        
        //if (isTreeScene)
        //{
        //    float pY = player.transform.position.y;
        //    if (pY < -8)
        //    {
        //        transform.position = new Vector3(player.transform.position.x, -8, -10);
        //    }
        //}
        //else
        //{
        //    transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        //}
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);

    }
}
