using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterTab : MonoBehaviour {

    public static int tabNumber;
    public Text tName;
    public Text tHealth;
    public Text tAp;
    public Text tMp;
    public GameObject spells;
    public GameObject effects;
    public Image characterIcon;
    PlayerBehaviour _player;
    bool isMyPlayerActive;

    private void Start()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        SubscribeOff();
    }

    private void Subscribe()
    {
        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
        TurnManager TM = GC.GetComponent<TurnManager>();
        TM.TurnChange += handleTurnChange;
    }

    //Voi laittaa myös pelaajan kuoleman kohdalle
    private void SubscribeOff()
    {
        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
        TurnManager TM = GC.GetComponent<TurnManager>();
        TM.TurnChange -= handleTurnChange;
    }

    private void handleTurnChange(PlayerBehaviour player)
    {
        if (_player == player)
            isMyPlayerActive = true;
        else
            isMyPlayerActive = false;
    }

    public void UpdateHp(int minHP, int maxHP)
    {
        tHealth.text ="HP: " + minHP.ToString() + " / " + maxHP.ToString();
    }

    public void UpdateAp(int AP)
    {
        tAp.text = "AP: " + AP.ToString();
    }

    public void UpdateMp(int MP)
    {
        tMp.text = "MP: " + MP.ToString();
    }

    public void UpdateName(string name)
    {
        tName.text = name;
    }
}
