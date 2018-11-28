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
    public int spell1CastedThisTurn = 0;
    public int spell2CastedThisTurn = 0;
    public int spell3CastedThisTurn = 0;
    public int spell4CastedThisTurn = 0;
    public int spell5CastedThisTurn = 0;
    public int spell6CastedThisTurn = 0;
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

        hpText.text = "HP: " + cv.currentHP + " / " + cv.maxHP;
        apText.text = "AP: " + cv.currentAp;
        mpText.text = "MP: " + cv.currentMp;    //  <----- ^---- Nää kannattaa ehkä siirtää jonnekki järkevämpään scriptiin kun spell castiin?

        
    }
    //public bool needTarget = false; //<
    //public bool needFreeSquare = false; //<
    //public bool inCooldown = false; //<
    //public int spellCooldownLeft;   //<
    //public int spellInitialCooldown;    //<
    //public int spellCooldown;   //<
    //public int spellCastPerturn;    //<
    //public int castPerTarget;   //<





    public void CastSpell(SpellValues spell, CharacterValues caster,Tile currentMouseTile)
    {
        //playerBehaviour.aControll.PlayAttack(caster, playerBehaviour.aControll.temp);
        playerBehaviour.aControll.PlaySpell(spell, playerBehaviour.aControll.temp2);


        Tile casterTile = gridController.GetTile(caster.currentTile.x, caster.currentTile.z);
        Tile targetTile = currentMouseTile;
        //Tile temp1 = gridController.GetTile(caster.currentTile.x,caster.currentTile.z);
        //Tile temp2 = gridController.GetTile(target.currentTile.x,target.currentTile.z);
        List<Tile> targetsList = abilities.AreaType(currentSpell.mySpellAreaType);
        foreach (var item in targetsList)
        {
            PlayerInfo checker = item.CharCurrentlyOnTile;
            CharacterValues target = null;
            if (checker)
                target = checker.thisCharacter;

            if (target)
            {
                Debug.Log("annetaan vAhinkoa");
                int damageStuff = 0;
                int healingIsFun = 0;

                if (spell.healsAlly == true)
                {
                    if (target.team == caster.team)
                    {
                        healingIsFun = TrueHealCalculator(spell.spellHealMax, spell.spellHealMin, target.healsReceived);
                        Debug.Log("annetaan healia");
                    }
                    else
                    {
                        damageStuff = TrueDamageCalculator(spell.spellDamageMax, spell.spellDamageMin, caster.damageChange, target.armorChange, caster.damagePlus, target.armorPlus);
                        Debug.Log("vahinko vihu");
                    }
                }
                else if (spell.hurtsAlly == false)
                {
                    if (target.team != caster.team)
                    {
                        damageStuff = TrueDamageCalculator(spell.spellDamageMax, spell.spellDamageMin, caster.damageChange, target.armorChange, caster.damagePlus, target.armorPlus);
                        Debug.Log("vahinko vihu");
                    }
                }
                else
                {
                    damageStuff = TrueDamageCalculator(spell.spellDamageMax, spell.spellDamageMin, caster.damageChange, target.armorChange, caster.damagePlus, target.armorPlus);
                    Debug.Log("vahinko vihu");
                }
                GetHit(target, damageStuff);
                GetHealed(target, healingIsFun);

                if (spell.effect)
                {
                    sEffects.ApplyEffect(caster, spell.effect, target);
                    Debug.Log("Effect annettu");
                    playerBehaviour.UpdateTabs();
                }

            }
        }
        if (spell.spellPull != 0)
        {
            abilities.SpellPull(spell.mySpellPullType);
        }
        if (spell.spellPushback != 0)
        {
            abilities.SpellPush(spell.mySpellPushType);
            Debug.Log("spell push");
        }

        if (spell.moveCloserToTarget != 0)
        {
            abilities.WalkTowardsTarget();
            Debug.Log("moving towards");
        }
        if (spell.moveAwayFromTarget != 0)
        {
            abilities.MoveAwayFromTarget();
            Debug.Log("moving away");
        }

        if (spell.teleportToTarget == true)
        {
            abilities.CasterTeleport(casterTile);
            Debug.Log("telepoting to target");
        }
        if (spell.chagePlaceWithTarget == true)
        {
            abilities.TeleportSwitch(casterTile, targetTile);
            Debug.Log("switching places");
        }

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
