using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManagerScript : MonoBehaviour
{
    
    void Start()
    {
        //reset the bugs in the level
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
        
        SceneManager.LoadScene("TutorialScene");
    }
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
