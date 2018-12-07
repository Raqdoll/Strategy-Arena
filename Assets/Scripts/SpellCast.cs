using System;
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
    }

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

        UpdateHpApMp();
        
    }

    public void UpdateHpApMp()
    {
        hpText.text = "HP: " + cv.currentHP + " / " + cv.maxHP;
        apText.text = "AP: " + cv.currentAp;
        mpText.text = "MP: " + cv.currentMp;
        DisableButtonsIfNotAp();
    }

        HanddleCooldownDecrease(cv.spell_1);
        HanddleCooldownDecrease(cv.spell_2);
        HanddleCooldownDecrease(cv.spell_3);
        HanddleCooldownDecrease(cv.spell_4);
        HanddleCooldownDecrease(cv.spell_5);
        HanddleCooldownDecrease(cv.spell_6);
    public void DisableButtonsIfNotAp()
    {
        if(cv.spell_1.spellApCost > cv.currentAp)
        {
            spellButton1.interactable = false;
        }
        else
        {
            spellButton1.interactable = true;
        }
        if (cv.spell_2.spellApCost > cv.currentAp)
        {
            spellButton2.interactable = false;
        }
        else
        {
            spellButton2.interactable = true;
        }
        if (cv.spell_3.spellApCost > cv.currentAp)
        {
            spellButton3.interactable = false;
        }
        else
        {
            spellButton3.interactable = true;
        }
        if (cv.spell_4.spellApCost > cv.currentAp)
        {
            spellButton4.interactable = false;
        }
        else
        {
            spellButton4.interactable = true;
        }
        if (cv.spell_5.spellApCost > cv.currentAp)
        {
            spellButton5.interactable = false;
        }
        else
        {
            spellButton5.interactable = true;
        }
        if (cv.spell_6.spellApCost > cv.currentAp)
        {
            spellButton6.interactable = false;
        }
        else
        {
            spellButton6.interactable = true;
        }
    }



    public void CastSpell(SpellValues spell, CharacterValues caster,Tile currentMouseTile)
    {
        //playerBehaviour.aControll.PlayAttack(caster, playerBehaviour.aControll.temp);
        playerBehaviour.aControll.PlaySpell(spell, playerBehaviour.aControll.temp2);


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
                HandleCoolDownIncrease(spell);
                if (spell.effect)
                {
                    sEffects.ApplyEffect(caster, spell.effect, target);
                    Debug.Log("Effect annettu");
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
            abilities.SpellPull(spell.mySpellPullType);
        }
        if (spell.spellPushback != 0)
        {
            abilities.SpellPush(spell.mySpellPushType);
        }

        if (spell.moveCloserToTarget != 0)
        {
            abilities.WalkTowardsTarget();
        }
        if (spell.moveAwayFromTarget != 0)
        {
            abilities.MoveAwayFromTarget();
        }

        if (spell.teleportToTarget == true)
        {
            abilities.CasterTeleport(casterTile);
        }
        if (spell.chagePlaceWithTarget == true)
        {
            abilities.TeleportSwitch(casterTile, targetTile);
        }
        UpdateHpApMp();
    }

    //Deal the actual damage V V V
    public void GetHit(CharacterValues target, int damage)
    {
        target.currentHP -= damage;
        playerBehaviour.UpdateTabs();
    }
    //Deal the actual healing V V V
    public void GetHealed(CharacterValues target, int heal)
    {
        target.currentHP += heal;
        if(target.currentHP > target.maxHP)
        {
            target.currentHP = target.maxHP;
        }
        playerBehaviour.UpdateTabs();
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
        int tempHealMax = MaxHealCacl(healMin, heals);
        int trueHeal = UnityEngine.Random.Range(tempHealMin, tempHealMax);

        return trueHeal;
    }
    public void StealHp(CharacterValues target, int damage)
    {
        int heal = damage / 2;
        target.currentHP += heal;
        if (target.currentHP > target.maxHP)
        {
            target.currentHP = target.maxHP;
        }
        playerBehaviour.UpdateTabs();
    }

    public void HanddleCooldownDecrease(SpellValues spell)
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
        if(spell.spellCastPerturncounter >= spell.spellCastPerturn)
        {
            spell.spellCooldownLeft = spell.spellCooldown;
        }
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
