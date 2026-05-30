/**
 * Cursor text visibility and movement
 */

using UnityEngine;

public class CursorTextGO : MonoBehaviour {
    public static CursorTextGO Instance { get; private set; }
    [SerializeField] private GameObject panel;
    [SerializeField] private TMPro.TextMeshProUGUI label;
    // offset so text appears below/beside cursor
    private Vector2 newPos = new(0 , 0);

    private float offsetX = 50;

    private float offsetYabove = 60;
    private float offsetYbelow = -80;

    private float limitX;
    private float limitY = 150;


    void Awake() { 
        Instance = this;
        Hide();
        limitX = Screen.width - 100;
    }

    void Update() {
        if (panel.activeSelf) {
            newPos.x = offsetX + ((Input.mousePosition.x < limitX) ? Input.mousePosition.x : limitX);
            newPos.y = Input.mousePosition.y + ((Input.mousePosition.y > limitY) ? offsetYbelow : offsetYabove);
            transform.position = newPos;
        } 
    }

    public void Show(string text) {
        Debug.Log("here");
        if (string.IsNullOrEmpty(text)) { 
            Hide(); 
            return; 
        }
        label.text = text;
        panel.SetActive(true);
    }

    public void Hide() {
        panel.SetActive(false);
    }
}