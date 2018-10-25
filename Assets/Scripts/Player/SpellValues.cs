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
    [Header("Map manipulation")]
    [Space(6)]
    public int spellPushback;   //<
    public int spellPull;   //<
    public bool teleportToTarget = false;   //<
    public bool chagePlaceWithTarget = false;   //<
    public int moveCloserToTarget;  //<
    public int MoveAwayFromTarget;  //<
    [Space(6)]
    [Header("Buffs/Debuffs")]
    [Space(6)]
    public int silenceTurns;    //<
    public int immuneTurns;   //<
    public int heavyStateTurns; //<
    [Space(6)]
    public int damageModifyPlus = 0;    //<
    public bool dmPlusAllies;
    public bool dmPlusEnemies;
    [Space(6)]
    [Range(-1f, 1f)]
    public float damageModifyPercent = 0;   //<
    public bool dmPercentAllies;
    public bool dmPercentEnemies;
    [Space(6)]
    [Range(-1f, 1f)]
    public float armorModify = 0;   //<
    public bool amAllies;
    public bool amEnemies;
    [Space(6)]
    [Range(-1f, 1f)]
    public float healModify = 0;    //<
    public bool hmAllies;
    public bool hmEnemies;
    [Space(6)]
    public int apModify = 0;    //<
    public bool apAllies;
    public bool apEnemies;
    [Space(6)]
    public int mpModify = 0;    //<
    public bool mpAllies;
    public bool mpEnemies;
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

