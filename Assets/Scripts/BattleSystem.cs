using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState
{
    START,
    PLAYERTURN,
    ENEMYTURN,
    WON,
    LOST
}

public class BattleSystem : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject combatButtons;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Character playerChar;
    Character enemyChar;

    public TextMeshProUGUI dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerChar = playerGO.GetComponent<Character>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyChar = enemyGO.GetComponent<Character>();

        dialogueText.text = enemyChar.charName + " stands in " + playerChar.charName + "'s path!\n";
        PauseText();

        playerHUD.SetHUD(playerChar);
        enemyHUD.SetHUD(enemyChar);

        yield return new WaitForSeconds(2.0f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    // ====================================
    //        PLAYER & ENEMY TURNS
    // ====================================
    void PlayerTurn()
    {
        if (playerChar.specialCurrentCooldown > 0)
        {
            playerChar.specialCurrentCooldown--;
        }
        dialogueText.text = playerChar.charName + " thinks about their next move...";
        combatButtons.SetActive(true);
    }

    IEnumerator EnemyTurn()
    {
        if (enemyChar.specialCurrentCooldown > 0)
        {
            enemyChar.specialCurrentCooldown--;
        }

        bool hasMadeMove = false;
        while (!hasMadeMove)
        {
            int action = Random.Range(0, 3);
            bool isDead;
            switch (action)
            {
                // Attacks
                case 0:
                    hasMadeMove = true;
                    dialogueText.text = enemyChar.charName + " attacks " + playerChar.charName + "!";
                    enemyChar.transform.localPosition = new Vector3(enemyChar.transform.localPosition.x - 1,
                        enemyChar.transform.localPosition.y,
                        enemyChar.transform.localPosition.z);
                    playerChar.transform.localPosition = new Vector3(playerChar.transform.localPosition.x,
                        playerChar.transform.localPosition.y + 0.5f,
                        playerChar.transform.localPosition.z);
                    yield return new WaitForSeconds(0.5f);
                    enemyChar.transform.localPosition = new Vector3(enemyChar.transform.localPosition.x + 1,
                        enemyChar.transform.localPosition.y,
                        enemyChar.transform.localPosition.z);
                    playerChar.transform.localPosition = new Vector3(playerChar.transform.localPosition.x,
                        playerChar.transform.localPosition.y - 0.5f,
                        playerChar.transform.localPosition.z);
                    
                    isDead = playerChar.TakeDamage(enemyChar.damagePower);
                    playerHUD.SetHP(playerChar.currentHP);
                    yield return new WaitForSeconds(1.5f);

                    if (isDead)
                    {
                        state = BattleState.LOST;
                        StartCoroutine(EndBattle());
                    }
                    else
                    {
                        state = BattleState.PLAYERTURN;
                        PlayerTurn();
                    }
                    break;
                // Special
                case 1:
                    if (enemyChar.specialCurrentCooldown == 0)
                    {
                        hasMadeMove = true;
                        dialogueText.text = enemyChar.charName + " ruthlessly attacks " + playerChar.charName
                            + " with their special move!";
                        enemyChar.transform.localPosition = new Vector3(enemyChar.transform.localPosition.x - 1,
                        enemyChar.transform.localPosition.y,
                        enemyChar.transform.localPosition.z);
                        playerChar.transform.localPosition = new Vector3(playerChar.transform.localPosition.x,
                            playerChar.transform.localPosition.y + 0.5f,
                            playerChar.transform.localPosition.z);
                        yield return new WaitForSeconds(0.5f);
                        enemyChar.transform.localPosition = new Vector3(enemyChar.transform.localPosition.x + 1,
                            enemyChar.transform.localPosition.y,
                            enemyChar.transform.localPosition.z);
                        playerChar.transform.localPosition = new Vector3(playerChar.transform.localPosition.x,
                            playerChar.transform.localPosition.y - 0.5f,
                            playerChar.transform.localPosition.z);

                        isDead = playerChar.TakeDamage(enemyChar.specialDamagePower);
                        playerHUD.SetHP(playerChar.currentHP);
                        enemyChar.specialCurrentCooldown = enemyChar.specialMaxCooldown;
                        yield return new WaitForSeconds(1.5f);

                        if (isDead)
                        {
                            state = BattleState.LOST;
                            StartCoroutine(EndBattle());
                        }
                        else
                        {
                            state = BattleState.PLAYERTURN;
                            PlayerTurn();
                        }
                    }
                    break;
                // Heal
                case 2:
                    if (enemyChar.currentHP < (enemyChar.maxHP / 3))
                    {
                        hasMadeMove = true;
                        enemyChar.Heal(enemyChar.healPower);
                        enemyHUD.SetHP(enemyChar.currentHP);
                        dialogueText.text = enemyChar.charName + " quickly treated their wounds!";
                        enemyChar.transform.localPosition = new Vector3(enemyChar.transform.localPosition.x,
                            enemyChar.transform.localPosition.y - 0.5f,
                            enemyChar.transform.localPosition.z);
                        yield return new WaitForSeconds(0.5f);
                        enemyChar.transform.localPosition = new Vector3(enemyChar.transform.localPosition.x,
                            enemyChar.transform.localPosition.y + 0.5f,
                            enemyChar.transform.localPosition.z);
                        yield return new WaitForSeconds(1.5f);
                        state = BattleState.PLAYERTURN;
                        PlayerTurn();
                    }
                    break;
                // Rest
                case 3:
                    if (enemyChar.specialCurrentCooldown > 1)
                    {
                        hasMadeMove = true;
                        enemyChar.Rest(enemyChar.restPower);
                        dialogueText.text = enemyChar.charName + " took a moment to catch their breathe!";
                        enemyChar.transform.localPosition = new Vector3(enemyChar.transform.localPosition.x,
                            enemyChar.transform.localPosition.y - 0.5f,
                            enemyChar.transform.localPosition.z);
                        yield return new WaitForSeconds(0.5f);
                        enemyChar.transform.localPosition = new Vector3(enemyChar.transform.localPosition.x,
                            enemyChar.transform.localPosition.y + 0.5f,
                            enemyChar.transform.localPosition.z);
                        yield return new WaitForSeconds(1.5f);
                        state = BattleState.PLAYERTURN;
                        PlayerTurn();
                    }
                    break;
            }
        }
    }

    // Method called when the battle has ended.
    IEnumerator EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = playerChar.charName + " has defeated " + enemyChar.charName + "!!";
            yield return new WaitForSeconds(1.5f);
            gameManager.GoToNextScene();
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = playerChar.charName + " was defeated by " + enemyChar.charName + ".";
            yield return new WaitForSeconds(1.5f);
            dialogueText.text = "You have failed your journey! You were not passionate enough.";
            yield return new WaitForSeconds(3.5f);
            gameManager.RestartGame();
        }
    }

    // ====================================
    //            PLAYER ACTIONS
    // ====================================
    IEnumerator PlayerAttack()
    {
        bool isDead = enemyChar.TakeDamage(playerChar.damagePower);
        enemyHUD.SetHP(enemyChar.currentHP);
        PlayerAttackText();
        playerChar.transform.localPosition = new Vector3(playerChar.transform.localPosition.x + 1,
            playerChar.transform.localPosition.y,
            playerChar.transform.localPosition.z);
        enemyChar.transform.localPosition = new Vector3(enemyChar.transform.localPosition.x,
            enemyChar.transform.localPosition.y + 0.5f,
            enemyChar.transform.localPosition.z);
        yield return new WaitForSeconds(0.5f);
        playerChar.transform.localPosition = new Vector3(playerChar.transform.localPosition.x - 1,
            playerChar.transform.localPosition.y,
            playerChar.transform.localPosition.z);
        enemyChar.transform.localPosition = new Vector3(enemyChar.transform.localPosition.x,
            enemyChar.transform.localPosition.y - 0.5f,
            enemyChar.transform.localPosition.z);
        yield return new WaitForSeconds(1.5f);

        if (isDead)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

    }

    IEnumerator PlayerSpecial()
    {
        playerChar.specialCurrentCooldown = playerChar.specialMaxCooldown;
        bool isDead = enemyChar.TakeDamage(playerChar.specialDamagePower);
        enemyHUD.SetHP(enemyChar.currentHP);
        PlayerSpecialText();
        playerChar.transform.localPosition = new Vector3(playerChar.transform.localPosition.x + 1,
            playerChar.transform.localPosition.y,
            playerChar.transform.localPosition.z);
        enemyChar.transform.localPosition = new Vector3(enemyChar.transform.localPosition.x,
            enemyChar.transform.localPosition.y + 0.5f,
            enemyChar.transform.localPosition.z);
        yield return new WaitForSeconds(0.5f);
        playerChar.transform.localPosition = new Vector3(playerChar.transform.localPosition.x - 1,
            playerChar.transform.localPosition.y,
            playerChar.transform.localPosition.z);
        enemyChar.transform.localPosition = new Vector3(enemyChar.transform.localPosition.x,
            enemyChar.transform.localPosition.y - 0.5f,
            enemyChar.transform.localPosition.z);
        yield return new WaitForSeconds(1.5f);

        if (isDead)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

    }

    IEnumerator PlayerHeal()
    {
        playerChar.Heal(playerChar.healPower);
        playerHUD.SetHP(playerChar.currentHP);
        PlayerHealText();
        playerChar.transform.localPosition = new Vector3(playerChar.transform.localPosition.x,
            playerChar.transform.localPosition.y - 0.5f,
            playerChar.transform.localPosition.z);
        yield return new WaitForSeconds(0.5f);
        playerChar.transform.localPosition = new Vector3(playerChar.transform.localPosition.x,
            playerChar.transform.localPosition.y + 0.5f,
            playerChar.transform.localPosition.z);
        yield return new WaitForSeconds(1.5f);
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerRest()
    {
        playerChar.Rest(playerChar.restPower);
        PlayerRestText();
        playerChar.transform.localPosition = new Vector3(playerChar.transform.localPosition.x,
            playerChar.transform.localPosition.y - 0.5f,
            playerChar.transform.localPosition.z);
        yield return new WaitForSeconds(0.5f);
        playerChar.transform.localPosition = new Vector3(playerChar.transform.localPosition.x,
            playerChar.transform.localPosition.y + 0.5f,
            playerChar.transform.localPosition.z);
        yield return new WaitForSeconds(1.5f);
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    // ====================================
    //          BUTTON LISTENERS
    // ====================================
    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        combatButtons.SetActive(false);
        StartCoroutine(PlayerAttack());
    }

    public void OnSpecialButton()
    {
        if (playerChar.specialCurrentCooldown == 0)
        {
            if (state != BattleState.PLAYERTURN)
                {
                    return;
                }
                combatButtons.SetActive(false);
                StartCoroutine(PlayerSpecial());
        }
        else
        {
            dialogueText.text = playerChar.charName + " is " + playerChar.specialCurrentCooldown +
                " turns away from being ready to use their special!";
            PauseText();
        }
    }

    public void OnHealButton()
    {
        if (playerChar.currentHP < playerChar.maxHP)
        {
            if (state != BattleState.PLAYERTURN)
            {
                return;
            }
            combatButtons.SetActive(false);
            StartCoroutine(PlayerHeal());
        }
        else
        {
            dialogueText.text = playerChar.charName + " already has full HP!";
            PauseText();
        }
    }

    public void OnRestButton()
    {
        if (playerChar.specialCurrentCooldown > 0)
        {
            if (state != BattleState.PLAYERTURN)
            {
                return;
            }
            combatButtons.SetActive(false);
            StartCoroutine(PlayerRest());
        }
        else
        {
            dialogueText.text = playerChar.charName + "'s special attack is already ready to use!";
            PauseText();
        }
    }


    // Method to add dramatic pause in dialogue.
    IEnumerator PauseText()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.5f);
            dialogueText.text += ".";
        }
    }

    public void PlayerAttackText()
    {
        if (playerChar.charName == "Amber")
        {
            dialogueText.text = playerChar.charName + " sings a song called ignite, which hurts " 
                + enemyChar.charName + "!!";
        }
        else if (playerChar.charName == "Carlos")
        {
            dialogueText.text = playerChar.charName + " swings his prop sword dealing damage to "
                + enemyChar.charName + "!!";
        }
        else if (playerChar.charName == "Théo")
        {
            dialogueText.text = playerChar.charName + " paints a black and white tiger which attacks " +
                enemyChar.charName + "!!";
        }
    }

    public void PlayerSpecialText()
    {
        if (playerChar.charName == "Amber")
        {
            dialogueText.text = playerChar.charName + " sings a song called colors, " +
                "the flow of the song deals massive damage towards " + enemyChar.charName + "!!";
        } 
        else if (playerChar.charName == "Carlos")
        {
            dialogueText.text = playerChar.charName + " gets ready for the big show dealing massive damage" +
                " against " + enemyChar.charName + "!!";
        }
        else if (playerChar.charName == "Théo")
        {
            dialogueText.text = playerChar.charName + " quickly paints a Black Dragon to attack " + enemyChar.charName +
            " dealing massive damage!!";
        }
    }

    public void PlayerHealText()
    {
        if (playerChar.charName == "Amber")
        {
            dialogueText.text = playerChar.charName + " sings a heartwarming song called Fight Together which " +
                "heals her heart!";
        }
        else if (playerChar.charName == "Carlos")
        {
            dialogueText.text = playerChar.charName + " takes five, which heals his wounds!";
        }
        else if (playerChar.charName == "Théo")
        {
            dialogueText.text = playerChar.charName + " draws a small black rose which heals his heart!";
        }
    }

    public void PlayerRestText()
    {
        if (playerChar.charName == "Amber")
        {
            dialogueText.text = playerChar.charName + " sings a sad song called Say Goodbye which puts one " +
                "charge toward the Colors!";
        }
        else if (playerChar.charName == "Carlos")
        {
            dialogueText.text = playerChar.charName + " takes a short rest off stage which charges up Showtime!";
        }
        else if (playerChar.charName == "Théo")
        {
            dialogueText.text = playerChar.charName + " takes a short break to draw the environment around him, " +
                "which charges up Black Dragon!";
        }
    }
}
