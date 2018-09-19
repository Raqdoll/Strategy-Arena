using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Muokka summarya vapaasti :)
/// <summary>
/// Lists abilities for the attached player. Generates the list using a class enum.
/// Stores the patterns used in different abilites.
/// </summary>
[RequireComponent(typeof(PlayerBehaviour), typeof(PlayerActions), typeof(Abilities))]

public class Abilities : MonoBehaviour {

    public enum SpellAreaType { Cross, Line, Normal, Square, Cone, Diagonal}; // Different types of AoE
    public enum SpellRangeType { Linear, Diagonal, Normal} // How Player Targets the spell
    public Button spellButton1, spellButton2, spellButton3, spellButton4, spellButton5, spellButton6;
    public PlayerBehaviour.CharacterClass mySpellClass;
    public SpellAreaType mySpellAreaType;
    public SpellRangeType mySpellRangeType;
    public int spellDamageMin;
    public int spellDamageMax;
    public int spellRangeMin;
    public int spellRangeMax;
    public int spellCooldown;
    public int spellCastPerturn;
    public int castPerTarget;
    public int spellInitialCooldown;
    public int spellApCost;
    public int spellPushback;
    public int spellPull;
    public int areaRange;
    public int spellSlotNumber;
    public int spellCooldownLeft;
    public bool needLineOfSight;
    static public bool spellOpen;
    public string spellName;

    GridController gridController;

    void Start () {
        Button cast1 = spellButton1.GetComponent<Button>();
        Button cast2 = spellButton2.GetComponent<Button>();
        Button cast3 = spellButton3.GetComponent<Button>();
        Button cast4 = spellButton4.GetComponent<Button>();
        Button cast5 = spellButton5.GetComponent<Button>();
        Button cast6 = spellButton6.GetComponent<Button>();

        gridController = GetComponent<GridController>();

        // Put Classes Here and Add spell functions to the class
        switch (mySpellClass)
        {
            case PlayerBehaviour.CharacterClass.Tank1:
                cast1.onClick.AddListener(SpellBase); // Adds Spell to Button
                cast1.GetComponentInChildren<Text>().text = "SpellBase"; // Add Spell Name Manually
                cast2.onClick.AddListener(SpellBase);
                cast2.GetComponentInChildren<Text>().text = "SpellBase";
                cast3.onClick.AddListener(SpellBase);
                cast3.GetComponentInChildren<Text>().text = "SpellBase";
                cast4.onClick.AddListener(SpellBase);
                cast4.GetComponentInChildren<Text>().text = "SpellBase";
                cast5.onClick.AddListener(SpellBase);
                cast5.GetComponentInChildren<Text>().text = "SpellBase";
                cast6.onClick.AddListener(SpellBase);
                cast6.GetComponentInChildren<Text>().text = "SpellBase";

                break;
            case PlayerBehaviour.CharacterClass.DmgDealer1:

                break;
            case PlayerBehaviour.CharacterClass.Healer1:

                break;
            case PlayerBehaviour.CharacterClass.Support1:

                break;
        }
    }
	
