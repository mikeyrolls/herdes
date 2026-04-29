/**
 * Hell on earth 2.0
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public static int getPerc() {
        return Random.Range(0, 100);
    }

    void HandleTreasureRoom() { // +x right, -x left, -y down
        bool stopLoot = false;
        while (!stopLoot) {
            //int perc = Random.Range(0, 100);
            if (getPerc() < 40) {
                stopLoot = true;
            } else {
                int amount = Random.Range(1, 50);
                SpawnTreasure(Random.Range(-4f, 4f), Random.Range(-4f, 0), LootType.Gold, amount);
            }
        }
    }

    void SpawnTreasure(float x, float y, LootType lootType, int value) {
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

        int round = 1;
        combatState = CombatState.PlayerTurn;


    }

    public void OnEnemyClicked()
    {
        if (combatState != CombatState.PlayerTurn) return;

        int heroDmg = GameManager.Instance.hero.GiveDmg();
        currentEnemy.TakeDmg(heroDmg);

        AddInfoText("Hit " + currentEnemy.nameStr + " for " + heroDmg + " dmg, " + currentEnemy.currHP + "/" + currentEnemy.maxHP + "hp left");

        //dodge too
        if(!currentEnemy.IsAlive()) {
            combatState = CombatState.Won;
            AddInfoText("You won! Leave");
        }

        




        // currentEnemy.takeDamage();
        // AddInfoText("You dealt " + GameManager.Instance.playerDamage + " damage");

        // if (!currentEnemy.isAlive)
        // {
        //     combatState = CombatState.Won;
        //     OnCombatWon();
        // }
        // else
        // {
        //     combatState = CombatState.EnemyTurn;
        //     EnemyTurn();
        // }
    }

    // void EnemyTurn()
    // {
    //     int dmg = currentEnemy.giveDmg();
    //     GameManager.Instance.TakeDmg(dmg);
    //     AddInfoText("Enemy dealt " + dmg + " damage");
    //     combatState = CombatState.PlayerTurn;
    //     AddInfoText("Your turn");
    // }

    // void OnCombatWon()
    // {
    //     GameManager.Instance.combat = false;
    //     Destroy(enemyObject);
    //     AddInfoText("You won!");
    // }

    void AddInfoText(string newText) {
        infoTextStrings[0] = infoTextStrings[1];
        infoTextStrings[1] = infoTextStrings[2];
        infoTextStrings[2] = newText;
        textTmp.text = infoTextStrings[0] + "\n" + infoTextStrings[1] + "\n" + infoTextStrings[2];
    }
}
