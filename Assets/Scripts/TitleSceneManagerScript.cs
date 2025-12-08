using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManagerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string bugsLoaded = PlayerPrefs.GetString("BugsLoaded", "");
        string[] bugs = bugsLoaded.Split(';');
        foreach (var item in bugs)
        {
            PlayerPrefs.SetInt(item, 0);
        }
        PlayerPrefs.SetInt("MapTotalBugs", 0);
        PlayerPrefs.SetInt("BugsCollected", 0);
        PlayerPrefs.SetInt("TotalBugs", 1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnStartButtonClicked()
    {
        
        //Debug.Log("Total: " + PlayerPrefs.GetInt("TotalBugs").ToString());
        SceneManager.LoadScene("TutorialScene");
    }
    public void OnExitButtonClicked()
    {
        Application.Quit(); //Is this correct?
    }
}
