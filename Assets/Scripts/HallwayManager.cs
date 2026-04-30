/**
 * Hell on earth 2.0
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HallwayManager : MonoBehaviour
{
    Room type;
    [SerializeField] GameObject lootPrefab;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] TMP_Text textTmp;

    string[] infoTextStrings = {"", "", ""};

    GameObject enemyObject;
    Enemy currentEnemy;

    public enum CombatState { PlayerTurn, EnemyTurn, Won, Lost }
    CombatState combatState;

    void Start() {
        type = GameManager.Instance.currentRoom;
        Debug.Log("Entered room type " + type);

        if (type == Room.Fight) {
            HandleFightRoom();
        } else if (type == Room.Treasure) {
            HandleTreasureRoom();
        }
    }

    void HandleTreasureRoom() {
        while (Helper.GetPerc() < 55) {
            int amount = Random.Range(1, 50);
            SpawnTreasure(LootType.Gold, amount);
        }
    }

    void SpawnTreasure(LootType lootType, int value) {
        SpawnTreasure(Random.Range(-4f, 4f), Random.Range(-4f, 0), lootType, value);
    }

    void SpawnTreasure(float x, float y, LootType lootType, int value) {    // +x right, -x left, -y down
        GameObject loot = Instantiate(lootPrefab, new Vector3(x, y, 0f), Quaternion.identity);
        loot.GetComponent<Loot>().Init(lootType, value, "Gold");
    }


    void HandleFightRoom() {

        GameManager.Instance.combat = true;
        AddInfoText("entered combat");

        enemyObject = Instantiate(enemyPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        currentEnemy = enemyObject.GetComponent<Enemy>();

        currentEnemy.InitEnemy(EnemyType.Bat);
        AddInfoText("spawned enemy: " + currentEnemy.nameStr);

        combatState = CombatState.PlayerTurn;
    }

    public void OnEnemyClicked() {

        if (combatState != CombatState.PlayerTurn) return;

        combatState = CombatState.EnemyTurn;
        StartCoroutine(CombatRound());

    }

    IEnumerator CombatRound() {
        //player attack
        AttackRound(GameManager.Instance.hero, currentEnemy);

        //check status TODO method somehow, for counterattacks and reusability
        if(!currentEnemy.IsAlive()) {
            GameManager.Instance.combat = false;
            combatState = CombatState.Won;

            EnemyDied();
            yield return new WaitForSeconds(1f);
            currentEnemy.Kill();
            AddInfoText("You won! Leave");
        } else {

            yield return new WaitForSeconds(0.7f); //TODO for now in place of animations
            //enemy attack
            AttackRound(currentEnemy, GameManager.Instance.hero);
            //todo check status one hero death matters

            combatState = CombatState.PlayerTurn;
        }
    }

    void AttackRound(Creature attacker, Creature attacked) {
        int hitDmg = attacker.GiveDmg();

        if(!attacker.LandHit()) {
            AddInfoText(attacker.nameStr + " missed " + attacked.nameStr);
            return;
        }
        if(attacked.Dodge()) {
            AddInfoText(attacked.nameStr + " dodged ");
        } else {
            attacked.TakeDmg(hitDmg);
            AddInfoText(attacker.nameStr + " hit " + attacked.nameStr + " for " + hitDmg + " dmg, " + attacked.currHP + "/" + attacked.maxHP + "hp left");
        }
    }

    void EnemyDied() {
        currentEnemy.ShowDamagedSprite();
        //drop loot too todo
        SpawnTreasure(LootType.Gold, currentEnemy.GetGoldValue());
    }

    void AddInfoText(string newText) {
        infoTextStrings[0] = infoTextStrings[1];
        infoTextStrings[1] = infoTextStrings[2];
        infoTextStrings[2] = newText;
        textTmp.text = infoTextStrings[0] + "\n" + infoTextStrings[1] + "\n" + infoTextStrings[2];
        UIManager.Instance.RefreshHUD();
    }
}
