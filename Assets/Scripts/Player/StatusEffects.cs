using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffects : MonoBehaviour {

    List<EffectValues> effectList;
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
        effectList.Add(clone);
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
                    effectList.Remove(effect);
                }
            } 
        }
    }
}//Last edited by Ilari
