using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurnManager : MonoBehaviour {

    public int turnNumber;
    public delegate void PlayerEvent(PlayerBehaviour player);
    public PlayerEvent TurnChange;
    public int activePlayer;
    public int playerRotation;
    public int currentCharacterAmount;
    public Button nextTurnButton;


    public void AnnounceTurnChange(GameObject newPlayer)
    {
        if (newPlayer)
        {
            PlayerBehaviour temp = newPlayer.GetComponent<PlayerBehaviour>();
            if (TurnChange != null && temp != null)
            {
                TurnChange(temp);
            }
        }
    }

	// Use this for initialization
		
    void Start () {
        playerRotation = 1;
        activePlayer = playerRotation;
        Button next = nextTurnButton.GetComponent<Button>();
        next.onClick.AddListener(NextTurn);
        currentCharacterAmount = 10;
	}
	
	void Update () {
		
	}

    public void NextTurn()
    {
        playerRotation++;
        if(playerRotation > currentCharacterAmount)
        {
            playerRotation = 1;
        }
        activePlayer = playerRotation;
    }
}
