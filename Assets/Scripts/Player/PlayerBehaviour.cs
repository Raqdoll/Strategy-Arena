using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Acts as the controller/nerve center for player classes and stores most of the variables.
/// All input goes through this class.
/// </summary>
public class PlayerBehaviour : MonoBehaviour {

    public enum CharacterClass { Tank1, Healer1, Support1, DmgDealer1 }; // Put Classes Here 
    //public Button spellButton1, spellButton2, spellButton3, spellButton4, spellButton5, spellButton6;
    public bool moving;  //e.g. Hover to get movement range vs ability range
    public CharacterClass myCharClass;
    public int maxHP;
    public int currentHP;
    public int maxAp;
    public int maxMp;
    public int currentAp;
    public int currentMp;
    public int playerNum;
    public float damageChange;
    public int damagePlus;
    public Tile currentTile;

    GridController gridController;
    Abilities abilities;

    Tile tilescripts;  

    void Start () {

        gridController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridController>();
        if (!gridController)
            Debug.LogWarning("Gridcontroller is null!");
        tilescripts = GetComponent<Tile>();
        currentTile = gridController.GetTile((int)transform.localPosition.x, (int)transform.localPosition.z);


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
                damageChange = 1;

                break;
            case CharacterClass.Support1:
                maxHP = 3000;
                maxAp = 12;
                maxMp = 6;
                currentHP = maxHP;
                currentAp = maxAp;
                currentMp = maxMp;
                damageChange = 1;
                break;
            case CharacterClass.Healer1:
                maxHP = 3000;
                maxAp = 12;
                maxMp = 6;
                currentHP = maxHP;
                currentAp = maxAp;
                currentMp = maxMp;
                damageChange = 1;
                break;
            case CharacterClass.DmgDealer1:
                maxHP = 3000;
                maxAp = 12;
                maxMp = 6;
                currentHP = maxHP;
                currentAp = maxAp;
                currentMp = maxMp;
                damageChange = 1;
                break;
        }
	}
	void Update () {
    }
}
