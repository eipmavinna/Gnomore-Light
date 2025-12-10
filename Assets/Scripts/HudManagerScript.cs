using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HudManagerScript : MonoBehaviour
{

    public Canvas gameOverlay;
    public TMP_Text bugTracker;
    public GameObject timeTracker;

    public Canvas levelCompleteOverlay;
    public TMP_Text levelCompleteText;
    public TMP_Text deathMessageText;
    public TMP_Text bugsCollectedText;
    public TMP_Text timeRemainingText;

    public Canvas pauseOverlay;

    public float timeLimit;

    PlayerScript playerScript;

    int bugsCollected;
    int totalBugs;
    float timeRemaining;
    float timeBarWidth;
    RectTransform timeBarTransform;
    float lastUpdateTime;
    bool gameOver;
    bool died = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOver = false;
        levelCompleteOverlay.gameObject.SetActive(false);
        pauseOverlay.gameObject.SetActive(false);

        playerScript = FindAnyObjectByType<PlayerScript>();
        lastUpdateTime = Time.time;
        timeBarTransform = timeTracker.GetComponent<RectTransform>();
        timeBarWidth = timeBarTransform.localPosition.x;

        //retrieving game data to show on the current scene
        if (PlayerPrefs.GetInt("InLevel", 0) == 0)
        {
            PlayerPrefs.SetFloat("TimeRemaining", timeLimit);
            PlayerPrefs.SetInt("InLevel", 1);
        } else
        {
            playerScript.RestorePosition();
        }
        
        //retrieving game data
        bugsCollected = PlayerPrefs.GetInt("BugsCollected", 0);
        totalBugs = PlayerPrefs.GetInt("TotalBugs", 0);
        timeRemaining = PlayerPrefs.GetFloat("TimeRemaining", timeLimit);
        UpdateBugTracker();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeRemaining();
    }

    //called if the player completes the level
    public void DisplayLevelCompleteOverlay()
    {
        gameOverlay.gameObject.SetActive(false);
        levelCompleteOverlay.gameObject.SetActive(true);

        //TODO update time remaining and bugs collected
        bugsCollectedText.text = "Bugs Collected: " + bugsCollected;
        timeRemainingText.text = "Time Remaining: " + Mathf.CeilToInt(timeRemaining) + "s";

        string currentLevel = PlayerPrefs.GetString("CurrentLevel", "");

        if (!died)
        {
            int prevBestBugs = PlayerPrefs.GetInt(currentLevel + "BugsBest", 0);
            float prevBestTime = PlayerPrefs.GetFloat(currentLevel + "TimeBest", 0);

            PlayerPrefs.SetInt(currentLevel + "BugsBest", Mathf.Max(prevBestBugs, bugsCollected));
            PlayerPrefs.SetFloat(currentLevel + "TimeBest", Mathf.Min(prevBestTime, timeLimit - timeRemaining));
        }
        Invoke("ReturnToMap", 3.5f);
    }


    //called if player dies
    public void DisplayDeathOverlay(string message)
    {
        if (gameOver)
        {
            return;
        }
        gameOver = true;
        string[] deathMessages = { "You died!", "Wasted" };
        int index = Random.Range(0, deathMessages.Length);

        levelCompleteText.text = deathMessages[index];
        levelCompleteText.color = new Color(0.6981132f, 0.059131f, 0);
        deathMessageText.text = "Cause of Death: " + message;

        died = true;
        DisplayLevelCompleteOverlay();
    }

   
    public void ReturnToMap()
    {
        SceneManager.LoadScene("MapScene");
    }


    //changes playerprefs if player contacts a bug
    public void OnBugFound(GameObject bug)
    {
        Destroy(bug);
        bugsCollected += 1;
        PlayerPrefs.SetInt("BugsCollected", bugsCollected);
        UpdateBugTracker();
    }


    //updates the time during a level
    void UpdateTimeRemaining()
    {
        //Do not update time if not in a level
        if (PlayerPrefs.GetInt("InLevel", 0) == 0)
        {
            return;
        }

        float deltaTime = Time.time - lastUpdateTime;
        timeRemaining -= deltaTime;
        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            playerScript.Die("Darkness");
        }
        lastUpdateTime = Time.time;
        PlayerPrefs.SetFloat("TimeRemaining", timeRemaining);
        Vector3 position = timeBarTransform.localPosition;
        timeBarTransform.localPosition = new Vector3((timeRemaining/timeLimit) * timeBarWidth, position.y, position.z);
    }

    void UpdateBugTracker()
    {
        bugTracker.text = "Bugs: " + bugsCollected + " / " + totalBugs;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseOverlay.gameObject.SetActive(true);
        gameOverlay.gameObject.SetActive(false);
    }

    public void Resume() { 
        Time.timeScale = 1;
        pauseOverlay.gameObject.SetActive(false);
        gameOverlay.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
