/**
 * EffectList and Effect classes
 */

using System.Collections.Generic;
using UnityEngine;

public class EffectList {
    Creature owner;
    public List<Effect> effects = new List<Effect> {};
    private int currId = 0;

    public int LeftRingEffectId;
    public int RightRingEffectId;

    public EffectList(Creature owner) {
        this.owner = owner;
    }

    public void RemoveDebuffs() {
        effects.RemoveAll(x => x.isDebuff);
    }

    public void AddEffectDurationValue(EffectName effectName, int duration, int value, int index = -1) {
        Effect effect = new Effect(effectName, duration, value, ++currId);
        effects.Add(effect);
        GameManager.Instance.hero.RecalculateStats();
        UIManager.Instance.RefreshHUD();
        GameManager.Instance.hero.sceneObject.ShowFloatingText(effect.received, effect.messageType);

        if (index == 7) LeftRingEffectId = currId;
        if (index == 8) RightRingEffectId = currId;
    }

    // public void DebugPrintList() {
    //     string s = "";
    //     foreach (var effect in effects) {
    //         s += ("[id:" + effect.id + ", eff:" + effect.name + "], ");
    //     }
    //     Debug.Log(s);
    // }

    public void RemoveEffectAtIndex(int index) {
        int id = 0;
        if (index == 7) id = LeftRingEffectId;
        if (index == 8) id = RightRingEffectId;
        RemoveEffect(id);
    }

    public void RemoveEffect(int id) {
        effects.RemoveAll(x => x.id == id);
        GameManager.Instance.hero.RecalculateStats();
    }

    public void ApplyEffectsFromList() {
        foreach (Effect effect in effects) {
            if (!owner.IsAlive()) break;
            effect.Apply();
        }
        effects.RemoveAll(x => x.duration == 0);
        GameManager.Instance.hero.RecalculateStats();
        UIManager.Instance.RefreshHUD();
    }

    public void CalculateEffects() {
        foreach (Effect effect in effects) {
            effect.Calculate();
        }
    }

}

public class Effect {

    public EffectName name;
    public int duration;
    public bool isDebuff;
    
    public StatType stat;
    public int value;
    public int id;

    public string received;
    public FloatingTextType messageType;

    public void Calculate() {
        if(stat == StatType.CurrHP) {
            // only when applying
        } else {
            if (isDebuff) {
                GameManager.Instance.hero.IncreaseStatTemporary(-value, stat);
            } else {
                GameManager.Instance.hero.IncreaseStatTemporary(value, stat);
            }
        }
    }

    public void Apply() {
        Calculate();
        if(stat == StatType.CurrHP) {
            if (isDebuff) {
                if(name == EffectName.Bleed || name == EffectName.BleedBig) {
                    GameManager.Instance.hero.TakeDotDmg(value, FloatingTextType.Bleed);
                } else {
                    GameManager.Instance.hero.TakeDotDmg(value, FloatingTextType.Poison);
                }
                
            } else {
                GameManager.Instance.hero.Heal(value);
            }
        }
        if(duration > 0)
            duration--;
    }

    public Effect(EffectName effectName, int duration, int value, int id) {
        if (EffectDB.effects.TryGetValue(effectName, out var data)) {
            name = effectName;
            this.duration = duration;
            isDebuff = data.isDebuff;
            stat = data.stat;
            this.value = value;
            this.id = id;

            this.received = data.received;
            this.messageType = data.messageType;
        } else {
            Debug.LogError($"Effect '{effectName}' not found in database!");
        }
    }

}