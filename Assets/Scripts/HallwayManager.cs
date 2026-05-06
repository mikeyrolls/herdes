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
    [SerializeField] GameObject itemPrefab;
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
            int amount = Random.Range(1, 30);
            SpawnGold(amount);
        }
    }

    void SpawnGold(int value) {
        SpawnGold(Random.Range(-4f, 4f), Random.Range(-4f, 0), value);
    }

    void SpawnGold(float x, float y, int value) {    // +x right, -x left, -y down
        GameObject item = Instantiate(itemPrefab, new Vector3(x, y, 0f), Quaternion.identity);
        item.GetComponent<ItemGold>().InitGold(value);
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
        Debug.Log("onEnemyClicked");
        if (combatState != CombatState.PlayerTurn) return;

        combatState = CombatState.EnemyTurn;
        StartCoroutine(CombatRound());

    }

    IEnumerator CombatRound() {
        Debug.Log("CombatRound");
        //player attack
        StartCoroutine(AttackRound(GameManager.Instance.hero, currentEnemy));

        //check status TODO method somehow, for counterattacks and reusability
        if(!currentEnemy.IsAlive()) {
            GameManager.Instance.combat = false;
            combatState = CombatState.Won;

            EnemyDied();
            yield return new WaitForSeconds(1f);
            currentEnemy.Kill();
            AddInfoText("You won! Leave");
        } else {
            Debug.Log("enemy attacking");
            yield return new WaitForSeconds(0.7f); //TODO for now in place of animations
            //enemy attack
            StartCoroutine(AttackRound(currentEnemy, GameManager.Instance.hero));
            //todo check status one hero death matters

            combatState = CombatState.PlayerTurn;
        }
    }

    IEnumerator AttackRound(Creature attacker, Creature attacked) {
        Debug.Log("attackRound entered, attacker = " + attacker.name + ", attacked = " + attacked.name);
        int hitDmg = attacker.GiveDmg();

        if(!attacker.LandHit()) {
            Debug.Log("if(!attacker.LandHit())");   
            AddInfoText(attacker.nameStr + " missed " + attacked.nameStr);
            yield return StartCoroutine(attacked.DodgeAnimation());
            yield break;
        }

        if(attacked.Dodge()) {
            Debug.Log("if(attacked.Dodge())");
            AddInfoText(attacked.nameStr + " dodged ");
            yield return StartCoroutine(attacked.DodgeAnimation());
        } else {
            Debug.Log("else");
            attacked.TakeDmg(hitDmg);
            AddInfoText(attacker.nameStr + " hit " + attacked.nameStr + " for " + hitDmg + " dmg, " + attacked.currHP + "/" + attacked.maxHP + "hp left");
            yield return StartCoroutine(attacked.GetHurtAnimation());
        }
    }

    void EnemyDied() {
        currentEnemy.ShowDamagedSprite();
        //drop loot too todo
        SpawnGold(currentEnemy.GetGoldValue());
    }

    void AddInfoText(string newText) {
        infoTextStrings[0] = infoTextStrings[1];
        infoTextStrings[1] = infoTextStrings[2];
        infoTextStrings[2] = newText;
        textTmp.text = infoTextStrings[0] + "\n" + infoTextStrings[1] + "\n" + infoTextStrings[2];
        UIManager.Instance.RefreshHUD();
    }

    // -------------------------[ animations? ]-----------------------------------------------------




}
