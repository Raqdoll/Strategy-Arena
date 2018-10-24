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
    LineOfSight lOS;

    void Start () {
        mouseController = GameObject.FindGameObjectWithTag("MouseManager").GetComponent<MouseController>();
        gridController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridController>();
        spellCast = GetComponent<SpellCast>();
        playerBehaviour = GetComponent<PlayerBehaviour>();
        lOS = GetComponent<LineOfSight>();
        if (!gridController)
            Debug.LogWarning("Gridcontroller is null!");
    }

   public List<Tile> AreaType(SpellAreaType mySpellAreaType)
    {
        int[][] dirList = new int[4][] { new int[]{0, 1 }, new int[] { 0,-1}, new int[] { 1, 0 }, new int[] { -1, 0 } };
        List<Tile> targetTiles = new List<Tile>();
        switch (mySpellAreaType)
        {

            //spellrange min ei saa olla 0
            case SpellAreaType.Line:
                if (playerBehaviour.currentCharacter.currentTile.z < mouseController.selected.locZ)
                    LineTargets(0, dirList, out targetTiles);
                else if (playerBehaviour.currentCharacter.currentTile.z > mouseController.selected.locZ)
                    LineTargets(1, dirList, out targetTiles);
                else if (playerBehaviour.currentCharacter.currentTile.x > mouseController.selected.locX)
                    LineTargets(3, dirList, out targetTiles);
                else
                    LineTargets(2, dirList, out targetTiles);
                break;
            case SpellAreaType.Cross:

                targetTiles.Add(mouseController.selected);
                for (int i = 1; i <= spellCast.currentSpell.aoeRange; i++)
                { 
                    foreach (var c in dirList) {
                        targetTiles.Add(gridController.GetTile(mouseController.selected.locX + c[0]*i, mouseController.selected.locZ+c[1]*i));
                    }
                }
                break;
            case SpellAreaType.Diagonal:
                targetTiles.Add(mouseController.selected);
                for (int i = 1; i <= spellCast.currentSpell.aoeRange; i++)
                {
                    targetTiles.Add(gridController.GetTile(mouseController.selected.locX + i, mouseController.selected.locZ + i));
                    targetTiles.Add(gridController.GetTile(mouseController.selected.locX + i, mouseController.selected.locZ - i));
                    targetTiles.Add(gridController.GetTile(mouseController.selected.locX - i, mouseController.selected.locZ + i));
                    targetTiles.Add(gridController.GetTile(mouseController.selected.locX - i, mouseController.selected.locZ - i));
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

                // spellrange ei saa olla 0
            case SpellAreaType.Cone:
                if (playerBehaviour.currentCharacter.currentTile.z < mouseController.selected.locZ)
                {
                    ConeTargets(0, dirList, out targetTiles);
                }
                else if (playerBehaviour.currentCharacter.currentTile.z > mouseController.selected.locZ)
                {
                    ConeTargets(1, dirList, out targetTiles);
                }
                else if (playerBehaviour.currentCharacter.currentTile.x > mouseController.selected.locX)
                {
                    ConeTargets(3, dirList, out targetTiles);
                }
                else
                {
                    ConeTargets(2, dirList, out targetTiles);
                }
                break;
            case SpellAreaType.Square:
                for (int i = 0 - spellCast.currentSpell.aoeRange; i <= spellCast.currentSpell.aoeRange; i++)
                {
                    for (int j = 0 - spellCast.currentSpell.aoeRange; j <= spellCast.currentSpell.aoeRange; j++)
                    {
                        targetTiles.Add(gridController.GetTile(mouseController.selected.locX + j, mouseController.selected.locZ + i));
                    }
                }
                    break;
        }
        List<Tile> returnables = new List<Tile>();
        foreach (var tile in targetTiles)
        {
            if (tile != null)
            {
                if (tile.myType == Tile.BlockType.BaseBlock)
                {
                    returnables.Add(tile);
                }
            }
        }
        return returnables;
    }

    private void LineTargets(int directionIndex, int[][] dirList, out List<Tile> targetTiles) {
    targetTiles = new List<Tile>();
        for (int i = 0; i <= spellCast.currentSpell.aoeRange; i++) {
            targetTiles.Add(gridController.GetTile(mouseController.selected.locX + dirList[directionIndex][0] * i, mouseController.selected.locZ + dirList[directionIndex][1] * i));
        }
    }

    private void ConeTargets(int directionIndex, int[][] dirList, out List<Tile> targetTiles ) {
        targetTiles = new List<Tile>();
        for (int i = 0; i <= spellCast.currentSpell.aoeRange; i++)
        {
            for (int j = 0 - i; j <= i; j++)
            {
                int x2 = dirList[directionIndex][0];
                int z2 = dirList[directionIndex][1];
                if (x2 == 0) {
                    x2 = j;
                    z2 *= i;
                }
                if (z2 == 0) {
                    z2 = j;
                    x2 *= i;
                }
                targetTiles.Add(gridController.GetTile(mouseController.selected.locX + x2, mouseController.selected.locZ + z2));
            }
        }
    }

    public List<Tile> RangeType(SpellRangeType mySpellRangeType)
    {      
    List<Tile> rangetiles = new List<Tile>();
        int i = spellCast.currentSpell.spellRangeMin;
        if (i == 0)
        {
            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x, playerBehaviour.currentCharacter.currentTile.z));
            i++;
        }
        switch (mySpellRangeType)
        {
            case SpellRangeType.Diagonal:              
                    for ( i = spellCast.currentSpell.spellRangeMin; i <= spellCast.currentSpell.spellRangeMax; i++)
                        {
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x + i, playerBehaviour.currentCharacter.currentTile.z + i));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x + i, playerBehaviour.currentCharacter.currentTile.z - i));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x - i, playerBehaviour.currentCharacter.currentTile.z + i));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x - i, playerBehaviour.currentCharacter.currentTile.z - i));
                        }
                break;
            case SpellRangeType.Linear:
                    for ( i = spellCast.currentSpell.spellRangeMin; i <= spellCast.currentSpell.spellRangeMax; i++)
                    {
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x, playerBehaviour.currentCharacter.currentTile.z + i));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x + i, playerBehaviour.currentCharacter.currentTile.z));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x, playerBehaviour.currentCharacter.currentTile.z - i));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x - i, playerBehaviour.currentCharacter.currentTile.z));
                    }
                break;
            case SpellRangeType.Normal:
                    for ( i = spellCast.currentSpell.spellRangeMin - spellCast.currentSpell.spellRangeMax; i <= spellCast.currentSpell.spellRangeMax; i++)
                    {
                        for (int j = 0 - spellCast.currentSpell.spellRangeMax; j <= spellCast.currentSpell.spellRangeMax; j++)
                        {
                            if (Mathf.Abs(i) + Mathf.Abs(j) <= spellCast.currentSpell.spellRangeMax)
                            {                           
                                rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x + j, playerBehaviour.currentCharacter.currentTile.z + i));
                            }
                        }
                    }
                break;
            default:
                break;
        }
        List<Tile> returnables = new List<Tile>();
        foreach (var tile in rangetiles)
        {
            if (tile != null)
            {
                if(spellCast.currentSpell.needLineOfSight == true)
                   if( lOS.LoSCheck(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x, playerBehaviour.currentCharacter.currentTile.z), tile) == true)
                        if (tile.myType == Tile.BlockType.BaseBlock)
                            returnables.Add(tile);
                else
                   if (tile.myType == Tile.BlockType.BaseBlock)               
                    returnables.Add(tile);
            }
        }
        
        return returnables;
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
