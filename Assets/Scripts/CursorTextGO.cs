using UnityEngine;

public class CursorTextGO : MonoBehaviour {
    public static CursorTextGO Instance { get; private set; }
    [SerializeField] private GameObject panel;
    [SerializeField] private TMPro.TextMeshProUGUI label;
    // offset so text appears below/beside cursor
    private Vector2 offset = new(50, -80);

    void Awake() { Instance = this; Hide(); }

    void Update() {
        if (panel.activeSelf)
            transform.position = (Vector2)Input.mousePosition + offset;
    }

    public void Show(string text) {
        Debug.Log("here");
        if (string.IsNullOrEmpty(text)) { Hide(); return; }
        label.text = text;
        panel.SetActive(true);
    }

    public void Hide() => panel.SetActive(false);
}