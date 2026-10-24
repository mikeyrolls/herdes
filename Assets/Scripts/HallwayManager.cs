/**
 * Hell on earth 2.0
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HallwayManager : MonoBehaviour {

    RoomType type;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject heroPrefab;
    [SerializeField] TMP_Text textTmp;

    string[] infoTextStrings = {"", "", ""};


    EnemyGO enemyGO;
    Enemy enemy;
    HeroGO heroGO;
    Hero hero;

    public enum CombatState { PlayerTurn, EnemyTurn, Won, Lost }
    CombatState combatState;
    public bool IsPlayerTurn => combatState == CombatState.PlayerTurn;

    void Start() {
        type = GameManager.Instance.currentRoomType;
        hero = GameManager.Instance.hero;

        heroGO = Instantiate(heroPrefab, new Vector3(-5.5f, -0.5f, 0f), Quaternion.identity).GetComponent<HeroGO>();
        heroGO.InitGO(hero.spriteSet);
        hero.sceneObject = heroGO;

        heroGO.onDeath = () => GameManager.Instance.Die();

        hero.ApplyEffects();

        switch (type) {
            case RoomType.Fight:
                HandleFightRoom();
                break;
            case RoomType.Treasure:
                HandleTreasureRoom();
                break;
            case RoomType.Empty:
                //nothing lol
                break;
            case RoomType.Fakewall:
                HandleFakewallRoom();
                break;
        }
    }

// -------------------------[ treasure ]-----------------------------------------------------

    void SpawnItem(ItemName itemName) {
        if (itemName == ItemName.None) return;
        GameObject item = Instantiate(itemPrefab, new Vector3(Random.Range(-2f, 4f), Random.Range(-2f, 0), 0f), Quaternion.identity);
        item.GetComponent<ItemGO>().InitializeFromDB(itemName);
    }

    void HandleTreasureRoom() {
        do {
            SpawnItem(ItemDB.GetRandom(GameManager.Instance.roomCount));
        } while (Helper.GetPerc() < 10);
    }

// -------------------------[ fakewall ]-----------------------------------------------------

    void HandleFakewallRoom() {
        do {
            int amount = Random.Range(1, 30);
            SpawnGold(amount);
        } while (Helper.GetPerc() < 30);
    }

    void SpawnGold(int value) {
        SpawnGold(Random.Range(-2f, 4f), Random.Range(-2f, 0), value);
    }

    void SpawnGold(float x, float y, int value) {    // +x right, -x left, -y down
        GameObject item = Instantiate(itemPrefab, new Vector3(x, y, 0f), Quaternion.identity);
        item.GetComponent<ItemGO>().InitGold(value);
    }

// -------------------------[ fight ]-----------------------------------------------------


    void HandleFightRoom() {

        AddInfoText("entered combat");

        //EnemyType spawnedEnemy = EnemyDB.GetRandomEnemy(GameManager.Instance.roomCount);

        //debugging picked enemy----------------------------
        EnemyType spawnedEnemy = EnemyType.Golem;
        //-------------------------

        enemy = new Enemy();
        enemy.InitEnemy(spawnedEnemy);
        enemyGO = Instantiate(enemyPrefab, new Vector3(2f, -0.5f, 0f), Quaternion.identity).GetComponent<EnemyGO>();
        enemyGO.InitGO(EnemyDB.sprites[spawnedEnemy]);
        enemy.sceneObject = enemyGO;
        enemyGO.onDeath = () => EnemyDied();

        GameManager.Instance.StartingCombatWith(enemy);
        AddInfoText("spawned enemy: " + enemy.nameStr);
        combatState = CombatState.PlayerTurn;
    }

    public void OnEnemyClicked() {
        if (combatState != CombatState.PlayerTurn) return;

        combatState = CombatState.EnemyTurn;
        StartCoroutine(CombatRound(0)); //change for mage if overcharging
    }

    public void OnHeroClicked() {
        if (combatState != CombatState.PlayerTurn) return;

        combatState = CombatState.EnemyTurn;
        StartCoroutine(CombatRound(hero.specialType)); //change for mage

    }

    IEnumerator CombatRound(int attackType) {

        //player attack
        StartCoroutine(AttackRound(hero, enemy, attackType)); //get hit, check hp
        yield return new WaitForSeconds(0.8f); //for anim

        //enemy attack
        StartCoroutine(AttackRound(enemy, hero, enemy.GetAttackType())); //get hit, check hp
        yield return new WaitForSeconds(0.60f); // enemy attack duration
        combatState = CombatState.PlayerTurn;
    }

    IEnumerator AttackRound(Creature attacker, Creature attacked, int attackType) {

        if(!attacked.IsAlive() || !attacker.IsAlive()) yield break; // pauses combat round

        // for self heals/leeching
        int healAttacker = 0;

        //currently attacked can dodge attacker's charged attack but attacker can't miss?
        //can be configured for repeated charges with attacktype for mage
        if (attacker.isCharging) {

                int hitDmg = attacker.DoSpecialCharged();
                if(attacked.GetAttacked(hitDmg)) { //else dodged
                    //poisons/bleeds from charged
                    foreach (var effectToApply in attacker.GetEffectsFromAtkType(2)) {
                        attacked.effectList.AddEffectDurationValue(effectToApply.effect, effectToApply.duration, effectToApply.value);
                    }

                    healAttacker = attacker.HealAfterAttack(2);
                } 

        } else {

            switch(attackType) {

                //normal attack
                case 0:
                    if (attacker.LandHit()) {
                        int hitDmg = attacker.Attack();
                        if(attacked.GetAttacked(hitDmg)) {  //else dodged
                            //poisons/bleeds from normal
                            foreach (var effectToApply in attacker.GetEffectsFromAtkType(0)) {
                                attacked.effectList.AddEffectDurationValue(effectToApply.effect, effectToApply.duration, effectToApply.value);
                            }
                            healAttacker = attacker.HealAfterAttack(0);
                        } 

                    } else {
                        attacker.Miss();
                    }
                    break;

                //special fast (buffs only, change if debuffs/attacks, attackeR)
                case 1:
                    attacker.DoSpecialFast();
                    foreach (var effectToApply in attacker.GetEffectsFromAtkType(1)) {
                        attacker.effectList.AddEffectDurationValue(effectToApply.effect, effectToApply.duration, effectToApply.value);
                    }
                    healAttacker = attacker.HealAfterAttack(1);
                    break;

                //special charged
                case 2:
                    attacker.DoSpecialCharging();
                    break;
            }
        }

        UIManager.Instance.RefreshHUD();
        yield return new WaitForSeconds(0.65f);

        if (healAttacker > 0)
            attacker.Heal(healAttacker);

        if(!attacked.IsAlive())
            attacked.Die();
        else
            attacked.ApplyEffects(); // die should happen here too hopefully?

        
        yield return null;

    }

    void EnemyDied() {
        GameManager.Instance.EndingCombat();
        combatState = CombatState.Won;

        SpawnItem(enemy.GetDrop());
        SpawnGold(enemy.GetGoldValue());
    }

    void AddInfoText(string newText) {
        infoTextStrings[0] = infoTextStrings[1];
        infoTextStrings[1] = infoTextStrings[2];
        infoTextStrings[2] = newText;
        textTmp.text = infoTextStrings[0] + "\n" + infoTextStrings[1] + "\n" + infoTextStrings[2];
        UIManager.Instance.RefreshHUD();
    }

}
