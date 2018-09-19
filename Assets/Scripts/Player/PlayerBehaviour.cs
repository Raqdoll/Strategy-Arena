using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acts as the controller/nerve center for player classes and stores most of the variables.
/// All input goes through this class.
/// </summary>

    [RequireComponent(typeof(PlayerMovement), typeof(PlayerActions), typeof(Abilities))]
public class PlayerBehaviour : MonoBehaviour {

    public enum CharacterClass { Tank1, Healer1, Support1, DmgDealer1 }; // Put Classes Here 
    public bool moving;  //e.g. Hover to get movement range vs ability range
    public CharacterClass myCharClass;
    public int maxHP;
    public int currentHP;
    public int maxAp;
    public int maxMp;
    public int currentAp;
    public int currentMp;
    public double damageChange;

    void Start () {
		// Put Information Of classes Here
        switch (myCharClass)
        {
            case CharacterClass.Tank1:
                maxHP = 4500;
                maxAp = 11;
                maxMp = 6;
                currentHP = maxHP;
                currentAp = maxAp;
                currentMp = maxMp;
                damageChange = 1.0;
                break;
            case CharacterClass.Support1:
                maxHP = 3000;
                maxAp = 12;
                maxMp = 6;
                currentHP = maxHP;
                currentAp = maxAp;
                currentMp = maxMp;
                damageChange = 1.0;
                break;
            case CharacterClass.Healer1:
                maxHP = 3000;
                maxAp = 12;
                maxMp = 6;
                currentHP = maxHP;
                currentAp = maxAp;
                currentMp = maxMp;
                damageChange = 1.0;
                break;
            case CharacterClass.DmgDealer1:
                maxHP = 3000;
                maxAp = 12;
                maxMp = 6;
                currentHP = maxHP;
                currentAp = maxAp;
                currentMp = maxMp;
                damageChange = 1.0;
                break;
        }
	}
	void Update () {
		
	}
}
