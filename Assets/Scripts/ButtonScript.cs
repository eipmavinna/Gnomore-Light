using UnityEngine;
using TMPro;

public class ButtonScript : MonoBehaviour
{
    public GameObject buttonIconSprite;
    public bool isTmpText;
    public TMP_Text text;
    public string btnText;
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
        if (!isTmpText)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //make image visible
                buttonIconSprite.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            }
        }
        else
        {
            text.text = btnText;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isTmpText)
        {
            //make image invisible when player leaves
            buttonIconSprite.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);

        }
        else
        {
            text.text = "";
        }

    }
}
