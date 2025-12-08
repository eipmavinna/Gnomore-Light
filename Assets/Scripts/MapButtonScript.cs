using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class MapButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image imageComponent;
    private SpriteRenderer spriteRenderer;
    public string nextScene;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        imageComponent = GetComponent<Image>();

    }

    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        spriteRenderer.color = new Color32(82, 54, 27,255);
        imageComponent.color = new Color32(82, 54, 27, 255);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        spriteRenderer.color = new Color32(82, 54, 27, 0);
        imageComponent.color = new Color32(82, 54, 27, 0);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerPrefs.SetInt("TotalBugs", 10);
        SceneManager.LoadScene(nextScene);

    }
}
