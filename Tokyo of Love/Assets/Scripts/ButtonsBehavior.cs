using UnityEngine;

public class ButtonsBehavior : MonoBehaviour
{
    private BattleHandler battleHandler;

    public void Start()
    {
        battleHandler = GameObject.Find("BattleHandler").GetComponent<BattleHandler>();
    }

    public void Atack()
    {
       if (battleHandler.state == BattleHandler.State.WaitingForPlayer)
       {
            battleHandler.state = BattleHandler.State.Busy;
            battleHandler.activeCharacter.Attack(battleHandler.selectedEnemy, () => {
                battleHandler.ChooseNextActiveCharacter();
            });
       }
    }

    public void Block()
    {
        if (battleHandler.state == BattleHandler.State.WaitingForPlayer)
        {
            battleHandler.state = BattleHandler.State.Busy;
            battleHandler.activeCharacter.blocking = true;
            battleHandler.ChooseNextActiveCharacter();
        }
    }

    public void Abilities()
    {

    }
}