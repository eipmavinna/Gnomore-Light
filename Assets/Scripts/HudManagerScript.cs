using TMPro;
using UnityEngine;

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

    public float timeLimit;

    PlayerScript playerScript;

    int bugsCollected;
    int totalBugs;
    float timeRemaining;
    float timeBarWidth;
    float lastUpdateTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelCompleteOverlay.gameObject.SetActive(false);
        playerScript = FindAnyObjectByType<PlayerScript>();
        lastUpdateTime = Time.time;
        timeBarWidth = timeTracker.transform.localScale.x;

        if (PlayerPrefs.GetInt("InLevel", 0) == 0)
        {
            PlayerPrefs.SetFloat("TimeRemaining", timeLimit);
            PlayerPrefs.SetInt("InLevel", 1);
        } else
        {
            playerScript.RestorePosition();
        }
        
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

    public void DisplayLevelCompleteOverlay()
    {
        gameOverlay.gameObject.SetActive(false);
        levelCompleteOverlay.gameObject.SetActive(true);

        //TODO update time remaining and bugs collected
        bugsCollectedText.text = "Bugs Collected: " + bugsCollected;
        timeRemainingText.text = "Time Remaining: " + Mathf.CeilToInt(timeRemaining) + "s";
        Invoke("ReturnToMap", 5f);
    }

    public void DisplayDeathOverlay(string message)
    {
        string[] deathMessages = { "You died!", "Wasted" };
        int index = Random.Range(0, deathMessages.Length);

        levelCompleteText.text = deathMessages[index];
        levelCompleteText.color = new Color(0.6981132f, 0.059131f, 0);
        deathMessageText.text = "Cause of Death: " + message;

        DisplayLevelCompleteOverlay();
    }
    public void ReturnToMap()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapScene");
    }

    public void OnBugFound(GameObject bug)
    {
        Destroy(bug);
        bugsCollected += 1;
        PlayerPrefs.SetInt("BugsCollected", bugsCollected);
        UpdateBugTracker();
    }

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
        timeTracker.transform.localScale = new Vector3((timeRemaining / timeLimit) * timeBarWidth, 1, 1);
    }

    void UpdateBugTracker()
    {
        bugTracker.text = "Bugs: " + bugsCollected + " / " + totalBugs;
    }
}
