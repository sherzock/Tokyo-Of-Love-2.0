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

        //Ally1
        AllyBattle1.Originaldamagemin = AllyBattle1.damagemin;
        AllyBattle1.Originaldamagemax = AllyBattle1.damagemax;
        AllyBattle1.Originalhealmax = AllyBattle1.healmax;
        AllyBattle1.Originalhealmin = AllyBattle1.healmin;
        AllyBattle1.OriginalhealthMax = AllyBattle1.healthMax;
        //Ally2
        AllyBattle2.Originaldamagemin = AllyBattle2.damagemin;
        AllyBattle2.Originaldamagemax = AllyBattle2.damagemax;
        AllyBattle2.Originalhealmax = AllyBattle2.healmax;
        AllyBattle2.Originalhealmin = AllyBattle2.healmin;
        AllyBattle2.OriginalhealthMax = AllyBattle2.healthMax;
        //Ally3
        AllyBattle3.Originaldamagemin = AllyBattle3.damagemin;
        AllyBattle3.Originaldamagemax = AllyBattle3.damagemax;
        AllyBattle3.Originalhealmax = AllyBattle3.healmax;
        AllyBattle3.Originalhealmin = AllyBattle3.healmin;
        AllyBattle3.OriginalhealthMax = AllyBattle3.healthMax;
        //Enemy1
        enemyBattle.Originaldamagemin = enemyBattle.damagemin;
        enemyBattle.Originaldamagemax = enemyBattle.damagemax;
        enemyBattle.Originalhealmax = enemyBattle.healmax;
        enemyBattle.Originalhealmin = enemyBattle.healmin;
        enemyBattle.OriginalhealthMax = enemyBattle.healthMax;
        //Enemy2
        enemy2Battle.Originaldamagemin = enemy2Battle.damagemin;
        enemy2Battle.Originaldamagemax = enemy2Battle.damagemax;
        enemy2Battle.Originalhealmax = enemy2Battle.healmax;
        enemy2Battle.Originalhealmin = enemy2Battle.healmin;
        enemy2Battle.OriginalhealthMax = enemy2Battle.healthMax;
        //Enemy3
        enemy3Battle.Originaldamagemin = enemy3Battle.damagemin;
        enemy3Battle.Originaldamagemax = enemy3Battle.damagemax;
        enemy3Battle.Originalhealmax = enemy3Battle.healmax;
        enemy3Battle.Originalhealmin = enemy3Battle.healmin;
        enemy3Battle.OriginalhealthMax = enemy3Battle.healthMax;
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

    public void LevelUpgrade(int NewLevel)
    {
        //Ally1
        if(NewLevel <= 1)
        {
            AllyBattle1.damagemin = AllyBattle1.Originaldamagemin + (AllyBattle1.Originaldamagemin / 100) * 5; 
            AllyBattle1.damagemax = AllyBattle1.Originaldamagemax + (AllyBattle1.Originaldamagemax / 100) * 9;
            AllyBattle1.healmax = AllyBattle1.Originalhealmax + (AllyBattle1.Originalhealmax / 100) * 7;
            AllyBattle1.healmin = AllyBattle1.Originalhealmin + (AllyBattle1.Originalhealmin / 100) * 7;
            AllyBattle1.healthMax = AllyBattle1.OriginalhealthMax + (AllyBattle1.OriginalhealthMax / 100) * 7;

        }
        else if(NewLevel == 2)
        {
            AllyBattle1.damagemin = AllyBattle1.Originaldamagemin + (AllyBattle1.Originaldamagemin / 100) * 12;
            AllyBattle1.damagemax = AllyBattle1.Originaldamagemax + (AllyBattle1.Originaldamagemax / 100) * 13;
            AllyBattle1.healmax = AllyBattle1.Originalhealmax + (AllyBattle1.Originalhealmax / 100) * 15;
            AllyBattle1.healmin = AllyBattle1.Originalhealmin + (AllyBattle1.Originalhealmin / 100) * 10;
            AllyBattle1.healthMax = AllyBattle1.OriginalhealthMax + (AllyBattle1.OriginalhealthMax / 100) * 11;
        }
        else if(NewLevel >= 3)
        {
            AllyBattle1.damagemin = AllyBattle1.Originaldamagemin + (AllyBattle1.Originaldamagemin / 100) * 17;
            AllyBattle1.damagemax = AllyBattle1.Originaldamagemax + (AllyBattle1.Originaldamagemax / 100) * 19;
            AllyBattle1.healmax = AllyBattle1.Originalhealmax + (AllyBattle1.Originalhealmax / 100) * 16;
            AllyBattle1.healmin = AllyBattle1.Originalhealmin + (AllyBattle1.Originalhealmin / 100) * 16;
            AllyBattle1.healthMax = AllyBattle1.OriginalhealthMax + (AllyBattle1.OriginalhealthMax / 100) * 20;
        }
        //Ally2
        if (NewLevel <= 1)
        {
            AllyBattle2.damagemin = AllyBattle2.Originaldamagemin + (AllyBattle2.Originaldamagemin / 100) * 9;
            AllyBattle2.damagemax = AllyBattle2.damagemax + (AllyBattle2.damagemax / 100) * 9;
            AllyBattle2.healmax = AllyBattle2.healmax + (AllyBattle2.healmax / 100) * 5;
            AllyBattle2.healmin = AllyBattle2.healmin + (AllyBattle2.healmin / 100) * 6;
            AllyBattle2.healthMax = AllyBattle2.healthMax + (AllyBattle2.healthMax / 100) * 6;
        }
        else if (NewLevel == 2)
        {
            AllyBattle2.damagemin = AllyBattle2.Originaldamagemin + (AllyBattle2.Originaldamagemin / 100) * 15;
            AllyBattle2.damagemax = AllyBattle2.Originaldamagemax + (AllyBattle2.Originaldamagemax / 100) * 13;
            AllyBattle2.healmax = AllyBattle2.Originalhealmax + (AllyBattle2.Originalhealmax / 100) * 11;
            AllyBattle2.healmin = AllyBattle2.Originalhealmin + (AllyBattle2.Originalhealmin / 100) * 11;
            AllyBattle2.healthMax = AllyBattle2.OriginalhealthMax + (AllyBattle2.OriginalhealthMax / 100) * 12;
        }
        else if (NewLevel >= 3)
        {
            AllyBattle2.damagemin = AllyBattle2.Originaldamagemin + (AllyBattle2.Originaldamagemin / 100) * 17;
            AllyBattle2.damagemax = AllyBattle2.Originaldamagemax + (AllyBattle2.Originaldamagemax / 100) * 16;
            AllyBattle2.healmax = AllyBattle2.Originalhealmax + (AllyBattle2.Originalhealmax / 100) * 19;
            AllyBattle2.healmin = AllyBattle2.Originalhealmin + (AllyBattle2.Originalhealmin / 100) * 17;
            AllyBattle2.healthMax = AllyBattle2.OriginalhealthMax + (AllyBattle2.OriginalhealthMax / 100) * 20;
        }
        //Ally3
        if (NewLevel <= 1)
        {
            AllyBattle3.damagemin = AllyBattle3.Originaldamagemin + (AllyBattle3.Originaldamagemin / 100) * 6;
            AllyBattle3.damagemax = AllyBattle3.damagemax + (AllyBattle3.damagemax / 100) * 6;
            AllyBattle3.healmax = AllyBattle3.healmax + (AllyBattle3.healmax / 100) * 7;
            AllyBattle3.healmin = AllyBattle3.healmin + (AllyBattle3.healmin / 100) * 10;
            AllyBattle3.healthMax = AllyBattle3.healthMax + (AllyBattle3.healthMax / 100) * 10;
        }
        else if (NewLevel == 2)
        {
            AllyBattle3.damagemin = AllyBattle3.Originaldamagemin + (AllyBattle3.Originaldamagemin / 100) * 12;
            AllyBattle3.damagemax = AllyBattle3.Originaldamagemax + (AllyBattle3.Originaldamagemax / 100) * 14;
            AllyBattle3.healmax = AllyBattle3.Originalhealmax + (AllyBattle3.Originalhealmax / 100) * 11;
            AllyBattle3.healmin = AllyBattle3.Originalhealmin + (AllyBattle3.Originalhealmin / 100) * 14;
            AllyBattle3.healthMax = AllyBattle3.OriginalhealthMax + (AllyBattle3.OriginalhealthMax / 100) * 14;
        }
        else if (NewLevel >= 3)
        {
            AllyBattle3.damagemin = AllyBattle3.Originaldamagemin + (AllyBattle3.Originaldamagemin / 100) * 16;
            AllyBattle3.damagemax = AllyBattle3.Originaldamagemax + (AllyBattle3.Originaldamagemax / 100) * 17;
            AllyBattle3.healmax = AllyBattle3.Originalhealmax + (AllyBattle3.Originalhealmax / 100) * 16;
            AllyBattle3.healmin = AllyBattle3.Originalhealmin + (AllyBattle3.Originalhealmin / 100) * 20;
            AllyBattle3.healthMax = AllyBattle3.OriginalhealthMax + (AllyBattle3.OriginalhealthMax / 100) * 20;
        }
        //Enemy1
        if (NewLevel <= 1)
        {
            enemyBattle.damagemin = enemyBattle.Originaldamagemin + (enemyBattle.Originaldamagemin / 100) * 9;
            enemyBattle.damagemax = enemyBattle.Originaldamagemax + (enemyBattle.damagemax / 100) * 10;
            enemyBattle.healmax = enemyBattle.Originalhealmax + (enemyBattle.Originalhealmax / 100) * 6;
            enemyBattle.healmin = enemyBattle.Originalhealmin + (enemyBattle.Originalhealmin / 100) * 7;
            enemyBattle.healthMax = enemyBattle.OriginalhealthMax + (enemyBattle.OriginalhealthMax / 100) * 6;
        }
        else if (NewLevel == 2)
        {
            enemyBattle.damagemin = enemyBattle.Originaldamagemin + (enemyBattle.Originaldamagemin / 100) * 14;
            enemyBattle.damagemax = enemyBattle.Originaldamagemax + (enemyBattle.Originaldamagemax / 100) * 15;
            enemyBattle.healmax = enemyBattle.Originalhealmax + (enemyBattle.Originalhealmax / 100) * 11;
            enemyBattle.healmin = enemyBattle.Originalhealmin + (enemyBattle.Originalhealmin / 100) * 12;
            enemyBattle.healthMax = enemyBattle.OriginalhealthMax + (enemyBattle.OriginalhealthMax / 100) * 11;
        }
        else if (NewLevel >= 3)
        {
            enemyBattle.damagemin = enemyBattle.Originaldamagemin + (enemyBattle.Originaldamagemin / 100) * 19;
            enemyBattle.damagemax = enemyBattle.Originaldamagemax + (enemyBattle.Originaldamagemax / 100) * 20;
            enemyBattle.healmax = enemyBattle.Originalhealmax + (enemyBattle.Originalhealmax / 100) * 16;
            enemyBattle.healmin = enemyBattle.Originalhealmin + (enemyBattle.Originalhealmin / 100) * 17;
            enemyBattle.healthMax = enemyBattle.OriginalhealthMax + (enemyBattle.OriginalhealthMax / 100) * 16;
        }
        //Enemy2
        if (NewLevel <= 1)
        {
            enemy2Battle.damagemin = enemy2Battle.Originaldamagemin + (enemy2Battle.Originaldamagemin / 100) * 8;
            enemy2Battle.damagemax = enemy2Battle.Originaldamagemax + (enemy2Battle.Originaldamagemax / 100) * 9;
            enemy2Battle.healmax = enemy2Battle.Originalhealmax + (enemy2Battle.Originalhealmax / 100) * 7;
            enemy2Battle.healmin = enemy2Battle.Originalhealmin + (enemy2Battle.Originalhealmin / 100) * 6;
            enemy2Battle.healthMax = enemy2Battle.OriginalhealthMax + (enemy2Battle.OriginalhealthMax / 100) * 10;
        }
        else if (NewLevel == 2)
        {
            enemy2Battle.damagemin = enemy2Battle.Originaldamagemin + (enemy2Battle.Originaldamagemin / 100) * 12;
            enemy2Battle.damagemax = enemy2Battle.Originaldamagemax + (enemy2Battle.Originaldamagemax / 100) * 13;
            enemy2Battle.healmax = enemy2Battle.Originalhealmax + (enemy2Battle.Originalhealmax / 100) * 11;
            enemy2Battle.healmin = enemy2Battle.Originalhealmin + (enemy2Battle.Originalhealmin / 100) * 13;
            enemy2Battle.healthMax = enemy2Battle.OriginalhealthMax + (enemy2Battle.OriginalhealthMax / 100) * 15;
        }
        else if (NewLevel >= 3)
        {
            enemy2Battle.damagemin = enemy2Battle.Originaldamagemin + (enemy2Battle.Originaldamagemin / 100) * 17;
            enemy2Battle.damagemax = enemy2Battle.Originaldamagemax + (enemy2Battle.damagemax / 100) * 16;
            enemy2Battle.healmax = enemy2Battle.Originalhealmax + (enemy2Battle.Originalhealmax / 100) * 15;
            enemy2Battle.healmin = enemy2Battle.Originalhealmin + (enemy2Battle.Originalhealmin / 100) * 15;
            enemy2Battle.healthMax = enemy2Battle.OriginalhealthMax + (enemy2Battle.OriginalhealthMax / 100) * 20;
        }
        //Enemy3
        if (NewLevel <= 1)
        {
            enemy3Battle.damagemin = enemy3Battle.Originaldamagemin + (enemy3Battle.Originaldamagemin / 100) * 8;
            enemy3Battle.damagemax = enemy3Battle.Originaldamagemax + (enemy3Battle.damagemax / 100) * 9;
            enemy3Battle.healmax = enemy3Battle.Originalhealmax + (enemy3Battle.Originalhealmax / 100) * 6;
            enemy3Battle.healmin = enemy3Battle.Originalhealmin + (enemy3Battle.Originalhealmin / 100) * 5;
            enemy3Battle.healthMax = enemy3Battle.healthMax + (enemy3Battle.OriginalhealthMax / 100) * 10;
        }
        else if (NewLevel == 2)
        {
            enemy3Battle.damagemin = enemy3Battle.Originaldamagemin + (enemy3Battle.Originaldamagemin / 100) * 12;
            enemy3Battle.damagemax = enemy3Battle.Originaldamagemax + (enemy3Battle.Originaldamagemax / 100) * 11;
            enemy3Battle.healmax = enemy3Battle.Originalhealmax + (enemy3Battle.Originalhealmax / 100) * 14;
            enemy3Battle.healmin = enemy3Battle.Originalhealmin + (enemy3Battle.Originalhealmin / 100) * 12;
            enemy3Battle.healthMax = enemy3Battle.OriginalhealthMax + (enemy3Battle.OriginalhealthMax / 100) * 13;
        }
        else if (NewLevel >= 3)
        {
            enemy3Battle.damagemin = enemy3Battle.Originaldamagemin + (enemy3Battle.Originaldamagemin / 100) * 17;
            enemy3Battle.damagemax = enemy3Battle.Originaldamagemax + (enemy3Battle.Originaldamagemax / 100) * 17;
            enemy3Battle.healmax = enemy3Battle.Originalhealmax + (enemy3Battle.Originalhealmax / 100) * 15;
            enemy3Battle.healmin = enemy3Battle.Originalhealmin + (enemy3Battle.Originalhealmin / 100) * 19;
            enemy3Battle.healthMax = enemy3Battle.OriginalhealthMax + (enemy3Battle.OriginalhealthMax / 100) * 20;
        }
    }
}
