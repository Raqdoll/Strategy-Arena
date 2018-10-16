﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCast : MonoBehaviour {

    public PlayerBehaviour playerBehaviour;

    public CharacterValues cv;
    public SpellValues currentSpell;
    public bool spellOpen = false;
    public int spell1CastedThisTurn;
    public int spell2CastedThisTurn;
    public int spell3CastedThisTurn;
    public int spell4CastedThisTurn;
    public int spell5CastedThisTurn;
    public int spell6CastedThisTurn;


    public Button spellButton1, spellButton2, spellButton3, spellButton4, spellButton5, spellButton6;


    void Start () {
        spellButton1 = spellButton1.GetComponent<Button>();
        spellButton1.onClick.AddListener(Spell1Cast);
        spellButton2 = spellButton2.GetComponent<Button>();
        spellButton2.onClick.AddListener(Spell2Cast);
        spellButton3 = spellButton3.GetComponent<Button>();
        spellButton3.onClick.AddListener(Spell3Cast);
        spellButton4 = spellButton4.GetComponent<Button>();
        spellButton4.onClick.AddListener(Spell4Cast);
        spellButton5 = spellButton5.GetComponent<Button>();
        spellButton5.onClick.AddListener(Spell5Cast);
        spellButton6 = spellButton6.GetComponent<Button>();
        spellButton6.onClick.AddListener(Spell6Cast);

    }

    public void CastSpell(SpellValues spell, CharacterValues caster, CharacterValues target)
    {
        if (spell.hurtsAlly ==false)
        {
            if (caster.team != target.team)
            {
                int damageStuff;
                damageStuff = TrueDamageCalculator(spell.spellDamageMax, spell.spellDamageMin, caster.damageChange, target.armorChange, caster.damagePlus, target.armorPlus);
            }
        }
        else if (spell.healsAlly == true)
        {
            if (caster.team = target.team)
            {
                int healingIsFun;
                healingIsFun = TrueHealCalculator(spell.spellDamageMax, spell.spellDamageMin, target.healsReceived);
            }
            else
            {
                int damageStuff;
                damageStuff = TrueDamageCalculator(spell.spellDamageMax, spell.spellDamageMin, caster.damageChange, target.armorChange, caster.damagePlus, target.armorPlus);
            }
        }
        else
        {
            int damageStuff;
            damageStuff = TrueDamageCalculator(spell.spellDamageMax, spell.spellDamageMin, caster.damageChange, target.armorChange, caster.damagePlus, target.armorPlus);
        }



    }
	public void Aftermath()
    {

        playerBehaviour.currentCharacter.currentAp -= currentSpell.spellApCost;
        spellOpen = false;
    }

	void Update () {
		
	}

    public int MinDamCacl(int damMin, float damChange,float armorChange, int damPlus, int armorPlus)
    {
        int tempdamage = Mathf.RoundToInt(damMin * (1 + (damChange - armorChange)) + (damPlus - armorPlus));

        return tempdamage;
    }

    public int MaxDamCacl(int damMax, float damChange, float armorChange, int damPlus, int armorPlus)
    {
        int tempdamage = Mathf.RoundToInt(damMax * (1 + (damChange - armorChange)) + (damPlus - armorPlus));

        return tempdamage;
    }

    public int TrueDamageCalculator(int damMax, int damMin, float damChange, float armorChange, int damPlus, int armorPlus)
    {
        int tempdamageMin = MinDamCacl(damMin, damChange, armorChange, damPlus, armorPlus);
        int tempdamageMax = MaxDamCacl(damMax, damChange, armorChange, damPlus, armorPlus);
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

    public void Spell1Cast()
    {
        if (playerBehaviour.currentCharacter.currentAp >= cv.spell_1.spellApCost && spell1CastedThisTurn <= cv.spell_1.spellCastPerturn)
        {
            currentSpell = cv.spell_1;
            spellOpen = true;
            
        }
    }

    public void Spell2Cast()
    {
        if (playerBehaviour.currentCharacter.currentAp >= cv.spell_2.spellApCost && spell1CastedThisTurn <= cv.spell_2.spellCastPerturn)
        {
            currentSpell = cv.spell_2;
            spellOpen = true; 
        }
    }

    public void Spell3Cast()
    {
        if (playerBehaviour.currentCharacter.currentAp >= cv.spell_3.spellApCost && spell1CastedThisTurn <= cv.spell_3.spellCastPerturn)
        {
            currentSpell = cv.spell_3;
            spellOpen = true; 
        }
    }

    public void Spell4Cast()
    {
        if (playerBehaviour.currentCharacter.currentAp >= cv.spell_4.spellApCost && spell1CastedThisTurn <= cv.spell_4.spellCastPerturn)
        {
            currentSpell = cv.spell_4;
            spellOpen = true; 
        }
    }

    public void Spell5Cast()
    {
        if (playerBehaviour.currentCharacter.currentAp >= cv.spell_5.spellApCost && spell1CastedThisTurn <= cv.spell_5.spellCastPerturn)
        {
            currentSpell = cv.spell_5;
            spellOpen = true; 
        }
    }

    public void Spell6Cast()
    {
        if (playerBehaviour.currentCharacter.currentAp >= cv.spell_6.spellApCost && spell1CastedThisTurn <= cv.spell_6.spellCastPerturn)
        {
            currentSpell = cv.spell_6;
            spellOpen = true; 
        }
    }

    public void SpellCancel()
    {
        currentSpell = null;
        spellOpen = false;
    }
}
