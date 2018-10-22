using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "New Spell")]
public class SpellValues : ScriptableObject
{   public string mySpellName;
    public Sprite spellIcon;
    public Abilities.SpellAreaType mySpellAreaType;
    public Abilities.SpellRangeType mySpellRangeType;
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
    public int aoeRange;
    public int spellCooldownLeft;
    public int trueDamage;
    public bool healsAlly = false;
    public bool hurtsAlly = false;
    public bool needLineOfSight = false;
    public bool spellLaunched = false;
    public bool inCooldown = false;
}

