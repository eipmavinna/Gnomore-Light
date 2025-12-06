using UnityEngine;
using TMPro;
public class MapSceneManagerScript : MonoBehaviour
{
    public TMP_Text bugCounter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bugCounter.text = "Bugs collected: " + PlayerPrefs.GetInt("MapTotalBugs").ToString();
        string bugsLoaded = PlayerPrefs.GetString("BugsLoaded", "");
        string[] bugs = bugsLoaded.Split(';');
        foreach (var item in bugs)
        {
            PlayerPrefs.SetInt(item, 0);
        }

        PlayerPrefs.SetInt("BugsCollected", 0);
        PlayerPrefs.SetInt("InLevel", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void LoadWoodsLevel()
    //{
    //    int total = PlayerPrefs.GetInt("TotalBugs");
    //    PlayerPrefs.SetInt("TotalBugs", 10);
    //    Debug.Log( "total before: " + total.ToString() + " after: "  + PlayerPrefs.GetInt("TotalBugs").ToString());

    //    UnityEngine.SceneManagement.SceneManager.LoadScene("TheWoodsScene");
    //}

//    public void LoadGrasslandsLevel()
//    {
//        int total = PlayerPrefs.GetInt("TotalBugs");
//        PlayerPrefs.SetInt("TotalBugs", 10);
//        Debug.Log("total before: " + total.ToString() + " after: " + PlayerPrefs.GetInt("TotalBugs").ToString());
//        UnityEngine.SceneManagement.SceneManager.LoadScene("Grasslands");
//    }
}
