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
        //different camera settings for different levels
        if (sceneName == "TreeScene")
        {
            if(player.transform.position.y < -8)
            {
                transform.position = new Vector3(player.transform.position.x, -8, -10);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y + offset, -10);
            }
        }
        else if (sceneName == "TheWoodsScene")
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + offset, -10);
        }
        else
        {
            //default camera movement: following player
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);

        }

    }
}
