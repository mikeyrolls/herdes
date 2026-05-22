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

    void Start() {
        type = GameManager.Instance.currentRoomType;
        Debug.Log("Entered room type " + type);

        hero = GameManager.Instance.hero;

        heroGO = Instantiate(heroPrefab, new Vector3(-5.5f, -1f, 0f), Quaternion.identity).GetComponent<HeroGO>();
        heroGO.InitGO(EnemyDB.sprites[EnemyType.Fishbone]);
        hero.sceneObject = heroGO;

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
        GameObject item = Instantiate(itemPrefab, new Vector3(Random.Range(-2f, 4f), Random.Range(-4f, 0), 0f), Quaternion.identity);
        item.GetComponent<ItemGO>().InitializeFromDB(itemName);
    }

    void HandleTreasureRoom() {
        SpawnItem(ItemName.Carrot);
    }

// -------------------------[ fakewall ]-----------------------------------------------------

    void HandleFakewallRoom() {
        do {
            int amount = Random.Range(1, 30);
            SpawnGold(amount);
        } while (Helper.GetPerc() < 55);
    }

    void SpawnGold(int value) {
        SpawnGold(Random.Range(-2f, 4f), Random.Range(-4f, 0), value);
    }

    void SpawnGold(float x, float y, int value) {    // +x right, -x left, -y down
        GameObject item = Instantiate(itemPrefab, new Vector3(x, y, 0f), Quaternion.identity);
        item.GetComponent<ItemGO>().InitGold(value);
    }

// -------------------------[ fight ]-----------------------------------------------------


    void HandleFightRoom() {

        GameManager.Instance.combat = true;
        AddInfoText("entered combat");

        EnemyType spawnedEnemy = EnemyDB.GetRandomEnemy(GameManager.Instance.roomCount);
        Debug.Log("spawnedEnemy = " + spawnedEnemy);

        enemy = new Enemy();
        enemy.InitEnemy(spawnedEnemy);
        enemyGO = Instantiate(enemyPrefab, new Vector3(2f, -1f, 0f), Quaternion.identity).GetComponent<EnemyGO>();
        enemyGO.InitGO(EnemyDB.sprites[spawnedEnemy]);
        enemy.sceneObject = enemyGO;
        enemyGO.onDeath = () => EnemyDied();

        AddInfoText("spawned enemy: " + enemy.nameStr);
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
        StartCoroutine(AttackRound(hero, enemy));

        //check status TODO method somehow, for counterattacks and reusability
        if(!enemy.IsAlive()) {
            GameManager.Instance.combat = false;
            combatState = CombatState.Won;
            
        } else {
            Debug.Log("enemy attacking");
            yield return new WaitForSeconds(1f); //TODO for now in place of animations
            //enemy attack
            StartCoroutine(AttackRound(enemy, hero));
            //todo check status one hero death matters

            yield return new WaitForSeconds(0.45f); // enemy attack duration
            combatState = CombatState.PlayerTurn;
        }
    }

    IEnumerator AttackRound(Creature attacker, Creature attacked) {
        Debug.Log("attackRound entered, attacker = " + attacker.nameStr + ", attacked = " + attacked.nameStr);

        if (attacker.LandHit()) {
            int hitDmg = attacker.Attack();
            if(attacked.GetAttacked(hitDmg)) {
                AddInfoText(attacker.nameStr + " hit " + attacked.nameStr + " for " + hitDmg + " dmg, " + attacked.currHP + "/" + attacked.maxHP + "hp left");
            } else {
                AddInfoText(attacked.nameStr + " dodged ");
            }

        } else {
            attacker.Miss();
            AddInfoText(attacker.nameStr + " missed " + attacked.nameStr);
        }

        yield return null;

    }

    

    void EnemyDied() {
        SpawnItem(enemy.dropItem);
        SpawnGold(enemy.GetGoldValue());
        AddInfoText("You won! Leave");
    }

    void AddInfoText(string newText) {
        infoTextStrings[0] = infoTextStrings[1];
        infoTextStrings[1] = infoTextStrings[2];
        infoTextStrings[2] = newText;
        textTmp.text = infoTextStrings[0] + "\n" + infoTextStrings[1] + "\n" + infoTextStrings[2];
        UIManager.Instance.RefreshHUD();
    }

}
