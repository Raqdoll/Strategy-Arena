using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurnManager : MonoBehaviour {

    public int turnNumber;
    public delegate void PlayerEvent(PlayerInfo player);
    public event PlayerEvent TurnChange;
    public Button nextTurnButton;
    public TeamManager teamManager;
    public SpellCast spellCast;


    public void AnnounceTurnChange(GameObject newPlayerGO)
    {
        if (newPlayerGO)
        {
            PlayerInfo temp = newPlayerGO.GetComponent<PlayerInfo>();
            AnnounceTurnChange(temp);
        }
    }

    public void AnnounceTurnChange(PlayerInfo newPlayer)
    {
        if (TurnChange != null && newPlayer != null)
        {
            TurnChange(newPlayer);
        }
    }
		
    void Start () {
        Button next = nextTurnButton.GetComponent<Button>();
        next.onClick.AddListener(NextTurn);
        if (!teamManager)
            teamManager = gameObject.GetComponent<TeamManager>();
        spellCast= GameObject.FindGameObjectWithTag("PlayerController").GetComponent<SpellCast>();
    }

    public void NextTurn()
    {
        if (teamManager)
        {
            spellCast.SpellCancel();
            PlayerInfo temp = teamManager.ChangeTurnUntilValidPlayer();
            temp.RefreshPoints();
            AnnounceTurnChange(temp);
        }
        else
            Debug.Log("Could not find team manager script!");
    }
}
//Made by Asser
