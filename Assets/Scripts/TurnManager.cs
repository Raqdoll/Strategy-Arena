using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurnManager : MonoBehaviour {

    public int turnNumber;
    public delegate void PlayerEvent(PlayerBehaviour player);
    public PlayerEvent TurnChange;


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
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
