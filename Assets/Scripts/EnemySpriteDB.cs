/**
 * Enemy sprite dictionary (currently hero too)
 */

using UnityEngine;
using System.Collections.Generic;

public class EnemySpriteDB : MonoBehaviour {
    
    [SerializeField] FightSpriteSet batSprites;
    [SerializeField] FightSpriteSet slimeSprites;
    [SerializeField] FightSpriteSet banditSprites;
    [SerializeField] FightSpriteSet spiderSprites;
    [SerializeField] FightSpriteSet snailSprites;
    [SerializeField] FightSpriteSet golemSprites;
    [SerializeField] FightSpriteSet fliesSprites;
    [SerializeField] FightSpriteSet ratSprites;
    [SerializeField] FightSpriteSet snakeSprites;

    [SerializeField] FightSpriteSet fishboneSprites;

    void Awake() {
        EnemyDB.InitSprites(new Dictionary<EnemyType, FightSpriteSet>() {
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