using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class MapButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private SpriteRenderer spriteRenderer;
    public string nextScene;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        spriteRenderer.color = new Color32(82, 54, 27,255);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        spriteRenderer.color = new Color32(82, 54, 27, 0);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(nextScene);
    }
}
