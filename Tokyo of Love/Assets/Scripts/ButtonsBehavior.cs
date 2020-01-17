using UnityEngine;
using UnityEngine.UI;

public class ButtonsBehavior : MonoBehaviour
{
    private BattleHandler battleHandler;
    public GameObject Inputfieldd;

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
        if (battleHandler.state == BattleHandler.State.WaitingForPlayer)
        {
            battleHandler.state = BattleHandler.State.Busy;
            battleHandler.activeCharacter.Heal(Random.Range(battleHandler.activeCharacter.healmin, battleHandler.activeCharacter.healmax));
            battleHandler.ChooseNextActiveCharacter();
        }
    }

    public void Play()
    {
        battleHandler.IsHumanPlaying = true;
        GameObject.Find("Play").SetActive(false);
        GameObject.Find("Bot Simulation").SetActive(false);
        GameObject.Find("Attack").GetComponent<Button>().enabled = true;
        GameObject.Find("Abilities").GetComponent<Button>().enabled = true;
        GameObject.Find("Block").GetComponent<Button>().enabled = true;
    }

    public void Simulation()
    {
        battleHandler.IsHumanPlaying = false;
        GameObject.Find("Play").SetActive(false);
        GameObject.Find("Bot Simulation").SetActive(false);
        GameObject.Find("Attack").SetActive(false);
        GameObject.Find("Abilities").SetActive(false);
        GameObject.Find("Block").SetActive(false);
        battleHandler.ChooseNextActiveCharacter();
        battleHandler.selectedEnemy.HideArrow();
        battleHandler.selectedEnemy = null;
    }

    public void Enter()
    {
        string Level;
        int level;
        Level = Inputfieldd.GetComponent<Text>().text;

        int.TryParse(Level, out level);
        battleHandler.LevelUpgrade(level);
    }
}