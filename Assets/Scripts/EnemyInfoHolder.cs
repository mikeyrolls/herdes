/**
 * May be used for easier sprite setup once moving onto textures
 */

using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpriteSet", menuName = "Game/Enemy Sprite Set")]
public class EnemySpriteSet : ScriptableObject
{
    public Sprite idle;
    //public Sprite damaged;
    public Sprite dead;
    //public AudioClip damagedSound;
    //public AnimationClip walkAnimation;
}

//unused for now