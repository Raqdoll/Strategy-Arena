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

    // Use this for initialization
    public enum SpellAreaType { Cross, Line, Normal, Square, Cone, Diagonal};
    public enum SpellRangeType { Linear, Diagonal, Normal}
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
    public bool spellOpen;

    void Start () {
        switch (mySpellClass)
        {
            case PlayerBehaviour.CharacterClass.Tank1:
                SpellBase();

                break;
            case PlayerBehaviour.CharacterClass.DmgDealer1:

                break;
            case PlayerBehaviour.CharacterClass.Healer1:

                break;
            case PlayerBehaviour.CharacterClass.Support1:

                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Tämä on metodikohtaisen summaryn esimerkki, poista!
    /// </summary>
    void AreaType()
    {
        switch (mySpellAreaType)
        {
            case SpellAreaType.Cone:


                DamageCalculator();
                break;
            case SpellAreaType.Cross:


                DamageCalculator();
                break;
            case SpellAreaType.Diagonal:


                DamageCalculator();
                break;
            case SpellAreaType.Normal:


                DamageCalculator();
                break;
            case SpellAreaType.Line:


                DamageCalculator();
                break;
            case SpellAreaType.Square:


                DamageCalculator();
                break;
        }
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
    void DamageCalculator()
    {


    }
    void SpellBase()
    {
        mySpellAreaType = SpellAreaType.Line;
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
        spellOpen = false;
        spellCooldownLeft = 0;

        RangeType(); //updateen
        AreaType(); // updateen
        
}

}
