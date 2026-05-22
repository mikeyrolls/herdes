/**
 * Enemy sprite dictionary (currently hero too)
 */

using UnityEngine;
using System.Collections.Generic;

public class EnemySpriteDB : MonoBehaviour {
    
    [SerializeField] EnemySpriteSet batSprites;
    [SerializeField] EnemySpriteSet slimeSprites;
    [SerializeField] EnemySpriteSet banditSprites;
    [SerializeField] EnemySpriteSet spiderSprites;
    [SerializeField] EnemySpriteSet snailSprites;
    [SerializeField] EnemySpriteSet golemSprites;
    [SerializeField] EnemySpriteSet fliesSprites;
    [SerializeField] EnemySpriteSet ratSprites;
    [SerializeField] EnemySpriteSet snakeSprites;

    [SerializeField] EnemySpriteSet fishboneSprites;

    void Awake() {
        EnemyDB.InitSprites(new Dictionary<EnemyType, EnemySpriteSet>() {
            // name           sprites
            [EnemyType.Bat] = batSprites,
            [EnemyType.Slime] = slimeSprites,
            [EnemyType.Bandit] = banditSprites,
            [EnemyType.Spider] = spiderSprites,
            [EnemyType.Snail] = snailSprites,
            [EnemyType.Golem] = golemSprites,
            [EnemyType.Flies] = fliesSprites,
            [EnemyType.Rat] = ratSprites,
            [EnemyType.Snake] = snakeSprites,

            [EnemyType.Fishbone] = fishboneSprites,
        });

    }
}