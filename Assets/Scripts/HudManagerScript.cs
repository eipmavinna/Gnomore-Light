using TMPro;
using UnityEngine;

public class HudManagerScript : MonoBehaviour
{

    public TMP_Text bugTracker;

    int bugsCollected;
    int totalBugs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bugsCollected = PlayerPrefs.GetInt("BugsCollected", 0);
        totalBugs = PlayerPrefs.GetInt("TotalBugs", 0);
        UpdateBugTracker();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBugFound(GameObject bug)
    {
        Destroy(bug);
        bugsCollected += 1;
        PlayerPrefs.SetInt("BugsCollected", bugsCollected);
        UpdateBugTracker();
    }

    void UpdateBugTracker()
    {
        bugTracker.text = "Bugs: " + bugsCollected + " / " + totalBugs;
    }
}
