using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {

    public List<PlayerInfo> teamA;
    public List<PlayerInfo> teamB;
    public bool firstTeamIsActive;
    public int playerPositionInTeam;
    [SerializeField]
    private int charactersPerTeam = 5;
    public PlayerInfo activePlayer; //Changed playerbehaviour to PlayerInfo
    public StatusEffects sEffects;
    public TurnTimelineController timeline;




    // Use this for initialization
    void Start () {
        firstTeamIsActive = true;
        playerPositionInTeam = 0;
        activePlayer = teamA[0];
        if (!sEffects)
            sEffects = gameObject.GetComponent<StatusEffects>();
        if (teamA == null || teamB == null)
        {
            Debug.LogWarning("One of the teams is null!");
        }
        else if (charactersPerTeam != teamA.Count || charactersPerTeam != teamB.Count)
        {
            Debug.LogWarning("Check team sizes!");
        }
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
        sEffects.UpdateEffects();
        timeline.MoveArrow(activePlayer.thisCharacter);
    }

    public PlayerInfo ChangeTurnUntilValidPlayer()
    {
        return ChangeTurnUntilValidPlayer(activePlayer);
    }

    private PlayerInfo ChangeTurnUntilValidPlayer(PlayerInfo playerEndingTurn)
    {
        playerEndingTurn.RefreshPoints();
        if (!IsOneTeamDead())
        {
            activePlayer = playerEndingTurn;
            ChangeTurn();
            while (activePlayer.thisCharacter.currentHP <= 0)
            {
                ChangeTurn();
                if (activePlayer == playerEndingTurn)  //Looped, we need a check for eg. 3 vs 0 situation, when someone is killed! -> Done -> CheckTeamHealths (not tested yet) -> Renamed to IsOneTeamDead
                {
                    //AnnounceEndOfGame();
                    break;
                }
            }
        }
        return activePlayer;
    }

    public bool IsOneTeamDead()
    {
        bool teamADead = true;
        bool teamBDead = true;
        foreach(var pb in teamA)
        {
            if (pb.thisCharacter.currentHP > 0)
            {
                teamADead = false;
                break;
            }
        }
        foreach (var pb in teamB)
        {
            if (pb.thisCharacter.currentHP > 0)
            {
                teamBDead = false;
                break;
            }
        }
        if (teamADead || teamBDead)
        {
            AnnounceEndOfGame();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AnnounceEndOfGame()
    {
        Debug.Log("End of game!");
    }

}//Made by Asser
