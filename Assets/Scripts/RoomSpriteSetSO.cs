/**
 * Sprite holder for hero out of combat
 */

using UnityEngine;

[CreateAssetMenu(fileName = "RoomSpriteSet", menuName = "Game/Room Sprite Set")]
public class RoomSpriteSet : ScriptableObject
{
    public Sprite idle;
    public Sprite damaged;
    public Sprite pet;
    
    //public AudioClip damagedSound;
    //public AnimationClip walkAnimation;
}