	void Update () {

        // this is used to cancel Spell
		if(spellOpen == true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                SpellCancel();
            }
            if (Input.GetMouseButtonDown(0))
            {
                LaunchSpell();
            }
        }
	}

    /// <summary>
    /// Tämä on metodikohtaisen summaryn esimerkki, poista!
    /// </summary>
    /// 
   public List<Tile> AreaType()
    {
        List<Tile> targetTiles = new List<Tile>();
        switch (mySpellAreaType)
        {
            case SpellAreaType.Line:
                if (gridController.playerTile.locZ <= gridController.hoverTile.locZ)
                {
                    for (int i = 1; i <= areaRange; i++)
                    {
                        targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX, gridController.hoverTile.locZ + i));
                    }
                }
                else if (gridController.playerTile.locZ >= gridController.hoverTile.locZ)
                    for (int i = 1; i <= areaRange; i++)
                    {
                        targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + i, gridController.hoverTile.locZ));
                    }
                else if (gridController.playerTile.locX >= gridController.hoverTile.locX)
                    for (int i = 1; i <= areaRange; i++)
                    {
                        targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX, gridController.hoverTile.locZ - i));
                    }
                else
                    for (int i = 1; i <= areaRange; i++)
                    {
                        targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX - i, gridController.hoverTile.locZ));
                    }
                break;
            case SpellAreaType.Cross:

                targetTiles.Add(gridController.hoverTile);
                for (int i = 1; i <= areaRange; i++)
                {   
                    targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX, gridController.hoverTile.locZ + i));
                    targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + i, gridController.hoverTile.locZ));
                    targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX, gridController.hoverTile.locZ - i));
                    targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX - i, gridController.hoverTile.locZ));
                }


                break;
            case SpellAreaType.Diagonal:
                targetTiles.Add(gridController.hoverTile);
                for (int i = 1; i <= areaRange; i++)
                {
                    targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + i, gridController.hoverTile.locZ + i));
                    targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + i, gridController.hoverTile.locZ - i));
                    targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX - i, gridController.hoverTile.locZ + i));
                    targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX - i, gridController.hoverTile.locZ - i));
                }
                break;
            case SpellAreaType.Normal:
                for (int i = 0 - areaRange; i <= areaRange; i++)
                {
                    for (int j = 0 - areaRange; j <= areaRange; j++)
                    {
                        if (Mathf.Abs(i) + Mathf.Abs(j) <= areaRange) {
                            targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + j, gridController.hoverTile.locZ + i));
                        }
                    }
                }
                break;
            case SpellAreaType.Cone:
                if (gridController.playerTile.locZ <= gridController.hoverTile.locZ)
                {
                    for (int i = 0; i <= areaRange; i++)
                    {
                        for (int j = 0 - i; j <= i; j++)
                        {
                            targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + j, gridController.hoverTile.locZ + i));
                        }
                    }
                }
                else if (gridController.playerTile.locZ >= gridController.hoverTile.locZ)
                {
                    for (int i = 0; i <= areaRange; i++)
                    {
                        for (int j = 0 - i; j <= i; j++)
                        {
                            targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + j, gridController.hoverTile.locZ - i));
                        }
                    }
                }
                else if (gridController.playerTile.locX >= gridController.hoverTile.locX)
                { 

                    for (int i = 0; i <= areaRange; i++)
                    {
                        for (int j = 0 - i; j <= i; j++)
                        {
                            targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX - j, gridController.hoverTile.locZ + i));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= areaRange; i++)
                    {
                        for (int j = 0 - i; j <= i; j++)
                        {
                            targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX - j, gridController.hoverTile.locZ - i));
                        }
                    }
                }


                break;
            case SpellAreaType.Square:
                for (int i = 0 - areaRange; i <= areaRange; i++)
                {
                    for (int j = 0 - areaRange; j <=  areaRange; j++)
                    {
                        targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + j, gridController.hoverTile.locZ + i));
                    }
                }
                    break;
        }
        return targetTiles;
    }
    void RangeType()
    {
        switch (mySpellRangeType)
        {
            case SpellRangeType.Diagonal:

                break;
            case SpellRangeType.Linear:

                break;
            case SpellRangeType.Normal:

                break;
        }
    }
    void DamageCalculator(Tile targetTile)
    {


    }
    void LaunchSpell(List<Tile> targetTiles)
    {
        targetTiles.Clear();
        AreaType();
        foreach (var tile in targetTiles)
        {
            DamageCalculator(tile);
        }
        targetTiles.Clear();
    }

    // This Spell serves as a Base for other Spells
    void SpellBase()
    {
        spellName = "test Spell";
        mySpellAreaType = SpellAreaType.Normal;
        mySpellRangeType = SpellRangeType.Normal;
        areaRange = 1;
        spellDamageMin = 210;
        spellDamageMax = 240;
        spellRangeMin = 1;
        spellRangeMax = 7;
        spellCooldown = 1;
        spellCastPerturn = 3;
        castPerTarget = 2;
        spellInitialCooldown = 0;
        spellApCost = 3;
        spellPushback = 0;
        spellPull = 0;
        spellSlotNumber = 1;
        needLineOfSight = false;
        spellOpen = true;
        spellCooldownLeft = 0;

        RangeType(); //updateen
        AreaType(); // updateen
        
}
    void SpellCancel()
    {
        spellName = "";
        areaRange = 0;
        spellDamageMin = 0;
        spellDamageMax = 0;
        spellRangeMin = 0;
        spellRangeMax = 0;
        spellCooldown = 0;
        spellCastPerturn = 0;
        castPerTarget = 0;
        spellInitialCooldown = 0;
        spellApCost = 0;
        spellPushback = 0;
        spellPull = 0;
        spellSlotNumber = 0;
        needLineOfSight = false;
        spellOpen = false;
        spellCooldownLeft = 0;

        RangeType(); //updateen
        AreaType(); // updateen

    }

}
