using UnityEngine;
using TMPro;
using System.Collections;
using Unity.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class MapSceneManagerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text headerText;
    public TMP_Text instructionsText;
    public TMP_Text statsHeader;
    public TMP_Text bugsRequired;
    public TMP_Text mostBugsCollected;
    public TMP_Text bestTime;
    public GameObject playButton;


    LevelData selectedLevel;
    void Start()
    {
        //Add levels up here (id [Scene Name], name, bugs required)
        LevelData.addLevel("TheWoodsScene","The Woods",10);


        showInstructions(true);

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

    void showInstructions(bool show)
    {
        headerText.text = "Controls";
        instructionsText.enabled = show;
        bugsRequired.enabled = !show;
        statsHeader.enabled = !show;
        mostBugsCollected.enabled = !show;
        bestTime.enabled = !show;
        playButton.SetActive(!show);
    } 
        

    public void LoadLevelInfo(string level)
    {
        selectedLevel = LevelData.getLevel(level);
        if (selectedLevel == null)
        {
            headerText.text = "Controls";
            showInstructions(true);
            return;
        }

        showInstructions(false);
        headerText.text = selectedLevel.GetName();
        bugsRequired.text = "Bugs Required: " + selectedLevel.GetBugsRequired();

        mostBugsCollected.text = "Highest Collected: " + selectedLevel.GetMostBugs();
        bestTime.text = "Best Time: " + selectedLevel.GetBestTime();
    }

    public void PlaySelectedLevel()
    {
        if (selectedLevel == null) {
            return;
        }

        selectedLevel.LoadLevel();
    }


    public class LevelData
    {
        private static Dictionary<string, LevelData> levels = new Dictionary<string, LevelData>();

        private int bugsRequired;
        private string levelId;
        private string levelName;

        public static void addLevel(string levelId, string name, int bugsRequired)
        {
            LevelData data = new LevelData(levelId, name, bugsRequired);
            levels[levelId] = data;
        }

        public static LevelData getLevel(string levelId)
        {
            return levels[levelId];
        }

        private LevelData(string levelId, string name, int bugsRequired)
        {
            this.levelId = levelId;
            this.bugsRequired = bugsRequired;
            this.levelName = name;
        }

        public string GetLevelId()
        {
            return levelId;
        }

        public int GetBugsRequired()
        {
            return bugsRequired;
        }

        public string GetName()
        {
            return levelName;
        }

        public int GetMostBugs()
        {
            return PlayerPrefs.GetInt(levelId + "BugsBest", 0);
        }

        public string GetBestTime()
        {
            return FormatTime(PlayerPrefs.GetFloat(levelId + "TimeBest", 0));
        }

        public void LoadLevel()
        {
            PlayerPrefs.SetInt("TotalBugs", bugsRequired);
            PlayerPrefs.SetString("CurrentLevel", levelId);
            SceneManager.LoadScene(levelId);
        }

        string FormatTime(float time)
        {
            float minutes = Mathf.Floor(time / 60);
            float seconds = time % 60;
            if (seconds < 10)
            {
                return minutes + $":0{seconds:F2}";
            }
            return minutes + $":{seconds:F2}";
        }
    }
}
