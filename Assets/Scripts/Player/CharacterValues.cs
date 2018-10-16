using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "New Character")]
public class CharacterValues : ScriptableObject
{
    public string characterName;
    public Sprite portrait;
    public int maxHP;
    public int maxAp;
    public int maxMp;
    public int currentHP;
    public int currentAp;
    public int currentMp;
    public float damageChange;
    public float armorChange;
    public float healsReceived;
    public int damagePlus;
    public int armorPlus;
    public bool moving;
    public Tile currentTile;
    public bool dead;
    public bool team; // true = 1 false = 2
    public SpellValues spell_1;
    public SpellValues spell_2;
    public SpellValues spell_3;
    public SpellValues spell_4;
    public SpellValues spell_5;
    public SpellValues spell_6;
}
