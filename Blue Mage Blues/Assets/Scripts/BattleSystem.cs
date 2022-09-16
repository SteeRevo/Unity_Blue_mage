using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public enum BattleState{START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    Unit playerUnit;
    Unit enemyUnit;

    public TextMeshProUGUI dialogueText;

    public BattleControls playerControls;


    public BattleHud playerHUD;
    public BattleHud enemyHUD;

    private InputAction attack;

    private void Awake(){
        playerControls = new BattleControls();
    }

    private void OnEnable(){
       attack = playerControls.UI.Submit;
       attack.Enable();       
    }

    private void OnDisable(){
        attack.Disable();
    }

    
    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());

    }

    IEnumerator SetupBattle(){
        GameObject playerGO = Instantiate(playerPrefab);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = enemyUnit.unitName + " approaches.";

        playerHUD.SetHud(playerUnit);
        enemyHUD.SetHud(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        StartCoroutine(PlayerTurn());

    }

    IEnumerator PlayerAttack(){
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        yield return new WaitForSeconds(2f);

        if(isDead){
            state = BattleState.WON;
            EndBattle();
        }
        else{
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn(){
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);
        
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if(isDead){
            state = BattleState.LOST;
            EndBattle();
           
        }
        else{
                state = BattleState.PLAYERTURN;
                StartCoroutine(PlayerTurn());
        }
    }

    void EndBattle(){
        if(state == BattleState.WON){
            dialogueText.text = "battle won";
        }
        else if(state == BattleState.LOST){
            dialogueText.text = "YA LOST DIPSHIT";
        }
    }

    IEnumerator PlayerTurn(){
        dialogueText.text = "Choose an action:";
        bool notAdvanced = true;
        yield return WaitForInput();
        
        StartCoroutine(PlayerAttack());
        
    }


    public void OnAttackButton(){
        if(state != BattleState.PLAYERTURN){
            return;
        }
        StartCoroutine(PlayerAttack());
    }

    private IEnumerator WaitForInput(){
        bool done = false;
        while(!done){
            if(attack.triggered){
                done = true;
            }
            yield return null;
        }
    }


   

}
