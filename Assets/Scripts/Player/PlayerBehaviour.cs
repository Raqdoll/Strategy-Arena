﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Acts as the controller/nerve center for player classes and stores most of the variables.
/// All input goes through this class.
/// </summary>
public class PlayerBehaviour : MonoBehaviour
{

    //public enum CharacterClass { Tank1, Healer1, Support1, DmgDealer1 }; // Put Classes Here 
    ////public Button spellButton1, spellButton2, spellButton3, spellButton4, spellButton5, spellButton6;
    //public bool moving;  //e.g. Hover to get movement range vs ability range
    //public CharacterClass myCharClass;
    //public int maxHP;
    //public int currentHP;
    //public int maxAp;
    //public int maxMp;
    //public int currentAp;
    //public int currentMp;
    //public int playerNum;
    //public float damageChange;
    //public float healsReceived;
    //public int damagePlus;
    //public Tile currentTile;
    public List<CharacterValues> charList;

    public CharacterValues currentCharacter;

    public List<GameObject> charTabList;

    GridController gridController;
    TurnManager turnManager;
    Abilities abilities;

    public AudioController aControll;

    public EffectValues testing;

    public SpellCast sCast;

    void Start()
    {
        if (!gridController)
            gridController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridController>();
        if (!gridController)
            Debug.LogWarning("Gridcontroller is null!");
        if (!aControll)
            aControll = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
        if (!sCast)
            sCast = gameObject.GetComponent<SpellCast>();

        //if (!currentTile)
        //    currentTile = gridController.GetTile((int)transform.localPosition.x, (int)transform.localPosition.z);

        currentCharacter.currentTile = new PositionContainer(12, 12);

        turnManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TurnManager>();
        turnManager.TurnChange += HandleTurnChange;

        //Tab testing V V V

        //foreach (GameObject tab in charTabList)
        //{
        //    tab.GetComponent<CharacterTab>().AddEffectIcon(testing);
        //    tab.GetComponent<CharacterTab>().AddEffectIcon(testing);
        //    tab.GetComponent<CharacterTab>().AddEffectIcon(testing);
        //}
    }

    private void OnDestroy()
    {
        turnManager.TurnChange -= HandleTurnChange;
    }

    private void HandleTurnChange(PlayerInfo player)
    {
        currentCharacter = player.thisCharacter;
    }

    public void UpdateTabs()
    {
        foreach (GameObject tab in charTabList)
        {
            CharacterTab tabby = tab.GetComponent<CharacterTab>();
            tabby.UpdateInfo();
            sCast.DisableButtonsIfNotAp();
        }
    }
    public void AddTabEffect(EffectValues effect, CharacterValues target)
    {
        foreach (GameObject tab in charTabList)
        {
            CharacterTab tabby = tab.GetComponent<CharacterTab>();
            if (tabby.characterVal == target)
            {
                tabby.AddEffectIcon(effect);
            }
        }
    }
    public CharacterTab GetTab(CharacterValues character)
    {
        foreach (GameObject tab in charTabList)
        {
            CharacterTab tabby = tab.GetComponent<CharacterTab>();
            if (tabby.characterVal == character)
            {
                return tabby;
            }
        }
        Debug.Log("No tab found");
        return null;
    }
    //public void RemoveTabEffect(EffectValues effect)
    //{
    //    foreach (GameObject tab in charTabList)
    //    {
    //        CharacterTab tabby = tab.GetComponent<CharacterTab>();
    //        if (GetComponentInChildren<GameObject>() != null)
    //        {
    //            GameObject removed = tabby.GetComponentInChildren<GridLayoutGroup>().GetComponentInChildren<GameObject>();
    //            if (removed.GetComponent<Tooltip>().effect == effect)
    //            {
    //                tabby.RemoveEffectIcon(removed.GetComponent<Tooltip>().effect);
    //            } 
    //        }
    //    }
    //}
}







