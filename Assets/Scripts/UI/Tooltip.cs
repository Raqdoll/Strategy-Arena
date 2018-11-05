using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public SpellValues spell;
    public CharacterValues character;
    public EffectValues effect;
    UImanager ui;
    Text _text;

    void Start () {
        if (!ui)
            ui = GameObject.FindGameObjectWithTag("UIcanvas").GetComponent<UImanager>();
    }

    public void UpdateInfo(SpellValues x)
    {
        spell = x;
    }
    public void UpdateInfo(CharacterValues x)
    {
        character = x;
    }
    public void UpdateInfo(EffectValues x)
    {
        effect = x;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (spell && !character && !effect)
        {
            ui._text = "";
            ui._text += spell.mySpellName + "\n";
            ui._text += "\nAP: " + spell.spellApCost;
            ui._text += "\nRange: " + spell.mySpellRangeType + " : " + spell.spellRangeMin;
            if(spell.spellRangeMin != spell.spellRangeMax)
            {
                ui._text += " - " + spell.spellRangeMax;
            }
            ui._text += "\nArea of effect: " + spell.mySpellAreaType + " : " + spell.aoeRange;
            ui._text += "\n";
            if (spell.spellDamageMin != 0)
            {
                ui._text += "\n" + "Base damage: " + spell.spellDamageMin + " - " + spell.spellDamageMax; 
            }
            if (spell.spellHealMin != 0)
            {
                ui._text += "\n" + "Base heal: " + spell.spellHealMin + " - " + spell.spellHealMax + "\n";
            }

            if (spell.needLineOfSight == false)
            {
                ui._text += "\n";
                ui._text += "\nNeeds line of sight: ";
                ui._text += "No";
            }
            
            if (spell.needTarget == true)
            {
                ui._text += "\nNeeds a target: ";
                ui._text += "Yes";
            }
            if (spell.needFreeSquare == true)
            {
                ui._text += "\nNeeds a free square: ";
                ui._text += "Yes";
            }



            if (spell.effect != null)
            {
                ui._text += "\n";
                //Effect info
                if (spell.effect.immune == true)
                {
                    ui._text += "\nImmune to damage: " + spell.effect.effectDuration + " turn(s)";
                }
                if (spell.effect.heavyState == true)
                {
                    ui._text += "\nHeavy state: " + spell.effect.effectDuration + " turn(s)";
                }
                if (spell.effect.damageModifyPlus != 0)
                {
                    if (spell.effect.damageModifyPlus > 0)
                    {
                        ui._text += "\nDamage: +" + effect.damageModifyPlus; 
                    }
                    else
                    {
                        ui._text += "\nDamage: " + effect.damageModifyPlus;
                    }
                }
                if (spell.effect.damageModifyPercent != 0)
                {
                    if (spell.effect.damageModifyPercent > 0)
                    {
                        ui._text += "\nDamage: +" + effect.damageModifyPercent * 100 + "%"; 
                    }
                    else
                    {
                        ui._text += "\nDamage: " + effect.damageModifyPercent * 100 + "%";
                    }
                }
                if (spell.effect.armorModifyPlus != 0)
                {
                    if (spell.effect.armorModifyPlus > 0)
                    {
                        ui._text += "\nArmor: +" + effect.damageModifyPlus; 
                    }
                    else
                    {
                        ui._text += "\nArmor: " + effect.damageModifyPlus;
                    }
                }
                if (spell.effect.armorModifyPercent != 0)
                {
                    if (spell.effect.armorModifyPercent > 0)
                    {
                        ui._text += "\nArmor: +" + effect.damageModifyPercent * 100 + "%"; 
                    }
                    else
                    {
                        ui._text += "\nArmor: " + effect.damageModifyPercent * 100 + "%";
                    }
                }
                if (spell.effect.healModify != 0)
                {
                    if (spell.effect.healModify > 0)
                    {
                        ui._text += "\nHeal modifier: +" + effect.healModify * 100 + "%"; 
                    }
                    else
                    {
                        ui._text += "\nHeal modifier: " + effect.healModify * 100 + "%";
                    }
                }
                if (spell.effect.apModify != 0)
                {
                    if (spell.effect.apModify > 0)
                    {
                        ui._text += "\nAP: +" + effect.apModify;
                    }
                    else
                    {
                        ui._text += "\nAP: " + effect.apModify;
                    }
                }
                if (spell.effect.mpModify != 0)
                {
                    if (spell.effect.mpModify > 0)
                    {
                        ui._text += "\nMP: +" + effect.mpModify; 
                    }
                    else
                    {
                        ui._text += "\nMP: " + effect.mpModify;
                    }
                }



                ui._text += "\n";
            }



            if (spell.spellPushback != 0)
            {
                ui._text += "\nPushback: " + spell.spellPushback;
            }
            if (spell.spellPull != 0)
            {
                ui._text += "\nPull: " + spell.spellPull;
            }
            if (spell.spellPushback != 0)
            {
                ui._text += "\nPushback: " + spell.spellPushback;
            }
            ui._text += "\n";
            if (spell.spellInitialCooldown != 0)
            {
                ui._text += "\nInitial cooldown: " + spell.spellInitialCooldown;
            }
            if (spell.spellCooldown != 0)
            {
                ui._text += "\nCooldown: " + spell.spellCooldown;
            }
            if (spell.spellCastPerturn != 0)
            {
                ui._text += "\nCast per turn: " + spell.spellCastPerturn;
            }
            if (spell.castPerTarget != 0)
            {
                ui._text += "\nCast per target: " + spell.castPerTarget;
            }
            ui._text += "\n";
            ui._text += "\n" + spell.description;
            

            ui.ShowTooltip();
        }
        else if (!spell && character && !effect)
        {
            ui._text = "";
            ui._text += character.name;
            ui._text += "\n";
            ui._text += "\nHP: " + character.currentHP + " / " + character.maxHP;
            ui._text += "\nAP: " + character.currentAp;
            ui._text += "\nMP: " + character.currentMp;
            ui.ShowTooltip();
        }
        else if (!spell && !character && effect)
        {
            ui._text = "";
            ui._text += effect.effectName;
            ui._text += "\n";
            ui._text += "\nTurns remaining: " + effect.remainingTurns;
            ui._text += "\n";
            if (effect.immune == true)
            {
                ui._text += "\nImmune to damage";
            }
            if (effect.heavyState == true)
            {
                ui._text += "\nHeavy state";
            }
            if(effect.damageModifyPlus != 0)
            {
                if (effect.damageModifyPlus > 0)
                {
                    ui._text += "\nDamage: +" + effect.damageModifyPlus;
                }
                else
                {
                    ui._text += "\nDamage: " + effect.damageModifyPlus;
                }
            }
            if (effect.damageModifyPercent != 0)
            {
                if (effect.damageModifyPercent > 0)
                {
                    ui._text += "\nDamage: +" + effect.damageModifyPercent * 100 + "%"; 
                }
                else
                {
                    ui._text += "\nDamage: " + effect.damageModifyPercent * 100 + "%";
                }
            }
            if (effect.armorModifyPlus != 0)
            {
                if (effect.armorModifyPlus > 0)
                {
                    ui._text += "\nArmor: +" + effect.armorModifyPlus; 
                }
                else
                {
                    ui._text += "\nArmor: " + effect.armorModifyPlus;
                }
            }
            if (effect.armorModifyPercent != 0)
            {
                if (effect.armorModifyPercent > 0)
                {
                    ui._text += "\nArmor: +" + effect.armorModifyPercent * 100 + "%";
                }
                else
                {
                    ui._text += "\nArmor: " + effect.armorModifyPercent * 100 + "%";
                }
            }
            if (effect.healModify != 0)
            {
                if (effect.healModify > 0)
                {
                    ui._text += "\nHeal modifier: +" + effect.healModify * 100 + "%";
                }
                else
                {
                    ui._text += "\nHeal modifier: " + effect.healModify * 100 + "%";
                }
            }
            if (effect.apModify != 0)
            {
                if (effect.apModify > 0)
                {
                    ui._text += "\nAP: +" + effect.apModify; 
                }
                else
                {
                    ui._text += "\nAP: " + effect.apModify;
                }
            }
            if (effect.mpModify != 0)
            {
                if (effect.mpModify > 0)
                {
                    ui._text += "\nMP: +" + effect.mpModify; 
                }
                else
                {
                    ui._text += "\nMP: " + effect.mpModify;
                }
            }



            ui.ShowTooltip();
        }
        else
        {
            Debug.Log("error");
        }
        ui.ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.HideTooltip();
        
    }
}
