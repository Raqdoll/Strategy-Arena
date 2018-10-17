using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Acts as the controller/nerve center for player classes and stores most of the variables.
/// All input goes through this class.
/// </summary>
public class PlayerBehaviour : MonoBehaviour {

    //public enum CharacterClass { Tank1, Healer1, Support1, DmgDealer1 }; // Put Classes Here 
    ////public Button spellButton1, spellButton2, spellButton3, spellButton4, spellButton5, spellButton6;
    //public bool moving;  //e.g. Hover to get movement range vs ability range
    //public CharacterClass myCharClass;
    //public int maxHP;
    //public int currentHP;
    //public int maxAp;
    //public int maxMp;
    //public int currentAp;
    //public int currentMp;
    //public int playerNum;
    //public float damageChange;
    //public float healsReceived;
    //public int damagePlus;
    //public Tile currentTile;
    public List<CharacterValues> charList;

    public CharacterValues currentCharacter;

    public List<GameObject> charTabList;

    GridController gridController;
    Abilities abilities;

    Tile tilescripts;

    void Start() {

        gridController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridController>();
        if (!gridController)
            Debug.LogWarning("Gridcontroller is null!");
        tilescripts = GetComponent<Tile>();
        //if (!currentTile)
        //    currentTile = gridController.GetTile((int)transform.localPosition.x, (int)transform.localPosition.z);



    }
    public void UpdateTabs()
    {
        foreach(GameObject tab in charTabList)
        {
            CharacterTab tabby = tab.GetComponent<CharacterTab>();
            tabby.UpdateInfo();
        }
    }
}
