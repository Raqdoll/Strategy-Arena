using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "New Effect")]
public class EffectValues : ScriptableObject {
    [Header("Info")]
    [Space(6)]
    public string effectName;
    public Sprite effectIcon;
    [Space(6)]
    [Header("Buffs/Debuffs")]
    [Space(6)]
    public int effectDuration;
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
    [Header("Do not touch!")]
    [Space(6)]

    public int remainingTurns;
    public CharacterValues caster;
    public CharacterValues target;
}
