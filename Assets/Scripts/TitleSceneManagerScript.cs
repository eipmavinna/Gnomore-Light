using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManagerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnStartButtonClicked()
    {
        PlayerPrefs.SetInt("TotalBugs", 1);
        //Debug.Log("Total: " + PlayerPrefs.GetInt("TotalBugs").ToString());
        SceneManager.LoadScene("TutorialScene");
    }
    public void OnExitButtonClicked()
    {
        Application.Quit(); //Is this correct?
    }
}
