using TMPro;
using UnityEngine;

public class HudManagerScript : MonoBehaviour
{
    public TMP_Text bugTracker;
    public GameObject timeTracker;
    public float timeLimit;

    int bugsCollected;
    int totalBugs;
    float timeRemaining;
    float timeBarWidth;
    float lastUpdateTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastUpdateTime = Time.time;
        timeBarWidth = timeTracker.transform.localScale.x;

        if (PlayerPrefs.GetInt("InLevel", 0) == 0)
        {
            PlayerPrefs.SetFloat("TimeRemaining", timeLimit);
        }
        PlayerPrefs.SetInt("InLevel", 1);
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

    public void OnBugFound(GameObject bug)
    {
        Destroy(bug);
        bugsCollected += 1;
        PlayerPrefs.SetInt("BugsCollected", bugsCollected);
        UpdateBugTracker();
    }

    void UpdateTimeRemaining()
    {
        float deltaTime = Time.time - lastUpdateTime;
        timeRemaining -= deltaTime;
        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
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
