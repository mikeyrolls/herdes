/**
 * Sprite holder for effect icons (combat only)
 */

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EffectSpriteDB", menuName = "Game/Effect Sprite DB")]
public class EffectSpriteDB : ScriptableObject {
    public Sprite hpInc;
    public Sprite atkInc;
    public Sprite accInc;
    public Sprite dodgeInc;
    public Sprite defInc;
    public Sprite hpDec;
    public Sprite atkDec;
    public Sprite accDec;
    public Sprite dodgeDec;
    public Sprite defDec;
    public Sprite heal;
    public Sprite poison;
    public Sprite poisonBig;

    private Dictionary<EffectName, Sprite> effectSpritesDict;
    public Dictionary<EffectName, Sprite> EffectSpritesDict {
        get {
            if (effectSpritesDict == null) BuildEffectSpritesDict();
            return effectSpritesDict;
        }
    }

    private void BuildEffectSpritesDict() {
        effectSpritesDict = new Dictionary<EffectName, Sprite> {
            [EffectName.HpInc]      = hpInc,
            [EffectName.AtkInc]     = atkInc,
            [EffectName.AccInc]     = accInc,
            [EffectName.DodgeInc]   = dodgeInc,
            [EffectName.DefInc]     = defInc,
            [EffectName.HpDec]      = hpDec,
            [EffectName.AtkDec]     = atkDec,
            [EffectName.AccDec]     = accDec,
            [EffectName.DodgeDec]   = dodgeDec,
            [EffectName.DefDec]     = defDec,
            [EffectName.Heal]       = heal,
            [EffectName.Poison]     = poison,
            [EffectName.PoisonBig]  = poisonBig,
        };
    }

    public Sprite Get(EffectName effect) => EffectSpritesDict.TryGetValue(effect, out var sprite) ? sprite : null;
}