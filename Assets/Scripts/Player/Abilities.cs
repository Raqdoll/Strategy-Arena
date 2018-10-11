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
    public Button spellButton;
    public enum SpellAreaType { Cross, Line, Normal, Square, Cone, Diagonal}; // Different types of AoE
    public enum SpellRangeType { Linear, Diagonal, Normal}; // How Player Targets the spell
    public enum SpellName { baseSpell, other, other2};
    public SpellAreaType mySpellAreaType;
    public SpellRangeType mySpellRangeType;
    public SpellName mySpellName;
    public int spellInitialCooldown;

    public int spellDamageMin;
    public int spellDamageMax;
    public int spellRangeMin;
    public int spellRangeMax;
    public int spellCooldown;
    public int spellCastPerturn;
    public int castPerTarget;
    public int spellApCost;
    public int spellPushback;
    public int spellPull;
    public int areaRange;
    public int spellCooldownLeft;
    public int trueDamage;
    public bool healsAlly = false;
    public bool hurtsAlly = false;
    public bool spellOpen = false;
    public bool needLineOfSight = false;
    public bool spellLaunched = false;
    public bool inCooldown = false;


    public string spellName;
    GridController gridController;
    TeamManager teamManager;
    PlayerBehaviour playerBehaviour;
    MouseController mouseController;
    Tile tilescripts;


    /*muistutus itsellleni
     * reworkkaa abilityt
     * tee spelleistä taas funktioita jotka muokkaavat perus arvoja 
     * tee kuitenkin sillä tavalla että on arvoja jotka ovat vain yhteen spelliin sidottuja
     * spell buttonut kutsuvat näitä funktioita
     * säilytä nykyiset spell enumit muokattuina
     * 
     * */

    void Start () {
        mouseController = GameObject.FindGameObjectWithTag("MouseManager").GetComponent<MouseController>();
        gridController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridController>();
        if (!gridController)
            Debug.LogWarning("Gridcontroller is null!");
        teamManager = gridController.gameObject.GetComponent<TeamManager>();
        tilescripts = GetComponent<Tile>();
       // Button cast = spellButton.GetComponent<Button>();


        switch (mySpellName)
        {
            case SpellName.baseSpell:
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
                needLineOfSight = false;
                inCooldown = false;
                healsAlly = false;
                hurtsAlly = false;
                spellCooldownLeft = 0;

                Button cast = spellButton.GetComponent<Button>();
                //cast.onClick.AddListener(LaunchSpell());
                Debug.Log("BaseSpell Selected");
                cast.onClick.AddListener(OpenSpell);
                //cast.GetComponentInChildren<Text>().text = "SpellBase";
                break;
            case SpellName.other:

                break;
            case SpellName.other2:

                break;
            default:

                break;
        }
    }
	void OpenSpell()
    {
        spellOpen = true;
    }

	void Update ()
    {
    }


   public List<Tile> AreaType()
    {
        List<Tile> targetTiles = new List<Tile>();
        switch (mySpellAreaType)
        {
            case SpellAreaType.Line:
                if (teamManager.activePlayer.currentTile.locZ <= gridController.hoverTile.locZ)
                {
                    for (int i = 1; i <= areaRange; i++)
                    {
                        targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX, gridController.hoverTile.locZ + i));
                    }
                }
                else if (teamManager.activePlayer.currentTile.locZ >= gridController.hoverTile.locZ)
                    for (int i = 1; i <= areaRange; i++)
                    {
                        targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + i, gridController.hoverTile.locZ));
                    }
                else if (teamManager.activePlayer.currentTile.locX >= gridController.hoverTile.locX)
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
                            targetTiles.Add(gridController.GetTile(mouseController.selected.locX + j, mouseController.selected.locZ + i));
                        }
                    }
                }
                break;
            case SpellAreaType.Cone:
                if (teamManager.activePlayer.currentTile.locZ <= gridController.hoverTile.locZ)
                {
                    for (int i = 0; i <= areaRange; i++)
                    {
                        for (int j = 0 - i; j <= i; j++)
                        {
                            targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + j, gridController.hoverTile.locZ + i));
                        }
                    }
                }
                else if (teamManager.activePlayer.currentTile.locZ >= gridController.hoverTile.locZ)
                {
                    for (int i = 0; i <= areaRange; i++)
                    {
                        for (int j = 0 - i; j <= i; j++)
                        {
                            targetTiles.Add(gridController.GetTile(gridController.hoverTile.locX + j, gridController.hoverTile.locZ - i));
                        }
                    }
                }
                else if (teamManager.activePlayer.currentTile.locX >= gridController.hoverTile.locX)
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
                    rangetiles.Add(gridController.GetTile(teamManager.activePlayer.currentTile.locX, teamManager.activePlayer.currentTile.locZ));
                    for (int i = spellRangeMin + 1; i <= spellRangeMax; i++)
                        {
                            rangetiles.Add(gridController.GetTile(teamManager.activePlayer.currentTile.locX + i, teamManager.activePlayer.currentTile.locZ + i));
                            rangetiles.Add(gridController.GetTile(teamManager.activePlayer.currentTile.locX + i, teamManager.activePlayer.currentTile.locZ - i));
                            rangetiles.Add(gridController.GetTile(teamManager.activePlayer.currentTile.locX - i, teamManager.activePlayer.currentTile.locZ + i));
                            rangetiles.Add(gridController.GetTile(teamManager.activePlayer.currentTile.locX - i, teamManager.activePlayer.currentTile.locZ - i));
                        }
                }
                break;
            case SpellRangeType.Linear:
                if (needLineOfSight == true)
                {
                    rangetiles.Add(gridController.GetTile(teamManager.activePlayer.currentTile.locX, teamManager.activePlayer.currentTile.locZ));
                    for (int i = spellRangeMin +1; i <= spellRangeMax; i++)
                    {
                            rangetiles.Add(gridController.GetTile(teamManager.activePlayer.currentTile.locX, teamManager.activePlayer.currentTile.locZ + i));
                            rangetiles.Add(gridController.GetTile(teamManager.activePlayer.currentTile.locX + i, teamManager.activePlayer.currentTile.locZ));
                            rangetiles.Add(gridController.GetTile(teamManager.activePlayer.currentTile.locX, teamManager.activePlayer.currentTile.locZ - i));
                            rangetiles.Add(gridController.GetTile(teamManager.activePlayer.currentTile.locX - i, teamManager.activePlayer.currentTile.locZ));
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
                                rangetiles.Add(gridController.GetTile(teamManager.activePlayer.currentTile.locX + j, teamManager.activePlayer.currentTile.locZ + i));
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


    public int MinDamCacl(int damMin, float damChange, int damPlus)
    {
        int tempdamage = Mathf.RoundToInt(damMin * (1 + damChange) + damPlus);

        return tempdamage;
    }
    public int MaxDamCacl(int damMax, float damChange, int damPlus)
    {
        int tempdamage = Mathf.RoundToInt(damMax * (1 + damChange) + damPlus);

        return tempdamage;
    }
    public int TrueDamageCalculator(int damMax, int damMin, float damChange, int damPlus)
    {
        int tempdamageMin = MinDamCacl(damMin, damChange, damPlus);
        int tempdamageMax = MaxDamCacl(damMax, damChange, damPlus);
        int trueDamage = Random.Range(tempdamageMin, tempdamageMax);

        return trueDamage;
    }


    public int MinHealCacl(int damMin, float heals)
    {
        int tempHeal = Mathf.RoundToInt(damMin * (1 + heals));

        return tempHeal;
    }
    public int MaxHealCacl(int damMax, float heals)
    {
        int tempHeal = Mathf.RoundToInt(damMax * (1 + heals));

        return tempHeal;
    }
    public int TrueHealCalculator(int damMax, int damMin, float heals)
    {
        int tempHealMin = MinHealCacl(damMin, heals);
        int tempHealMax = MaxHealCacl(damMin, heals);
        int trueHeal = Random.Range(tempHealMin, tempHealMax);

        return trueHeal;
    }


    //void LaunchSpell(List<Tile> targetTiles)
    //{
    //    targetTiles.Clear();
    //    AreaType();
    //    foreach (var tile in targetTiles)
    //    {
    //        TrueDamageCalculator();
    //    }
    //    targetTiles.Clear();
    //}

    // This Spell serves as a Base for other Spells

    void SpellOpened()
    {
        SpellRangeType spellMyRange = mySpellRangeType;
        SpellAreaType spellMyArea = mySpellAreaType;
        spellOpen = true;
    }

   public void SpellBaseCast()
    {

        int spellAreaRange = areaRange;
        int damageMyMin = spellDamageMin;
        int damageMyMax = spellDamageMax;
        int myRangeMin =  spellRangeMin;
        int myRangeMax = spellRangeMax;
        int myCooldown = spellCooldown;
        float myDamageChange = playerBehaviour.damageChange;
        int myDamagePlus = playerBehaviour.damagePlus;
        int myCastPerTurn = spellCastPerturn;
        int myCastPerTarget = castPerTarget;
        int myApCost = spellApCost;
        int myPushBack = spellPushback;
        int myPull = spellPull;
        bool myLineOfsight = needLineOfSight;
        bool myInCooldown = inCooldown;
        bool myHealsAlly = healsAlly;
        bool myHurtsAlly = hurtsAlly;
        int myCooldownLeft = spellCooldownLeft;

        //playerBehaviour.currentAp = playerBehaviour.currentAp - myApCost;
        Debug.Log("BaseSpell Selected");
    }

    public void SpellCancel()
    {
        spellOpen = false;
        var derp = RangeType();
        Color ihana = new Color(1, 0, 1);
        
        foreach(var herp in derp)
        {
            herp.BaseMaterial.SetColor("wut", ihana);
        }
    }
}
