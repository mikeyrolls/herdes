/**
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CharInfo : MonoBehaviour {

    public GameObject charInfoScreen;
    public Image heroImage;
    public Hero hero;

    [SerializeField] TMP_Text heroName;
    [SerializeField] TMP_Text statText;
    [SerializeField] TMP_Text specialSkill;

    [SerializeField] Transform effectsContainer;
    [SerializeField] CharInfoEffectRow effectRowPrefab;

    bool isLoaded = false;

    public void Unload() {
        isLoaded = false;
    }

    void Load() {
        hero = GameManager.Instance.hero;

        heroName.text = hero.nameStr;
        heroImage.sprite = hero.spriteSet.idle; //todo change
        specialSkill.text = hero.specialDescription;
    }

    private void UpdateCharInfo() {
        // stats
        statText.text = hero.currMaxHP + "\n";
        if (hero.currMinDMG == hero.currMaxDMG)
            statText.text += hero.currMinDMG + "\n";
        else 
            statText.text += hero.currMinDMG + " - " + hero.currMaxDMG + "\n";
        statText.text += hero.currDef + "\n";
        statText.text += hero.currDodge + "\n";
        statText.text += hero.currAcc + "";

        //effects
        UpdateEffects();
    }

    private void UpdateEffects() {
        foreach (Transform child in effectsContainer)
            Destroy(child.gameObject);

        foreach (Effect effect in hero.effectList.effects) {
            CharInfoEffectRow row = Instantiate(effectRowPrefab, effectsContainer);
            row.Init(effect);
        }
    }

    public void OpenCharInfo() {
        if (!isLoaded) Load();
        charInfoScreen.SetActive(true);
        UpdateCharInfo();
    }

    public void CloseCharInfo() {
        charInfoScreen.SetActive(false);
    }
}