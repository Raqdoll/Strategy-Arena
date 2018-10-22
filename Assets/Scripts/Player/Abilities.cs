using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Muokka summarya vapaasti 
/// <summary>
/// Lists abilities for the attached player. Generates the list using a class enum.
/// Stores the patterns used in different abilites.
/// </summary>

public class Abilities : MonoBehaviour {
    public enum SpellAreaType { Cross, Line, Normal, Square, Cone, Diagonal}; // Different types of AoE
    public enum SpellRangeType { Linear, Diagonal, Normal}; // How Player Targets the spell

    GridController gridController;
    PlayerBehaviour playerBehaviour;
    MouseController mouseController;
    SpellCast spellCast;


    void Start () {
        mouseController = GameObject.FindGameObjectWithTag("MouseManager").GetComponent<MouseController>();
        gridController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridController>();
        spellCast = GetComponent<SpellCast>();
        playerBehaviour = GetComponent<PlayerBehaviour>();
        if (!gridController)
            Debug.LogWarning("Gridcontroller is null!");
    }

   public List<Tile> AreaType(SpellAreaType mySpellAreaType)
    {
        List<Tile> targetTiles = new List<Tile>();
        switch (mySpellAreaType)
        {
            case SpellAreaType.Line:
                if (playerBehaviour.currentCharacter.currentTile.z <= gridController.hoverTile.locZ)
                {
                    for (int i = 1; i <= spellCast.currentSpell.aoeRange; i++)
                    {
                        targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX, gridController.hoverTile.locZ + i));
                    }
                }
                else if (playerBehaviour.currentCharacter.currentTile.z >= gridController.hoverTile.locZ)
                    for (int i = 1; i <= spellCast.currentSpell.aoeRange; i++)
                    {
                        targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + i, gridController.hoverTile.locZ));
                    }
                else if (playerBehaviour.currentCharacter.currentTile.x >= gridController.hoverTile.locX)
                    for (int i = 1; i <= spellCast.currentSpell.aoeRange; i++)
                    {
                        targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX, gridController.hoverTile.locZ - i));
                    }
                else
                    for (int i = 1; i <= spellCast.currentSpell.aoeRange; i++)
                    {
                        targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX - i, gridController.hoverTile.locZ));
                    }
                break;
            case SpellAreaType.Cross:

                targetTiles.Add(gridController.hoverTile);
                for (int i = 1; i <= spellCast.currentSpell.aoeRange; i++)
                {   
                    targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX, gridController.hoverTile.locZ + i));
                    targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + i, gridController.hoverTile.locZ));
                    targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX, gridController.hoverTile.locZ - i));
                    targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX - i, gridController.hoverTile.locZ));
                }
                break;
            case SpellAreaType.Diagonal:
                targetTiles.Add(gridController.hoverTile);
                for (int i = 1; i <= spellCast.currentSpell.aoeRange; i++)
                {
                    targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + i, gridController.hoverTile.locZ + i));
                    targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + i, gridController.hoverTile.locZ - i));
                    targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX - i, gridController.hoverTile.locZ + i));
                    targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX - i, gridController.hoverTile.locZ - i));
                }
                break;
            case SpellAreaType.Normal:
                for (int i = 0 - spellCast.currentSpell.aoeRange; i <= spellCast.currentSpell.aoeRange; i++)
                {
                    for (int j = 0 - spellCast.currentSpell.aoeRange; j <= spellCast.currentSpell.aoeRange; j++)
                    {
                        if (Mathf.Abs(i) + Mathf.Abs(j) <= spellCast.currentSpell.aoeRange) {
                            targetTiles.Add(gridController.GetTile(mouseController.selected.locX + j, mouseController.selected.locZ + i));
                        }
                    }
                }
                break;
            case SpellAreaType.Cone:
                if (playerBehaviour.currentCharacter.currentTile.z <= gridController.hoverTile.locZ)
                {
                    for (int i = 0; i <= spellCast.currentSpell.aoeRange; i++)
                    {
                        for (int j = 0 - i; j <= i; j++)
                        {
                            targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + j, gridController.hoverTile.locZ + i));
                        }
                    }
                }
                else if (playerBehaviour.currentCharacter.currentTile.z >= gridController.hoverTile.locZ)
                {
                    for (int i = 0; i <= spellCast.currentSpell.aoeRange; i++)
                    {
                        for (int j = 0 - i; j <= i; j++)
                        {
                            targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + j, gridController.hoverTile.locZ - i));
                        }
                    }
                }
                else if (playerBehaviour.currentCharacter.currentTile.x >= gridController.hoverTile.locX)
                { 

                    for (int i = 0; i <= spellCast.currentSpell.aoeRange; i++)
                    {
                        for (int j = 0 - i; j <= i; j++)
                        {
                            targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX - j, gridController.hoverTile.locZ + i));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= spellCast.currentSpell.aoeRange; i++)
                    {
                        for (int j = 0 - i; j <= i; j++)
                        {
                            targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX - j, gridController.hoverTile.locZ - i));
                        }
                    }
                }
                break;
            case SpellAreaType.Square:
                for (int i = 0 - spellCast.currentSpell.aoeRange; i <= spellCast.currentSpell.aoeRange; i++)
                {
                    for (int j = 0 - spellCast.currentSpell.aoeRange; j <= spellCast.currentSpell.aoeRange; j++)
                    {
                        targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + j, gridController.hoverTile.locZ + i));
                    }
                }
                    break;
        }
        return targetTiles;
    }

    public List<Tile> RangeType(SpellRangeType mySpellRangeType)
    {
        
    List<Tile> rangetiles = new List<Tile>();
        switch (mySpellRangeType)
        {
            case SpellRangeType.Diagonal:
                if(spellCast.currentSpell.needLineOfSight == false)
                {
                    rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x, playerBehaviour.currentCharacter.currentTile.z));
                    for (int i = spellCast.currentSpell.spellRangeMin + 1; i <= spellCast.currentSpell.spellRangeMax; i++)
                        {
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x + i, playerBehaviour.currentCharacter.currentTile.z + i));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x + i, playerBehaviour.currentCharacter.currentTile.z - i));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x - i, playerBehaviour.currentCharacter.currentTile.z + i));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x - i, playerBehaviour.currentCharacter.currentTile.z - i));
                        }
                }
                break;
            case SpellRangeType.Linear:
                if (spellCast.currentSpell.needLineOfSight == true)
                {
                    rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x, playerBehaviour.currentCharacter.currentTile.z));
                    for (int i = spellCast.currentSpell.spellRangeMin +1; i <= spellCast.currentSpell.spellRangeMax; i++)
                    {
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x, playerBehaviour.currentCharacter.currentTile.z + i));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x + i, playerBehaviour.currentCharacter.currentTile.z));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x, playerBehaviour.currentCharacter.currentTile.z - i));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x - i, playerBehaviour.currentCharacter.currentTile.z));
                    }
                }
                break;
            case SpellRangeType.Normal:
                if (spellCast.currentSpell.needLineOfSight == false)
                {
                    for (int i = spellCast.currentSpell.spellRangeMin - spellCast.currentSpell.spellRangeMax; i <= spellCast.currentSpell.spellRangeMax; i++)
                    {
                        for (int j = 0 - spellCast.currentSpell.spellRangeMax; j <= spellCast.currentSpell.spellRangeMax; j++)
                        {
                            if (Mathf.Abs(i) + Mathf.Abs(j) <= spellCast.currentSpell.aoeRange)
                            {                           
                                rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x + j, playerBehaviour.currentCharacter.currentTile.z + i));
                            }
                        }
                    }
                }
                break;
            default:
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

    //public void Tester()
    //{
    //    var derp = RangeType();
    //    Color ihana = new Color(1, 0, 1);

    //    foreach (var herp in derp)
    //    {
    //        herp.BaseMaterial.SetColor("wut", ihana);
    //    }
    //}
}
