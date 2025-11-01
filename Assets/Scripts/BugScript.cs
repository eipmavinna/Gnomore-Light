using UnityEngine;

public class BugScript : MonoBehaviour
{

    public string id;
    HudManagerScript hudManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hudManager = FindAnyObjectByType<HudManagerScript>();
        int collected = PlayerPrefs.GetInt(id, 0);
        if (collected != 0)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetInt(id, 1);
            hudManager.OnBugFound(gameObject);
        }
    }
}
