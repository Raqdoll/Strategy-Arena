using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Acts as the controller/nerve center for player classes and stores most of the variables.
/// All input goes through this class.
/// </summary>
public class PlayerBehaviour : MonoBehaviour {

    public enum CharacterClass { Tank1, Healer1, Support1, DmgDealer1 }; // Put Classes Here 
    //public Button spellButton1, spellButton2, spellButton3, spellButton4, spellButton5, spellButton6;
    public bool moving;  //e.g. Hover to get movement range vs ability range
    public CharacterClass myCharClass;
    public int maxHP;
    public int currentHP;
    public int maxAp;
    public int maxMp;
    public int currentAp;
    public int currentMp;
    public double damageChange;
    public bool spellOpen = false;
    GridController gridController;
    Abilities abilities;

    Tile tilescripts;

    void Start () {

        gridController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridController>();
        if (!gridController)
            Debug.LogWarning("Gridcontroller is null!");
        tilescripts = GetComponent<Tile>();


        //Button cast1 = spellButton1.GetComponent<Button>();
        //Button cast2 = spellButton2.GetComponent<Button>();
        //Button cast3 = spellButton3.GetComponent<Button>();
        //Button cast4 = spellButton4.GetComponent<Button>();
        //Button cast5 = spellButton5.GetComponent<Button>();
        //Button cast6 = spellButton6.GetComponent<Button>();
        // Put Information Of classes Here
        switch (myCharClass)
        {
            
            case CharacterClass.Tank1:
                maxHP = 4500;
                maxAp = 11;
                maxMp = 6;
                currentHP = maxHP;
                currentAp = maxAp;
                currentMp = maxMp;
                damageChange = 1.0;
                //cast1.onClick.AddListener(GetComponentInChildren(tag("spell1"))); // Adds Spell to Button
                //cast1.GetComponentInChildren<Text>().text = "SpellBase"; // Add Spell Name Manually
                //cast2.onClick.AddListener(SpellBase);
                //cast2.GetComponentInChildren<Text>().text = "SpellBase";
                //cast3.onClick.AddListener(SpellBase);
                //cast3.GetComponentInChildren<Text>().text = "SpellBase";
                //cast4.onClick.AddListener(SpellBase);
                //cast4.GetComponentInChildren<Text>().text = "SpellBase";
                //cast5.onClick.AddListener(SpellBase);
                //cast5.GetComponentInChildren<Text>().text = "SpellBase";
                //cast6.onClick.AddListener(SpellBase);
                //cast6.GetComponentInChildren<Text>().text = "SpellBase";
                break;
            case CharacterClass.Support1:
                maxHP = 3000;
                maxAp = 12;
                maxMp = 6;
                currentHP = maxHP;
                currentAp = maxAp;
                currentMp = maxMp;
                damageChange = 1.0;
                break;
            case CharacterClass.Healer1:
                maxHP = 3000;
                maxAp = 12;
                maxMp = 6;
                currentHP = maxHP;
                currentAp = maxAp;
                currentMp = maxMp;
                damageChange = 1.0;
                break;
            case CharacterClass.DmgDealer1:
                maxHP = 3000;
                maxAp = 12;
                maxMp = 6;
                currentHP = maxHP;
                currentAp = maxAp;
                currentMp = maxMp;
                damageChange = 1.0;
                break;
        }
	}
	void Update () {
        if (spellOpen == true)
        {
            Debug.Log("Spelll Open");
            abilities.RangeType();
            foreach (var tile in abilities.RangeType())
            {
                tile.GetComponent<Renderer>().material.color = tilescripts.RangeMaterial.color;
            }
            if (Input.GetMouseButtonDown(1))
            {
                foreach (var tile in abilities.RangeType())
                {
                    tile.GetComponent<Renderer>().material.color = tilescripts.BaseMaterial.color;
                }
                abilities.SpellCancel();
            }
            //if (Input.GetMouseButtonDown(0))
            //{
            //    LaunchSpell();
            //}
        }
    }
}
