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


    public void updateHp(int minHP, int maxHP)
    {
        tHealth.text ="HP: " + minHP.ToString() + " / " + maxHP.ToString();
    }

    public void updateAp(int AP)
    {
        tAp.text = "AP: " + AP.ToString();
    }

    public void updateMp(int MP)
    {
        tMp.text = "MP: " + MP.ToString();
    }

    public void updateName(string name)
    {
        tName.text = name;
    }
}
