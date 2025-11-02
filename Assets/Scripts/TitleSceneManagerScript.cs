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
        SceneManager.LoadScene("MapScene");
    }
    public void OnExitButtonClicked()
    {
        Application.Quit(); //Is this correct?
    }
}
