using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCast : MonoBehaviour {

    public PlayerBehaviour playerBehaviour;
    public MouseController mc;
    public CharacterValues cv;  //Active player's charactervalues?
    TurnManager turnManager;
    public SpellValues currentSpell;
    public bool spellOpen = false;
    public int spell1CastedThisTurn = 0;
    public int spell2CastedThisTurn = 0;
    public int spell3CastedThisTurn = 0;
    public int spell4CastedThisTurn = 0;
    public int spell5CastedThisTurn = 0;
    public int spell6CastedThisTurn = 0;
    


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

        turnManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TurnManager>();
        turnManager.TurnChange += HandleTurnChange;
    }

    private void OnDestroy()
    {
        turnManager.TurnChange -= HandleTurnChange;
    }

    private void HandleTurnChange(PlayerInfo player)
    {
        cv = player.thisCharacter;
        //Debug.Log("Handling event");
    }

    public void CastSpell(SpellValues spell, CharacterValues caster, CharacterValues target)
    {
        int damageStuff = 0;
        int healingIsFun = 0;
        if (spell.hurtsAlly ==false)
        {
            if (caster.team != target.team)
            {
                
                damageStuff = TrueDamageCalculator(spell.spellDamageMax, spell.spellDamageMin, caster.damageChange, target.armorChange, caster.damagePlus, target.armorPlus);
            }
        }
        else if (spell.healsAlly == true)
        {
            if (caster.team = target.team)
            {
                
                healingIsFun = TrueHealCalculator(spell.spellHealMax, spell.spellHealMin, target.healsReceived);
            }
            else
            {
                
                damageStuff = TrueDamageCalculator(spell.spellDamageMax, spell.spellDamageMin, caster.damageChange, target.armorChange, caster.damagePlus, target.armorPlus);
            }
        }
        else
        {
            
            damageStuff = TrueDamageCalculator(spell.spellDamageMax, spell.spellDamageMin, caster.damageChange, target.armorChange, caster.damagePlus, target.armorPlus);
        }

        GetHit(target, damageStuff);
        GetHealed(target, healingIsFun);
        playerBehaviour.UpdateTabs();

    }
    //Deal the actual damage V V V
    public void GetHit(CharacterValues target, int damage)
    {
        target.currentHP -= damage;
    }
    //Deal the actual healing V V V
    public void GetHealed(CharacterValues target, int heal)
    {
        target.currentHP += heal;
        if(target.currentHP > target.maxHP)
        {
            target.currentHP = target.maxHP;
        }
    }



	public void Aftermath()
    {

        playerBehaviour.currentCharacter.currentAp -= currentSpell.spellApCost;
        spellOpen = false;
    }

	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Spell1Cast();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Spell2Cast();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Spell3Cast();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Spell4Cast();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Spell5Cast();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Spell6Cast();
        }

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
        int trueDamage = UnityEngine.Random.Range(tempdamageMin, tempdamageMax);

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

    public int TrueHealCalculator(int healMax, int healMin, float heals)
    {
        int tempHealMin = MinHealCacl(healMin, heals);
        int tempHealMax = MaxHealCacl(healMin, heals);
        int trueHeal = UnityEngine.Random.Range(tempHealMin, tempHealMax);

        return trueHeal;
    }

    public void Spell1Cast()
    {
            SpellCancel();
        if (cv.currentAp >= cv.spell_1.spellApCost && spell1CastedThisTurn <= cv.spell_1.spellCastPerturn)
        {
            currentSpell = cv.spell_1;
            spellOpen = true;
        }
    }

    public void Spell2Cast()
    {
            SpellCancel();

        if (cv.currentAp >= cv.spell_2.spellApCost && spell2CastedThisTurn <= cv.spell_2.spellCastPerturn)
        {
            currentSpell = cv.spell_2;
            spellOpen = true; 
        }
    }

    public void Spell3Cast()
    {
            SpellCancel();
        
        if (cv.currentAp >= cv.spell_3.spellApCost && spell3CastedThisTurn <= cv.spell_3.spellCastPerturn)
        {
            currentSpell = cv.spell_3;
            spellOpen = true; 
        }
    }

    public void Spell4Cast()
    {
            SpellCancel();
        
        if (cv.currentAp >= cv.spell_4.spellApCost && spell4CastedThisTurn <= cv.spell_4.spellCastPerturn)
        {
            currentSpell = cv.spell_4;
            spellOpen = true; 
        }
    }

    public void Spell5Cast()
    {
            SpellCancel();
        
        if (cv.currentAp >= cv.spell_5.spellApCost && spell5CastedThisTurn <= cv.spell_5.spellCastPerturn)
        {
            currentSpell = cv.spell_5;
            spellOpen = true; 
        }
    }

    public void Spell6Cast()
    {
            SpellCancel();
        
        if (cv.currentAp >= cv.spell_6.spellApCost && spell6CastedThisTurn <= cv.spell_6.spellCastPerturn)
        {
            currentSpell = cv.spell_6;
            spellOpen = true; 
        }
    }

    public void SpellCancel()
    {
        if (mc.rangeTiles != null)
        {
            mc.ResetTileMaterials(mc.rangeTiles);
            mc.rangeTiles = null;
        }
        if (mc.nullTiles != null)
        {
            mc.ResetTileMaterials(mc.nullTiles);
            mc.nullTiles = null;
        }
        if (mc.targetedTiles != null)
        {
            mc.ResetTileMaterials(mc.targetedTiles);
            mc.targetedTiles = null;
        }
        currentSpell = null;
        spellOpen = false;
    }
}
