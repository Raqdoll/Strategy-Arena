using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "New Spell")]
public class SpellValues : ScriptableObject
{
    [Header("Basic info")]
    [Space(6)]
    public string mySpellName;
    public Sprite spellIcon;
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
    public int spellApCost; //<
    public int spellDamageMin;  //<
    public int spellDamageMax;  //<
    public int spellHealMin;    //<
    public int spellHealMax;    //<
    public bool healsAlly = false;  //<
    public bool hurtsAlly = false;  //<
    public bool needLineOfSight = true; //<
    [Space(6)]
    [Header("Effect")]
    public EffectValues effect;
    [Space(6)]
    [Header("Map manipulation")]
    [Space(6)]
    public int spellPushback;   //<
    public int spellPull;   //<
    [Tooltip("ON: pushes/pulls from caster, OFF: -from AoE center")]
    public bool moveFromCaster = true; //<
    public bool teleportToTarget = false;   //<
    public bool chagePlaceWithTarget = false;   //<
    public int moveCloserToTarget;  //<
    public int MoveAwayFromTarget;  //<
    [Space(6)]
    [Header("Cooldowns")]
    [Space(6)]
    public int spellInitialCooldown;    //<
    public int spellCooldown;   //<
    public int spellCastPerturn;    //<
    public int castPerTarget;   //<
    [Space(6)]
    [Header("Ingame values, do not touch")]
    [Space(6)]
    public int spellCooldownLeft;   //<
    public int trueDamage;  //<
    public bool spellLaunched = false;  //<
    public bool inCooldown = false; //<
}

