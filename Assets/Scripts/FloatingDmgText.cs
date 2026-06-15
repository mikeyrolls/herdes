/**
 * Floating damage text
 */

using System.Collections;
using UnityEngine;
using TMPro;

public class FloatingDmgText : MonoBehaviour {
    [SerializeField] TMP_Text textObject;

    public void Show(string message, FloatingTextType type) {
        Color color;
        switch(type) {  //Miss, Hit, Poison, Heal, Debuff
            case FloatingTextType.Miss:
                color = new Color(0.8f, 0.8f, 0.8f, 1f);
                textObject.GetComponent<TextMeshProUGUI>().fontSize = 0.6f;
                break;
            case FloatingTextType.Hit:
                color = Color.red;
                break;
            case FloatingTextType.Poison:
                color = new Color(0.2f, 0.9f, 0f, 1f);
                break;
            case FloatingTextType.Heal:
                color = new Color(0.6f, 1f, 0f, 1f);
                message = "-" + message;
                break;
            case FloatingTextType.Debuff:
                color = new Color(0.8f, 0.5f, 1f, 1f);
                break;
            default:
                color = Color.white;
                break;
        }

        textObject.text = message;
        textObject.color = color;

        StartCoroutine(Fade());
    }

    private IEnumerator Fade() {
        textObject.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        textObject.gameObject.SetActive(true);

        float duration = 0.3f;
        float elapsed = 0f;
        Color c = textObject.color;

        yield return new WaitForSeconds(0.7f);

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            textObject.color = new Color(c.r, c.g, c.b, Mathf.Lerp(1f, 0f, elapsed / duration));
            yield return null;
        }

        Destroy(gameObject);
    }
}