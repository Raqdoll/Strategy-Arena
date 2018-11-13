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
    public enum SpellRangeType { Linear, Diagonal, Normal, LinDiag}; // How Player Targets the spell
    public enum SpellPushType { LineFromPlayer, DiagonalFromPlayer, BothFromPlayer, LineFromMouse, DiagonalFromMouse, BothFromMouse };
    public enum SpellPullType { LineTowardsPlayer, DiagonalTowardsPlayer, BothTowardsPlayer, LineTowardsMouse, DiagonalTowardsMouse, BothTowardsMouse, };

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

    public List<Tile> RangeType(SpellRangeType mySpellRangeType, bool ilaririkkootaman)
    {      
    List<Tile> rangetiles = new List<Tile>();
        List<Tile> returnables = new List<Tile>();
        int i = spellCast.currentSpell.spellRangeMin;
        int x = spellCast.currentSpell.spellRangeMin;
        if (i == 0 && spellCast.currentSpell.mySpellRangeType != SpellRangeType.Normal)
        {
            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x, playerBehaviour.currentCharacter.currentTile.z));
            i++;
            x++;
        }
        switch (mySpellRangeType)
        {
            case SpellRangeType.Diagonal:              
                    for ( i = x; i <= spellCast.currentSpell.spellRangeMax; i++)
                        {
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x + i, playerBehaviour.currentCharacter.currentTile.z + i));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x + i, playerBehaviour.currentCharacter.currentTile.z - i));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x - i, playerBehaviour.currentCharacter.currentTile.z + i));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x - i, playerBehaviour.currentCharacter.currentTile.z - i));
                        }
                break;
            case SpellRangeType.Linear:
                    for ( i =x; i <= spellCast.currentSpell.spellRangeMax; i++)
                    {
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x, playerBehaviour.currentCharacter.currentTile.z + i));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x + i, playerBehaviour.currentCharacter.currentTile.z));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x, playerBehaviour.currentCharacter.currentTile.z - i));
                            rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x - i, playerBehaviour.currentCharacter.currentTile.z));
                    }
                break;
            case SpellRangeType.Normal:
                List<Tile> badTiles = new List<Tile>();
                if (spellCast.currentSpell.spellRangeMin != 0)
                {
                    List<Tile> tempo = new List<Tile>();
                    for (int z = 0 - spellCast.currentSpell.spellRangeMin; z < spellCast.currentSpell.spellRangeMin; z++)
                    {
                        for (int j = 0 - spellCast.currentSpell.spellRangeMin; j <= spellCast.currentSpell.spellRangeMin; j++)
                        {
                            if (Mathf.Abs(z) + Mathf.Abs(j) < spellCast.currentSpell.spellRangeMin)
                            {
                                tempo.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x + j, playerBehaviour.currentCharacter.currentTile.z + z));
                            }
                        }
                    }
                    foreach (var tile in tempo)
                    {
                        if (tile != null)
                        {
                            badTiles.Add(tile);
                        }
                    }
                }
                for ( i = 0 - spellCast.currentSpell.spellRangeMax; i <= spellCast.currentSpell.spellRangeMax; i++)
                    {
                        for (int j = 0 - spellCast.currentSpell.spellRangeMax; j <= spellCast.currentSpell.spellRangeMax; j++)
                        {
                            if (Mathf.Abs(i) + Mathf.Abs(j) <= spellCast.currentSpell.spellRangeMax)
                            {                           
                                rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x + j, playerBehaviour.currentCharacter.currentTile.z + i));
                            }
                        }
                    }
                foreach (var tile in rangetiles)
                {
                    if (tile != null)
                    {
                        if (spellCast.currentSpell.needLineOfSight == true)
                        {
                            if (ilaririkkootaman == false)
                            {
                                if (lOS.LoSCheck(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x, playerBehaviour.currentCharacter.currentTile.z), tile) == true)
                                {
                                    if (badTiles != null)
                                    {
                                        bool isOk = true;
                                        foreach (var item in badTiles)
                                        {
                                            if (gridController.GetTile(item.locX, item.locZ) == gridController.GetTile(tile.locX, tile.locZ))
                                            {
                                                isOk = false;
                                            }
                                        }
                                        if (tile.myType == Tile.BlockType.BaseBlock && isOk == true)
                                        {
                                            returnables.Add(tile);
                                        }
                                    }
                                    else
                                    {
                                        if (tile.myType == Tile.BlockType.BaseBlock)
                                        {
                                            returnables.Add(tile);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (lOS.LoSCheck(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x, playerBehaviour.currentCharacter.currentTile.z), tile) == false)
                                {
                                    if (badTiles != null)
                                    {
                                        bool isOk = true;
                                        foreach (var item in badTiles)
                                        {
                                            if (gridController.GetTile(item.locX, item.locZ) == gridController.GetTile(tile.locX, tile.locZ))
                                            {
                                                isOk = false;
                                            }
                                        }
                                        if (tile.myType == Tile.BlockType.BaseBlock && isOk == true)
                                        {
                                            returnables.Add(tile);
                                        }
                                    }
                                    else
                                    {
                                        if (tile.myType == Tile.BlockType.BaseBlock)
                                        {
                                            returnables.Add(tile);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (badTiles != null)
                            {
                                bool isOk = true;
                                foreach (var item in badTiles)
                                {
                                    if (gridController.GetTile(item.locX, item.locZ) == gridController.GetTile(tile.locX, tile.locZ))
                                    {
                                        isOk = false;
                                    }
                                }
                                if (tile.myType == Tile.BlockType.BaseBlock && isOk == true)
                                {
                                    returnables.Add(tile);
                                }
                            }
                            else
                            {
                                if (tile.myType == Tile.BlockType.BaseBlock)
                                {
                                    returnables.Add(tile);
                                }
                            }
                        }
                    }
                }
                break;
            case SpellRangeType.LinDiag:
                for (i =x; i <= spellCast.currentSpell.spellRangeMax; i++)
                {
                    rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x, playerBehaviour.currentCharacter.currentTile.z + i));
                    rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x + i, playerBehaviour.currentCharacter.currentTile.z));
                    rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x, playerBehaviour.currentCharacter.currentTile.z - i));
                    rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x - i, playerBehaviour.currentCharacter.currentTile.z));
                }
                for (i =x; i <= spellCast.currentSpell.spellRangeMax; i++)
                {
                    rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x + i, playerBehaviour.currentCharacter.currentTile.z + i));
                    rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x + i, playerBehaviour.currentCharacter.currentTile.z - i));
                    rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x - i, playerBehaviour.currentCharacter.currentTile.z + i));
                    rangetiles.Add(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x - i, playerBehaviour.currentCharacter.currentTile.z - i));
                }
                break;
            default:
                break;
        }

        if (spellCast.currentSpell.mySpellRangeType != SpellRangeType.Normal)
        {
            foreach (var tile in rangetiles)
            {
                if (tile != null)
                {
                    if (spellCast.currentSpell.needLineOfSight == true)
                    {
                        if (ilaririkkootaman == false)
                        {
                            if (lOS.LoSCheck(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x, playerBehaviour.currentCharacter.currentTile.z), tile) == true)
                            {
                                if (tile.myType == Tile.BlockType.BaseBlock)
                                    returnables.Add(tile);
                            }
                        }
                        else
                        {
                            if (lOS.LoSCheck(gridController.GetTile(playerBehaviour.currentCharacter.currentTile.x, playerBehaviour.currentCharacter.currentTile.z), tile) == false)
                            {
                                if (tile.myType == Tile.BlockType.BaseBlock)
                                    returnables.Add(tile);
                            }
                        }
                    }
                    else
                       if (tile.myType == Tile.BlockType.BaseBlock)
                        returnables.Add(tile);
                }
            } 
        }
        return returnables;
    }



    public void SpellPull(SpellPullType myPullRangeType)
    {
        List<Tile> AoeList = AreaType(spellCast.currentSpell.mySpellAreaType);
        List<Tile> targetList = new List<Tile>();
        Tile anchor = new Tile();
        Tile caster = new Tile();
        caster.locX = playerBehaviour.currentCharacter.currentTile.x;
        caster.locZ = playerBehaviour.currentCharacter.currentTile.z;
        anchor = mouseController.selected;     
        // etsii siirrettävät pelaajat aoe:sta
        foreach (var tile in AoeList)
        {
            if (tile.CharCurrentlyOnTile)
            {
                targetList.Add(tile);
            }
        }
        // tekee siirrot ei valmis tarvitsee asserin apua
        foreach (var item in targetList)
        {
            switch (myPullRangeType)
            {
                case SpellPullType.BothTowardsMouse:
                    if (item.locX == anchor.locX && item.locZ < anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, pasta);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locX == anchor.locX && item.locZ > anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, pasta);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locZ == anchor.locZ && item.locX < anchor.locX)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, pasta);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locZ == anchor.locZ && item.locX > anchor.locX)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, pasta);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locX < anchor.locX && item.locZ < anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.right);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, bacon);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locX < anchor.locX && item.locZ > anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.right);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, bacon);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locX > anchor.locX && item.locZ < anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.left);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, bacon);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locX > anchor.locX && item.locZ > anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.left);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, bacon);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                case SpellPullType.BothTowardsPlayer:
                    if (anchor.locX == caster.locX && anchor.locZ < caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, pasta);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX == caster.locX && anchor.locZ > caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, pasta);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locZ == caster.locZ && anchor.locX < caster.locX)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, pasta);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locZ == caster.locZ && anchor.locX > caster.locX)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, pasta);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX < caster.locX && anchor.locZ < caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.right);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, bacon);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX < caster.locX && anchor.locZ > caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.right);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, bacon);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX > caster.locX && anchor.locZ < caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.left);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, bacon);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX > caster.locX && anchor.locZ > caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.left);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, bacon);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                case SpellPullType.DiagonalTowardsMouse:
                    if (item.locX < anchor.locX && item.locZ < anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.right);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, bacon);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locX < anchor.locX && item.locZ > anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.right);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, bacon);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locX > anchor.locX && item.locZ < anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.left);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, bacon);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locX > anchor.locX && item.locZ > anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.left);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, bacon);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                case SpellPullType.DiagonalTowardsPlayer:
                    if (anchor.locX < caster.locX && anchor.locZ < caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.right);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, bacon);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX < caster.locX && anchor.locZ > caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.right);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, bacon);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX > caster.locX && anchor.locZ < caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.left);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, bacon);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX > caster.locX && anchor.locZ > caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.left);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, bacon);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                case SpellPullType.LineTowardsMouse:
                    if (item.locX == anchor.locX && item.locZ < anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, pasta);
                            }
                            else
                            {
                                break;
                            } 
                        }
                    }
                    if (item.locX == anchor.locX && item.locZ > anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, pasta);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locZ == anchor.locZ && item.locX < anchor.locX)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, pasta);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locZ == anchor.locZ && item.locX > anchor.locX)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, pasta);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                case SpellPullType.LineTowardsPlayer:
                    if (anchor.locX == caster.locX && anchor.locZ < caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, pasta);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX == caster.locX && anchor.locZ > caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, pasta);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locZ == caster.locZ && anchor.locX < caster.locX)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, pasta);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locZ == caster.locZ && anchor.locX > caster.locX)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                PullPushAct(item, pasta);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            } 
        }
    }
    public void SpellPush(SpellPushType myPushRangeType)
    {
        List<Tile> AoeList = AreaType(spellCast.currentSpell.mySpellAreaType);
        List<Tile> targetList = new List<Tile>();
        Tile anchor = new Tile();
        Tile caster = new Tile();
        caster.locX = playerBehaviour.currentCharacter.currentTile.x;
        caster.locZ = playerBehaviour.currentCharacter.currentTile.z;
        anchor = mouseController.selected;
        // etsii siirrettävät pelaajat aoe:sta
        foreach (var tile in AoeList)
        {
            if (tile.CharCurrentlyOnTile)
            {
                targetList.Add(tile);
            }
        }
        // tekee siirrot ei valmis tarvitsee asserin apua
        foreach (var item in targetList)
        {
            switch (myPushRangeType)
            {
                case SpellPushType.BothFromMouse:
                    if (item.locX == anchor.locX && item.locZ < anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva pastan päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locX == anchor.locX && item.locZ > anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva pastan päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locZ == anchor.locZ && item.locX < anchor.locX)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva pastan päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locZ == anchor.locZ && item.locX > anchor.locX)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva pastan päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locX < anchor.locX && item.locZ < anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.left);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva beansin päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locX < anchor.locX && item.locZ > anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.left);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva beansin päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locX > anchor.locX && item.locZ < anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.right);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva beansin päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locX > anchor.locX && item.locZ > anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.right);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva beansin päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                case SpellPushType.BothFromPlayer:
                    if (anchor.locX == caster.locX && anchor.locZ < caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva pastan päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX == caster.locX && anchor.locZ > caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva pastan päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locZ == caster.locZ && anchor.locX < caster.locX)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva pastan päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locZ == caster.locZ && anchor.locX > caster.locX)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva pastan päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX < caster.locX && anchor.locZ < caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.left);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva beansin päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX < caster.locX && anchor.locZ > caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.left);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva beansin päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX > caster.locX && anchor.locZ < caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.right);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva beansin päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX > caster.locX && anchor.locZ > caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.right);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva beansin päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                case SpellPushType.DiagonalFromMouse:
                    if (item.locX < anchor.locX && item.locZ < anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.left);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva beansin päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locX < anchor.locX && item.locZ > anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.left);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva beansin päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locX > anchor.locX && item.locZ < anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.right);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva beansin päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locX > anchor.locX && item.locZ > anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.right);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva beansin päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                case SpellPushType.DiagonalFromPlayer:
                    if (anchor.locX < caster.locX && anchor.locZ < caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.left);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva beansin päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX < caster.locX && anchor.locZ > caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.left);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva beansin päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX > caster.locX && anchor.locZ < caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.right);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva beansin päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX > caster.locX && anchor.locZ > caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile bacon = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            Tile ham = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            Tile beans = gridController.GetTileInDirection(gridController.GetTile(bacon.locX, bacon.locZ), 1, GridController.Directions.right);
                            if (bacon.myType == Tile.BlockType.BaseBlock && ham.myType == Tile.BlockType.BaseBlock && beans.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva beansin päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                case SpellPushType.LineFromMouse:
                    if (item.locX == anchor.locX && item.locZ < anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva pastan päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locX == anchor.locX && item.locZ > anchor.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva pastan päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locZ == anchor.locZ && item.locX < anchor.locX)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva pastan päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (item.locZ == anchor.locZ && item.locX > anchor.locX)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva pastan päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                case SpellPushType.LineFromPlayer:
                    if (anchor.locX == caster.locX && anchor.locZ < caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.down);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva pastan päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locX == caster.locX && anchor.locZ > caster.locZ)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.up);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva pastan päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locZ == caster.locZ && anchor.locX < caster.locX)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.left);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva pastan päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (anchor.locZ == caster.locZ && anchor.locX > caster.locX)
                    {
                        for (int i = 0; i < spellCast.currentSpell.spellPull; i++)
                        {
                            Tile pasta = gridController.GetTileInDirection(gridController.GetTile(item.locX, item.locZ), 1, GridController.Directions.right);
                            if (pasta.myType == Tile.BlockType.BaseBlock)
                            {
                                // siirrä itemin päälläoleva pastan päälle Asser HEBL
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void PullPushAct(Tile start, Tile end)
    {
        // move player on tile start onto tile end
        // Jan: Lisää tämä metodi pasta carbonaran sekaan. Suosittelen edelleen miettimään apumetodia ylläolevan switchin if lausekkeille, jotta mahdolliset muokkaukset helpottuvat kummasti.

        PlayerMovement playerMovement = start.CharCurrentlyOnTile.gameObject.GetComponent<PlayerMovement>();
        if (playerMovement)
        {
            //playerMovement.MoveToTile(end, PlayerMovement.MovementMethod.push);    //Tämä tulee lopulliseen versioon, ei vielä implementoitu
            playerMovement.MoveToTile(end, PlayerMovement.MovementMethod.Teleport);  //Väliaikainen liikkuminen
        }
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
