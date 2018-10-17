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
    public GameObject spell1;
    public GameObject spell2;
    public GameObject spell3;
    public GameObject spell4;
    public GameObject spell5;
    public GameObject spell6;
    public GameObject effects;
    public Image characterIcon;
    PlayerBehaviour _player;
    bool isMyPlayerActive;
    public CharacterValues characterVal;
    public GameObject healthBar;

    void Start()
    {
        //Subscribe();

        if (characterVal)
        {
            if (characterVal.portrait)
            {
                characterIcon.sprite = characterVal.portrait;
            }
            UpdateSpellIcons();
            UpdateInfo();
        }
    }

    public void UpdateInfo()
    {
        UpdateName(characterVal.characterName);
        UpdateHp(characterVal.currentHP, characterVal.maxHP);
        UpdateAp(characterVal.currentAp);
        UpdateMp(characterVal.currentMp);
    }
    public void UpdateSpellIcons()
    {
        spell1.GetComponent<Image>().sprite = characterVal.spell_1.spellIcon;
        spell2.GetComponent<Image>().sprite = characterVal.spell_2.spellIcon;
        spell3.GetComponent<Image>().sprite = characterVal.spell_3.spellIcon;
        spell4.GetComponent<Image>().sprite = characterVal.spell_4.spellIcon;
        spell5.GetComponent<Image>().sprite = characterVal.spell_5.spellIcon;
        spell6.GetComponent<Image>().sprite = characterVal.spell_6.spellIcon;
    }

    private void OnDestroy()
    {
        //SubscribeOff();
    }

    //private void Subscribe()
    //{
    //    GameObject GC = GameObject.FindGameObjectWithTag("GameController");
    //    TurnManager TM = GC.GetComponent<TurnManager>();
    //    TM.TurnChange += handleTurnChange;
    //}

    ////Voi laittaa myös pelaajan kuoleman kohdalle
    //private void SubscribeOff()
    //{
    //    GameObject GC = GameObject.FindGameObjectWithTag("GameController");
    //    TurnManager TM = GC.GetComponent<TurnManager>();
    //    TM.TurnChange -= handleTurnChange;
    //}

    private void handleTurnChange(PlayerBehaviour player)
    {
        if (_player == player)
            isMyPlayerActive = true;
        else
            isMyPlayerActive = false;
    }

    public void UpdateHp(int minHP, int maxHP)
    {
        tHealth.text = minHP.ToString() + "/" + maxHP.ToString();

        UpdateHpBar();
    }

    public void UpdateAp(int AP)
    {
        tAp.text = AP.ToString();
    }

    public void UpdateMp(int MP)
    {
        tMp.text = MP.ToString();
    }

    public void UpdateName(string name)
    {
        tName.text = name;
    }

    public void UpdateHpBar()
    {
        float minHPf = characterVal.currentHP;
        float maxHPf = characterVal.maxHP;
        float hpBarAmount = minHPf / maxHPf;
        healthBar.GetComponent<Image>().fillAmount = hpBarAmount;
    }
}
