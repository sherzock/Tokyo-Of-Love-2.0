using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandler : MonoBehaviour
{
    private static BattleHandler instance;
    public static BattleHandler GetInstance()
    {
        return instance;
    }

    [SerializeField] private Transform beta;
    [SerializeField] private Transform ween;
    [SerializeField] private Transform bunker;
    [SerializeField] private Transform enemy;
    public CharacterBattle activeCharacter;
    public CharacterBattle selectedEnemy;
    public CharacterBattle allyTarget;

    public State state;
    public uint enemies = 3;
    public uint allies = 3;

    private CharacterBattle betaBattle;
    private CharacterBattle bunkerBattle;
    private CharacterBattle weenBattle;

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
        bunkerBattle = SpawnCharacter(true);
        weenBattle = SpawnCharacter(true);
        betaBattle = SpawnCharacter(true);
        enemyBattle = SpawnCharacter(false);
        enemy2Battle = SpawnCharacter(false);
        enemy3Battle = SpawnCharacter(false);

        SetActiveCharacterBattle(betaBattle);
        selectedEnemy = enemyBattle;
        selectedEnemy.ShowArrow();
        allyTarget = betaBattle;
        state = State.WaitingForPlayer;
    }

    private void Update()
    {
        if (state == State.WaitingForPlayer)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (selectedEnemy == enemy3Battle)
                {
                    enemy3Battle.HideArrow();
                    selectedEnemy = enemy2Battle;
                }
                else if (selectedEnemy == enemy2Battle)
                {
                    enemy2Battle.HideArrow();
                    selectedEnemy = enemyBattle;
                }

                selectedEnemy.ShowArrow();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (selectedEnemy == enemyBattle)
                {
                    enemyBattle.HideArrow();
                    selectedEnemy = enemy2Battle;
                }
                else if (selectedEnemy == enemy2Battle)
                {
                    enemy2Battle.HideArrow();
                    selectedEnemy = enemy3Battle;
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
                characterTransform = Instantiate(beta, position, Quaternion.identity);
            }
            else if (characterSpawn == 2)
            {
                position = new Vector3(90, 20);
                characterTransform = Instantiate(bunker, position, Quaternion.identity);
            }
            else if (characterSpawn == 1)
            {
                position = new Vector3(90, -20);
                characterTransform = Instantiate(ween, position, Quaternion.identity);
            }
        }
        else
        {
            enemySpawn++;
            if (enemySpawn == 1) position = new Vector3(-90, 20);
            else if (enemySpawn == 2) position = new Vector3(-70, 0);
            else if (enemySpawn == 3) position = new Vector3(-90, -20);

            characterTransform = Instantiate(enemy, position, Quaternion.identity);
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
        if (activeCharacter == betaBattle)
        {
            SetActiveCharacterBattle(enemyBattle);
            if (enemyBattle.IsDead() == false) {
                state = State.Busy;
                enemyBattle.Attack(allyTarget, () => {
                    ChooseNextActiveCharacter();
                });
            } else ChooseNextActiveCharacter();
        }
        // Bunker turn
        else if (activeCharacter == enemyBattle)
        {
            SetActiveCharacterBattle(bunkerBattle);
            if (bunkerBattle.IsDead() == false) {
                activeCharacter.blocking = false;
                state = State.WaitingForPlayer;
            } else ChooseNextActiveCharacter();
        }
        // Enemy 2 turn
        else if (activeCharacter == bunkerBattle)
        {
            SetActiveCharacterBattle(enemy2Battle);
            if (enemy2Battle.IsDead() == false) {
                state = State.Busy;
                enemy2Battle.Attack(allyTarget, () => {
                    ChooseNextActiveCharacter();
                });
            } else ChooseNextActiveCharacter();
        }
        // Ween turn
        else if (activeCharacter == enemy2Battle)
        {
            SetActiveCharacterBattle(weenBattle);
            if (weenBattle.IsDead() == false) {
                activeCharacter.blocking = false;
                state = State.WaitingForPlayer;
            } else ChooseNextActiveCharacter();
        }
        // Enemy 3 turn
        else if (activeCharacter == weenBattle)
        {
            SetActiveCharacterBattle(enemy3Battle);
            if (enemy3Battle.IsDead() == false) {
                state = State.Busy;
                enemy3Battle.Attack(allyTarget, () => {
                    ChooseNextActiveCharacter();
                });
            } else ChooseNextActiveCharacter();
        }
        // Beta turn
        else
        {
            SetActiveCharacterBattle(betaBattle);
            if (betaBattle.IsDead() == false) {
                activeCharacter.blocking = false;
                state = State.WaitingForPlayer;
            } else ChooseNextActiveCharacter();
        }
    }

    private bool BattleOver()
    {
        if (betaBattle.IsDead())
            allyTarget = bunkerBattle;

        if (bunkerBattle.IsDead())
            allyTarget = weenBattle;

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
