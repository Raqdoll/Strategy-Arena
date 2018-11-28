using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {

    public CharacterValues thisCharacter;



    /// <summary>
    /// Quick fix for refreshing movement and action points at the beginning of a turn. Does not take status effects into account and might be moved elsewhere.
    /// </summary>
    /// 
    void Start()
    {

    }

    internal void RefreshPoints()
    {
        thisCharacter.currentMp = thisCharacter.maxMp;
        thisCharacter.currentAp = thisCharacter.maxAp;
    }
}
