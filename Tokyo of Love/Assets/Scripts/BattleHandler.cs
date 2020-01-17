﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // Enemy 1 turn
        if (activeCharacter == AllyBattle1)
        {
            SetActiveCharacterBattle(enemyBattle);
            if (enemyBattle.IsDead() == false) {
                state = State.Busy;
                enemyBattle.Attack(allyTarget, () => {
                    ChooseNextActiveCharacter();
                });
            } else ChooseNextActiveCharacter();
        }
        // Ally2 turn
        else if (activeCharacter == enemyBattle)
        {
            SetActiveCharacterBattle(AllyBattle3);
            if (AllyBattle3.IsDead() == false) {
                activeCharacter.blocking = false;
                state = State.WaitingForPlayer;
            } else ChooseNextActiveCharacter();
        }
        // Enemy 2 turn
        else if (activeCharacter == AllyBattle3)
        {
            SetActiveCharacterBattle(enemy2Battle);
            if (enemy2Battle.IsDead() == false) {
                state = State.Busy;
                enemy2Battle.Attack(allyTarget, () => {
                    ChooseNextActiveCharacter();
                });
            } else ChooseNextActiveCharacter();
        }
        // Ally3 turn
        else if (activeCharacter == enemy2Battle)
        {
            SetActiveCharacterBattle(AllyBattle2);
            if (AllyBattle2.IsDead() == false) {
                activeCharacter.blocking = false;
                state = State.WaitingForPlayer;
            } else ChooseNextActiveCharacter();
        }
        // Enemy 3 turn
        else if (activeCharacter == AllyBattle2)
        {
            SetActiveCharacterBattle(enemy3Battle);
            if (enemy3Battle.IsDead() == false) {
                state = State.Busy;
                enemy3Battle.Attack(allyTarget, () => {
                    ChooseNextActiveCharacter();
                });
            } else ChooseNextActiveCharacter();
        }
        // Ally1 turn
        else
        {
            SetActiveCharacterBattle(AllyBattle1);
            if (AllyBattle1.IsDead() == false) {
                activeCharacter.blocking = false;
                state = State.WaitingForPlayer;
            } else ChooseNextActiveCharacter();
        }
    }

    private bool BattleOver()
    {
        if (AllyBattle1.IsDead())
            allyTarget = AllyBattle3;

        if (AllyBattle3.IsDead())
            allyTarget = AllyBattle2;

        if (allies == 0)
        {
            Debug.Log("Enemy wins");
            return true;
        }
        else if (enemies == 0)
        {
            Debug.Log("Player wins");
            return true;
        }

        return false;
    }
}
