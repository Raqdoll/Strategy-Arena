﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCast : MonoBehaviour {

    public PlayerBehaviour playerBehaviour;
    public MouseController mc;
    public Abilities abilities;
    public GridController gridController;
    public CharacterValues cv;  //Active player's charactervalues?
    TurnManager turnManager;
    public HitText hitText;
    public StatusEffects sEffects;
    public SpellValues currentSpell;
    public bool spellOpen = false;
    public List<GameObject> bodyList;
    public EndTurnBlink endTurn;

    //Käytä spellin 

    //
    //    hitText.DamageText(target, damage);
    //



    public Button spellButton1, spellButton2, spellButton3, spellButton4, spellButton5, spellButton6;
    public Text hpText, apText, mpText;


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

        hitText = GetComponent<HitText>();

        turnManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TurnManager>();
        gridController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridController>();
        abilities = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<Abilities>();
        turnManager.TurnChange += HandleTurnChange;
        turnManager.TurnEnd += HandleTurnEnd;
        if (!gridController)
            Debug.LogWarning("Gridcontroller is null!");
        if (!turnManager)
            Debug.Log("Could not find turnManager component in parents!");
        if (!abilities)
            Debug.Log("Could not find abilities component");
        if (!sEffects)
            sEffects = GameObject.FindGameObjectWithTag("GameController").GetComponent<StatusEffects>();
        //cv = turnManager.teamManager.activePlayer.thisCharacter;
        UpdateHpApMp();
    }

    private void OnDestroy()
    {
        turnManager.TurnChange -= HandleTurnChange;
        turnManager.TurnEnd -= HandleTurnEnd;
    }

    #region EventHandlers

    private void HandleTurnChange(PlayerInfo player)
    {
        cv = player.thisCharacter;
        //Debug.Log("Handling event");
        spellButton1.GetComponent<Image>().sprite = cv.spell_1.spellIcon;
        spellButton2.GetComponent<Image>().sprite = cv.spell_2.spellIcon;
        spellButton3.GetComponent<Image>().sprite = cv.spell_3.spellIcon;
        spellButton4.GetComponent<Image>().sprite = cv.spell_4.spellIcon;
        spellButton5.GetComponent<Image>().sprite = cv.spell_5.spellIcon;
        spellButton6.GetComponent<Image>().sprite = cv.spell_6.spellIcon;

        spellButton1.GetComponent<Tooltip>().spell = cv.spell_1;
        spellButton2.GetComponent<Tooltip>().spell = cv.spell_2;
        spellButton3.GetComponent<Tooltip>().spell = cv.spell_3;
        spellButton4.GetComponent<Tooltip>().spell = cv.spell_4;
        spellButton5.GetComponent<Tooltip>().spell = cv.spell_5;
        spellButton6.GetComponent<Tooltip>().spell = cv.spell_6;

        HandleCooldownDecrease(cv.spell_1);
        HandleCooldownDecrease(cv.spell_2);
        HandleCooldownDecrease(cv.spell_3);
        HandleCooldownDecrease(cv.spell_4);
        HandleCooldownDecrease(cv.spell_5);
        HandleCooldownDecrease(cv.spell_6);

        playerBehaviour.UpdateTabs();
        UpdateHpApMp();
    }

    private void HandleTurnEnd(PlayerInfo player)
    {
        DisableButtonsIfNotAp();
    }

    #endregion

    public void UpdateHpApMp()
    {
        hpText.text = "HP: " + cv.currentHP + " / " + cv.currentMaxHP;
        apText.text = "AP: " + cv.currentAp;
        mpText.text = "MP: " + cv.currentMp;
        DisableButtonsIfNotAp();
        endTurn.CheckBlink(cv);
    }

    public void DisableButtonsIfNotAp()
    {
        if(cv.spell_1.spellApCost > cv.currentAp || abilities.SpellCooldownCheck(cv.spell_1) == false)
        {
            spellButton1.interactable = false;
            endTurn.s1 = false;
        }
        else
        {
            spellButton1.interactable = true;
            endTurn.s1 = true;
        }
        if (cv.spell_2.spellApCost > cv.currentAp || abilities.SpellCooldownCheck(cv.spell_2) == false)
        {
            spellButton2.interactable = false;
            endTurn.s2 = false;
        }
        else
        {
            spellButton2.interactable = true;
            endTurn.s2 = true;
        }
        if (cv.spell_3.spellApCost > cv.currentAp || abilities.SpellCooldownCheck(cv.spell_3) == false)
        {
            spellButton3.interactable = false;
            endTurn.s3 = false;
        }
        else
        {
            spellButton3.interactable = true;
            endTurn.s3 = true;
        }
        if (cv.spell_4.spellApCost > cv.currentAp || abilities.SpellCooldownCheck(cv.spell_4) == false)
        {
            spellButton4.interactable = false;
            endTurn.s4 = false;
        }
        else
        {
            spellButton4.interactable = true;
            endTurn.s4 = true;
        }
        if (cv.spell_5.spellApCost > cv.currentAp || abilities.SpellCooldownCheck(cv.spell_5) == false)
        {
            spellButton5.interactable = false;
            endTurn.s5 = false;
        }
        else
        {
            spellButton5.interactable = true;
            endTurn.s5 = true;
        }
        if (cv.spell_6.spellApCost > cv.currentAp || abilities.SpellCooldownCheck(cv.spell_6) == false)
        {
            spellButton6.interactable = false;
            endTurn.s6 = false;
        }
        else
        {
            spellButton6.interactable = true;
            endTurn.s6 = true;
        }
        //Tabs
        if (playerBehaviour.GetTab(cv) != null && cv.spell_1.spellCooldownLeft > 1)
        {
            playerBehaviour.GetTab(cv).spell1.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            if (playerBehaviour.GetTab(cv) != null)
            {
                playerBehaviour.GetTab(cv).spell1.GetComponent<Image>().color = Color.white;
            }
        }

        if (playerBehaviour.GetTab(cv) != null && cv.spell_2.spellCooldownLeft > 1)
        {
            playerBehaviour.GetTab(cv).spell2.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            if (playerBehaviour.GetTab(cv) != null)
            {
                playerBehaviour.GetTab(cv).spell2.GetComponent<Image>().color = Color.white;
            }
        }

        if (playerBehaviour.GetTab(cv) != null && cv.spell_3.spellCooldownLeft > 1)
        {
            playerBehaviour.GetTab(cv).spell3.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            if (playerBehaviour.GetTab(cv) != null)
            {
                playerBehaviour.GetTab(cv).spell3.GetComponent<Image>().color = Color.white;
            }
        }

        if (playerBehaviour.GetTab(cv) != null && cv.spell_4.spellCooldownLeft > 1)
        {
            playerBehaviour.GetTab(cv).spell4.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            if (playerBehaviour.GetTab(cv) != null)
            {
                playerBehaviour.GetTab(cv).spell4.GetComponent<Image>().color = Color.white;
            }
        }

        if (playerBehaviour.GetTab(cv) != null && cv.spell_5.spellCooldownLeft > 1)
        {
            playerBehaviour.GetTab(cv).spell5.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            if (playerBehaviour.GetTab(cv) != null)
            {
                playerBehaviour.GetTab(cv).spell5.GetComponent<Image>().color = Color.white;
            }
        }

        if (playerBehaviour.GetTab(cv) != null && cv.spell_6.spellCooldownLeft > 1)
        {
            playerBehaviour.GetTab(cv).spell6.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            if (playerBehaviour.GetTab(cv) != null)
            {
                playerBehaviour.GetTab(cv).spell6.GetComponent<Image>().color = Color.white;
            }
        }
    }



    public void CastSpell(SpellValues spell, CharacterValues caster,Tile currentMouseTile)
    {
        playerBehaviour.aControll.PlayAttack(caster);
        playerBehaviour.aControll.PlaySpell(spell);


        Tile casterTile = gridController.GetTile(caster.currentTile.x, caster.currentTile.z);
        Tile targetTile = currentMouseTile;
        //Tile temp1 = gridController.GetTile(caster.currentTile.x,caster.currentTile.z);
        //Tile temp2 = gridController.GetTile(target.currentTile.x,target.currentTile.z);
        List<Tile> targetsList = abilities.AreaType(currentSpell.mySpellAreaType);
        int leach = 0;
        foreach (var item in targetsList)
        {
            PlayerInfo checker = item.CharCurrentlyOnTile;
            CharacterValues target = null;
            if (checker)
                target = checker.thisCharacter;

            if (target)
            {
                int damageStuff = 0;
                int healingIsFun = 0;

                if (spell.healsAlly == true)
                {
                    if (target.team == caster.team)
                    {
                        healingIsFun = TrueHealCalculator(spell.spellHealMax, spell.spellHealMin, target.healsReceived);
                    }
                    else
                    {
                        damageStuff = TrueDamageCalculator(spell.spellDamageMax, spell.spellDamageMin, caster.damageChange, target.armorChange, caster.damagePlus, target.armorPlus);
                    }
                }
                else if (spell.hurtsAlly == false)
                {
                    if (target.team != caster.team)
                    {
                        damageStuff = TrueDamageCalculator(spell.spellDamageMax, spell.spellDamageMin, caster.damageChange, target.armorChange, caster.damagePlus, target.armorPlus);
                    }
                }
                else
                {
                    damageStuff = TrueDamageCalculator(spell.spellDamageMax, spell.spellDamageMin, caster.damageChange, target.armorChange, caster.damagePlus, target.armorPlus);
                }
                leach = leach + damageStuff;
                GetHit(target, damageStuff);
                GetHealed(target, healingIsFun);
                if (spell.effect && spell.effectOnTarget == true && target != caster)
                {
                    sEffects.ApplyEffect(caster, spell.effect, target);
                    playerBehaviour.UpdateTabs();
                }
                if (spell.effect && spell.effectOnCaster == true && target == caster)
                {
                    sEffects.ApplyEffect(caster, spell.effect, target);
                    playerBehaviour.UpdateTabs();
                }

            }
        }
        if (spell.damageStealsHp == true)
        {
            StealHp(caster, leach); 
        }
        if (spell.spellPull != 0)
        {
            //abilities.SpellPull(spell.mySpellPullType);
        }
        if (spell.spellPushback != 0)
        {
            //abilities.SpellPush(spell.mySpellPushType);
        }

        if (spell.moveCloserToTarget != 0)
        {
            //abilities.WalkTowardsTarget();
        }
        if (spell.moveAwayFromTarget != 0)
        {
            //abilities.MoveAwayFromTarget();
        }

        if (spell.teleportToTarget == true)
        {
            abilities.CasterTeleport(casterTile);
        }
        if (spell.chagePlaceWithTarget == true)
        {
            abilities.TeleportSwitch(casterTile, targetTile);
        }
        HandleCoolDownIncrease(spell);
        UpdateHpApMp();
    }

    //Deal the actual damage V V V
    public void GetHit(CharacterValues target, int damage)
    {
        //Damage dealt
        target.currentHP -= damage;
        //Popup
        if (damage != 0)
        {
            hitText.DamageText(target, damage * (-1)); 
        }
        //Max health reduction
        target.currentMaxHP -= Mathf.RoundToInt(damage * turnManager.maxHealthReduction);
        if(target.currentHP <= 0)
        {
            GetDead(target);
        }
        playerBehaviour.UpdateTabs();
    }
    //Deal the actual healing V V V
    public void GetHealed(CharacterValues target, int heal)
    {
        target.currentHP += heal;
        //Popup
        if (target.currentHP != target.currentMaxHP && heal != 0)
        {
            hitText.DamageText(target, heal); 
        }
        //Max hp check
        if (target.currentHP > target.currentMaxHP)
        {
            target.currentHP = target.currentMaxHP;
        }
        playerBehaviour.UpdateTabs();
    }

    public void GetDead(CharacterValues target)
    {
        target.dead = true; //  muista korjata startissa
        foreach(var temp in bodyList)
        {
            PlayerInfo info = temp.GetComponentInParent<PlayerInfo>();
            CharacterValues ego = info.thisCharacter;
            PlayerMovement kaikkiHajoaa = info.gameObject.GetComponent<PlayerMovement>();
            kaikkiHajoaa.CurrentTile = null;
            if (ego == target)
            {
                temp.SetActive(false);
            }
        }
    }

    public void ActivateBodies()
    {
        foreach (var temp in bodyList)
        {
                temp.SetActive(true);
        }
    }

	public void Aftermath()
    {

        playerBehaviour.currentCharacter.currentAp -= currentSpell.spellApCost;
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
        UpdateHpApMp();
        playerBehaviour.UpdateTabs();
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
        int tempHealMax = MaxHealCacl(healMax, heals);
        int trueHeal = UnityEngine.Random.Range(tempHealMin, tempHealMax);

        return trueHeal;
    }
    public void StealHp(CharacterValues target, int damage)
    {
        int heal = damage / 2;
        target.currentHP += heal;
        if (target.currentHP > target.currentMaxHP)
        {
            target.currentHP = target.currentMaxHP;
        }
        playerBehaviour.UpdateTabs();
        hitText.DamageText(target, heal);
    }


    public void HandleCooldownReset(SpellValues spell)
    {
        spell.spellInitialCooldowncounter = spell.spellInitialCooldown;
        spell.spellCooldownLeft = 0;
        spell.spellCastPerturncounter = 0;
    }
    public void HandleCooldownDecrease(SpellValues spell)
    {
        if (spell.spellInitialCooldowncounter > 0)
            spell.spellInitialCooldowncounter--;

        if (spell.spellCooldownLeft > 0)
        spell.spellCooldownLeft--;

        spell.spellCastPerturncounter = 0;
    }
    public void HandleCoolDownIncrease(SpellValues spell)
    {
        if(spell.spellCastPerturn != 0)
        {
            spell.spellCastPerturncounter++;
        }
        if((spell.spellCastPerturncounter >= spell.spellCastPerturn && spell.spellCastPerturn != 0) || spell.spellCooldown > 0)
        {
            spell.spellCooldownLeft = spell.spellCooldown;
        }
    }

    public void Spell1Cast()
    {
            SpellCancel();
            currentSpell = cv.spell_1;
            spellOpen = true;
    }
    public void Spell2Cast()
    {
            SpellCancel();
            currentSpell = cv.spell_2;
            spellOpen = true; 
    }
    public void Spell3Cast()
    {
            SpellCancel();
            currentSpell = cv.spell_3;
            spellOpen = true; 
    }
    public void Spell4Cast()
    {
            SpellCancel();
            currentSpell = cv.spell_4;
            spellOpen = true; 
    }
    public void Spell5Cast()
    {
            SpellCancel();
            currentSpell = cv.spell_5;
            spellOpen = true; 
    }
    public void Spell6Cast()
    {
            SpellCancel();
            currentSpell = cv.spell_6;
            spellOpen = true; 
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
