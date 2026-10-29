/**
 * Script for show char info menu
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CharInfo : MonoBehaviour {

    public GameObject charInfoScreen;
    public Hero hero;

    public Image heroImage;
    [SerializeField] Image heroName;
    
    [SerializeField] CharInfoStatRow rowHp;
    [SerializeField] CharInfoStatRow rowAtk;
    [SerializeField] CharInfoStatRow rowDef;
    [SerializeField] CharInfoStatRow rowDodge;
    [SerializeField] CharInfoStatRow rowAcc;

    [SerializeField] TMP_Text specialSkill;

    [SerializeField] Transform effectsContainer;
    [SerializeField] CharInfoEffectRow effectRowPrefab;

    bool isLoaded = false;

    public void Unload() {
        isLoaded = false;
    }

    void Load() {
        hero = GameManager.Instance.hero;

        heroName.sprite = ((HeroSpriteSet)hero.spriteSet).namePic;
        heroImage.sprite = ((HeroSpriteSet)hero.spriteSet).bigPic; //todo change
        specialSkill.text = hero.specialDescription;
    }

    private void UpdateCharInfo() {
        // stats
        rowHp.SetValuesCurrBase(hero.currMaxHP, hero.maxHP);
        rowAtk.SetValuesCurrBase(hero.currMinDMG, hero.minDMG, hero.maxDMG);
        rowDef.SetValuesCurrBase(hero.currDef, hero.def);
        rowDodge.SetValuesCurrBase(hero.currDodge, hero.dodge);
        rowAcc.SetValuesCurrBase(hero.currAcc, hero.acc);

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