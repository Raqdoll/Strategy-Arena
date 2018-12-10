using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInfo : MonoBehaviour {

    public CharacterValues character;
    public GameObject arrow;

    void Start()
    {
        GetComponent<Tooltip>().character = character;
    }
}
