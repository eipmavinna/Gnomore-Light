using UnityEngine;

public class MapSceneManagerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetInt("InLevel", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadWoodsLevel()
    {
        PlayerPrefs.SetInt("TotalBugs", 10);
        UnityEngine.SceneManagement.SceneManager.LoadScene("TheWoodsScene");
    }

    public void LoadGrasslandsLevel()
    {
        PlayerPrefs.SetInt("TotalBugs", 10);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Grasslands");
    }
}
