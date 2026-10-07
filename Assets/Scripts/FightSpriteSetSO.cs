/**
 * Sprite holder for enemies and heroes (combat only)
 */

using UnityEngine;

[CreateAssetMenu(fileName = "FightSpriteSet", menuName = "Game/Fight Sprite Set")]
public class FightSpriteSet : ScriptableObject
{
    public Sprite idle;
    public Sprite damaged;
    public Sprite attacking;
    public Sprite dead;

    public Sprite specialFast;
    public Sprite specialCharged1;
    public Sprite specialCharged2;
    
    //public AudioClip damagedSound;
    //public AnimationClip walkAnimation;
}