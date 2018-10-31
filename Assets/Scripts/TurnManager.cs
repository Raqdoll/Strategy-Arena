using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurnManager : MonoBehaviour {

    public int turnNumber;
    public delegate void PlayerEvent(PlayerInfo player);
    public event PlayerEvent TurnChange;
    public int activePlayer;
    public int playerRotation;
    public int currentCharacterAmount;
    public Button nextTurnButton;
    public TeamManager teamManager;


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
        playerRotation = 1;
        activePlayer = playerRotation;
        Button next = nextTurnButton.GetComponent<Button>();
        next.onClick.AddListener(NextTurn);
        currentCharacterAmount = 10;
        if (!teamManager)
            teamManager = gameObject.GetComponent<TeamManager>();

    }

    void Update () {
		
	}

    public void NextTurn()
    {
        //Asserin koodia
        if (teamManager)
        {
            PlayerInfo temp = teamManager.ChangeTurnUntilValidPlayer();
            AnnounceTurnChange(temp);
        }
        else
            Debug.Log("Could not find team manager script!");

        //Aiemmat koodit
        playerRotation++;
        if(playerRotation > currentCharacterAmount)
        {
            playerRotation = 1;
        }
        activePlayer = playerRotation;
    }
}
//Made by Asser
