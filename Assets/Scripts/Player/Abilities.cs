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
    public Button spellButton1, spellButton2, spellButton3, spellButton4, spellButton5, spellButton6; // UI button Creation
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
    public string spellName;

    void Start () {
        Button cast1 = spellButton1.GetComponent<Button>();
        Button cast2 = spellButton2.GetComponent<Button>();
        Button cast3 = spellButton3.GetComponent<Button>();
        Button cast4 = spellButton4.GetComponent<Button>();
        Button cast5 = spellButton5.GetComponent<Button>();
        Button cast6 = spellButton6.GetComponent<Button>();

        // Put Classes Here and Add spell functions to the class
        switch (mySpellClass)
        {
            case PlayerBehaviour.CharacterClass.Tank1:
                cast1.onClick.AddListener(SpellBase); // Adds Spell to Button
                cast2.onClick.AddListener(SpellBase);
                cast3.onClick.AddListener(SpellBase);
                cast4.onClick.AddListener(SpellBase);
                cast5.onClick.AddListener(SpellBase);
                cast6.onClick.AddListener(SpellBase);

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
        }
	}

    /// <summary>
    /// Tämä on metodikohtaisen summaryn esimerkki, poista!
    /// </summary>
    /// 
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

    // This Spell serves as a Base for other Spells
    void SpellBase()
    {
        spellName = "test Spell";
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
