using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {

    public List<PlayerBehaviour> teamA;
    public List<PlayerBehaviour> teamB;
    public bool firstTeamIsActive;
    public int playerPositionInTeam;
    //int charactersPerTeam = 5;  //Amount in the final version of the game
    int charactersPerTeam = 1;
    //public int activePlayers;   //needed?
    public PlayerBehaviour activePlayer;




    // Use this for initialization
    void Start () {
        firstTeamIsActive = true;
        playerPositionInTeam = 0;
        activePlayer = teamA[0];
    }
	

    void ChangeTurn()
    {
        if (firstTeamIsActive)
        {
            firstTeamIsActive = false;
            activePlayer = teamB[playerPositionInTeam];
        }
        else
        {
            firstTeamIsActive = true;
            playerPositionInTeam++;
            if (playerPositionInTeam > charactersPerTeam - 1)
                playerPositionInTeam = 0;
            activePlayer = teamA[playerPositionInTeam];
        }
    }

    public PlayerBehaviour ChangeTurnUntilValidPlayer(PlayerBehaviour playerEndingTurn)
    {
        activePlayer = playerEndingTurn;
        ChangeTurn();
        while (activePlayer.currentCharacter.currentHP <= 0)
        {
            ChangeTurn();
            if (activePlayer == playerEndingTurn)  //Looped, we need a check for eg. 3 vs 0 situation, when someone is killed! -> Done -> CheckTeamHealths (not tested yet)
            {
                AnnounceEndOfGame();
                break;
            }
        }
        return activePlayer;
    }

    public void CheckTeamHealths()
    {
        bool teamADead = true;
        bool teamBDead = true;
        foreach(var pb in teamA)
        {
            if (pb.currentCharacter.currentHP > 0)
            {
                teamADead = false;
                break;
            }
        }
        foreach (var pb in teamB)
        {
            if (pb.currentCharacter.currentHP > 0)
            {
                teamBDead = false;
                break;
            }
        }
        if (teamADead || teamBDead)
            AnnounceEndOfGame();
    }

    public void AnnounceEndOfGame()
    {
        Debug.Log("End of game!");
    }

}
