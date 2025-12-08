using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreLabelScript : MonoBehaviour
{
    public string levelId;

    TMP_Text m_TextMeshPro;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_TextMeshPro = GetComponent<TMP_Text>();
        m_TextMeshPro.text = "Collected: " + PlayerPrefs.GetInt(levelId, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
