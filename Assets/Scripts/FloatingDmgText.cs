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
                break;
            case FloatingTextType.Hit:
                color = Color.red;
                break;
            case FloatingTextType.Poison:
                color = new Color(0.05f, 0.55f, 0.3f, 1f);
                break;
            case FloatingTextType.Bleed:
                color = new Color(0.6f, 0f, 0.4f, 1f);
                break;
            case FloatingTextType.Heal:
                color = new Color(0.6f, 1f, 0f, 1f);
                if (message != "Regeneration")
                    message = "+" + message;
                break;
            case FloatingTextType.Debuff:
                color = new Color(0.6f, 0.3f, 1f, 1f);
                break;
            default:
                color = Color.white;
                break;
        }
        int length = message.Length;
        if (length > 3)
            textObject.GetComponent<TextMeshProUGUI>().fontSize = 0.5f;

        textObject.text = message;
        textObject.color = color;

        StartCoroutine(Fade(length));
    }

    private IEnumerator Fade(int length) {
        textObject.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);  //half animation size? todo unify
        textObject.gameObject.SetActive(true);

        float duration = 0.3f;
        float elapsed = 0f;
        Color c = textObject.color;

        yield return new WaitForSeconds(0.6f + length / 10.0f);

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            textObject.color = new Color(c.r, c.g, c.b, Mathf.Lerp(1f, 0f, elapsed / duration));
            yield return null;
        }

        Destroy(gameObject);
    }
}