using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "New Spell")]
public class SpellValues : ScriptableObject
{
    [Header("Basic info")]
    [Space(6)]
    public string mySpellName;
    public Sprite spellIcon;
    public AudioClip spellSound; //<
    [Space(6)]
    [Header("Ranges")]
    [Space(6)]
    public Abilities.SpellRangeType mySpellRangeType;
    public int spellRangeMin;
    public int spellRangeMax;
    public Abilities.SpellAreaType mySpellAreaType;
    public int aoeRange;
    [Space(6)]
    [Header("Damage/Heal")]
    [Space(6)]
    public int spellApCost;
    public bool damageStealsHp = false; //<
    public int spellDamageMin;
    public int spellDamageMax;
    public int spellHealMin;
    public int spellHealMax;
    public bool healsAlly = false;
    public bool hurtsAlly = false;
    public bool needLineOfSight = true;
    public bool needTarget = false;
    public bool needFreeSquare = false;
    [Space(6)]
    [Header("Effect")]
    public EffectValues effect;
    public bool effectOnCaster = false; //<
    public bool effectOnTarget = true; //<
    [Space(6)]
    [Header("Map manipulation")]
    [Space(6)]
    public int spellPushback;
    public Abilities.SpellPushType mySpellPushType;
    public int spellPull;
    public Abilities.SpellPullType mySpellPullType;
    
    public bool teleportToTarget = false;  
    public bool chagePlaceWithTarget = false;
    public int moveCloserToTarget;
    public int moveAwayFromTarget;
    [Space(6)]
    [Header("Cooldowns")]
    [Space(6)]
    public int spellInitialCooldown; 
    public int spellCooldown;
    public int spellCastPerturn;
    public int castPerTarget;   //<
    [Header("Cooldowns counters")]
    [Space(6)]
    public int spellInitialCooldowncounter;
    public int spellCastPerturncounter; 
    public int castPerTargetcounter; //<
    [Space(6)]
    [Header("Description for the spell")]
    [Space(6)]
    [TextArea]
    public string description;
    [Space(6)]
    [Header("Ingame values, do not touch")]
    [Space(6)]
    public int spellCooldownLeft = 0; 
    public int trueDamage;
}

