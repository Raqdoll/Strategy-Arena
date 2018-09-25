using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Muokka summarya vapaasti :)
/// <summary>
/// Lists abilities for the attached player. Generates the list using a class enum.
/// Stores the patterns used in different abilites.
/// </summary>
[RequireComponent(typeof(PlayerBehaviour), typeof(PlayerActions), typeof(Tile))]

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
    public int trueDamage;
    public bool needLineOfSight = false;
    public bool spellLaunched = false;
    static public bool spellOpen = false;
    public string spellName;
    GridController gridController;
    Tile tilescripts;



    void Start () {
        Button cast1 = spellButton1.GetComponent<Button>();
        Button cast2 = spellButton2.GetComponent<Button>();
        Button cast3 = spellButton3.GetComponent<Button>();
        Button cast4 = spellButton4.GetComponent<Button>();
        Button cast5 = spellButton5.GetComponent<Button>();
        Button cast6 = spellButton6.GetComponent<Button>();
        gridController = GetComponent<GridController>();
        tilescripts = GetComponent<Tile>();
        // Put Classes Here and Add spell functions to the class
        switch (mySpellClass)
        {
            case PlayerBehaviour.CharacterClass.Tank1:
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
            Debug.Log("Spelll Open");
            RangeType();
            foreach (var tile in RangeType())
            {
                tile.GetComponent<Renderer>().material.color = tilescripts.RangeMaterial.color;
            }
            if (Input.GetMouseButtonDown(1))
            {
                foreach (var tile in RangeType())
                {
                    tile.GetComponent<Renderer>().material.color = tilescripts.BaseMaterial.color;
                }
                SpellCancel();
            }
            //if (Input.GetMouseButtonDown(0))
            //{
            //    LaunchSpell();
            //}
        }
	}





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





    public List<Tile> RangeType()
    {
        List<Tile> rangetiles = new List<Tile>();
        switch (mySpellRangeType)
        {
            case SpellRangeType.Diagonal:
                if(needLineOfSight == false)
                {
                    rangetiles.Add(gridController.GetTile(gridController.playerTile.locX, gridController.playerTile.locZ));
                    for (int i = spellRangeMin + 1; i <= spellRangeMax; i++)
                        {
                            rangetiles.Add(gridController.GetTile(gridController.playerTile.locX + i, gridController.playerTile.locZ + i));
                            rangetiles.Add(gridController.GetTile(gridController.playerTile.locX + i, gridController.playerTile.locZ - i));
                            rangetiles.Add(gridController.GetTile(gridController.playerTile.locX - i, gridController.playerTile.locZ + i));
                            rangetiles.Add(gridController.GetTile(gridController.playerTile.locX - i, gridController.playerTile.locZ - i));
                        }
                }
                break;
            case SpellRangeType.Linear:
                if (needLineOfSight == true)
                {
                    rangetiles.Add(gridController.GetTile(gridController.playerTile.locX, gridController.playerTile.locZ));
                    for (int i = spellRangeMin +1; i <= spellRangeMax; i++)
                    {
                            rangetiles.Add(gridController.GetTile(gridController.playerTile.locX, gridController.playerTile.locZ + i));
                            rangetiles.Add(gridController.GetTile(gridController.playerTile.locX + i, gridController.playerTile.locZ));
                            rangetiles.Add(gridController.GetTile(gridController.playerTile.locX, gridController.playerTile.locZ - i));
                            rangetiles.Add(gridController.GetTile(gridController.playerTile.locX - i, gridController.playerTile.locZ));
                    }
                }
                break;
            case SpellRangeType.Normal:
                if (needLineOfSight == false)
                {
                    for (int i = spellRangeMin - spellRangeMax; i <= spellRangeMax; i++)
                    {
                        for (int j = 0 - spellRangeMax; j <= spellRangeMax; j++)
                        {
                            if (Mathf.Abs(i) + Mathf.Abs(j) <= areaRange)
                            {                           
                                rangetiles.Add(gridController.GetTile(gridController.playerTile.locX + j, gridController.playerTile.locZ + i));
                            }
                        }
                    }
                }
                break;
        }
        foreach (var tile in rangetiles)
        {
            if (tile.myType != Tile.BlockType.BaseBlock)
            {
                rangetiles.Remove(tile);
            }
        }
        return rangetiles;
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


        AreaType(); // updateen
        Debug.Log("BaseSpell Selected");
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
        Debug.Log("Spell Is Off");
    }
}
