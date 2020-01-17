using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHandler : MonoBehaviour
{
    private static BattleHandler instance;
    public static BattleHandler GetInstance()
    {
        return instance;
    }

    [SerializeField] private Transform Ally1;
    [SerializeField] private Transform Ally2;
    [SerializeField] private Transform Ally3;
    [SerializeField] private Transform Enemy1;
    [SerializeField] private Transform Enemy2;
    [SerializeField] private Transform Enemy3;
    public CharacterBattle activeCharacter;
    public CharacterBattle selectedEnemy;
    public CharacterBattle allyTarget;
    public CharacterBattle enemyTarget;

    public State state;
    public uint enemies = 3;
    public uint allies = 3;

    private CharacterBattle AllyBattle1;
    private CharacterBattle AllyBattle2;
    private CharacterBattle AllyBattle3;

    private CharacterBattle enemyBattle;
    private CharacterBattle enemy2Battle;
    private CharacterBattle enemy3Battle;

    uint characterSpawn = 0;
    uint enemySpawn = 0;

    public bool IsHumanPlaying = false;

    public enum State
    {
        WaitingForPlayer,
        Busy,
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        AllyBattle3 = SpawnCharacter(true);
        AllyBattle2 = SpawnCharacter(true);
        AllyBattle1 = SpawnCharacter(true);
        enemyBattle = SpawnCharacter(false);
        enemy2Battle = SpawnCharacter(false);
        enemy3Battle = SpawnCharacter(false);

        SetActiveCharacterBattle(AllyBattle1);
        selectedEnemy = enemy3Battle;
        selectedEnemy.ShowArrow();
        allyTarget = AllyBattle1;
        enemyTarget = enemyBattle;
        state = State.WaitingForPlayer;
    }

    private void Update()
    {
        if (state == State.WaitingForPlayer)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (selectedEnemy == enemy2Battle)
                {
                    enemy2Battle.HideArrow();
                    selectedEnemy = enemyBattle;
                }
                else if (selectedEnemy == enemyBattle)
                {
                    enemyBattle.HideArrow();
                    selectedEnemy = enemy3Battle;
                }

                selectedEnemy.ShowArrow();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (selectedEnemy == enemy3Battle)
                {
                    enemy3Battle.HideArrow();
                    selectedEnemy = enemyBattle;
                }
                else if (selectedEnemy == enemyBattle)
                {
                    enemyBattle.HideArrow();
                    selectedEnemy = enemy2Battle;
                }

                selectedEnemy.ShowArrow();
            }
        }
    }

    private CharacterBattle SpawnCharacter(bool isPlayerAlly)
    {
        Vector3 position = Vector3.zero;
        Transform characterTransform = null;

        if (isPlayerAlly)
        {
            characterSpawn++;
            if (characterSpawn == 3)
            {
                position = new Vector3(70, 0);
                characterTransform = Instantiate(Ally1, position, Quaternion.identity);
            }
            else if (characterSpawn == 2)
            {
                position = new Vector3(90, 20);
                characterTransform = Instantiate(Ally3, position, Quaternion.identity);
            }
            else if (characterSpawn == 1)
            {
                position = new Vector3(90, -20);
                characterTransform = Instantiate(Ally2, position, Quaternion.identity);
            }
        }
        else
        {
            enemySpawn++;
            if (enemySpawn == 3)
            {
                position = new Vector3(-90, 20);
                characterTransform = Instantiate(Enemy1, position, Quaternion.identity);
            }
            else if (enemySpawn == 2)
            {
                position = new Vector3(-90, -20);
                characterTransform = Instantiate(Enemy3, position, Quaternion.identity);
            }
            else if (enemySpawn == 1)
            {
                position = new Vector3(-70, 0);
                characterTransform = Instantiate(Enemy2, position, Quaternion.identity);
            }
        }
                
        CharacterBattle characterBattle = characterTransform.GetComponent<CharacterBattle>();
        characterBattle.Setup(isPlayerAlly);

        return characterBattle;
    }

    private void SetActiveCharacterBattle(CharacterBattle characterBattle)
    {
        if (activeCharacter != null)
        {
            activeCharacter.HideSelectionCircle();
        }

        activeCharacter = characterBattle;
        activeCharacter.ShowSelectionCircle();
    }

    public void ChooseNextActiveCharacter()
    {
        if (BattleOver()) return;
        if (IsHumanPlaying)
        {
            // Enemy 1 turn
            if (activeCharacter == AllyBattle1)
            {
                SetActiveCharacterBattle(enemyBattle);
                if (enemyBattle.IsDead() == false)
                {

                    activeCharacter.blocking = false;
                    state = State.Busy;

                    int randomAction = Random.Range(1, 4);

                    if (randomAction == 1)
                    {
                        enemyBattle.Attack(allyTarget, () =>
                        {
                            ChooseNextActiveCharacter();
                        });
                    }
                    else if (randomAction == 2)
                    {
                        enemyBattle.blocking = true;
                        ChooseNextActiveCharacter();
                    }
                    else if (randomAction >= 3)
                    {
                        enemyBattle.Heal(Random.Range(enemyBattle.healmin, enemyBattle.healmax));
                        ChooseNextActiveCharacter();
                    }
                }
                else ChooseNextActiveCharacter();
            }
            // Ally2 turn
            else if (activeCharacter == enemyBattle)
            {
                SetActiveCharacterBattle(AllyBattle3);
                if (AllyBattle3.IsDead() == false)
                {
                    activeCharacter.blocking = false;
                    state = State.WaitingForPlayer;
                }
                else ChooseNextActiveCharacter();
            }
            // Enemy 2 turn
            else if (activeCharacter == AllyBattle3)
            {
                SetActiveCharacterBattle(enemy2Battle);
                if (enemy2Battle.IsDead() == false)
                {
                    activeCharacter.blocking = false;
                    state = State.Busy;

                    int randomAction = Random.Range(1, 4);

                    if (randomAction == 1)
                    {
                        enemy2Battle.Attack(allyTarget, () =>
                        {
                            ChooseNextActiveCharacter();
                        });
                    }
                    else if (randomAction == 2)
                    {
                        enemy2Battle.blocking = true;
                        ChooseNextActiveCharacter();
                    }
                    else if (randomAction >= 3)
                    {
                        enemy2Battle.Heal(Random.Range(enemy2Battle.healmin, enemy2Battle.healmax));
                        ChooseNextActiveCharacter();
                    }
                }
                else ChooseNextActiveCharacter();
            }
            // Ally3 turn
            else if (activeCharacter == enemy2Battle)
            {
                SetActiveCharacterBattle(AllyBattle2);
                if (AllyBattle2.IsDead() == false)
                {
                    activeCharacter.blocking = false;
                    state = State.WaitingForPlayer;
                }
                else ChooseNextActiveCharacter();
            }
            // Enemy 3 turn
            else if (activeCharacter == AllyBattle2)
            {
                SetActiveCharacterBattle(enemy3Battle);
                if (enemy3Battle.IsDead() == false)
                {
                    activeCharacter.blocking = false;
                    state = State.Busy;

                    int randomAction = Random.Range(1, 4);

                    if (randomAction == 1)
                    {
                        enemy3Battle.Attack(allyTarget, () =>
                        {
                            ChooseNextActiveCharacter();
                        });
                    }
                    else if (randomAction == 2)
                    {
                        enemy3Battle.blocking = true;
                        ChooseNextActiveCharacter();
                    }
                    else if (randomAction >= 3)
                    {
                        enemy3Battle.Heal(Random.Range(enemy3Battle.healmin, enemy3Battle.healmax));
                        ChooseNextActiveCharacter();
                    }
                }
                else ChooseNextActiveCharacter();
            }
            // Ally1 turn
            else
            {
                SetActiveCharacterBattle(AllyBattle1);
                if (AllyBattle1.IsDead() == false)
                {
                    activeCharacter.blocking = false;
                    state = State.WaitingForPlayer;
                }
                else ChooseNextActiveCharacter();
        }
        }
        else//-----------------------------------------
        {
            // Enemy 1 turn
            if (activeCharacter == AllyBattle1)
            {
                SetActiveCharacterBattle(enemyBattle);
                if (enemyBattle.IsDead() == false)
                {

                    activeCharacter.blocking = false;
                    state = State.Busy;

                    int randomAction = Random.Range(1, 4);

                    if (randomAction == 1)
                    {
                        enemyBattle.Attack(allyTarget, () =>
                        {
                            ChooseNextActiveCharacter();
                        });
                    }
                    else if (randomAction == 2)
                    {
                        enemyBattle.blocking = true;
                        ChooseNextActiveCharacter();
                    }
                    else if (randomAction >= 3)
                    {
                        enemyBattle.Heal(Random.Range(enemyBattle.healmin, enemyBattle.healmax));
                        ChooseNextActiveCharacter();
                    }
                }
                else ChooseNextActiveCharacter();
            }
            // Ally2 turn
            else if (activeCharacter == enemyBattle)
            {
                SetActiveCharacterBattle(AllyBattle3);
                if (AllyBattle3.IsDead() == false)
                {
                    activeCharacter.blocking = false;
                    state = State.Busy;

                    int randomAction = Random.Range(1, 4);

                    if (randomAction == 1)
                    {
                        AllyBattle3.Attack(enemyTarget, () =>
                        {
                            ChooseNextActiveCharacter();
                        });
                    }
                    else if (randomAction == 2)
                    {
                        AllyBattle3.blocking = true;
                        ChooseNextActiveCharacter();
                    }
                    else if (randomAction >= 3)
                    {
                        AllyBattle3.Heal(Random.Range(AllyBattle3.healmin, AllyBattle3.healmax));
                        ChooseNextActiveCharacter();
                    }
                }
                else ChooseNextActiveCharacter();
            }
            // Enemy 2 turn
            else if (activeCharacter == AllyBattle3)
            {
                SetActiveCharacterBattle(enemy2Battle);
                if (enemy2Battle.IsDead() == false)
                {
                    activeCharacter.blocking = false;
                    state = State.Busy;

                    int randomAction = Random.Range(1, 4);

                    if (randomAction == 1)
                    {
                        enemy2Battle.Attack(allyTarget, () =>
                        {
                            ChooseNextActiveCharacter();
                        });
                    }
                    else if (randomAction == 2)
                    {
                        enemy2Battle.blocking = true;
                        ChooseNextActiveCharacter();
                    }
                    else if (randomAction >= 3)
                    {
                        enemy2Battle.Heal(Random.Range(enemy2Battle.healmin, enemy2Battle.healmax));
                        ChooseNextActiveCharacter();
                    }
                }
                else ChooseNextActiveCharacter();
            }
            // Ally3 turn
            else if (activeCharacter == enemy2Battle)
            {
                SetActiveCharacterBattle(AllyBattle2);
                if (AllyBattle2.IsDead() == false)
                {
                    activeCharacter.blocking = false;
                    state = State.Busy;

                    int randomAction = Random.Range(1, 4);

                    if (randomAction == 1)
                    {
                        AllyBattle2.Attack(enemyTarget, () =>
                        {
                            ChooseNextActiveCharacter();
                        });
                    }
                    else if (randomAction == 2)
                    {
                        AllyBattle2.blocking = true;
                        ChooseNextActiveCharacter();
                    }
                    else if (randomAction >= 3)
                    {
                        AllyBattle2.Heal(Random.Range(AllyBattle2.healmin, AllyBattle2.healmax));
                        ChooseNextActiveCharacter();
                    }
                }
                else ChooseNextActiveCharacter();
            }
            // Enemy 3 turn
            else if (activeCharacter == AllyBattle2)
            {
                SetActiveCharacterBattle(enemy3Battle);
                if (enemy3Battle.IsDead() == false)
                {
                    activeCharacter.blocking = false;
                    state = State.Busy;

                    int randomAction = Random.Range(1, 4);

                    if (randomAction == 1)
                    {
                        enemy3Battle.Attack(allyTarget, () =>
                        {
                            ChooseNextActiveCharacter();
                        });
                    }
                    else if (randomAction == 2)
                    {
                        enemy3Battle.blocking = true;
                        ChooseNextActiveCharacter();
                    }
                    else if (randomAction >= 3)
                    {
                        enemy3Battle.Heal(Random.Range(enemy3Battle.healmin, enemy3Battle.healmax));
                        ChooseNextActiveCharacter();
                    }
                }
                else ChooseNextActiveCharacter();
            }
            // Ally1 turn
            else
            {
                SetActiveCharacterBattle(AllyBattle1);
                if (AllyBattle1.IsDead() == false)
                {
                    activeCharacter.blocking = false;
                    state = State.Busy;

                    int randomAction = Random.Range(1, 4);

                    if (randomAction == 1)
                    {
                        AllyBattle1.Attack(enemyTarget, () =>
                        {
                            ChooseNextActiveCharacter();
                        });
                    }
                    else if (randomAction == 2)
                    {
                        AllyBattle1.blocking = true;
                        ChooseNextActiveCharacter();
                    }
                    else if (randomAction >= 3)
                    {
                        AllyBattle1.Heal(Random.Range(AllyBattle1.healmin, AllyBattle1.healmax));
                        ChooseNextActiveCharacter();
                    }
                }
                else ChooseNextActiveCharacter();
            }
        }
    }

    private bool BattleOver()
    {
        if (AllyBattle1.IsDead())
            allyTarget = AllyBattle3;

        if (AllyBattle3.IsDead())
            allyTarget = AllyBattle2;

        if (enemyBattle.IsDead())
            enemyTarget = enemy3Battle;

        if (enemy3Battle.IsDead())
            enemyTarget = enemy2Battle;

        if (allies == 0)
        {
            Debug.Log("Enemy wins");
            GameObject.Find("BattleEndBg").GetComponent<Image>().enabled = true;
            GameObject.Find("Enemies Win").GetComponent<Text>().enabled = true;
            return true;
        }
        else if (enemies == 0)
        {
            Debug.Log("Player wins");
            GameObject.Find("BattleEndBg").GetComponent<Image>().enabled = true;
            GameObject.Find("Allies Win").GetComponent<Text>().enabled = true;
            return true;
        }

        return false;
    }
}
