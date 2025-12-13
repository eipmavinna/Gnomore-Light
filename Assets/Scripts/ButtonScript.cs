using UnityEngine;
using TMPro;

public class ButtonScript : MonoBehaviour
{
    public GameObject buttonIconSprite;
    public bool isTmpText;
    public TMP_Text text;
    public string btnText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //show the button image when the player is inside the button trigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isTmpText)
        {
            if (buttonIconSprite != null)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    //make image visible
                    buttonIconSprite.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                }
            }
        }
        else
        {
            text.text = btnText;
        }
    }

    //make the button invisible again when the player leaves the area
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isTmpText)
        {
            if (buttonIconSprite != null)
            {
                //make image invisible when player leaves
                buttonIconSprite.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 0);
            }

        }
        else
        {
            text.text = "";
        }

    }
}
