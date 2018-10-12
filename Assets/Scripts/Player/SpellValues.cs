using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "New Spell")]
public class SpellValues : ScriptableObject
{   public string mySpellName;
    public Sprite spellIcon;
    public int mySpellAreaType;
    public int mySpellRangeType;
    public int spellInitialCooldown;
    public int spellDamageMin;
    public int spellDamageMax;
    public int spellRangeMin;
    public int spellRangeMax;
    public int spellCooldown;
    public int spellCastPerturn;
    public int castPerTarget;
    public int spellApCost;
    public int spellPushback;
    public int spellPull;
    public int areaRange;
    public int spellCooldownLeft;
    public int trueDamage;
    public bool healsAlly = false;
    public bool hurtsAlly = false;
    public bool spellOpen = false;
    public bool needLineOfSight = false;
    public bool spellLaunched = false;
    public bool inCooldown = false;
}

