﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffects : MonoBehaviour {

    public List<EffectValues> effectList = new List<EffectValues>();
    public TeamManager tManager;
    public PlayerBehaviour pBehaviour;

    void Start()
    {
        if (!tManager)
            tManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TeamManager>();
        if (!pBehaviour)
            pBehaviour = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerBehaviour>();
    }

    //Call this when you add another effect
    public void ApplyEffect(CharacterValues caster, EffectValues effect, CharacterValues target)
    {
        EffectValues clone = Object.Instantiate(effect) as EffectValues;
        clone.caster = caster;
        clone.target = target;
        clone.remainingTurns = clone.effectDuration;



        bool inUse = false;
        if (effectList.Count != 0)
        {
            foreach (EffectValues eff in effectList)
            {
                if (eff.target == clone.target && eff.name == clone.name)
                {
                    inUse = true;
                }
                else
                {
                    effectList.Add(clone);
                    pBehaviour.AddTabEffect(clone, target);
                }
            } 
        }
        else
        {
            effectList.Add(clone);
            pBehaviour.AddTabEffect(clone, target);
        }

        if (clone.stacks == true || (clone.stacks == false && inUse == false))
        {
            target.damagePlus += clone.damageModifyPlus;
            target.damageChange += clone.damageModifyPercent;
            target.armorPlus += clone.armorModifyPlus;
            target.armorChange += clone.armorModifyPercent;
            target.healsReceived += clone.healModify;
            target.maxAp += clone.apModify;
            target.maxMp += clone.mpModify;
            if (clone.immune == true)
            {
                target.armorChange += 1000;
            }
            if (clone.heavyState == true)
            {
                target.heavy = true;
            }
        }
        if (clone.stacks == false && inUse == true)
        {
            foreach (EffectValues eff in effectList)
            {
                if (eff.name == clone.name && eff.target == clone.target)
                {
                    eff.remainingTurns = clone.effectDuration;
                }
            }
        }
    }

    //Calls all effects of a certain character
    public List<EffectValues> GetEffects(CharacterValues character)
    {
        List<EffectValues> tempList = null;
        foreach (EffectValues effect in effectList)
        {
            if(effect.target == character)
            {
                tempList.Add(effect);
            }
        }
        return tempList;
    }

    //Updates remaining turns
    public void UpdateEffects()
    {
        if (effectList != null)
        {
            foreach (EffectValues effect in effectList)
            {
                //If caster's turn, decrease timer
                if (effect.caster == tManager.activePlayer.thisCharacter)
                {
                    effect.remainingTurns -= 1;
                }
                //Checks if the spell stays active
                if (effect.remainingTurns <= 0)
                {
                    effect.target.damagePlus -= effect.damageModifyPlus;
                    effect.target.damageChange -= effect.damageModifyPercent;
                    effect.target.armorPlus -= effect.armorModifyPlus;
                    effect.target.armorChange -= effect.armorModifyPercent;
                    effect.target.healsReceived -= effect.healModify;
                    effect.target.maxAp -= effect.apModify;
                    effect.target.maxMp -= effect.mpModify;
                    if (effect.immune == true)
                    {
                        effect.target.armorChange -= 1000;
                    }
                    if (effect.heavyState == true)
                    {
                        effect.target.heavy = false; //saattaa tuottaa ongelmia jos useampi heavy state käytössä! Korjaus kun tarve
                    }
                    //pBehaviour.RemoveTabEffect(effect);
                    effectList.Remove(effect);
                }
            } 
        }
    }
}//Last edited by Ilari
