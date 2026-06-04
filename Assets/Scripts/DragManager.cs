using UnityEngine;
using UnityEngine.UI;

public class DragManager : MonoBehaviour
{
    public static DragManager Instance { get; private set; }

    [SerializeField] Image ghostImage;

    GameObject source;
    Image sourceImage;
    Color originalColor;

    void Awake() {
        Instance = this;
        ghostImage.enabled = false;
    }

    void Update() {
        if (ghostImage.enabled) {
            ghostImage.transform.position = Input.mousePosition;
        }
    }

    public void BeginDrag(GameObject sourceObject, Sprite sprite) {
        source = sourceObject;
        sourceImage = sourceObject.GetComponent<Image>();
        originalColor = sourceImage.color;

        ghostImage.sprite = sprite;
        ghostImage.enabled = true;

        if (sourceImage != null)
            SetOpacity(sourceImage, 0.5f);
    }

    public void EndDrag(bool cancelled) {
        if (cancelled) Debug.Log("end drag cancelled true");
        else Debug.Log("end drag cancelled false");

        ghostImage.enabled = false;

        if (sourceImage != null)
            sourceImage.color = cancelled ? originalColor : Color.clear;

        source = null;
        sourceImage = null;
    }

    void SetOpacity(Image image, float alpha) {
        Color c = image.color;
        c.a = alpha;

        float grey = (c.r + c.g + c.b) / 3f;
        c.r = Mathf.Lerp(grey, c.r, alpha);
        c.g = Mathf.Lerp(grey, c.g, alpha);
        c.b = Mathf.Lerp(grey, c.b, alpha);


        image.color = c;
    }
}