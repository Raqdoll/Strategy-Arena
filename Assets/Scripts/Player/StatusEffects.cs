using System.Collections;
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
        effectList.Clear();
    }

    //Call this when you add another effect
    public void ApplyEffect(CharacterValues caster, EffectValues effect, CharacterValues target)
    {
        EffectValues clone = Object.Instantiate(effect) as EffectValues;
        clone.caster = caster;
        clone.target = target;
        clone.remainingTurns = clone.effectDuration;

        foreach (EffectValues eff in effectList)
        {
            if (eff.target == clone.target)
            {
                if (eff.name == clone.name)
                {
                    if (clone.stacks == false)
                    {
                        eff.remainingTurns = clone.effectDuration;      //New spells are always better!
                    }
                    else
                    {
                        effectList.Add(clone);
                        pBehaviour.AddTabEffect(clone, target);
                    }
                }
                else
                {
                    effectList.Add(clone);
                    pBehaviour.AddTabEffect(clone, target);
                }
            }
        }
        CalculateEffects(target);
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
                    //effect.target.damagePlus -= effect.damageModifyPlus;
                    //effect.target.damageChange -= effect.damageModifyPercent;
                    //effect.target.armorPlus -= effect.armorModifyPlus;
                    //effect.target.armorChange -= effect.armorModifyPercent;
                    //effect.target.healsReceived -= effect.healModify;
                    //effect.target.maxAp -= effect.apModify;
                    //effect.target.maxMp -= effect.mpModify;
                    //if (effect.immune == true)
                    //{
                    //    effect.target.armorChange -= 1000;
                    //}
                    //if (effect.heavyState == true)
                    //{
                    //    effect.target.heavy = false; //saattaa tuottaa ongelmia jos useampi heavy state käytössä! Korjaus kun tarve
                    //}
                    ////pBehaviour.RemoveTabEffect(effect);
                    effectList.Remove(effect);
                }
            } 
        }
    }

    public void CalculateEffects(CharacterValues effectTarget)
    {
        if (effectTarget == null)
        {
            Debug.Log("EffectTarget was null!");
            return;
        }
        List<EffectValues> effects = GetEffects(effectTarget);
        if (effects == null)
        {
            Debug.Log("EffectList was null!");
            return;
        }
        ClearEffectValues(effectTarget);
                //Laita tähän tabien clearaus!
        if (effects.Count == 0)
        {
            Debug.Log("No active effects");
            return;
        }
        List<string> effectNames = new List<string>();
        foreach (EffectValues effect in effects)
        {
            if (effect.stacks || !effectNames.Contains(effect.name))
            {
                effectTarget.damagePlus += effect.damageModifyPlus;
                effectTarget.damageChange += effect.damageModifyPercent;
                effectTarget.armorPlus += effect.armorModifyPlus;
                effectTarget.armorChange += effect.armorModifyPercent;
                effectTarget.healsReceived += effect.healModify;
                effectTarget.currentAp += effect.apModify;
                effectTarget.currentMp += effect.mpModify;
                if (effect.immune == true)
                {
                    effectTarget.armorChange = 1000;
                }
                if (effect.heavyState == true)
                {
                    effectTarget.heavy = true;
                }
                if (!effectNames.Contains(effect.name))
                {
                    effectNames.Add(effect.name);
                }
                //pBehaviour.AddTabEffect(effect, effectTarget);
            }
        }
    }

    public void ClearEffectValues(CharacterValues effectTarget)
    {
        effectTarget.damagePlus = 0;
        effectTarget.damageChange = 0;
        effectTarget.armorPlus = 0;
        effectTarget.armorChange = 0;
        effectTarget.healsReceived = 0;
        effectTarget.maxAp = 0;
        effectTarget.maxMp = 0;
        effectTarget.armorChange = 0;
        effectTarget.heavy = false;
    }

}//Last edited by Ilari
