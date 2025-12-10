using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CameraScript : MonoBehaviour
{
    string sceneName;
    public GameObject player;
    public float optionalLowestStopX = -99999;
    public float stopX;
    public float optionalLowestStopY = -99999;
    public float stopY;
    public float cameraZoom = -30;

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
        float cameraY = player.transform.position.y;
        float cameraX = player.transform.position.x;
        if(player.transform.position.y < optionalLowestStopY)
        {
            cameraY = stopY;
        }
        if (player.transform.position.x < optionalLowestStopX)
        {
            cameraX = stopX;
        }
        transform.position = new Vector3(cameraX,cameraY + offset, -30);

    }
}
