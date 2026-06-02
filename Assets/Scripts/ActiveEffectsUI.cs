using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ActiveEffectsUI : MonoBehaviour {
    [SerializeField] private EffectSpriteDB effectSpriteDB;
    [SerializeField] private Transform container;
    [SerializeField] private Image iconPrefab;

    private List<Image> spawnedIcons = new List<Image>();

    public void Refresh() {
        // Clear existing icons
        foreach (var icon in spawnedIcons)
            Destroy(icon.gameObject);
        spawnedIcons.Clear();

        // Spawn one icon per active effect
        foreach (var effect in GameManager.Instance.hero.effectList.effects) {
            Sprite sprite = effectSpriteDB.Get(effect.name);
            if (sprite == null) continue;

            Image icon = Instantiate(iconPrefab, container);
            icon.sprite = sprite;
            spawnedIcons.Add(icon);
        }
    }
}