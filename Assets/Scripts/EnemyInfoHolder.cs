/**
 * Sprite holder for enemies and currently heroes (combat only)
 */

using UnityEngine;

[CreateAssetMenu(fileName = "FightSpriteSet", menuName = "Game/Fight Sprite Set")]
public class FightSpriteSet : ScriptableObject
{
    public Sprite idle;
    public Sprite damaged;
    public Sprite attacking;
    public Sprite dead;
    //public AudioClip damagedSound;
    //public AnimationClip walkAnimation;
}