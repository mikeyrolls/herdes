/**
 * Sprite holder heroes (adding out of combat sprites + cursors)
 */

using UnityEngine;

[CreateAssetMenu(fileName = "HeroSpriteSet", menuName = "Game/Hero Sprite Set")]
public class HeroSpriteSet : FightSpriteSet
{
    public Sprite bigPic;
    public Sprite namePic;

    public Sprite roomIdle;
    public Sprite roomDamaged;
    public Sprite roomDead;
    public Sprite pet;

    public Texture2D normalCursor;
    public Texture2D grabCursor;
    public Texture2D attackCursor;
    public Texture2D specialCursor;
    public Texture2D holdingCursor;
    public Texture2D leaveCursor;

    public Sprite stateIconHp1;
    public Sprite stateIconHp2;
    public Sprite stateIconHp3;
    public Sprite stateIconHp4;

    public Sprite StateIcon(int hpPerc) {
        if(hpPerc > 50) {
            return stateIconHp1;
        } else if (hpPerc > 25) {
            return stateIconHp2;
        } else if (hpPerc > 0) {
            return stateIconHp3;
        } else {
            return stateIconHp4;
        }
    }

    //public AudioClip damagedSound;
    //public AnimationClip walkAnimation;
}