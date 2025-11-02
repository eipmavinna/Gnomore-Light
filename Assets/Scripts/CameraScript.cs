using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CameraScript : MonoBehaviour
{
    string sceneName;
    public GameObject player;

    public float offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        sceneName = SceneManager.GetActiveScene().name;
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        //transform.position = new Vector3(player.transform.position.x,0,-10);
        //^^ works for side-to-side levels, but when going up the tree it keeps the camera too low

        if (sceneName == "TreeScene" && player.transform.position.y < -8)
        {
            //float pY = player.transform.position.y;
            //if (pY < -8)
            //{
            transform.position = new Vector3(player.transform.position.x, -8, -10);
            //}
        }
        else if (sceneName == "TheWoodsScene")
        {
            //Debug.Log("woods scene");
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + offset, -10);
        }
        else
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);

        }

    }
}
