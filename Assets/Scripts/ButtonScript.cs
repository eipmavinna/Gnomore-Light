using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public GameObject buttonIconSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("click");
            buttonIconSprite.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }
        //Debug.Log("sprite visible");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("sprite invisible");
        buttonIconSprite.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);

    }
}
