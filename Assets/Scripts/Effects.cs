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
        Effect effect = new Effect(effectName, duration, value, ++currId, owner);
        effects.Add(effect);
        owner.RecalculateStats();
        UIManager.Instance.RefreshHUD();
        owner.sceneObject.ShowFloatingText(effect.received, effect.messageType);

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
        owner.RecalculateStats();
    }

    public void ApplyEffectsFromList() {
        foreach (Effect effect in effects) {
            if (!owner.IsAlive()) break;
            effect.Apply();
        }
        effects.RemoveAll(x => x.duration == 0);
        owner.RecalculateStats();
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

    Creature owner;

    public void Calculate() {
        if(stat == StatType.CurrHP) {
            // only when applying
        } else {
            if (isDebuff) {
                owner.IncreaseStatTemporary(-value, stat);
            } else {
                owner.IncreaseStatTemporary(value, stat);
            }
        }
    }

    public void Apply() {
        Calculate();
        if(stat == StatType.CurrHP) {
            if (isDebuff) {
                if(name == EffectName.Bleed || name == EffectName.BleedBig) {
                    owner.TakeDotDmg(value, FloatingTextType.Bleed);
                } else {
                    owner.TakeDotDmg(value, FloatingTextType.Poison);
                }
                
            } else {
                owner.Heal(value);
            }
        }
        if(duration > 0)
            duration--;
    }

    public Effect(EffectName effectName, int duration, int value, int id, Creature owner) {
        if (EffectDB.effects.TryGetValue(effectName, out var data)) {
            name = effectName;
            this.duration = duration;
            isDebuff = data.isDebuff;
            stat = data.stat;
            this.value = value;
            this.id = id;

            this.received = data.received;
            this.messageType = data.messageType;

            this.owner = owner;
        } else {
            Debug.LogError($"Effect '{effectName}' not found in database!");
        }
    }

}